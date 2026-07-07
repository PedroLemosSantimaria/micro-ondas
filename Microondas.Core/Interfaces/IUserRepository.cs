using Microondas.Core.Models;

namespace Microondas.Core.Interfaces
{
    public interface IUserRepository
    {
        AuthUser GetByUserName(string userName);
    }
}