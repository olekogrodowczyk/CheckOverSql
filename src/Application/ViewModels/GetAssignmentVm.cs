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
    public class GetAssignmentVm : IMap
    {
        public GetUserVm User { get; set; }
        public string Role { get; set; }
        public DateTime? Joined { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Assignment, GetAssignmentVm>()
                .ForMember(x => x.Role, y => y.MapFrom(z => z.GroupRole.Name))
                .ForMember(x => x.Joined, y => y.MapFrom(z => z.Created));
        }
    }
}
