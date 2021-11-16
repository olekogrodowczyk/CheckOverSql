using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class GetComparisonVm : IMap
    {
        public int SolutionId { get; set; }
        public string SolutionSolver { get; set; }
        public int ExerciseId { get; set; }
        public string ExerciseTitle { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Result { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Comparison, GetComparisonVm>()
                .ForMember(x => x.SolutionSolver, y => y.MapFrom
                   (z => z.Solution.Creator.FirstName + " " + z.Solution.Creator.LastName))
                .ForMember(x => x.ExerciseTitle, y => y.MapFrom(z => z.Exercise.Title));
        }
    }
}
