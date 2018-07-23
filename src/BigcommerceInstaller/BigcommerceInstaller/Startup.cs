using BigcommerceInstaller.Middlewares;
using BigcommerceInstaller.Servcices;
using BigcommerceInstaller.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BigcommerceInstaller
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
              .ReadFrom.Configuration(Configuration)
              .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<BigCommerceConfig>(Configuration.GetSection("BigCommerceConfig"));
            services.Configure<ChatigyConfig>(Configuration.GetSection("ChatigyConfig"));

            services.AddMvc();

            services.AddScoped<IBigCommerceService, BigCommerceService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddSerilog();

            app.UseRequestResponseLogging();

            app.UseMvc();
        }
    }
}
