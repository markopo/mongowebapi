using System;
using MongoWebApi.Models;
using MongoWebApi.Repositories;
using System.Threading.Tasks;
using JwtAuthentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoWebApi.Models.Dto;

namespace MongoWebApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[Controller]")]
    public class LoginController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly IJwtTokenHandler _jwtTokenHandler;
        

        public LoginController(IUserRepository repository, IJwtTokenHandler jwtTokenHandler)
        {
            _repository = repository;
            _jwtTokenHandler = jwtTokenHandler;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserLoggedIn>> Login([FromBody] UserRegister userRegister)
        {
            try
            {
                var tupleUser = await _repository.Authenticate(userRegister.UserName, userRegister.Password);
                var userName = tupleUser.Item1;
                var isLoggedIn = tupleUser.Item2;
                
                if (!isLoggedIn) return new BadRequestObjectResult("Could not log in! Username or Password is incorrect!");

                var token = await _jwtTokenHandler.GetToken(userName);

                return new OkObjectResult(new UserLoggedIn
                {
                    UserName = userName,
                    Token = token
                });
            }
            catch (Exception exception)
            {
                // TODO: Logging!
                var message = exception.Message;
                
                return new BadRequestObjectResult("Could not log in!");
            }
        }

        [AllowAnonymous]
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