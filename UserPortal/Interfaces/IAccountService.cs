using UserPortal.Models.Dtos;

namespace UserPortal.Interfaces
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        public string GenerateJwt(LoginDto dto);
    }
}
