using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoApp.Model;
using TodoApp.Model.Configuration;
using TodoApp.Model.Entities;
using TodoApp.Service.Contract;
using TodoApp.Service.Model;

namespace TodoApp.Service
{
    public class TodoService : ITodoService
    {

        private readonly ILogger<TodoService> _logger;
        private readonly DataContext _dataContext;

        public TodoService(
             ILogger<TodoService> logger,
             DataContext dataContext
             ) {
            _logger = logger;
            _dataContext = dataContext;
        
        }

        public async Task<List<TodoActivity>> GetTodosAsync(string userId)
        {
            var getResults = await _dataContext.TodoActivities.Where(x => x.CreatedBy == userId).ToListAsync();
            return getResults;
        }

        public async Task<TodoActivity> GetTodoAsync(Guid todoId , string userId)
        {
            var getResult = await _dataContext.TodoActivities.Where(x => x.CreatedBy == userId && x.Id == todoId).FirstOrDefaultAsync();
            return getResult;
        }


        public async Task<long> GetLatestActivitiesNo(string userId)
        {
            var getMaxNumber =  await _dataContext.TodoActivities.Where(x => x.CreatedBy == userId).OrderByDescending(x => x.ActivitiesNo)
                .FirstOrDefaultAsync();

            if(getMaxNumber != null)
            {
                return getMaxNumber.ActivitiesNo;
            }  

            return 0;
        }


        public async Task<ResponseResult<TodoActivity>> CreateTodoAsync(TodoActivity inputTodoActivity)
        {
            using var transaction = await _dataContext.Database.BeginTransactionAsync();
            try
            {
                var getMaxNumber = await GetLatestActivitiesNo(inputTodoActivity.CreatedBy??"");

                inputTodoActivity.ActivitiesNo = getMaxNumber + 1;
                inputTodoActivity.CreatedDate = DateTime.Now;

                await _dataContext.TodoActivities.AddAsync(inputTodoActivity);

                await _dataContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ResponseResult<TodoActivity>(inputTodoActivity, true);

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new ResponseResult<TodoActivity>(inputTodoActivity, false, ex.Message);

            }
        }

        public async Task<ResponseResult<TodoActivity>> UpdateTodoAsync(TodoActivity inputTodoActivity)
        {
            var getExisting = await _dataContext.TodoActivities.Where(x => x.Id == inputTodoActivity.Id).FirstAsync();

            if (!string.IsNullOrEmpty(inputTodoActivity.Status) && (getExisting.Subject != inputTodoActivity.Subject ||
            getExisting.Description != inputTodoActivity.Description))
            {
                return new ResponseResult<TodoActivity>(inputTodoActivity, false, "You can't change subject or description if with status done or canceled");
            }

            using var transaction = await _dataContext.Database.BeginTransactionAsync();
            try
            {

                getExisting.Subject = inputTodoActivity.Subject;
                getExisting.Description = inputTodoActivity.Description;
                getExisting.Status = inputTodoActivity.Status;
                getExisting.UpdatedDate = DateTime.Now;

                await _dataContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ResponseResult<TodoActivity>(inputTodoActivity, true);

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResponseResult<TodoActivity>(inputTodoActivity, false, ex.Message);

            }
        }

        public async Task<ResponseResult<TodoActivity>> DeleteTodoAsync(Guid todoActivityId)
        {
            var getExisting = await _dataContext.TodoActivities.Where(x => x.Id == todoActivityId).FirstAsync();

            if (!string.IsNullOrEmpty(getExisting.Status)){
                return new ResponseResult<TodoActivity>(getExisting, false, "You can't delete todo with status done or deleted");
            }

            using var transaction = await _dataContext.Database.BeginTransactionAsync();
            try
            {
                await _dataContext.TodoActivities.Where(x => x.Id == todoActivityId).ExecuteDeleteAsync();

                await _dataContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ResponseResult<TodoActivity>(null, true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResponseResult<TodoActivity>(null, false, ex.Message);

            }


        }


    }
}
