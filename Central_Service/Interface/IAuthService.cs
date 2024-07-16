using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository_DAL_.Model;
using Central_Service.Model;

namespace Central_Service.Interface
{
    public interface IAuthService
    {
        Task<User> Login( Login cred);
        Task<bool> Signup( User user);
        Task SaveRefreshToken( string username, string refreshToken );
        Task<string> GetRefreshToken( string username );
    }
}
