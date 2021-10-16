//using Application.Dto.GetGroupDto;
//using Application.Interfaces;
//using Application.Services;
//using AutoMapper;
//using Domain.Interfaces;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace Application.UnitTests
//{
//    public class GroupServiceTests
//    {
//        private readonly GroupService _sut;
//        private readonly Mock<IMapper> _mapperMock;
//        private readonly Mock<IUserContextService> _userContextServiceMock;
//        private readonly Mock<IGroupRoleRepository> _groupRoleRepositoryMock;
//        private readonly Mock<IAssignmentRepository> _assignmentRepositoryMock;
//        private readonly Mock<IGroupRepository> _groupRepositoryMock;

//        public GroupServiceTests()
//        {
//            _sut = new GroupService(_mapperMock.Object, _userContextServiceMock.Object
//                ,_groupRoleRepositoryMock.Object, _assignmentRepositoryMock.Object, _groupRepositoryMock.Object);
//        }

//        [Fact]
//        public async Task GetAllGroups_ShouldReturnAllGroupsCreatedByUser()
//        {
            
//        }
//    }
//}
