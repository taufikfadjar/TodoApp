using TodoApp.BlazorServer.DTO;

namespace TodoApp.BlazorServer.Service.Contract
{
    public interface IAuthService
    {
        Task<bool> Register(UserDto dto);
        Task<bool> Login(LoginDto dto);
        public Task Logout();
    }
}
