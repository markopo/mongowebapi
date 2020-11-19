using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoWebApi.Controllers;
using MongoWebApi.Models;
using MongoWebApi.Repositories;
using Moq;
using Xunit;
using Bogus;
using MongoWebApi.Tests.TestData;

namespace MongoWebApi.Tests.Controllers
{
    public class TodoControllerTests
    {
        
        [Fact]
        public async Task TestGetSuccess()
        {
            var todo = BogusTodo.Create();
            
            var mockRepo = new Mock<ITodoRepository>();
            
            mockRepo
                .Setup(x => x.GetTodo(todo.Id))
                .ReturnsAsync(todo);
            
            var controller = new TodoController(mockRepo.Object);

            var result = await controller.Get(todo.Id);

            Assert.NotNull(result);

            var objResult = result.Result as ObjectResult;
            
            Assert.Equal(todo, objResult.Value);
    
        }
        
        [Fact]
        public async Task TestGetFailed()
        {
            var mockRepo = new Mock<ITodoRepository>();
            
            mockRepo
                .Setup(x => x.GetTodo(11000))
                .Returns( Task.FromResult<Todo>(null));
            
            var controller = new TodoController(mockRepo.Object);

            var result = await controller.Get(11000);

            Assert.IsType<NotFoundResult>(result.Result);
        }


        [Fact]
        public async Task TestGetListSuccess()
        {
            var mockRepo = new Mock<ITodoRepository>();
            
            mockRepo
                .Setup(x => x.GetAllTodos())
                .ReturnsAsync(new List<Todo>
                {
                    BogusTodo.Create(),
                    BogusTodo.Create(),
                    BogusTodo.Create()
                });
            
            var controller = new TodoController(mockRepo.Object);

            var result = await controller.Get();

            Assert.NotNull(result);

            var objResult = result.Result as ObjectResult;
            var listResult = objResult.Value as List<Todo>;
            
            Assert.Equal(3, listResult.Count);
            
            
        }

        [Fact]
        public async Task TestPostSuccess()
        {
            var todo = BogusTodo.Create();
            
            var mockRepo = new Mock<ITodoRepository>();
            
            mockRepo.Setup(x => x.Create(todo))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var controller = new TodoController(mockRepo.Object);

            var result = await controller.Post(todo);
            
            Assert.NotNull(result);
            Assert.Equal(typeof(OkObjectResult), result.Result.GetType());
        }
    }
}