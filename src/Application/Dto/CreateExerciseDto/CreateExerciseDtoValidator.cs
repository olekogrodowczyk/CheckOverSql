using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.CreateExerciseDto
{
    public class CreateExerciseDtoValidator : AbstractValidator<CreateExerciseDto>
    {
        public CreateExerciseDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Nie podano tytułu zadania")
                .MinimumLength(10).WithMessage("Minimalna długość tytułu to 10")
                .MaximumLength(200).WithMessage("Maksymalna długość tytułu to 200");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Nie podano opisu zadania")
                .MinimumLength(10).WithMessage("Minimalna długość opisu to 10")
                .MaximumLength(3000).WithMessage("Maksymalna długość opisu to 3000");

            RuleFor(x => x.MaxPoints)
                .NotEmpty().WithMessage("Nie podano maksymalnej ilości punktów zadania")
                .GreaterThanOrEqualTo(1).WithMessage("Minimalna liczba maksymalnej ilości punktów zadania to 1")
                .LessThanOrEqualTo(100).WithMessage("Maksymalna liczba maksymalnej ilości punktów zadania to 100");

            RuleFor(x => x.Database)
                .NotEmpty().WithMessage("Nie wybrano bazy danych do zadania")
                .MaximumLength(25).WithMessage("Maksymalna długość nazwy bazy danych to 25");

            RuleFor(x => x.Title)
                .MinimumLength(10).WithMessage("Minimalna długość odpowiedzi do zadania to 5")
                .MaximumLength(200).WithMessage("Maksymalna długość odpowiedzi do zadania to 500");


        }
    }
}
