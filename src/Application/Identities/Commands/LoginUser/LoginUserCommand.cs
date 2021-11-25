using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Identities.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<string>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IIdentityService _identityService;

        public LoginUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<string> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            string result = await _identityService.Login(command);
            return result;
        }
    }
}
