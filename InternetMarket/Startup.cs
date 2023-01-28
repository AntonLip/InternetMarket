using Contributors.DataAccess;
using Contributors.Interfaces.Repository;
using InternetMarket.DataAccess;
using InternetMarket.Interfaces.IService;
using InternetMarket.Interfaces.Repository;
using InternetMarket.Models;
using InternetMarket.Models.DbModels;
using InternetMarket.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InternetMarket
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.Configure<DbSettings>(Configuration.GetSection(nameof(DbSettings)));
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 10;
                options.Password.RequiredUniqueChars = 3;
            }).AddEntityFrameworkStores<AppDbContext>();
            services.AddDatabase(Configuration);
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IShoppingCartService, ShoppingCartService>();
            services.AddTransient<IProductTypeService, ProductTypeService>();
            services.AddTransient<IConnectionParamsService, ConnectionParamsService>();

            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddTransient<IProductTypeRepository, ProductTypeRepository>();
            services.AddTransient<IConnectionParamsRepository, ConnectionParamsRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseRouting();

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
