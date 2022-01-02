using Application.Identities.Commands.RegisterUser;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Identities.Commands.LoginUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("Login has not been defined");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email has not been defined");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Password confirmation has not been defined");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password cannot be empty")
                .Equal(x => x.ConfirmPassword).WithMessage("Password cannot be different");
          
        }
    }
}
