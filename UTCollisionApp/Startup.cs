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
using System.Net;
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

            services.AddDbContext<CollisionDbContext>(options =>
            {
                options.UseMySql(Environment.GetEnvironmentVariable("RDSConnectionStringCrash"));
            });

            services.AddDbContext<AppIdentityDBContext>(options =>
            {
                options.UseMySql(Environment.GetEnvironmentVariable("RDSConnectionStringIdentity"));
            });


            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
                
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = (int)HttpStatusCode.TemporaryRedirect;
                options.HttpsPort = 443;
            });
            // Identity

            services.AddIdentity<IdentityUser, IdentityRole> (options =>
            {

                //Password settings.
                options.SignIn.RequireConfirmedAccount = false;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 12;
                options.Password.RequiredUniqueChars = 5;
            })

                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDBContext>();

            
            services.AddControllersWithViews();

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });


            services.AddScoped<ICollisionRepository, EFCollisionRepository>();

            services.AddSingleton<InferenceSession>(
              new InferenceSession("wwwroot/severity_predictor.onnx")
            );

            services.AddSingleton<DataProtectionPurposeStrings>();

            services.AddDistributedMemoryCache();
            services.AddSession();

           
            

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection =
                        Configuration.GetSection("Authentication:Google");

                    options.ClientId = Environment.GetEnvironmentVariable("AuthClientID");
                    options.ClientSecret = Environment.GetEnvironmentVariable("AuthClientSecret");
                    
                });

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
                app.UseHsts();
            }
            
            
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.Use(async (ctx, next) =>
            //{
            //    ctx.Response.Headers.Add("Content-Security-Policy",
            //                            "default-src 'self'");
            //    await next();
            //});

            app.UseEndpoints(endpoints =>
            {
                // Admin County Filering
                endpoints.MapControllerRoute("Counties",
                    "Admin/CrashTable/{county}/PageNum{pageNum}/{level?}",
                    new { Controller = "Admin", action = "CrashTable", pageNum = 1 });
                
                // Normal User Filtering
                endpoints.MapControllerRoute("Counties",
                    "Home/AccidentTable/{counties}/PageNum{pageNum}/{severity?}",
                    new { Controller = "Home", action = "AccidentTable", pageNum = 1 });

                // Admin Pagination
                endpoints.MapControllerRoute("Paging",
                    "CrashesPage{pageNum}",
                    new { Controller = "Admin", action = "CrashTable", pageNum = 1 });

                // Normal User Pagination
                endpoints.MapControllerRoute("Paging",
                    "AccidentsPage{page}",
                    new { Controller = "Home", action = "AccidentTable", pageNum = 1 });

                endpoints.MapDefaultControllerRoute();
            });

        }
    }
}
