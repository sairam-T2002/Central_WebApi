using Central_Service.Interface;
using Central_Service.Model;
using Repository_DAL_.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Central_Service.JWT;

namespace Central_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Private
        private readonly IAuthService _service;
        private readonly IConfiguration _configuration;
        private readonly TokenHelper _token;

        #endregion

        #region Constructor
        public AuthController( IAuthService service, IConfiguration configuration, TokenHelper token)
        {
            _service = service;
            _configuration = configuration;
            _token = token;
        }
        #endregion

        #region Actions

        /// <summary>
        /// Authenticates a user and returns JWT tokens.
        /// </summary>
        /// <param name="cred">The login credentials</param>
        /// <returns>Access and refresh tokens if authentication is successful</returns>
        /// <response code="200">Returns the JWT tokens</response>
        /// <response code="401">If the credentials are invalid</response>
        [HttpPost("Login")]
        public async Task<IActionResult> Login( [FromBody] Login cred )
        {
            var usr = await _service.Login(cred);
            if (usr != null)
            {
                var accessToken = _token.GenerateAccessToken(cred.Username, _configuration);
                var refreshToken = _token.GenerateRefreshToken();
                await _service.SaveRefreshToken(cred.Username, refreshToken).ConfigureAwait(false); ;
                return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken, Username = usr.Usr_Nam });
            }
            return Unauthorized("No user was found");
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="user">The user registration details</param>
        /// <returns>A success message if registration is successful</returns>
        /// <response code="200">Returns a success message</response>
        /// <response code="400">If the registration details are invalid</response>
        [HttpPost("Signup")]
        public async Task<IActionResult> Signup( [FromBody] User user )
        {
            var usr = await _service.Signup(user).ConfigureAwait(false); ;
            if (usr == 1)
            {
                return Ok("User Registered successfully");
            } 
            else if (usr == -1)
            {
                return BadRequest("User already exists for the specified email");
            }
            else if (usr == -2)
            {
                return BadRequest("User already exists for the specified username");
            }
            else
            {
                return BadRequest("Invalid Parameters");
            }
        }

        /// <summary>
        /// Refreshes the access token using a refresh token.
        /// </summary>
        /// <param name="refreshRequest">The refresh token request</param>
        /// <returns>New access and refresh tokens</returns>
        /// <response code="200">Returns new JWT tokens</response>
        /// <response code="400">If the refresh token is invalid</response>
        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh( [FromBody] RefreshTokenRequest refreshRequest )
        {
            try
            {
                var principal = _token.GetPrincipalFromExpiredToken(refreshRequest.AccessToken,_configuration);
                if (principal == null)
                {
                    return BadRequest("Invalid access token");
                }

                var username = principal?.Identity?.Name ?? "";
                var savedRefreshToken = await _service.GetRefreshToken(username).ConfigureAwait(false);
                if (savedRefreshToken != refreshRequest.RefreshToken)
                {
                    return BadRequest("Invalid refresh token");
                }

                var newAccessToken = _token.GenerateAccessToken(username,_configuration);
                var newRefreshToken = _token.GenerateRefreshToken();
                await _service.SaveRefreshToken(username, newRefreshToken);

                return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}