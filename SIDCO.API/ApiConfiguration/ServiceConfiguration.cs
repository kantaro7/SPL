using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using SIDCO.API.AutoMapper;
using SIDCO.Domain.Artifactdesign;
using SIDCO.Infrastructure.Artifacdesign;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SIDCO.API.ApiConfiguration
{
    public static class ServiceConfiguration
    {
        private const string AllowSpecificOrigins = "AllowAllOrigin";

        internal static void ConfigureDependencies(this IServiceCollection services)
        {
             services.AddAutoMapper(typeof(Startup));
             //Infrastructure
             services.AddTransient<IArtifactdesignInfrastructure, ArtifactdesignInfrastructure>();
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
