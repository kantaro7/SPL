using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using SPL.Artifact.Api.AutoMapper;
using SPL.Artifact.Infrastructure;
using SPL.Artifact.Infrastructure.Artifacdesign;
using SPL.Artifact.Infrastructure.BaseTemplate;
using SPL.Artifact.Infrastructure.Nozzles;
using SPL.Domain.SPL.Artifact.ArtifactDesign;
using SPL.Domain.SPL.Artifact.BaseTemplate;
using SPL.Domain.SPL.Artifact.Nozzles;
using SPL.Domain.SPL.Artifact.PlateTension;

using System.Globalization;

namespace SPL.Artifact.Api.ApiConfiguration
{
    public static class ServiceConfiguration
    {
        private const string AllowSpecificOrigins = "AllowAllOrigin";

        internal static void ConfigureDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            //Infrastructure
            services.AddTransient<IArtifactdesignInfrastructure, ArtifactdesignInfrastructure>();
            services.AddTransient<IPlateTensionInfrastructure, PlateTensionInfrastructure>();
            services.AddTransient<IBaseTemplateInfrastructure, BaseTemplateInfrastructure>();
            services.AddTransient<INozzlesInfrastructure, NozzlesInfrastructure>();
            services.AddAutoMapper(typeof(GeneralArtifactProfile).Assembly);

        }

        internal static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy(
                AllowSpecificOrigins,
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }));
        }

        internal static void UseCorsDev(this IApplicationBuilder app)
        {
            app.UseCors(AllowSpecificOrigins);
        }

        internal static void UseLocalization(this IApplicationBuilder app)
        {
            var localizedOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizedOptions.Value);
        }

        internal static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
           // app.UseMiddleware(typeof(ExceptionHandler));
        }

        internal static void ConfigureLocalization(this IServiceCollection services)
        {
            services.AddLocalization(opt => opt.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
               {
                    new CultureInfo("es"),
                    new CultureInfo("en")
                };

                options.DefaultRequestCulture = new RequestCulture("es");
                options.SupportedCultures = supportedCultures;
            });
        }
    }
}
