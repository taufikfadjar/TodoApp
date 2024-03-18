using Blazored.LocalStorage;
using TodoApp.BlazorServer.DTO;
using TodoApp.BlazorServer.Service.Contract;

namespace TodoApp.BlazorServer.Service
{
    public class TodoService : BaseService<TodoDto>, ITodoService
    {
        public TodoService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService, 
            IConfiguration configuration) : base(httpClientFactory, localStorageService, configuration)
        {
        }
    }
}
