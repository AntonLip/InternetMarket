using InternetMarket.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InternetMarket
{
    public static class AddDatabaseExtention
    {
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(GetConnectionString(configuration), new MySqlServerVersion(new Version(8, 0, 11))
                ));
        }

        public static string GetConnectionString(IConfiguration configuration)
        {
            
            var dbServer = configuration["DbSettings:DbServer"];
            var dbPort = configuration["DbSettings:DbPort"];
            var dbUser = configuration["DbSettings:DbUser"];
            var dbPassword = configuration["DbSettings:DbPassword"];
            var database = configuration["DbSettings:Database"];
            return $"server={dbServer};user={dbUser};password={dbPassword};database={database};";
        }
    }
}
