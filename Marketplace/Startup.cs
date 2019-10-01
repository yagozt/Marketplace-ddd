

using Marketplace.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using static Marketplace.Contracts.ClassifiedAds;

namespace Marketplace
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        private IConfiguration Configuration { get; }
        private IHostingEnvironment Environment { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IEntityStore, RavenDbEntityStore>();
            services.AddScoped<IHandleCommand<Contracts.ClassifiedAds.V1.Create>>(c => 
            new RetryingCommandHandler<V1.Create>(new CreateClassifiedAdHandler(c.GetService<RavenDbEntityStore>())));
            services.AddSingleton(new ClassifiedAdsApplicationService());
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Info
            {
                Title = "ClassifiedAds",
                Version = "v1"
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json",
                                                    "ClassifiedAds v1"));
        }
    }
}
