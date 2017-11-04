using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TvProgramDB.Core.Entities;

namespace TvProgramDB.Infrastructure.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TvProgramContext>
    {
        public TvProgramContext CreateDbContext(string[] args)
        {
            return new TvProgramContext(new DbContextOptions<TvProgramContext>(), GetConfiguration());
        }
        static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }

    public class TvProgramContext : DbContext
    {
        private IConfiguration _configuration;

        public TvProgramContext(DbContextOptions<TvProgramContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Country>()
                .HasKey(m => m.Id);
            builder.Entity<Country>()
                 .HasIndex(u => u.Name)
                 .IsUnique();
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var sqlConnectionString = _configuration.GetConnectionString("DataAccessPostgreSqlProvider");

            optionsBuilder.UseNpgsql(
                sqlConnectionString);

            base.OnConfiguring(optionsBuilder);
        }
    }
}
