using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
            builder.Entity<Country>(ConfigureCountry);
            builder.Entity<Chanel>(ConfigureChanel);
            builder.Entity<Program>(ConfigureProgram);
            builder.Entity<Source>(ConfigureSource);
            builder.Entity<ChanelName>(ConfigureChanelName);
            base.OnModelCreating(builder);
        }

        private void ConfigureSource(EntityTypeBuilder<Source> builder)
        {
            builder.ToTable("Source");
            builder.HasKey(m => m.Id);
            builder.Property(ci => ci.Id)
                .ForNpgsqlUseSequenceHiLo("Source_hilo")
                .IsRequired();
            builder.HasIndex(u => u.Name)
                 .IsUnique();
            builder.Property(ci => ci.Name)
                 .HasMaxLength(50);
            builder.Property(ci => ci.Code)
                 .HasMaxLength(10);
            builder.HasIndex(u => u.Code)
                 .IsUnique();
        }

        private void ConfigureProgram(EntityTypeBuilder<Program> builder)
        {
            builder.ToTable("Program");
            builder.HasKey(m => m.Id);
            builder.Property(ci => ci.Id)
                .ForNpgsqlUseSequenceHiLo("Program_hilo")
                .IsRequired();
            builder.Property(ci => ci.Name)
                 .HasMaxLength(100)
                 .IsRequired();
            builder.Property(ci => ci.Description)
                 .HasMaxLength(100)
                 .IsRequired();
            builder.Property(ci => ci.StartDate)
                 .IsRequired();
            builder.Property(ci => ci.EndDate)
                 .IsRequired();
        }

        private void ConfigureChanel(EntityTypeBuilder<Chanel> builder)
        {
            builder.ToTable("Chanel");
            builder.HasKey(m => m.Id);
            builder.Property(ci => ci.Id)
                .ForNpgsqlUseSequenceHiLo("Chanel_hilo")
                .IsRequired();

            builder.HasIndex(u => u.Name)
                 .IsUnique();
            builder.Property(ci => ci.Name)
                 .HasMaxLength(50);

            var navigation = builder.Metadata.FindNavigation(nameof(Chanel.ChanelNames));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }



        private void ConfigureChanelName(EntityTypeBuilder<ChanelName> builder)
        {
            builder.ToTable("ChanelName");
            builder.HasKey(m => m.Id);
            builder.Property(ci => ci.Id)
                .ForNpgsqlUseSequenceHiLo("ChanelName_hilo")
                .IsRequired();
            builder.Property(ci => ci.Id).ForNpgsqlUseSequenceHiLo();
            builder.HasIndex(u => u.Name)
                 .IsUnique();
            builder.Property(ci => ci.Name)
                 .HasMaxLength(50);
        }

        private void ConfigureCountry(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Country");
            builder.HasKey(m => m.Id);
            builder.Property(ci => ci.Id)
                .ForNpgsqlUseSequenceHiLo("Country_hilo")
                .IsRequired();
            builder.Property(ci => ci.Name)
                 .HasMaxLength(50);
            builder.HasIndex(u => u.Name)
                 .IsUnique();
            builder.Property(ci => ci.Code)
                 .HasMaxLength(2);
            builder.HasIndex(u => u.Code)
                 .IsUnique();

            var navigation = builder.Metadata.FindNavigation(nameof(Country.Chanels));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
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
