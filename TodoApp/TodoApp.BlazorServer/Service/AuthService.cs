using Blazored.LocalStorage;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using TodoApp.BlazorServer.Configuration;
using TodoApp.BlazorServer.DTO;
using TodoApp.BlazorServer.Service.Contract;

namespace TodoApp.BlazorServer.Service
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationProvider _authenticationProvider;
        private readonly IConfiguration _configuration;
        public AuthService(AuthenticationProvider authenticationProvider, 
            IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService,
            IConfiguration configuration) {
            _httpClientFactory = httpClientFactory;
            _localStorageService = localStorageService;
            _authenticationProvider = authenticationProvider;
            _configuration = configuration;
        }

        public async Task<bool> Login(LoginDto dto)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _configuration["ApiEndPoint"] + ApiEndpoint.AutLoginApiEndpoint)
            {
                Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json")
            };
            var client = _httpClientFactory.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseDto>(content);
            await _localStorageService.SetItemAsync("AuthJwtToken", result.TokenString);
            await _localStorageService.SetItemAsync("UserName", result.UserName);
            await ((AuthenticationProvider)_authenticationProvider).LoggedIn();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", result.TokenString);
            return true;
        }

        public async Task Logout()
        {
            await _localStorageService.RemoveItemAsync("AuthJwtToken");
            ((AuthenticationProvider)_authenticationProvider).LoggedOut();

        }

        public async Task<bool> Register(UserDto dto)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _configuration["ApiEndPoint"] + ApiEndpoint.AutRegisterApiEndpoint)
            {
                Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json")
            };
            var client = _httpClientFactory.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
