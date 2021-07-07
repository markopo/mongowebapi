using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoWebApi.Controllers;
using MongoWebApi.Models;
using MongoWebApi.Repositories;
using Moq;
using Xunit;
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

            mockRepo.Setup(x => x.GetNextId())
                .ReturnsAsync(todo.Id);
            
            mockRepo.Setup(x => x.Create(todo))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var controller = new TodoController(mockRepo.Object);

            var result = await controller.Post(todo);
            
            Assert.NotNull(result);
            Assert.Equal(typeof(OkObjectResult), result.Result.GetType());
            
            var res = result.Result as OkObjectResult;
            
            Assert.Equal(typeof(Todo), res.Value.GetType());

            var postTodo = res.Value as Todo;
            
            Assert.NotNull(postTodo);
            
            Assert.Equal(todo.Id, postTodo.Id);
        }

        [Fact]
        public async Task TestPutSuccess()
        {
            var todo = BogusTodo.Create();
            
            var mockRepo = new Mock<ITodoRepository>();

            mockRepo.Setup(x => x.GetTodo(todo.Id))
                .ReturnsAsync(todo);

            mockRepo.Setup(x => x.Update(todo))
                .ReturnsAsync(true);

            var controller = new TodoController(mockRepo.Object);

            var result = await controller.Put(todo.Id, todo);
            
            var res = result.Result as OkObjectResult;
            
            Assert.Equal(typeof(Todo), res.Value.GetType());

            var putTodo = res.Value as Todo;
            
            Assert.Equal(todo.Id, putTodo.Id);
        }
        
        [Fact]
        public async Task TestPutFailureOne()
        {
            var todo = BogusTodo.Create();
            
            var mockRepo = new Mock<ITodoRepository>();

            mockRepo.Setup(x => x.GetTodo(todo.Id))
                .Returns( Task.FromResult<Todo>(null));

            mockRepo.Setup(x => x.Update(todo))
                .ReturnsAsync(true);

            var controller = new TodoController(mockRepo.Object);

            var result = await controller.Put(todo.Id, todo);
            
            Assert.Equal(typeof(NotFoundResult), result.Result.GetType());
            
        }
        
        [Fact]
        public async Task TestPutFailureTwo()
        {
            var todo = BogusTodo.Create();
            
            var mockRepo = new Mock<ITodoRepository>();

            mockRepo.Setup(x => x.GetTodo(todo.Id))
                .ReturnsAsync(todo);

            mockRepo.Setup(x => x.Update(todo))
                .ReturnsAsync(false);

            var controller = new TodoController(mockRepo.Object);

            var result = await controller.Put(todo.Id, todo);
            
            Assert.Equal(typeof(NotFoundResult), result.Result.GetType());
            
        }

        [Fact]
        public async Task TestDeleteSuccess()
        {
            var todo = BogusTodo.Create();
            
            var mockRepo = new Mock<ITodoRepository>();

            mockRepo.Setup(x => x.GetTodo(todo.Id))
                .ReturnsAsync(todo);

            mockRepo.Setup(x => x.Delete(todo.Id))
                .ReturnsAsync(true);
            
            var controller = new TodoController(mockRepo.Object);

            var result = await controller.Delete(todo.Id);
            
            Assert.NotNull(result);
            Assert.Equal(typeof(OkResult), result.GetType());
        }
        
        [Fact]
        public async Task TestDeleteFailureOne()
        {
            var mockRepo = new Mock<ITodoRepository>();

            mockRepo.Setup(x => x.GetTodo(18888))
                .Returns(Task.FromResult<Todo>(null));

            mockRepo.Setup(x => x.Delete(18888))
                .ReturnsAsync(true);
            
            var controller = new TodoController(mockRepo.Object);

            var result = await controller.Delete(18888);
            
            Assert.NotNull(result);
            Assert.Equal(typeof(NotFoundResult), result.GetType());
        }
        
        [Fact]
        public async Task TestDeleteFailureTwo()
        {
            var todo = BogusTodo.Create();
            
            var mockRepo = new Mock<ITodoRepository>();

            mockRepo.Setup(x => x.GetTodo(18888))
                .ReturnsAsync(todo);

            mockRepo.Setup(x => x.Delete(18888))
                .ReturnsAsync(false);
            
            var controller = new TodoController(mockRepo.Object);

            var result = await controller.Delete(18888);
            
            Assert.NotNull(result);
            Assert.Equal(typeof(NotFoundResult), result.GetType());
        }
        
    }
}