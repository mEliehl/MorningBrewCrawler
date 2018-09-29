using Crawler.Commands;
using DbUp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Runner.StartupBlocks;

namespace Hangfire.Runner
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
            var cs = this.Configuration.GetConnectionString("hangfire");
            EnsureDatabase.For.SqlDatabase(cs);
            services.AddHangfire(configuration =>
            {
                configuration.UseSqlServerStorage(cs);
            });
            services
               .AddHttpClients()
               .AddRepositories(Configuration)
               .AddCommanHandlers();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ICommandHandler<CrawlerCommand> command)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app
                .UseHttpsRedirection()
                .UseHangfireServer()
                .UseHangfireDashboard("/hangfire", new DashboardOptions()
                {
                    Authorization = new[] { new DashboardFilter() }
                })
                .UseMvc();

            RecurringJob.AddOrUpdate(() => command.HandleAsync(100), "0 9 * * 1-5");
        }
    }
}