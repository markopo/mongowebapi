using System;
using MongoWebApi.Models;
using MongoWebApi.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MongoWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[Controller]")]
    public class LoginController : Controller
    {
        private readonly IUserRepository _repository;

        public LoginController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<ActionResult<User>> Register([FromBody] User user)
        {
            try
            {
                user.Id = await _repository.GetNextId();
                await _repository.CreateUser(user);
                return new OkObjectResult(user);
            }
            catch(Exception exception)
            {
                // TODO: Log this.
                var message = exception.Message;
                return new BadRequestObjectResult("Could not Register!");
            }
        }
    }
}