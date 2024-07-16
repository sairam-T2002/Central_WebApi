using Central_Service.Interface;
using Central_Service.Model;
using Repository_DAL_;
using Repository_DAL_.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_Service.Service
{
    public class AuthService : IAuthService
    {
        private IRepository<User> _user;

        public AuthService( IRepository<User> user )
        {
            _user = user;
        }

        public async Task<User> Login( Login cred )
        {
            var usrs = await _user.Find(x => x.Usr_Nam == cred.Username && x.Pwd == cred.Password);
            var usr = usrs.FirstOrDefault();
            return usr;
        }

        public async Task<bool> Signup( User user )
        {
            try
            {
                var usrs = await _user.Find(x => x.Usr_Nam == user.Usr_Nam || x.E_Mail == user.E_Mail);
                var usr = usrs.FirstOrDefault();
                if (usr != null)
                {
                    return false;
                }
                await _user.Add(user);
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