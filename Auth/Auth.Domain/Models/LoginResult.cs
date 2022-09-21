namespace Auth.Domain.Models
{
    public class LoginResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public bool IsInvalidPassword { get; set; }
        public bool UserNotFound { get; set; }

    }
}
