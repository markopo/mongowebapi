using System.Xml.Schema;
using Bogus;
using MongoWebApi.Models;

namespace MongoWebApi.Tests.TestData
{
    public static class BogusTodo
    {

        public static Todo Create()
        {
            var faker = new Faker();

            return new Todo
            {
                Id = faker.Random.Int(),
                Content = faker.Lorem.Text(),
                Title = faker.Lorem.Word()
            };
        }
        
    }
}