using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IceShopBL;
using IceShopBL.Services;
using IceShopDB;
using IceShopDB.Repos;
using IceShopDB.Repos.DBRepos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IceShopAPI
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
            services.AddControllers();//.AddXmlSerializerFormatters();

            services.AddMvc(options => {
                options.Filters.Add(new ProducesAttribute("application/json"));
                options.Filters.Add(new ConsumesAttribute("application/json"));
                //options.Filters.Add(new ApiControllerAttribute());
            });

            // TODO: Add session services and implementation

            services.AddDbContext<IceShopContext>(options => options.UseNpgsql(Configuration.GetConnectionString("IceShopDB")));



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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
