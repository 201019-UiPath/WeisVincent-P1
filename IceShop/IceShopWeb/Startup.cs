using IceShopDB;
using IceShopDB.Repos;
using IceShopDB.Repos.DBRepos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IceShopWeb
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
            // This adds the views alongside the controllers that utilize them.
            services.AddControllersWithViews();
            services.AddDbContext<IceShopContext>(options => options.UseNpgsql(Configuration.GetConnectionString("IceShopDB")));
            // This adds a scoped service, meaning that a new one is made and reused within a request made, which means multiple can be used at once by many users.
            // Think of this as shorthand for, "Hey, ASP.Net. If any of my controllers need an IRepository, use the DBRepo implementation."
            services.AddScoped<IRepository, DBRepo>();
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    // This maps the default route of the application on opening, like a home page.
                    name: "default",
                    // The id in this pattern has a ? mark to make it optional.
                    // THIS is conventional based routing.
                    pattern: "{controller=Home}/{action=Index}/{id?}");


                endpoints.MapControllerRoute(name: "customer",
                    pattern: "Customer/{*Index}",
                    defaults: new { controller = "Customer", action = "Index" });

            });
        }
    }
}
