using System.ComponentModel.DataAnnotations;

namespace TodoApp.BlazorServer.DTO
{
    public class LoginDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
