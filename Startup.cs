using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StudentManagement.Models;
using StudentManagement.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement
{
    public class Startup
    {
        private IConfiguration _config;
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();           
            services.AddMvc(config => {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddScoped<IStudentRepository, SQLStudentRepository>();
            services.AddScoped<ITeacherRepository, SQLTeacherRepository>();

            services.AddHttpContextAccessor();

            services.AddDbContextPool<AppDbContext>(options =>
                    options.UseSqlServer(_config.GetConnectionString("StudentDBConnection")));

            services.AddDbContextPool<AppDbContext>(options =>
                    options.UseSqlServer(_config.GetConnectionString("TeacherDBConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;       //changing defaults
                options.Password.RequiredUniqueChars = 3;
                options.SignIn.RequireConfirmedEmail = true;

           }).AddEntityFrameworkStores<AppDbContext>()
             .AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy",
                    policy => policy.RequireClaim("Delete Role")
                    );

                options.AddPolicy("EditRolePolicy",
                    policy => policy.RequireClaim("Edit Role", "true"));
                
                options.AddPolicy("AdminRolePolicy",
                    policy => policy.RequireRole("Admin"));

                options.AddPolicy("AdminEditRolePolicy",
                    policy => policy.RequireRole("Admin").RequireClaim("Edit Role", "true"));

                options.AddPolicy("AdminCreateRolePolicy",
                    policy => policy.RequireRole("Admin").RequireClaim("Create Role", "true"));

                options.AddPolicy("AdminDeleteRolePolicy",
                   policy => policy.RequireRole("Admin").RequireClaim("Delete Role", "true"));

                options.AddPolicy("ClassTeacherRolePolicy",
                    policy => policy.RequireRole("Class Teacher"));

                options.AddPolicy("StudentRolePolicy",
                    policy => policy.RequireRole("Student"));
            });

            //to change default path of access denied from account to admin
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });

            services.AddSingleton<IAuthorizationHandler,
                CanEditOnlyOtherAdminRolesAndClaimsHandler>();
     
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("/Error/{0}");
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
