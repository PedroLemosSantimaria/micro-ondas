using Microondas.Core.Dtos.Requests;
using Microondas.Core.Dtos.Responses;
using Microondas.Core.Exceptions;
using Microondas.Core.Helpers;
using Microondas.Core.Interfaces;

namespace Microondas.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenService tokenService;

        public AuthService(IUserRepository userRepository, ITokenService tokenService)
        {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
        }

        public LoginResponse Login(LoginRequest request)
        {
            if (request == null)
                throw new BusinessException("Dados de login não informados.");

            if (string.IsNullOrWhiteSpace(request.UserName))
                throw new BusinessException("Informe o usuário.");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new BusinessException("Informe a senha.");

            var user = userRepository.GetByUserName(request.UserName);

            if (user == null)
                throw new BusinessException("Usuário ou senha inválidos.");

            var passwordHash = PasswordHasher.ToSha256(request.Password);

            if (user.PasswordHash != passwordHash)
                throw new BusinessException("Usuário ou senha inválidos.");

            var token = tokenService.GenerateToken(user);

            return new LoginResponse
            {
                Token = token,
                UserName = user.UserName,
                Message = "Login realizado com sucesso."
            };
        }
    }
}