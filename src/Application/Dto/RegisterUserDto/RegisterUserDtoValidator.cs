using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.RegisterUserVm
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name has not been defined");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name has not been defined");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email has not been defined");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password cannot be empty")
                .Equal(x => x.ConfirmPassword).WithMessage("Password cannot be different");
          
        }
    }
}
