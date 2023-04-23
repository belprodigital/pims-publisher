using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using PimsPublisher.Application;
using PimsPublisher.Infrastructure;

namespace PimsPublisher.Web
{
    public class StartUp
    {
        public StartUp(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();

            services.AddInfrastructure("publisherDbConnectionString", Configuration);

            // Add services to the container.
            services.AddControllersWithViews();


            services.AddHangfire(cfg =>
            {
                cfg.UseSqlServerStorage(Configuration.GetConnectionString("hangfiredbConnectionString"));
            });

            services.AddHangfireServer();

        }

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
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseHangfireDashboard("/jobs");
        }

    }
}
