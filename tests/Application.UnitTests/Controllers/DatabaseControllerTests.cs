using Application.Dto.SendQueryDto;
using Application.Interfaces;
using Application.Responses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Controllers;
using Xunit;

namespace Application.UnitTests.Controllers
{
    public class DatabaseControllerTests
    {
        private readonly DatabaseController _sut;
        private readonly Mock<IDatabaseService> _queryServiceMock = new Mock<IDatabaseService>();

        public DatabaseControllerTests()
        {
            _sut = new DatabaseController(_queryServiceMock.Object);
        }

       

        [Fact]
        public async Task SendQuery_ShouldReturnAffectedRows()
        {
            //Arrange
            var sendQueryDto = new SendQueryDto
            {
                Database = Guid.NewGuid().ToString(),
                Query = Guid.NewGuid().ToString()
            };
            Random rand=new Random();
            int expectedValue = rand.Next(1000);
            _queryServiceMock.Setup(x => x.SendQueryNoData(sendQueryDto.Query, sendQueryDto.Database, true))
                .ReturnsAsync(expectedValue);

            //Act
            var result = await _sut.SendQuery(sendQueryDto) as OkObjectResult;
            var item = result.Value as Result<int>;
            
            //Assert
            Assert.IsType<Result<int>>(item);
            Assert.Equal(expectedValue, item.Value);
            
        }

    }
}
