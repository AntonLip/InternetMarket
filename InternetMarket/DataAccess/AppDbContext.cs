using InternetMarket.Models;
using InternetMarket.Models.DbModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace InternetMarket.DataAccess
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        private static DbSettings _dbSettings;
        public AppDbContext(DbContextOptions<AppDbContext> options, IOptions<DbSettings> settings)
            : base(options)
        {
            _dbSettings = settings.Value;
            Database.EnsureCreated();
        }
        public DbSet<Product> Products{ get; set; }
        public DbSet<ProductType> ProductTypes{ get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<ConnectionParams> ConnectionParams { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                GetConnectionString(),
                new MySqlServerVersion(new Version(8, 0, 11))
            );
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           modelBuilder.Seed();
        }
        private static string GetConnectionString()
        {
            var conn  = $"server={_dbSettings.DbServer};user={_dbSettings.DbUser};password={_dbSettings.DbPassword};database={_dbSettings.Database};";
            return conn;
        }
    }
}