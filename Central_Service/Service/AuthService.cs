using Central_Service.Interface;
using Central_Service.DTO;
using Repository_DAL_;
using Repository_DAL_.Model;
using ExtensionMethods;
using Microsoft.Extensions.Logging;
using Central_Service.Core;

namespace Central_Service.Service
{
    public class AuthService : ServiceBase,IAuthService
    {
        private readonly IRepository<User> _user;
        private readonly ILogger<AuthService> _logger;

        public AuthService( IRepository<User> user, ILogger<AuthService> logger, IServiceProvider serviceProvider ) : base(logger, serviceProvider)
        {
            _user = user;
            _logger = logger;
        }

        public async Task<User?> Login( Login cred )
        {
            var usrs = await _user.Find(x => x.Usr_Nam == cred.Username || x.E_Mail == cred.Username);
            var usr = usrs.FirstOrDefault();

            if (usr != null && BCrypt.Net.BCrypt.Verify(cred.Password, usr.Pwd))
            {
                return usr;
            }

            return default;
        }

        public async Task<int> Signup( User user )
        {
            try
            {
                User temp = user.DeepClone<User>();
                var usrs = await _user.Find(x => x.Usr_Nam == temp.Usr_Nam || x.E_Mail == temp.Usr_Nam);
                var usr = usrs.FirstOrDefault();
                if (usr != null)
                {
                    if(usr.E_Mail == temp.E_Mail)
                    {
                        return -1;
                    }
                    return -2;
                }

                temp.Pwd = BCrypt.Net.BCrypt.HashPassword(temp.Pwd);
                temp.CreateDate = DateOnly.FromDateTime(DateTime.UtcNow);
                temp.ModifiedDate = DateOnly.FromDateTime(DateTime.UtcNow);

                await _user.Add(temp);
                return 1;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return 0;
            }
        }

        public async Task SaveRefreshToken( string username, string refreshToken )
        {
            var usrs = await _user.Find(x => x.Usr_Nam == username);
            var usr = usrs.FirstOrDefault();
            if (usr != null)
            {
                usr.RefreshToken = refreshToken;
                await _user.Update(usr);
            }
        }

        public async Task<string?> GetRefreshToken( string username )
        {
            var usrs = await _user.Find(x => x.Usr_Nam == username);
            var usr = usrs.FirstOrDefault();
            return usr?.RefreshToken;
        }
    }
}