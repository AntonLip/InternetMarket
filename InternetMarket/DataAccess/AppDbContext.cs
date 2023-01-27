using InternetMarket.Models;
using InternetMarket.Models.DbModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.IO;

namespace InternetMarket.DataAccess
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Product> Products{ get; set; }
        public DbSet<ProductType> ProductTypes{ get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
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
            using (StreamReader r = new StreamReader("appsettings.json"))
            {
                string json = r.ReadToEnd();
                var dbSettings = JsonConvert.DeserializeObject<DbSettings>(json);
                return $"server={dbSettings.DbServer};user={dbSettings.DbUser};password={dbSettings.DbPassword};database={dbSettings.Database};";
             }
        }
    }
}