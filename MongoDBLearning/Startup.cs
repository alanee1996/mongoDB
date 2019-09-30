using AutoMapper;
using Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using MongoDBLearning.Middlewares;
using Repositories;
using Repositories.ARepositories;
using Repositories.Implementations;
using Repositories.Seeder;
using Services;
using Services.Implementations;
using Services.IServices;
using System.Text;
using System.Threading.Tasks;

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
            services.AddSignalR();
            services.AddScoped<DBEntities>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapConfig());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            AddJWTTokenService(services, settings);
            services.AddSingleton(mapper);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
            app.UseRouter(BuildRouterNormalApi(app));
            app.UseStaticFiles();
            app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            app.UseSignalR(r =>
            {
                r.MapHub<ChatService>("/chat");
            });
            app.UseJsonResponse();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        //build the custom route for specific middleware
        public IRouter BuildRouterNormalApi(IApplicationBuilder app)
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
                c.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/chat")))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    },
                };
            });
        }
    }

}
