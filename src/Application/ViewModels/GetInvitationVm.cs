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
    public class GetInvitationVm : IMap
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Status { get; set; }
        public string RoleName { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime AnsweredAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Invitation, GetInvitationVm>()
                .ForMember(x => x.GroupName, y => y.MapFrom(z => z.Group.Name))
                .ForMember(x => x.Sender, y => y.MapFrom(z => z.Sender.FirstName + " " + z.Sender.LastName))
                .ForMember(x => x.Receiver, y => y.MapFrom(z => z.Receiver.FirstName + " " + z.Receiver.LastName))
                .ForMember(x => x.RoleName, y => y.MapFrom(z => z.GroupRole.Name))
                .ForMember(x => x.Status, y => y.MapFrom(z => z.Status))
                .ForMember(x => x.SentAt, y => y.MapFrom(z => z.Created))
                .ForMember(x => x.AnsweredAt, y => y.MapFrom(z => z.AnsweredAt.ToString()));
        }
    }
}
