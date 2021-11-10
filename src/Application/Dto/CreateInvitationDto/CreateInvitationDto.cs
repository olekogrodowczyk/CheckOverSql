using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.CreateInvitationDto
{
    public class CreateInvitationDto
    {
        public string ReceiverEmail { get; set; }
        public string RoleName { get; set; }     
    }
}
