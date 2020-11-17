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
                .Returns(Task.FromResult<Todo>(null));
            
            var controller = new TodoController(mockRepo.Object);

            var result = await controller.Get(11000);

            Assert.IsType<NotFoundResult>(result.Result);

        }
        
    }
}