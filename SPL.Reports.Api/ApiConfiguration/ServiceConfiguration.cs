namespace SPL.Masters.Api.ApiConfiguration
{
    using System.Globalization;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    using SPL.Domain.SPL.Reports;
    using SPL.Reports.Api;
    using SPL.Reports.Api.AutoMapper;
    using SPL.Reports.Infrastructure.Reports;

    public static class ServiceConfiguration
    {
        private const string AllowSpecificOrigins = "AllowAllOrigin";

        internal static void ConfigureDependencies(this IServiceCollection services)
        {
            _ = services.AddAutoMapper(typeof(Startup));
            //Infrastructure
            _ = services.AddTransient<IReportsInfrastructure, ReportsInfrastructure>();
            _ = services.AddAutoMapper(typeof(ReportsProfile).Assembly);
        }

        internal static void ConfigureCors(this IServiceCollection services) => services.AddCors(options => options.AddPolicy(
                                                                                  AllowSpecificOrigins,
                                                                                  builder => _ = builder.AllowAnyOrigin()
                                                                                          .AllowAnyHeader()
                                                                                          .AllowAnyMethod()));

        internal static void UseCorsDev(this IApplicationBuilder app) => app.UseCors(AllowSpecificOrigins);

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
