using Microsoft.AspNetCore.Mvc;
using UserPortal.Entities;
using UserPortal.Interfaces;

namespace UserPortal.Controllers
{
    [ApiController]
    [Route("api/tokens")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult<List<Token>> Get()
        {
            var tokens = _tokenService.GetTokens();
            return Ok(tokens);
        }
    }
}
