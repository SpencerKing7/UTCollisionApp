using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.ML.OnnxRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UTCollisionApp.Models;

namespace UTCollisionApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            

            services.AddControllersWithViews();

            services.AddDbContext<CollisionDbContext>(options =>
            {
                options.UseMySql(Configuration["ConnectionStrings:UTCollisionsDbConnection"]);
                
            });

            services.AddDbContext<AppIdentityDBContext>(options =>
                options.UseMySql(Configuration["ConnectionStrings:IdentityConnection"]));

            

            services.AddIdentity<IdentityUser, IdentityRole> (options =>
            {
                
                //Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 12;
                options.Password.RequiredUniqueChars = 5;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDBContext>();

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });


            services.AddScoped<ICollisionRepository, EFCollisionRepository>();

            services.AddSingleton<InferenceSession>(
              new InferenceSession("severity_predictor.onnx")
            );

            services.AddSession();
            services.Configure<CookieAuthenticationOptions>(options =>
            {
                options.LoginPath = new PathString("/Home/Login");
                options.AccessDeniedPath = new PathString("/Home/AccessDenied");
                
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("Counties",
                    "{county}/Page{pageNum}/{severity?}",
                    new { Controller = "Admin", action = "CrashTable", pageNum = 1 });

                endpoints.MapControllerRoute("Paging",
                    "CrashesPage{pageNum}",
                    new { Controller = "Admin", action = "CrashTable", pageNum = 1 });

                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
