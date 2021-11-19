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
    public class GetSolvingVm : IMap
    {
        public string AssignedBy { get; set; }
        public string Solver { get; set; }
        public DateTime AssignedAt { get; set; }
        public DateTime? SentAt { get; set; }
        public DateTime? DeadLine { get; set; }
        public string Status { get; set; }
        public GetExerciseVm Exercise { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Solving, GetSolvingVm>()
                .ForMember(x => x.Solver, y => y.MapFrom(z => z.Assignment.User.FirstName + " " + z.Assignment.User.LastName))
                .ForMember(x => x.AssignedBy, y => y.MapFrom(z => z.Creator.FirstName + z.Creator.LastName))
                .ForMember(x => x.AssignedAt, y => y.MapFrom(z => z.Created))
                .ForMember(x => x.Exercise, y => y.MapFrom(z => z.Exercise));
        }
    }
}
