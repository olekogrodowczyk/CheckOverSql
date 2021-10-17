using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.CreateExerciseDto
{
    public class CreateExerciseDto : IMap
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int MaxPoints { get; set; }
        public string Database { get; set; }
        public string Answer { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateExerciseDto, Exercise>()
                .ForMember(x => x.Database, opt => opt.Ignore());
        }
    }
}
