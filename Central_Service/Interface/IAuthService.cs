using Repository_DAL_.Model;
using Central_Service.DTO;

namespace Central_Service.Interface;

public interface IAuthService
{
    Task<User> Login( Login cred );
    Task<int> Signup( User user );
    Task SaveRefreshToken( string username, string refreshToken );
    Task<string> GetRefreshToken( string username );
}