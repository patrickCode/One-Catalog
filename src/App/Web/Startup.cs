using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Catalog.Azure.Search;
using Microsoft.Catalog.Database.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Catalog.Common.Converters;
using Microsoft.Catalog.Azure.Search.Models;
using Microsoft.Catalog.Common.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Catalog.Azure.Search.Interfaces;
using Microsoft.Catalog.Database.Repositories.Read;
using Microsoft.Catalog.Database.Repositories.Write;
using Microsoft.Catalog.Common.Interfaces.Repository;
using Microsoft.Catalog.Domain.ProjectContext.Interfaces;
using Microsoft.Catalog.Domain.TechnologyContext.Interfaces;
using Microsoft.Catalog.Domain.ProjectContext.ApplicationServices;
using Microsoft.Catalog.Domain.TechnologyContext.ApplicationServices;

namespace Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin());
            });

            const string AzureSearchServiceName = "srch-onecatalog";
            const string AzureSearchSecretKey = "3304CCABCBCDBDE38790BBB4049A2300";
            var config = new AzureSearchConfiguration()
            {
                ServiceName = AzureSearchServiceName,
                ServiceSecretKey = AzureSearchSecretKey,
                Version = "2015-02-28-Preview",
                IsExponentialRetry = true,
                MaxRetryCount = 3,
                RetryInterval = TimeSpan.FromSeconds(1)
            };

            services.AddDbContext<OneCatalogDbContext>(options => 
                options.UseSqlServer(@"Server=tcp:sql-msonecatalogdev.database.windows.net,1433;Database=dbmsonecatalogdev;Trusted_Connection=False;User ID=catalogdevadmin;Password=CltgServerdev#312"));


            services.AddSingleton(config);
            services.AddSingleton<IConverter<SearchResponse>, JsonConverter<SearchResponse>>();
            services.AddSingleton<IConverter<SuggestionResponse>, JsonConverter<SuggestionResponse>>();
            services.AddScoped<IAzureSearchContext, AzureSearchContext>();
            services.AddScoped<IProjectSearchService, ProjectSearchService>();

            services.AddScoped<IReadOnlyRepository<Project>, ProjectReadOnlyRepository>();
            services.AddScoped<IReadOnlyRepository<ProjectTechnologies>, ProjectTechnologiesReadOnlyRepository>();
            services.AddScoped<IReadOnlyRepository<Technology>, TechnologyReadOnlyRepository>();
            services.AddScoped<IProjectQueryService, ProjectQueryService>();

            services.AddScoped<IRepository<Project>, ProjectRepository>();
            services.AddScoped<IRepository<ProjectTechnologies>, ProjectTechnologiesRepository>();
            services.AddScoped<IProjectService, ProjectService>();

            services.AddScoped<IReadOnlyRepository<Technology>, TechnologyReadOnlyRepository>();
            services.AddScoped<ITechnologyReadService, TechnologyReadService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseCors("AllowAll");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}