using System.Threading.Tasks;

namespace JwtAuthentication
{
    public interface IJwtTokenHandler
    {
        public Task<string> GetToken(string userId);
    }
}