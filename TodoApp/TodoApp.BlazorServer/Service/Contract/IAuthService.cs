using TodoApp.BlazorServer.DTO;

namespace TodoApp.BlazorServer.Service.Contract
{
    public interface IAuthService
    {
        Task<bool> Register(UserDto dto);
    }
}
