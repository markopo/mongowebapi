using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoWebApi.Controllers;
using MongoWebApi.Models;
using MongoWebApi.Repositories;
using Moq;
using Xunit;

namespace MongoWebApi.Tests.Controllers
{
    public class TodoControllerTests
    {
        
        [Fact]
        public async Task TestGetSuccess()
        {
            var todo = new Todo
            {
                Id = 11,
                Content = "random string",
                Title = "random title"
            };
            
            var mockRepo = new Mock<ITodoRepository>();
            
            mockRepo
                .Setup(x => x.GetTodo(11))
                .ReturnsAsync(todo);
            
            var controller = new TodoController(mockRepo.Object);

            var result = await controller.Get(11);

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
                    new Todo
                    {
                        Id = 11,
                        Content = "random string",
                        Title = "random title"
                    },
                    new Todo
                    {
                        Id = 12,
                        Content = "random string 2",
                        Title = "random title 2"
                    },
                    new Todo
                    {
                        Id = 13,
                        Content = "random string 3",
                        Title = "random title 3"
                    }
                });
            
            var controller = new TodoController(mockRepo.Object);

            var result = await controller.Get();

            Assert.NotNull(result);

            var objResult = result.Result as ObjectResult;
            var listResult = objResult.Value as List<Todo>;
            
            Assert.Equal(3, listResult.Count);
            
            
        }
    }
}