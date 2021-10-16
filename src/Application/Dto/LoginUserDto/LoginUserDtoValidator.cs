using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.LoginUserVm
{
    public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Podany email jest pusty");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Podane hasło jest puste");              
        }
    }
}
