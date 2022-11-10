namespace Auth.Domain.Models
{
    public class AuthToken
    {
        public string Id { get; set; }
        public Guid AccessTokenId { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
        public Guid RefreshTokenId { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
