using System;
using MongoWebApi.Models;
using MongoWebApi.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoWebApi.Models.Dto;

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

        [Route("Register")]
        [HttpPost]
        public async Task<ActionResult<UserRegister>> Register([FromBody] UserRegister userRegister)
        {
            try
            {
                var user = new User
                {
                    UserName = userRegister.UserName,
                    Password = userRegister.Password,
                    Id = await _repository.GetNextId()
                };
                
                await _repository.CreateUser(user);
                
                return new OkObjectResult(new
                {
                    Message = "User Created",
                    user.UserName
                });
            }
            catch(Exception exception)
            {
                // TODO: Log this.
                var message = "Could not Register! ";

                // A write operation resulted in an error.
                // E11000 duplicate key error collection
                if (exception.Message.Contains("duplicate key error"))
                {
                    message += "User already exists!";
                }

                return new BadRequestObjectResult(message);
            }
        }
    }
}