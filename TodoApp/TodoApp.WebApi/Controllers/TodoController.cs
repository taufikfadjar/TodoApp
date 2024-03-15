using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoApp.Model.Entities;
using TodoApp.Service.Contract;
using TodoApp.WebApi.DTO;

namespace TodoApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly ILogger<TodoController> _logger;
        private readonly IMapper _mapper;
        public TodoController(ITodoService todoService, ILogger<TodoController> logger, IMapper mapper)
        {
            _todoService = todoService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetTodos")]
        public async Task<ActionResult<List<TodoDto>>> Get()
        {
            List<TodoDto> todoList = new List<TodoDto>();
            try
            {
                var list = await _todoService.GetTodosAsync("f53f4178-8849-47f8-8b02-f871d36e8e05");

                if (list == null)
                {
                    return NotFound();
                }

                todoList = _mapper.Map<List<TodoDto>>(list);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error  GetTodos in {nameof(Get)}");
            }
            return Ok(todoList);
        }


        [HttpGet("{id}")]

        public async Task<ActionResult<TodoDto>> Get(string id)
        {
            TodoDto todo = new TodoDto();
            try
            {
                var getTodo = await _todoService.GetTodoAsync(new Guid(id), "f53f4178-8849-47f8-8b02-f871d36e8e05");
                if (getTodo == null)
                {
                    return NotFound();
                }
                todo = _mapper.Map<TodoDto>(getTodo);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Error GetTodo By Id in {nameof(Get)}-ID:{id}");
            }
            return Ok(todo);
        }


        [HttpPost]
        [Route("AddTodo")]
        [Authorize]
        public async Task<ActionResult<TodoDto>> Post(TodoDto todoDTO)
        {
            try
            {
                if (todoDTO == null)
                {
                    _logger.LogError($"Product Info is required:{nameof(Post)}");
                    return NotFound();
                }
                var todo = _mapper.Map<TodoActivity>(todoDTO);
                todo.Id = Guid.NewGuid();
                todo.CreatedBy = "f53f4178-8849-47f8-8b02-f871d36e8e05";

                var result = await _todoService.CreateTodoAsync(todo);

                if (result.Result)
                {
                    return CreatedAtAction(nameof(Post), new { id = todo.Id }, todo);
                }
                else
                {
                    ModelState.AddModelError(todo.Id.ToString(), result.Message);
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error save Todo in {nameof(Post)}");
                return StatusCode(500, ex.ToString());
            }
        }


        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Put(string id, TodoDto todoDTO)
        {
            try
            {
                var getExisiting = await _todoService.GetTodoAsync(new Guid(id), "f53f4178-8849-47f8-8b02-f871d36e8e05");
            if (getExisiting == null)
                {
                    return NotFound();
                }

                var todo = _mapper.Map(todoDTO, getExisiting);
                var result = await _todoService.UpdateTodoAsync(todo);

                if (result.Result)
                {
                    return Ok(todo);
                }
                else
                {
                    ModelState.AddModelError(todo.Id.ToString(), result.Message);
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error update Todo in {nameof(Put)}");
                return StatusCode(500, ex.ToString());
            }
        }


        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var getExisiting = await _todoService.GetTodoAsync(new Guid(id), "f53f4178-8849-47f8-8b02-f871d36e8e05");
                if (getExisiting == null)
                {
                    return NotFound();
                }


                var result = await _todoService.DeleteTodoAsync(new Guid(id));

                if (result.Result)
                {
                    return Ok();
                }
                else
                {
                    ModelState.AddModelError(id, result.Message);
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error delete Todo in {nameof(Delete)}");
                return StatusCode(500, ex.ToString());
            }
        }


    }
}
