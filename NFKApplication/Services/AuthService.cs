using NFKApplication.Database;

namespace NFKApplication.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        public AuthService(ILogger<AuthService> logger, IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public bool TryValidate(string username, string password, out string message)
        {
            return _authRepository.VerifyUser(username, password, out message);
        }
    }

    public interface IAuthService
    {
        bool TryValidate(string username, string password, out string message);
    }
}
