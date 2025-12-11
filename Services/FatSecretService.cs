using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FastTrak.Services
{
    public class FatSecretService
    {
        private readonly HttpClient _http;

        // IMPORTANT: Replace these with your real keys.
        private readonly string _clientId = "YOUR_CLIENT_ID";
        private readonly string _clientSecret = "YOUR_CLIENT_SECRET";

        private string _accessToken = string.Empty;
        private DateTime _tokenExpiresUtc;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public FatSecretService(HttpClient httpClient = null)
        {
            _http = httpClient ?? new HttpClient();
        }

        // ============================================================
        // OAuth2 Token Request
        // ============================================================
        private async Task<string> GetAccessTokenAsync(CancellationToken ct = default)
        {
            if (!string.IsNullOrEmpty(_accessToken) && DateTime.UtcNow < _tokenExpiresUtc)
                return _accessToken;

            var authString = $"{_clientId}:{_clientSecret}";
            var authBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(authString));

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://oauth.fatsecret.com/connect/token");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Basic", authBase64);

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["scope"] = "basic"
            });

            using var response = await _http.SendAsync(request, ct);
            var content = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Token request failed: {content}");

            var token = JsonSerializer.Deserialize<FatSecretTokenResponse>(content, JsonOptions)
                        ?? throw new Exception("Invalid token response");

            _accessToken = token.access_token;
            _tokenExpiresUtc = DateTime.UtcNow.AddSeconds(token.expires_in - 60);

            return _accessToken;
        }

        // ============================================================
        // foods.search v1 (REST GET)
        // ============================================================
        public async Task<List<FatSecretFoodSearchItem>> SearchFoodsAsync(
            string query,
            int maxResults = 20,
            int pageNumber = 0,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<FatSecretFoodSearchItem>();

            query = query.Trim();

            var token = await GetAccessTokenAsync(ct);

            var url =
                $"https://platform.fatsecret.com/rest/foods/search/v1" +
                $"?format=json" +
                $"&max_results={maxResults}" +
                $"&page_number={pageNumber}" +
                $"&search_expression={Uri.EscapeDataString(query)}";

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var response = await _http.SendAsync(request, ct);
            var content = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"foods.search failed: {content}");

            var result = JsonSerializer.Deserialize<FatSecretSearchResponse>(content, JsonOptions);
            return result?.foods?.food ?? new List<FatSecretFoodSearchItem>();
        }

        // ============================================================
        // food.get (POST method=food.get)
        // ============================================================
        public async Task<FatSecretFoodDetails> GetFoodDetailsAsync(
            string foodId,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(foodId))
                throw new ArgumentException("foodId is required");

            var token = await GetAccessTokenAsync(ct);

            var body = new Dictionary<string, string>
            {
                ["method"] = "food.get",
                ["food_id"] = foodId,
                ["format"] = "json"
            };

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://platform.fatsecret.com/rest/server.api")
            {
                Content = new FormUrlEncodedContent(body)
            };

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using var response = await _http.SendAsync(request, ct);
            var content = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"food.get failed: {content}");

            var result = JsonSerializer.Deserialize<FatSecretFoodDetails>(content, JsonOptions)
                        ?? throw new Exception("Invalid food.get JSON");

            return result;
        }
    }

    // ============================================================
    // MODELS — EXACT FatSecret JSON Structures
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
    }

    public class FatSecretFoodDetails
    {
        public FatSecretFood food { get; set; }
    }

    public class FatSecretFood
    {
        public FatSecretServings servings { get; set; }
    }

    public class FatSecretServings
    {
        public FatSecretServing serving { get; set; }
    }

    public class FatSecretServing
    {
        public double calories { get; set; }
        public double protein { get; set; }
        public double carbohydrate { get; set; }
        public double fat { get; set; }
        public double sodium { get; set; }
    }
}
