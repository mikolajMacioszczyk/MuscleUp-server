using Carnets.Application.Models;

namespace Carnets.Application.Interfaces
{
    public interface IEntryTokenService
    {
        string GenerateToken(string gympassId);

        bool ValidateToken(string entryToken);

        EntryTokenPayload DecodeToken(string entryToken);
    }
}
