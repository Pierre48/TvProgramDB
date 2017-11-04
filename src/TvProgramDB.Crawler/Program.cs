using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using TvProgramDB.Core.Entities;
using TvProgramDB.Core.Interfaces;
using TvProgramDB.Core.Services;
using TvProgramDB.Infrastructure.Data;

namespace TvProgramDB.Crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = ConfigureService();

            Console.WriteLine("Hello World!");
            var service = serviceProvider.GetRequiredService<ICountryService>();
            service.Create("France");
        }

        private static ServiceProvider ConfigureService()
        {
            //setup our DI
            var services = new ServiceCollection()
                .AddLogging()
                .AddSingleton<DbContext, TvProgramContext>()
                .AddSingleton<IConfiguration>(GetConfiguration())
                .AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
                .AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>))
                .AddScoped<ICountryService, CountryService>()
                .AddDbContext<TvProgramContext>()
                .BuildServiceProvider();

            //configure console logging
            services
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);

            var logger = services.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            logger.LogDebug("Starting application");

            logger.LogDebug("All done!");

            return services;
        }

        static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return  builder.Build();
        }
    }
}
