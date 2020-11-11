using System.ComponentModel.DataAnnotations;

namespace MongoWebApi.Models.Dto
{
    public class UserRegister
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}