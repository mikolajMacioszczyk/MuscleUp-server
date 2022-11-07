namespace Auth.Application.Common.Interfaces
{
    public interface IFacebookLoginService
    {
        Task<bool> ValidateToken(string accessToken, string userId, string email);
    }
}
