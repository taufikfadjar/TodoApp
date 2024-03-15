using AutoMapper;
using TodoApp.Model.Entities;
using TodoApp.WebApi.DTO;

namespace TodoApp.WebApi.Configuration
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration() {

            CreateMap<UserDto, ApplicationUser>().ReverseMap();
            CreateMap<TodoDto, TodoActivity>().ReverseMap();

        }    
    }
}
