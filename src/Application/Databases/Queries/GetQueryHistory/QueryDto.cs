using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Databases.Queries.GetQueryHistory
{
    public class QueryDto : IMap
    {
        public string QueryValue { get; set; }
        public DateTime Created { get; set; }
        public int DatabaseId { get; set; }
        public string DatabaseName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Query, QueryDto>()
                .ForMember(x=>x.DatabaseName, opt=>opt.MapFrom(y=>y.Database.Name));
        }
    }
}
