using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.CreateSolutionDto
{
    public class CreateSolutionDto : IMap
    {
        public string Dialect { get; set; }
        public string Query { get; set; }
        public int? SolvingId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateSolutionDto, Solution>().ReverseMap();
        }
    }
}
