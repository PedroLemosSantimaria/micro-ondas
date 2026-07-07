using Microondas.Core.Dtos.Requests;
using Microondas.Core.Dtos.Responses;

namespace Microondas.Core.Interfaces
{
    public interface IAuthService
    {
        LoginResponse Login(LoginRequest request);
    }
}