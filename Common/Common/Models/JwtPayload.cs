using Common.Consts;
using Common.Enums;

namespace Common.Models
{
    public class JwtPayload
    {
        public string UserId { get; set; }
        public RoleType Role { get; set; }
        public Guid Jti { get; set; }
        public TokenType TokenType { get; set; }
    
        public void MapInto(IDictionary<object, object> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            items[JwtPayloadKey.UserId] = UserId;
            items[JwtPayloadKey.Role] = Role;
            items[JwtPayloadKey.Jti] = Jti;
            items[JwtPayloadKey.TokenType] = TokenType;
        }
    }
}
