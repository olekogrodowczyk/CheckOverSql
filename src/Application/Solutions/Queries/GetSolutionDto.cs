using Application.Exercises.Queries;
using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Solutions.Queries
{
    public class GetSolutionDto : IMap
    {
        public int Id { get; set; }
        public string Dialect { get; set; }
        public string Query { get; set; }
        public string Creator { get; set; }
        public GetExerciseDto Exercise { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Solution, GetSolutionDto>()
                .ForMember(x => x.Creator, y => y.MapFrom(z => z.Creator.FirstName + z.Creator.LastName));
        }
    }
}
