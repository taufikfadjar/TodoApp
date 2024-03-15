using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Model;
using TodoApp.Model.Configuration;
using TodoApp.Model.Entities;
using TodoApp.Service;
using TodoApp.Service.Contract;

namespace TodoApp.Test
{
    public class TodoTest
    {
        WebApplication app;
        private static Random random = new Random();

        [SetUp]
        public async Task Setup()
        {
           var builder = WebApplication.CreateBuilder();

            var tets = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<DataContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<ITodoService, TodoService>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

            app = builder.Build();

            var dbContext = app.Services.GetService<DataContext>();
            await dbContext.Database.EnsureCreatedAsync();

        }


        [TearDown]
        public async Task TearDownAsync()
        {
            await app.DisposeAsync();

        }

        [Test]
        public async Task CreateTodoTest()
        {
            var dbContext = app.Services.GetService<DataContext>();
            var toDoService = app.Services.GetService<ITodoService>();
            

            var user = dbContext.Users.Where(x=>x.UserName == "taufikfadjar").FirstOrDefault();
            var latestNumber = await toDoService.GetLatestActivitiesNo(user.Id);


            var createNewTodo = new TodoActivity();
            createNewTodo.Id = Guid.NewGuid();
            createNewTodo.Description = "Aku Test";
            createNewTodo.Subject = "Aku Test";
            createNewTodo.CreatedBy = user.Id;

            var result = await toDoService.CreateTodoAsync(createNewTodo);


            Assert.IsTrue(result.Result);
            Assert.AreEqual(result.Data.ActivitiesNo, latestNumber + 1);
        }

        [Test]
        public async Task GetTodosTest()
        {

            var dbContext = app.Services.GetService<DataContext>();
            var toDoService = app.Services.GetService<ITodoService>();


            var user = dbContext.Users.Where(x => x.UserName == "taufikfadjar").FirstOrDefault();

            var createNewTodo = new TodoActivity();
            createNewTodo.Id = Guid.NewGuid();
            createNewTodo.Description = "Aku Test";
            createNewTodo.Subject = "Aku Test";
            createNewTodo.CreatedBy = user.Id;

            await toDoService.CreateTodoAsync(createNewTodo);

            var getTodoList = await toDoService.GetTodosAsync(user.Id);

            Assert.IsTrue(getTodoList.Count > 0);
        }


        [Test]
        public async Task GetTodoTest()
        {

            var dbContext = app.Services.GetService<DataContext>();
            var toDoService = app.Services.GetService<ITodoService>();


            var user = dbContext.Users.Where(x => x.UserName == "taufikfadjar").FirstOrDefault();

            var createNewTodo = new TodoActivity();
            createNewTodo.Id = Guid.NewGuid();
            createNewTodo.Description = "Aku Test";
            createNewTodo.Subject = "Aku Test";
            createNewTodo.CreatedBy = user.Id;

            await toDoService.CreateTodoAsync(createNewTodo);

            var getTodo = await toDoService.GetTodoAsync(createNewTodo.Id, user.Id);

            Assert.IsNotNull(getTodo);
        }


        [Test]
        public async Task UpdateStatusTodoTest()
        {

            var dbContext = app.Services.GetService<DataContext>();
            var toDoService = app.Services.GetService<ITodoService>();


            var user = dbContext.Users.Where(x => x.UserName == "taufikfadjar").FirstOrDefault();

            var getTodoList = await toDoService.GetTodosAsync(user.Id);
            var getUnmarked = getTodoList.Where(x => x.Status == null || x.Status?.Length == 0).FirstOrDefault();

            getUnmarked.Status = Constants.Done;

            var updatedTodo = await toDoService.UpdateTodoAsync(getUnmarked);

            Assert.AreEqual(updatedTodo.Data.Status , Constants.Done);
        }


        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [Test]
        public async Task UpdateSubjectUnmarkedTodoTest()
        {

            var dbContext = app.Services.GetService<DataContext>();
            var toDoService = app.Services.GetService<ITodoService>();


            var user = dbContext.Users.Where(x => x.UserName == "taufikfadjar").FirstOrDefault();

            var getTodoList = await toDoService.GetTodosAsync(user.Id);
            var getUnmarked = getTodoList.Where(x => x.Status == null || x.Status?.Length == 0).FirstOrDefault();


            var setRandomSubject = RandomString(20);
            getUnmarked.Subject = setRandomSubject;

            var updatedTodo = await toDoService.UpdateTodoAsync(getUnmarked);

            Assert.AreEqual(updatedTodo.Data.Subject, setRandomSubject);
        }

        [Test]
        public async Task DeleteUnmarkedTodoTest()
        {

            var dbContext = app.Services.GetService<DataContext>();
            var toDoService = app.Services.GetService<ITodoService>();


            var user = dbContext.Users.Where(x => x.UserName == "taufikfadjar").FirstOrDefault();

            var getTodoList = await toDoService.GetTodosAsync(user.Id);
            var getUnmarked = getTodoList.Where(x => x.Status == null || x.Status?.Length == 0).FirstOrDefault();
            var getId = getUnmarked.Id;

            var updatedTodo = await toDoService.DeleteTodoAsync(getUnmarked.Id);
            var tryGetDeleted = await toDoService.GetTodoAsync(getUnmarked.Id, user.Id);

            Assert.IsNull(tryGetDeleted);
        }


        [Test]
        public async Task DeleteNonUnmarkedTodoTest()
        {

            var dbContext = app.Services.GetService<DataContext>();
            var toDoService = app.Services.GetService<ITodoService>();


            var user = dbContext.Users.Where(x => x.UserName == "taufikfadjar").FirstOrDefault();

            var getTodoList = await toDoService.GetTodosAsync(user.Id);
            var getNonUnmarked = getTodoList.Where(x => x.Status != null || x.Status?.Length > 0).FirstOrDefault();


            var updatedTodo = await toDoService.DeleteTodoAsync(getNonUnmarked.Id);

            Assert.IsFalse(updatedTodo.Result);
        }




    }
}
