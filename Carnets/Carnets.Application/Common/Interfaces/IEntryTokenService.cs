using Carnets.Application.Models;

namespace Carnets.Application.Interfaces
{
    public interface IEntryTokenService
    {
        string GenerateToken(string gympassId);

        EntryTokenPayload DecodeToken(string entryToken);
    }
}
