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
using System.Security.Cryptography;
using System.Threading.Tasks;
using UTCollisionApp.Models;
using UTCollisionApp.Security;

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

            // Database Connections
            string crash = Environment.GetEnvironmentVariable("RDS_CONNECTION_STRING_CRASH");
            string identity = Environment.GetEnvironmentVariable("RDS_CONNECTION_STRING_IDENTITY");

            services.AddDbContext<CollisionDbContext>(options =>
            {
                options.UseMySql(crash);
                
            });

            services.AddDbContext<AppIdentityDBContext>(options =>
                options.UseMySql(identity));

            // Identity
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

            services.AddSingleton<DataProtectionPurposeStrings>();

            services.AddDistributedMemoryCache();
            services.AddSession();

            services.ConfigureApplicationCookie(options =>
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

            app.Use(async (ctx, next) =>
            {
                ctx.Response.Headers.Add("Content-Security-Policy",
                                        "default-src 'self'");
                await next();
            });

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
