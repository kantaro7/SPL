namespace Gateway.Api
{
    using Gateway.Api.Handlers;
    using Gateway.Api.HttpServices;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Ocelot.DependencyInjection;
    using Ocelot.Middleware;

    public class Startup
    {
        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            _ = services.AddControllers();
            _ = services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gateway.Api", Version = "v1" }));

            _ = services.AddOcelot(this.Configuration)
                .AddDelegatingHandler<RemoveEncodingDelegatingHandler>(true);

            _ = services.AddHttpClient<IReportHttpClientService, ReportHttpClientService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                _ = app.UseDeveloperExceptionPage();
          
            }
            _ = app.UseSwagger();
            _ = app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway.Api v1"));
            _ = app.UseHttpsRedirection();

            _ = app.UseRouting();
            _ = app.UseEndpoints(endpoints => _ = endpoints.MapControllers());

            app.UseOcelot().Wait();
        }
    }
}
