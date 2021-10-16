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
                .NotEmpty().WithMessage("Nie podano imienia");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Nie podano nazwiska");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Nie podano adresu e-mail");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Nie podano hasła");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Nie podano daty urodzenia");
        }
    }
}
