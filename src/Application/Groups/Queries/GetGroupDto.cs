using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Groups.Queries
{
    public class GetGroupDto : IMap
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Group, GetGroupDto>()
                .ForMember(x => x.ImagePath, opt => opt.MapFrom(y => $"images/groups/{y.ImageName}"));
        }
    }
}
