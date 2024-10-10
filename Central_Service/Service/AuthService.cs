using Central_Service.Interface;
using Central_Service.DTO;
using Repository_DAL_;
using Repository_DAL_.Model;
using ExtensionMethods;
using Microsoft.Extensions.Logging;
using Central_Service.Core;

namespace Central_Service.Service;

public class AuthService : ServiceBase, IAuthService
{
    #region Private Declarations

    private readonly IRepository<User> _user;
    private readonly ILogger<AuthService> _logger;

    #endregion

    public AuthService( IRepository<User> user, ILogger<AuthService> logger, IServiceProvider serviceProvider ) : base(logger, serviceProvider)
    {
        _user = user;
        _logger = logger;
    }

    #region Services

    public async Task<User?> Login( Login cred )
    {
        var usrs = await _user.Find(x => x.usr_nam == cred.Username || x.e_mail == cred.Username);
        var usr = usrs.FirstOrDefault();

        if (usr != null && BCrypt.Net.BCrypt.Verify(cred.Password, usr.pwd))
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
            var usrs = await _user.Find(x => x.usr_nam == temp.usr_nam || x.e_mail == temp.usr_nam);
            var usr = usrs.FirstOrDefault();
            if (usr != null)
            {
                if (usr.e_mail == temp.e_mail)
                {
                    return -1;
                }
                return -2;
            }

            temp.pwd = BCrypt.Net.BCrypt.HashPassword(temp.pwd);
            temp.createdate = DateOnly.FromDateTime(DateTime.UtcNow);
            temp.modifieddate = DateOnly.FromDateTime(DateTime.UtcNow);

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
        var usrs = await _user.Find(x => x.usr_nam == username);
        var usr = usrs.FirstOrDefault();
        if (usr != null)
        {
            usr.refreshtoken = refreshToken;
            await _user.Update(usr);
        }
    }

    public async Task<string?> GetRefreshToken( string username )
    {
        var usrs = await _user.Find(x => x.usr_nam == username);
        var usr = usrs.FirstOrDefault();
        return usr?.refreshtoken;
    }

    #endregion
}