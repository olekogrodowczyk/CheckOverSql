using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.GetExerciseDto
{
    public class GetExerciseDto : IMap
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int MaxPoints { get; set; }
        public string Creator { get; set; }
        public string Database { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Exercise, GetExerciseDto>()
                .ForMember(x => x.Creator, y => y.MapFrom(z => z.Creator.FirstName + z.Creator.LastName));
        }
    }
}
