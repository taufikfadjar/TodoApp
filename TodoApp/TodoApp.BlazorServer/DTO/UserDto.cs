using System.ComponentModel.DataAnnotations;

namespace TodoApp.BlazorServer.DTO
{
    public class UserDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
