using Application.Services;
using Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.Services
{
    public class DatabaseServiceTests
    {
        private readonly Random random = new Random();
        private readonly DatabaseService _sut;
        private readonly Mock<IDatabaseQuery> _databaseQuery = new Mock<IDatabaseQuery>();
        private readonly Mock<IDatabaseRepository> _databaseRepository = new Mock<IDatabaseRepository>();

        public DatabaseServiceTests()
        {
            _sut = new DatabaseService(_databaseQuery.Object, _databaseRepository.Object);
        }
   
    }
}
