using Application.Interfaces;
using Application.Dto.LoginUserVm;
using Application.Dto.RegisterUserVm;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Responses;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public AccountController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto model)
        {
            var result = await _identityService.Register(model);
            return Ok(new Result<int>(result, "Pomyślnie zarejestrowano użytkownika"));

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto model)
        {
            var result = await _identityService.Login(model);
            return Ok(new Result<string>(result, "Pomyślnie zalogowano"));
        }
    }
}
