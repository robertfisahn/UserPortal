using Newtonsoft.Json;
using UserPortal.Entities;
using UserPortal.Exceptions;
using UserPortal.Interfaces;

namespace UserPortal.Services
{
    public class TokenService : ITokenService
    {
        private readonly HttpClient _httpClient;
        public TokenService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<Token> GetTokens()
        {
            var response = _httpClient.GetAsync($"https://localhost:44317/api/token").Result;
            response.EnsureSuccessStatusCode();
            if (response == null) throw new NotFoundException("Tokens not found");
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var tokens = JsonConvert.DeserializeObject<List<Token>>(responseBody);
            if (tokens == null) throw new NotFoundException("Tokens not found");
            return tokens;
        }
    }
}
