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
            
            var service = serviceProvider.GetRequiredService<CrawlerServiceBase>();
            service.Initialize();
            service.Start();
            Console.WriteLine("started");
            Console.ReadLine();
            service.Stop();
        }

        private static ServiceProvider ConfigureService()
        {
            //setup our DI
            var services = new ServiceCollection()
                .AddLogging(builder => builder.SetMinimumLevel(LogLevel.Trace))
                .AddSingleton<DbContext, TvProgramContext>()
                .AddSingleton(GetConfiguration())
                .AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
                .AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>))
                .AddScoped<CrawlerServiceBase, BeinCrawlerService>()
                .AddDbContext<TvProgramContext>()
                .BuildServiceProvider();

            //configure console logging
            var loggerFactory = services.GetService<ILoggerFactory>();
            loggerFactory.AddConsole(LogLevel.Trace);

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
