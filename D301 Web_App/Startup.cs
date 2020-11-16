using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using D301_WebApp.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace D301_WebApp
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
            // Add Database
            services
                .AddDbContext<ApplicationDbContext>(
                    options =>
                    {
                        options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
                        //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                    }
                );

            // Add Identity Framework
            services
                .AddDefaultIdentity<ApplicationUser>(
                    options =>
                    {
                        options.SignIn.RequireConfirmedAccount = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireDigit = false;
                        options.Password.RequiredLength = 5;
                    }
                )
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Add Razor Pages
            services
                .AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizePage("/Index");
                    options.Conventions.AuthorizePage("/Intake");
                    options.Conventions.AuthorizePage("/Trainer");
                    options.Conventions.AuthorizePage("/Dashboard");
                    options.Conventions.AuthorizePage("/Report");
                    options.Conventions.AuthorizePage("/Details");
                    options.Conventions.AuthorizePage("/Delete");
                    options.Conventions.AuthorizePage("/Calculators/CalculateMacroRatio");
                    options.Conventions.AuthorizePage("/ExportIntakes");
                });

            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddHttpClient();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Error");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(name: "default", pattern: "api/foods/{id}");
                endpoints.MapControllerRoute(name: "default", pattern: "api/intakes/{id}");
                endpoints.MapControllerRoute(name: "default", pattern: "api/dashboard/delete/{id}");
            });
        }
    }
}
