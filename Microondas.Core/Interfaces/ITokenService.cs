using Microondas.Core.Models;

namespace Microondas.Core.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(AuthUser user);
    }
}