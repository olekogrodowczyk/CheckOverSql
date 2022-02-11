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
        public string Outcome { get; set; }
        public int ExerciseId { get; set; }
        public DateTime CreationTime { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Solution, GetSolutionDto>()
                .ForMember(x => x.Creator, opt => opt.MapFrom(y => y.Creator.FirstName + " " + y.Creator.LastName))
                .ForMember(x => x.CreationTime, opt => opt.MapFrom(y => y.Created));
        }
    }
}
