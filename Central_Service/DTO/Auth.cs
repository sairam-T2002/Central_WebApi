namespace Central_Service.DTO
{
    public struct Login
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class RefreshTokenRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
