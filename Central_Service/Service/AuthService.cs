using Central_Service.Interface;
using Central_Service.Model;
using Repository_DAL_;
using Repository_DAL_.Model;
using BCrypt.Net;
using ExtensionMethods;

namespace Central_Service.Service
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _user;

        public AuthService( IRepository<User> user )
        {
            _user = user;
        }

        public async Task<User> Login( Login cred )
        {
            // Find the user by username
            var usrs = await _user.Find(x => x.Usr_Nam == cred.Username);
            var usr = usrs.FirstOrDefault();

            // If user is found and password matches
            if (usr != null && BCrypt.Net.BCrypt.Verify(cred.Password, usr.Pwd))
            {
                return usr;
            }

            // If user is not found or password does not match
            return null;
        }

        public async Task<bool> Signup( User user )
        {
            try
            {
                User temp = user.DeepClone<User>();
                var usrs = await _user.Find(x => x.Usr_Nam == temp.Usr_Nam || x.E_Mail == temp.E_Mail);
                var usr = usrs.FirstOrDefault();
                if (usr != null)
                {
                    return false;
                }

                temp.Pwd = BCrypt.Net.BCrypt.HashPassword(temp.Pwd);

                await _user.Add(temp);
                return true;
            }
            catch (Exception ex)
            {
                return false;
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

        public async Task<string> GetRefreshToken( string username )
        {
            var usrs = await _user.Find(x => x.Usr_Nam == username);
            var usr = usrs.FirstOrDefault();
            return usr?.RefreshToken;
        }
    }
}