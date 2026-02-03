using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace FastTrak.Services
{
    public class FatSecretService
    {
        private readonly HttpClient _http;

        // Prefer injecting these via DI/config. Defaults retained for compatibility.
        private readonly string _clientId;
        private readonly string _clientSecret;

        private string _accessToken = string.Empty;
        private DateTime _tokenExpiresUtc;

        // Toggle if you want to silence raw logs in production builds.
        public bool EnableDebugLogging { get; set; } = true;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public FatSecretService(
            HttpClient httpClient = null,
            string clientId = "a4c9b42e86d44062bd0bcad7c04b3e14",
            string clientSecret = "95b2a27d07fc4350a815a17496791662")
        {
            _http = httpClient ?? new HttpClient();
            _clientId = clientId ?? string.Empty;
            _clientSecret = clientSecret ?? string.Empty;
        }

        private void Log(string message)
        {
            if (!EnableDebugLogging) return;
            Debug.WriteLine($"[FatSecret] {message}");
        }

        private static string Truncate(string s, int maxChars = 1200)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            if (s.Length <= maxChars) return s;
            return s.Substring(0, maxChars) + "…(truncated)";
        }

        /// <summary>
        /// Gets OAuth2 access token (cached until expiration).
        /// </summary>
        private async Task<string> GetAccessTokenAsync(CancellationToken ct = default)
        {
            if (!string.IsNullOrEmpty(_accessToken) && DateTime.UtcNow < _tokenExpiresUtc)
                return _accessToken;

            if (string.IsNullOrWhiteSpace(_clientId) || _clientId.Contains("YOUR_CLIENT_ID", StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrWhiteSpace(_clientSecret) || _clientSecret.Contains("YOUR_CLIENT_SECRET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("FatSecret client credentials are not configured. Provide clientId/clientSecret via DI or constructor.");
            }

            var authString = $"{_clientId}:{_clientSecret}";
            var authBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(authString));

            using var request = new HttpRequestMessage(HttpMethod.Post, "https://oauth.fatsecret.com/connect/token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authBase64);

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["scope"] = "basic"
            });

            Log("Requesting OAuth token…");

            using var response = await _http.SendAsync(request, ct);
            var content = await response.Content.ReadAsStringAsync(ct);

            Log($"Token status: {(int)response.StatusCode} {response.StatusCode}");
            Log($"Token body: {Truncate(content)}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Token request failed: {response.StatusCode} - {content}");

            var token = JsonSerializer.Deserialize<FatSecretTokenResponse>(content, JsonOptions)
                        ?? throw new Exception("Invalid token response JSON (null)");

            if (string.IsNullOrWhiteSpace(token.access_token))
                throw new Exception("Token response missing access_token");

            _accessToken = token.access_token;

            // Guard against small/zero expires_in values.
            var safeSeconds = Math.Max(token.expires_in - 60, 30);
            _tokenExpiresUtc = DateTime.UtcNow.AddSeconds(safeSeconds);

            Log($"Token cached. Expires UTC: {_tokenExpiresUtc:o}");

            return _accessToken;
        }



        /// <summary>
        /// Searches for foods using FatSecret foods.search v1 API. Test as of 12-11-25
        /// </summary>
        public async Task<List<FatSecretFoodSearchItem>> SearchFoodsAsync(string query, int maxResults = 20)
        {
            var result = new List<FatSecretFoodSearchItem>();

            string token;
            try
            {
                token = await GetAccessTokenAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FatSecret] ❌ Token error: {ex.Message}");
                return result;
            }

            var url = $"https://platform.fatsecret.com/rest/server.api?method=foods.search&search_expression={Uri.EscapeDataString(query)}&format=json&max_results={maxResults}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            Debug.WriteLine($"[FatSecret] 🔍 Sending request to: {url}");

            var response = await _http.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            Debug.WriteLine($"[FatSecret] 🌐 Status: {response.StatusCode}");
            Debug.WriteLine($"[FatSecret] 📄 Raw JSON: {json}");

            if (!response.IsSuccessStatusCode)
                return result;

            using var doc = JsonDocument.Parse(json);

            try
            {
                if (doc.RootElement.TryGetProperty("foods", out var foodsObj) &&
                    foodsObj.TryGetProperty("food", out var foodArray))
                {
                    if (foodArray.ValueKind == JsonValueKind.Object)
                    {
                        result.Add(JsonSerializer.Deserialize<FatSecretFoodSearchItem>(foodArray.GetRawText()));
                    }
                    else if (foodArray.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var food in foodArray.EnumerateArray())
                        {
                            result.Add(JsonSerializer.Deserialize<FatSecretFoodSearchItem>(food.GetRawText()));
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("[FatSecret] ⚠️ No 'foods.food' array found in response.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[FatSecret] ❌ JSON parse error: {ex}");
            }

            return result;
        }


        /// <summary>
        /// Gets detailed nutrition info for a specific food (food.get).
        /// </summary>
        public async Task<FatSecretFoodDetails> GetFoodDetailsAsync(
            string foodId,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(foodId))
                throw new ArgumentException("foodId is required", nameof(foodId));

            var token = await GetAccessTokenAsync(ct);

            var body = new Dictionary<string, string>
            {
                ["method"] = "food.get",
                ["food_id"] = foodId,
                ["format"] = "json"
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, "https://platform.fatsecret.com/rest/server.api")
            {
                Content = new FormUrlEncodedContent(body)
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            Log($"POST https://platform.fatsecret.com/rest/server.api (method=food.get, food_id={foodId})");

            using var response = await _http.SendAsync(request, ct);
            var content = await response.Content.ReadAsStringAsync(ct);

            Log($"food.get status: {(int)response.StatusCode} {response.StatusCode}");
            Log($"food.get body: {Truncate(content)}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"food.get failed: {response.StatusCode} - {content}");

            var result = JsonSerializer.Deserialize<FatSecretFoodDetails>(content, JsonOptions);

            if (result?.food == null)
                throw new Exception($"food.get returned unexpected JSON (food is null). Body: {Truncate(content)}");

            return result;
        }
    }

    // ============================================================
    // MODELS — FatSecret JSON Structures
    // ============================================================

    public class FatSecretTokenResponse
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
    }

    public class FatSecretSearchResponse
    {
        public FatSecretFoods foods { get; set; }
    }

    public class FatSecretFoods
    {
        public int max_results { get; set; }
        public int total_results { get; set; }
        public int page_number { get; set; }
        public List<FatSecretFoodSearchItem> food { get; set; }
    }

    public class FatSecretFoodSearchItem
    {
        public string food_id { get; set; }
        public string food_name { get; set; }
        public string food_description { get; set; }
        public string food_type { get; set; }
        public string brand_name { get; set; }
    }

    public class FatSecretFoodDetails
    {
        public FatSecretFood food { get; set; }
    }

    public class FatSecretFood
    {
        public string food_id { get; set; }
        public string food_name { get; set; }
        public FatSecretServings servings { get; set; }
    }

    /// <summary>
    /// FatSecret can return servings.serving as either an object OR an array.
    /// We store it as JsonElement and normalize later.
    /// </summary>
    public class FatSecretServings
    {
        public JsonElement serving { get; set; }
    }

    public class FatSecretServing
    {
        public string serving_id { get; set; }
        public string serving_description { get; set; }
        public string metric_serving_amount { get; set; }
        public string metric_serving_unit { get; set; }
        public string measurement_description { get; set; }

        // Nutrition values (FatSecret returns these as strings)
        public string calories { get; set; }
        public string protein { get; set; }
        public string carbohydrate { get; set; }
        public string fat { get; set; }
        public string sodium { get; set; }
        public string sugar { get; set; }
        public string fiber { get; set; }
    }
}
