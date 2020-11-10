using MongoWebApi.Models;
using MongoWebApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MongoWebApi.Controllers
{
  
    
    [Produces("application/json")]
    [Route("api/[Controller]")]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _repository;
        
        public TodoController(ITodoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> Get()
        {
            var todos = await _repository.GetAllTodos();
            return new ObjectResult(todos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> Get(long id)
        {
            var todo = await _repository.GetTodo(id);

            if (todo == null)
            {
                return new NotFoundResult();
            }
            
            return new ObjectResult(todo);
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> Post([FromBody] Todo todo)
        {
            todo.Id = await _repository.GetNextId();
            await _repository.Create(todo);
            return new OkObjectResult(todo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Todo>> Put(long id, [FromBody] Todo todo)
        {
            var todoFromDb = await _repository.GetTodo(id);

            if (todoFromDb == null)
            {
                return new NotFoundResult();
            }

            todo.Id = todoFromDb.Id;
            todo.InternalId = todoFromDb.InternalId;

            await _repository.Update(todo);
            
            return new OkObjectResult(todo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var post = await _repository.GetTodo(id);

            if (post == null)
            {
                return new NotFoundResult();
            }

            await _repository.Delete(id);
            
            return new OkResult();
        }
        
    }
}