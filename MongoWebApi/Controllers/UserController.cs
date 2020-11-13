using System;
using MongoWebApi.Models;
using MongoWebApi.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoWebApi.Models.Dto;

namespace MongoWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[Controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get()
        {
            var activeUsers = await _repository.GetUsers(true);
            return new OkObjectResult(activeUsers.Select(u => new UserDto
            {
                UserName = u.UserName
            }));
        }
        
    }
}