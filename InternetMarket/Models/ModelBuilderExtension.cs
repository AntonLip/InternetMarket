using InternetMarket.Models.DbModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace InternetMarket.Models
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ProductType>().HasData(
                  new ProductType
                  {
                      Id = Guid.NewGuid(),
                      Name = "Бытовая техника"
                  },
                  new ProductType
                  {
                      Id = Guid.NewGuid(),
                      Name = "Смартфоны"
                  },
                  new ProductType
                  {
                      Id = Guid.NewGuid(),
                      Name = "Сад и огрод"
                  }
                  );
            SeedUsers(modelBuilder);
            SeedRoles(modelBuilder);
            SeedUserRoles(modelBuilder);
        }

        private static void SeedUsers(ModelBuilder builder)
        {
            ApplicationUser user = new ApplicationUser()
            {
                Id = "b74ddd14-6340-4840-95c2-db12554843e5",
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                EmailConfirmed = true,
                ConcurrencyStamp = "avadvd",
                Email = "admin@gmail.com",
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                SecurityStamp = "avebgdfvs",
                PhoneNumber = "1234567890"
            };

            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
            var str = passwordHasher.HashPassword(user, "2732011Qw!");
            user.PasswordHash = str;
            builder.Entity<ApplicationUser>().HasData(user);
        }

        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = "fab4fac1-c546-41de-aebc-a14da6895711",
                    Name = "Admin",
                    ConcurrencyStamp = "1",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole()
                {
                    Id = "c7b013f0-5201-4317-abd8-c211f91b7330"
                    ,
                    Name = "User",
                    ConcurrencyStamp = "2",
                    NormalizedName = "Human Resource"
                });
        }

        private static void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() 
                { 
                    RoleId = "fab4fac1-c546-41de-aebc-a14da6895711", 
                    UserId = "b74ddd14-6340-4840-95c2-db12554843e5"
                });
        }
    }
}