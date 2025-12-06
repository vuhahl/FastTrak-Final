using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FastTrak.Services
{
    public class FatSecretService
    {

        private readonly HttpClient _http = new();
        private readonly string _clientId = "a4c9b42e86d44062bd0bcad7c04b3e14";
        private readonly string _clientSecret = "32e5b55158a24e798c143d1393eed153";

        private string _accessToken = string.Empty;
        private DateTime _tokenExpires;

        // ----------------------------
        // TOKEN REQUEST
        // ----------------------------
        private async Task<string> GetAccessTokenAsync()
        {
            if (!string.IsNullOrEmpty(_accessToken) && DateTime.Now < _tokenExpires)
                return _accessToken;

            var authString = $"{_clientId}:{_clientSecret}";
            var authBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(authString));

            var request = new HttpRequestMessage(HttpMethod.Post, "https://oauth.fatsecret.com/connect/token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authBase64);
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "scope", "basic" }
            });

            var response = await _http.SendAsync(request);
            var json = await response.Content.ReadFromJsonAsync<FatSecretTokenResponse>();

            _accessToken = json.access_token;
            _tokenExpires = DateTime.Now.AddSeconds(json.expires_in - 30);

            return _accessToken;
        }

        // ----------------------------
        // SEARCH FOODS
        // ----------------------------
        public async Task<List<FatSecretFoodSearchItem>> SearchFoodsAsync(string query)
        {
            await GetAccessTokenAsync();

            var body = new Dictionary<string, string>
            {
                { "method", "foods.search" },
                { "search_expression", query },
                { "format", "json" }
            };

            var req = new HttpRequestMessage(HttpMethod.Post,
                "https://platform.fatsecret.com/rest/server.api")
            {
                Content = new FormUrlEncodedContent(body)
            };

            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            var response = await _http.SendAsync(req);

            var result = await response.Content.ReadFromJsonAsync<FatSecretSearchResponse>();

            return result?.foods?.food ?? new List<FatSecretFoodSearchItem>();
        }

        // ----------------------------
        // GET FULL FOOD DETAILS
        // ----------------------------
        public async Task<FatSecretFoodDetails> GetFoodDetailsAsync(string foodId)
        {
            await GetAccessTokenAsync();

            var body = new Dictionary<string, string>
            {
                { "method", "food.get" },
                { "food_id", foodId },
                { "format", "json" }
            };

            var req = new HttpRequestMessage(HttpMethod.Post,
                "https://platform.fatsecret.com/rest/server.api")
            {
                Content = new FormUrlEncodedContent(body)
            };

            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            var response = await _http.SendAsync(req);

            return await response.Content.ReadFromJsonAsync<FatSecretFoodDetails>();
        }
    }
}

// ----------------------------
// PUBLIC RESPONSE MODELS
// ----------------------------

namespace FastTrak.Services
{
    public class FatSecretTokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }

    public class FatSecretSearchResponse
    {
        public FatSecretFoods foods { get; set; }
    }

    public class FatSecretFoods
    {
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
 

