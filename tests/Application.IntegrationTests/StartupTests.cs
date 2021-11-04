using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Controllers;
using Xunit;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace WebAPI.IntegrationTests
{
    public class StartupTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly List<Type> _controllerTypes;

        public StartupTests(WebApplicationFactory<Startup> factory)
        {
            _controllerTypes = typeof(Startup)
                .Assembly
                .GetTypes()
                .Where(x => x.IsSubclassOf(typeof(ControllerBase)))
                .ToList();

            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    _controllerTypes.ForEach(x => services.AddScoped(x));
                });
            });
        }

        [Fact]
        public void ConfigureServices_ForControllers_RegistersAllDependencies()
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();

            var controller = scope.ServiceProvider.GetService<AccountController>();

            _controllerTypes.ForEach(x =>
            {
                var controller = scope.ServiceProvider.GetService(x);
                controller.Should().NotBeNull();
            });
        }
    }
}
