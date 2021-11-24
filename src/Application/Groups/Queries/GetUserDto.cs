using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Groups
{
    public class GetUserDto : IMap
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, GetUserDto>()
                .ForMember(x => x.Name, y => y.MapFrom(z => z.FirstName + " " + z.LastName));
        }
    }
}
