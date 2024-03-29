﻿using Blazored.LocalStorage;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TodoApp.BlazorServer.Configuration;
using TodoApp.BlazorServer.Service.Contract;

namespace TodoApp.BlazorServer.Service
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocalStorageService localStorageService;
        private readonly IConfiguration _configuration;

        public BaseService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            this.localStorageService = localStorageService;
            _configuration = configuration;
        }

        public async Task<bool> Create(string url, T entity)
        {
            url = _configuration["ApiEndPoint"] + url;
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (entity == null)
            {
                return false;
            }
            request.Content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetBearerToken());
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(string url, string id)
        {
            url = _configuration["ApiEndPoint"] + url;
            var request = new HttpRequestMessage(HttpMethod.Delete, url + id);
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetBearerToken());
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        public async Task<IList<T>> GetAll(string url)
        {
            url = _configuration["ApiEndPoint"] + url;
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetBearerToken());
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IList<T>>(content);

            }
            return null;
        }

        public Task<bool> Save()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(string url, string id, T entity)
        {
            url = _configuration["ApiEndPoint"] + url;
            var request = new HttpRequestMessage(HttpMethod.Put, url + id);
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }
            request.Content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetBearerToken());
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
        public async Task<string> GetBearerToken()
        {
            return await localStorageService.GetItemAsync<string>("AuthJwtToken");
        }

        public async Task<T> GetById(string url, string id)
        {
            url = _configuration["ApiEndPoint"] + url;
            var request = new HttpRequestMessage(HttpMethod.Get, url + id);

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetBearerToken());
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);

            }
            return null;
        }
    }
}
