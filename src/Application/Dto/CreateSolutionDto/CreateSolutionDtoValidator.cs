using Domain.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.CreateSolutionDto
{
    public class CreateSolutionDtoValidator : AbstractValidator<CreateSolutionDto>
    {
        private readonly IDatabaseRepository _databaseRepository;   
        
        public CreateSolutionDtoValidator(IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
            
            RuleFor(x => x.Dialect)
                .NotEmpty().WithMessage("Wybrany dialekt nie może być pusty")
                .MaximumLength(25).WithMessage("Wybrany dialekt nie może mieć więcej niż 25 znaków");            

            RuleFor(x => x.Query)
                .NotEmpty().WithMessage("Nie przesłano rozwiązania do zadania")
                .MaximumLength(500).WithMessage("Maksymalna długość przesłanego zadania to 500");       
        }
    }
}
