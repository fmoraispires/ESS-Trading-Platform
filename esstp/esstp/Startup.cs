
using System;
using AutoMapper;
using esstp.Controllers;
using esstp.Models;
using esstp.Models.Services;
using esstp.Models.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace esstp
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = AppConfig.Config;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddControllersAsServices();



            services.AddDbContext<DataContext>(options =>
                options.UseMySQL(AppConfig.Config["Data:ConnectionString"]));

            services.AddDbContext<DataContext>(ServiceLifetime.Scoped);

            // Start Registering and Initializing AutoMapper
            services.AddAutoMapper(typeof(Startup));

            // configure DI for application services
            services.AddSingleton<Queue>();
            services.AddSingleton<ConcreteSubject>();
            services.AddSingleton<ConcreteObserver>();
            //services.AddSingleton<Trigger>();
            services.AddScoped<IUserService, UserService>();
            //services.AddScoped<IPermissionService, PermissionService>();
            //services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IMarketService, MarketService>();
            services.AddScoped<IPortfolioService, PortfolioService>();
            services.AddScoped<ICfdService, CfdService>();
            services.AddScoped<IAssetService, AssetService>();
            services.AddScoped<IOperationService, OperationService>();
            services.AddScoped<IOperationTypeService, OperationTypeService>();
            services.AddHostedService<ProducerCatalogueService>();
            services.AddHostedService<ConsumerMarketService>();
            
            services.AddControllersWithViews();


        }




        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //default is home controller
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
