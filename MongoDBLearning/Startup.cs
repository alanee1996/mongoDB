using AutoMapper;
using Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDBLearning.Middlewares;
using Repositories;
using Repositories.ARepositories;
using Repositories.Implementations;
using Services;
using Services.Implementations;
using Services.IServices;

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
            services.AddSingleton(mapper);
            //custom services
            //register repositories
            services.AddScoped<UserRepository, UserRepositoryImpl>();
            //register services
            services.AddScoped<IUserService, UserService>();
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
                a.UseJsonResponse();
                a.UseMvc();
            });

            return builder.Build();
        }
    }

}
