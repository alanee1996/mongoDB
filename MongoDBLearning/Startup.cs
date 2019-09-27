using AutoMapper;
using Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDBLearning.Middlewares;
using Repositories;
using Repositories.ARepositories;
using Repositories.Implementations;
using Repositories.Seeder;
using Services;
using Services.Implementations;
using Services.IServices;
using Services.Security;
using System.Text;

namespace MongoDBLearning
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
            var settings = this.Configuration.GetSection("AppSettings");
            services.Configure<AppSetting>(settings);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddScoped<DBEntities>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapConfig());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            AddJWTTokenService(services, settings);
            services.AddSingleton(mapper);

            //register repositories
            services.AddScoped<UserRepository, UserRepositoryImpl>();
            services.AddScoped<RoleRepository, RoleRepositoryImpl>();
            services.AddScoped<RolePermissionRepository, RolePermissionRepositoryImpl>();

            services.AddScoped<RepoCollections>();
            //register services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouter(BuildRouter(app));
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        //build the custom route for specific middleware
        public IRouter BuildRouter(IApplicationBuilder app)
        {
            var builder = new RouteBuilder(app);

            builder.MapMiddlewareRoute("/api/authenticated/{*controller}", a =>
            {
                a.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                a.UseJsonResponse();
                a.UseAuthentication();
                a.UseMvc();
            });

            return builder.Build();
        }


        public void AddJWTTokenService(IServiceCollection services, IConfigurationSection settings)
        {
            var secret = Encoding.ASCII.GetBytes(settings["tokenKey"]);
            services.AddAuthentication(c =>
            {
                c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                c.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //.AddScheme<TokenAuthenticationOptions, TokenAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, c =>
            //{
            //    c.saveToken = false;
            //    c.tokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(secret),
            //        ValidateIssuer = false,
            //        ValidateAudience = false
            //    };
            //});
            .AddJwtBearer(c =>
            {
                c.RequireHttpsMetadata = false;
                c.SaveToken = true;
                c.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
    }

}
