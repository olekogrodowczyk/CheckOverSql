using AutoMapper;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.GroupRoles.Queries.GetAllGroupRoles
{
    public class GetAllGroupRolesQuery : IRequest<IEnumerable<GroupRoleDto>>
    {
    }

    public class GetAllGroupRolesQueryHandler : IRequestHandler<GetAllGroupRolesQuery, IEnumerable<GroupRoleDto>>
    {
        private readonly IGroupRoleRepository _groupRoleRepository;
        private readonly IMapper _mapper;

        public GetAllGroupRolesQueryHandler(IGroupRoleRepository groupRoleRepository, IMapper mapper)
        {
            _groupRoleRepository = groupRoleRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GroupRoleDto>> Handle(GetAllGroupRolesQuery request, CancellationToken cancellationToken)
        {
            var groupRoles = await _groupRoleRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GroupRoleDto>>(groupRoles);
        }
    }
}
