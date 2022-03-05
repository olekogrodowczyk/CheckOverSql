using Domain.ValueObjects;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace WebAPI.IntegrationTests.Helpers
{
    public class GetSecretDataHelper
    {
        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            return config;
        }

        public static ConnectionString GetConnectionString(string value)
        {
            ConnectionString connectionString = new ConnectionString();
            var config = InitConfiguration();
            config.GetSection(value).Bind(connectionString, x => x.BindNonPublicProperties = true);
            return connectionString;
        }
    }
}
