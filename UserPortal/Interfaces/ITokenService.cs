using UserPortal.Entities;

namespace UserPortal.Interfaces
{
    public interface ITokenService
    {
        List<Token> GetTokens();
    }
}
