using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Model.Entities;
using TodoApp.Service.Model;

namespace TodoApp.Service.Contract
{
    public interface ITodoService
    {
         Task<List<TodoActivity>> GetTodosAsync(string userId);
         Task<TodoActivity> GetTodoAsync(Guid todoId, string userId);
         Task<ResponseResult<TodoActivity>> CreateTodoAsync(TodoActivity inputTodoActivity);
         Task<ResponseResult<TodoActivity>> UpdateTodoAsync(TodoActivity inputTodoActivity);
         Task<ResponseResult<TodoActivity>> DeleteTodoAsync(Guid todoActivityId);
         Task<long> GetLatestActivitiesNo(string userId);


    }
}
