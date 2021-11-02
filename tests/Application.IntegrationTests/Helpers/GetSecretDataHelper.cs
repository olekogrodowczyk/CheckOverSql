using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace WebAPI.IntegrationTests.Helpers
{
    public class GetSecretDataHelper
    {
        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettingstests.json")
                .Build();
            return config;
        }

        public static string GetDatabaseReadOnlyConnectionString(string databaseName)
        {
            var config = InitConfiguration();
            return config.GetConnectionString(databaseName);
        }            
    }
}
