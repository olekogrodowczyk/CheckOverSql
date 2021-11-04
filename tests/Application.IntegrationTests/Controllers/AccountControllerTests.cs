using Application.Dto.LoginUserVm;
using Application.Dto.RegisterUserVm;
using Application.Interfaces;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebAPI.IntegrationTests.Helpers;
using Xunit;

namespace WebAPI.IntegrationTests.Controllers
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Startup> _factory;
        private Mock<IIdentityService> _identityServiceMock = new Mock<IIdentityService>();

        public AccountControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                               .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                        services.Remove(dbContextOptions);
                        services.AddSingleton(_identityServiceMock.Object);
                        services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("InMemoryDatabase"));

                    });
                });
            _client = _factory.CreateClient();

            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            //Seed necessary data like roles and other databases
            SeedDataHelper.SeedDatabeses(context, "FootballLeague").Wait();
            SeedDataHelper.SeedRoles(context).Wait();
        }

        [Fact]
        public async Task RegisterUser_ForValidModel_ReturnsOk()
        {
            // Arrange
            var registerUser = new RegisterUserDto()
            {
                Email = "test@test.com",
                Password = "password123",
                ConfirmPassword = "password123",
                FirstName = "John",
                LastName = "Smith",
                DateOfBirth = DateTime.UtcNow.AddYears(-20)
            };

            var httpContent = registerUser.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync("/api/account/register", httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);            
        }

        [Fact]
        public async Task RegisterUser_ForInvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var registerUser = new RegisterUserDto()
            {
                Email = "test@test.com",
                Password = "password123",
                ConfirmPassword = "password1234",
                FirstName = "John",
                LastName = "",
                DateOfBirth = DateTime.UtcNow.AddYears(-20)
            };

            var httpContent = registerUser.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync("/api/account/register", httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RegisterUser_ForTwoSameEmails_ReturnsBadRequest()
        {
            // Arrange
            var registerUser1 = new RegisterUserDto()
            {
                Email = "test@test.com",
                Password = "password123",
                ConfirmPassword = "password123",
                FirstName = "John",
                LastName = "Smith",
                DateOfBirth = DateTime.UtcNow.AddYears(-20)
            };         

            var httpContent = registerUser1.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync("/api/account/register", httpContent);
            var response2 = await _client.PostAsync("/api/account/register", httpContent);

            //Assert
            response2.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_ForRegisteredUser_ReturnsOk()
        {
            //Arrange
            _identityServiceMock.Setup(x => x.Login(It.IsAny<LoginUserDto>()))
                .ReturnsAsync("jwt");

            var loginUserDto = new LoginUserDto()
            {
                Email = "test@test.com",
                Password = "password123"
            };

            var httpContent = loginUserDto.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync("api/account/login", httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
