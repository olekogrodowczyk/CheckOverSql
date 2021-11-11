using Application.Dto.LoginUserVm;
using Application.Dto.RegisterUserVm;
using Application.Interfaces;
using Domain.Entities;
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
    public class AccountControllerTests : SharedUtilityClass, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public AccountControllerTests(CustomWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task RegisterUser_ForValidModel_ReturnsOk()
        {
            // Arrange
            var httpContent = new RegisterUserDto()
            {
                Email = "test@test.com",
                Password = "password123",
                ConfirmPassword = "password123",
                FirstName = "John",
                LastName = "Smith",
                DateOfBirth = DateTime.UtcNow.AddYears(-20)
            }.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync(ApiRoutes.Account.Register, httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);            
        }

        [Fact]
        public async Task RegisterUser_ForInvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var httpContent = new RegisterUserDto()
            {
                Email = "test@test.com",
                Password = "password123",
                ConfirmPassword = "password1234",
                FirstName = "John",
                LastName = "",
                DateOfBirth = DateTime.UtcNow.AddYears(-20)
            }.ToJsonHttpContent();

            //Act
            var response = await _client.PostAsync(ApiRoutes.Account.Register, httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RegisterUser_ForTwoSameEmails_ReturnsBadRequest()
        {
            // Arrange
            var httpContent = new RegisterUserDto()
            {
                Email = "testsameemail@test.com",
                Password = "password123",
                ConfirmPassword = "password123",
                FirstName = "John",
                LastName = "Smith",
                DateOfBirth = DateTime.UtcNow.AddYears(-20)
            }.ToJsonHttpContent();

            await ClearTableInContext<User>();

            //Act
            var response = await _client.PostAsync(ApiRoutes.Account.Register, httpContent);
            var response2 = await _client.PostAsync(ApiRoutes.Account.Register, httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response2.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        
    }
}
