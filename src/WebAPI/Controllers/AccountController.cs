using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Responses;
using Application.Identities.Commands.RegisterUser;
using Application.Identities.Commands.LoginUser;
using Microsoft.AspNetCore.Authorization;
using System;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ApiControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IUserContextService _userContextService;

        public AccountController(IIdentityService identityService, IUserContextService userContextService)
        {
            _identityService = identityService;
            _userContextService = userContextService;
        }

        [HttpPost("Register")]
        [ProducesResponseType(200, Type = typeof(Result<int>))]
        [ProducesResponseType(400, Type= typeof(ErrorResult))]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(new Result<int>(result, "User registered successfully"));

        }

        [HttpPost("Login")]
        [ProducesResponseType(200, Type = typeof(Result<string>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(new Result<string>(result, "User signed in successfully"));
        }

        [Authorize]
        [HttpGet("GetLoggedUserId")]
        [ProducesResponseType(200, Type = typeof(Result<int>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public IActionResult GetLoggedUserId()
        {
            int? loggedUserId = _userContextService.GetUserId;
            if(loggedUserId is null) { throw new UnauthorizedAccessException(); }
            return Ok(new Result<int>((int)loggedUserId, "Logged user's id returned successfully"));
        }
    }
}
