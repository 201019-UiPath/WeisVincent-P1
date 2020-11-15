using AutoMapper;
using IceShopBL;
using IceShopBL.Services;
using IceShopDB;
using IceShopDB.Repos;
using IceShopDB.Repos.DBRepos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Net;

namespace IceShopAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddCors(options=> {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder => {
                        builder.WithOrigins(""/*new string[]{ "https://localhost:44329","http://localhost:65347"}*//*TODO: Add specifically permitted origin, the front end*/)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    }
                    );
            
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();//.AddXmlSerializerFormatters();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Ice Shop API",
                    Description = "An API that exposes the business logic required to sell ice online with IceShop LLC."
                });
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(new ProducesAttribute("application/json"));
                options.Filters.Add(new ConsumesAttribute("application/json"));
                //options.Filters.Add(new ApiControllerAttribute());
            });

            // TODO: Add session services and implementation

            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, errors) => { return true; };

            services.AddDbContext<IceShopContext>(options => options.UseNpgsql(Configuration.GetConnectionString("IceShopDB")));

            services.AddAuthorization();

            // This adds a scoped service, meaning that a new one is made and reused within a request made, which means multiple can be used at once by many users.
            // Think of this as shorthand for, "Hey, ASP.Net. If any of my controllers need an IRepository, use the DBRepo implementation."
            services.AddScoped<IRepository, DBRepo>();

            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IManagerService, ManagerService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IStartService, StartService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });


            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
