using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

            

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDBContext>();

            services.AddScoped<ICollisionRepository, EFCollisionRepository>();

            services.AddSingleton<InferenceSession>(
              new InferenceSession("severity_predictor.onnx")
            );
            services.AddSession();
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
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("Counties",
                    "{county}/Page{pageNum}",
                    new { Controller = "Admin", action = "CrashTable", pageNum = 1 });

                endpoints.MapControllerRoute("Paging",
                    "CrashesPage{pageNum}",
                    new { Controller = "Admin", action = "CrashTable", pageNum = 1 });

                endpoints.MapDefaultControllerRoute();
            });

            IdentitySeedData.EnsurePopulated(app);
        }
    }
}
