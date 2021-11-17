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
    public class GetExerciseVm : IMap
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int MaxPoints { get; set; }
        public string Creator { get; set; }
        public int DatabaseId { get; set; }
        public string ValidAnswer { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Exercise, GetExerciseVm>()
                .ForMember(x => x.Creator, y => y.MapFrom(z => z.Creator.FirstName + " " + z.Creator.LastName));
        }
    }
}
