using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.CreateGroupVm
{
    public class CreateGroupDtoValidator : AbstractValidator<CreateGroupDto>
    {
        public CreateGroupDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Wymagane jest podanie nazwy grupy")
                .MinimumLength(5).WithMessage("Nazwa grupa powinna mieć minimum 5 znaków");               
        }
    }
}
