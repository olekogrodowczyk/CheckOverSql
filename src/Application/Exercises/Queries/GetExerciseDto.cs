using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exercises.Queries
{
    public class GetExerciseDto : IMap
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public int DatabaseId { get; set; }
        public string ValidAnswer { get; set; }
        public string CreationTime { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Exercise, GetExerciseDto>()
                .ForMember(x => x.Creator, opt => opt.MapFrom(y => y.Creator.FirstName + " " + y.Creator.LastName))
                .ForMember(x => x.CreationTime, opt => opt.MapFrom(y => y.Created.ToString()));
        }
    }
}
