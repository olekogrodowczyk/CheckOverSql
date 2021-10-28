using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Application.Services;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConfigureLogger();
            Log.Information("Application Started");
            CreateHostBuilder(args).Build().Run();
            Log.CloseAndFlush();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseSerilog();
                });

        public static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("C:\\logs\\log.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .WriteTo.File("C:\\logs\\logError.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error)
                .Enrich.WithEnvironmentName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithExceptionData()
                .CreateLogger();
        }
    }
}
