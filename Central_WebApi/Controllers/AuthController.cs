using Central_Service.Interface;
using Central_Service.Model;
using Central_WebApi.Core;
using Repository_DAL_.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace Central_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly IConfiguration _configuration;

        public AuthController( IAuthService service, IConfiguration configuration )
        {
            _service = service;
            _configuration = configuration;
        }

        /// <summary>
        /// Authenticates a user and returns JWT tokens.
        /// </summary>
        /// <param name="cred">The login credentials</param>
        /// <returns>Access and refresh tokens if authentication is successful</returns>
        /// <response code="200">Returns the JWT tokens</response>
        /// <response code="401">If the credentials are invalid</response>
        [HttpPost("Login")]
        public async Task<ActionResult> Login( [FromBody] Login cred )
        {
            var usr = await _service.Login(cred);
            if (usr != null)
            {
                var accessToken = GenerateAccessToken(cred.Username);
                var refreshToken = GenerateRefreshToken();
                await _service.SaveRefreshToken(cred.Username, refreshToken);
                return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
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
        public async Task<ActionResult> Signup( [FromBody] User user )
        {
            var usr = await _service.Signup(user);
            if (usr)
            {
                return Ok(user);
            }
            return BadRequest("User already exists");
        }

        /// <summary>
        /// Refreshes the access token using a refresh token.
        /// </summary>
        /// <param name="refreshRequest">The refresh token request</param>
        /// <returns>New access and refresh tokens</returns>
        /// <response code="200">Returns new JWT tokens</response>
        /// <response code="400">If the refresh token is invalid</response>
        [HttpPost("Refresh")]
        public async Task<ActionResult> Refresh( [FromBody] RefreshTokenRequest refreshRequest )
        {
            var principal = GetPrincipalFromExpiredToken(refreshRequest.AccessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token");
            }

            var username = principal.Identity.Name;
            var savedRefreshToken = await _service.GetRefreshToken(username);
            if (savedRefreshToken != refreshRequest.RefreshToken)
            {
                return BadRequest("Invalid refresh token");
            }

            var newAccessToken = GenerateAccessToken(username);
            var newRefreshToken = GenerateRefreshToken();
            await _service.SaveRefreshToken(username, newRefreshToken);

            return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
        }

        private string GenerateAccessToken( string username )
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
            };
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15), // Short-lived access token
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken( string token )
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}