using Common.Enums;

namespace Common.Models
{
    public class HttpAuthContext
    {
        public virtual string UserId { get; set; }
        public virtual RoleType UserRole { get; set; }
        public virtual TokenType TokenType { get; set; }

        public static readonly HttpAuthContext Anonymous = new HttpAuthContext();

        public HttpAuthContext MapInto(HttpAuthContext newContext)
        {
            if (newContext == null) throw new ArgumentNullException(nameof(newContext));

            newContext.UserId = UserId;
            newContext.UserRole = UserRole;
            newContext.TokenType = TokenType;

            return newContext;
        }

        public HttpAuthContext MapFrom(JwtPayload payload)
        {
            if (payload == null) throw new ArgumentNullException(nameof(payload));

            UserId = payload.UserId;
            UserRole = payload.Role;
            TokenType = payload.TokenType;
            
            return this;
        }
    }
}
