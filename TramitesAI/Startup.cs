using TramitesAI.Business.Services.Interfaces;

namespace TramitesAI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure services here
            services.AddScoped<IBusinessService, BusinessService>();
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
                // Add error handling middleware for non-development environments
                // For example: app.UseExceptionHandler("/Home/Error");
                // Or: app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            // Add other middleware configurations here
        }
    }
}
