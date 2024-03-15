using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using TodoApp.BlazorServer.Configuration;
using TodoApp.BlazorServer.DTO;
using TodoApp.BlazorServer.Service.Contract;

namespace TodoApp.BlazorServer.Service
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AuthService(IHttpClientFactory httpClientFactory) {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<bool> Register(UserDto dto)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, ApiEndpoint.AutRegisterApiEndpoint)
            {
                Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json")
            };
            var client = _httpClientFactory.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
