namespace SPL.Security.Api.ApiConfiguration
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    using SPL.Security.Api.AutoMapper;

    using SPL.Domain.SPL.Security;

   

    using System.Globalization;
    using SPL.Security.Infrastructure.Security;

    public static class ServiceSecurity
    {
        private const string AllowSpecificOrigins = "AllowAllOrigin";

        internal static void ConfigureDependencies(this IServiceCollection services)
        {
            _ = services.AddAutoMapper(typeof(Startup));
            //Infrastructure
            _ = services.AddTransient<ISecurityInfrastructure, SecurityInfrastructure>();
            _ = services.AddAutoMapper(typeof(SecurityProfile).Assembly);
        }

        internal static void ConfigureCors(this IServiceCollection services) => _ = services.AddCors(options => options.AddPolicy(
                                                                                    AllowSpecificOrigins,
                                                                                    builder => _ = builder.AllowAnyOrigin()
                                                                                            .AllowAnyHeader()
                                                                                            .AllowAnyMethod()));

        internal static void UseCorsDev(this IApplicationBuilder app) => _ = app.UseCors(AllowSpecificOrigins);

        internal static void UseLocalization(this IApplicationBuilder app)
        {
            IOptions<RequestLocalizationOptions> localizedOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            _ = app.UseRequestLocalization(localizedOptions.Value);
        }

        internal static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
           // app.UseMiddleware(typeof(ExceptionHandler));
        }

        internal static void ConfigureLocalization(this IServiceCollection services)
        {
            _ = services.AddLocalization(opt => opt.ResourcesPath = "Resources");
            
            _ = services.Configure<RequestLocalizationOptions>(options =>
              {
                  CultureInfo[] supportedCultures = new[]
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
