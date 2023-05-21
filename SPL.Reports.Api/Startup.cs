namespace SPL.Reports.Api
{
    using System.IdentityModel.Tokens.Jwt;

    using MediatR;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Identity.Web;
    using Microsoft.OpenApi.Models;

    using SPL.Masters.Api.ApiConfiguration;
    using SPL.Reports.Infrastructure;

    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            _ = services.AddControllers();
            _ = services.AddMediatR(typeof(Application.Queries.Reports.GetReportsQuery).Assembly);
            _ = services.AddMediatR(typeof(Application.Queries.Reports.GetGeneralTypesReportsQuery).Assembly);
            _ = services.AddMediatR(typeof(Application.Queries.Reports.GetFilterReportsQuery).Assembly);
            _ = services.AddMediatR(typeof(Application.Queries.Reports.GetConfigurationReportsQuery).Assembly);

            _ = services.AddMediatR(typeof(Application.Queries.Reports.GetConfigFileETDQuery).Assembly);

            _ = services.AddMediatR(typeof(Application.Queries.Reports.GetColumsConfigRDTQuery).Assembly);
            _ = services.AddMediatR(typeof(Application.Queries.Reports.GetColumsConfigRADQuery).Assembly);
            _ = services.AddMediatR(typeof(Artifact.Application.Commands.Reports.SaveReportRANCommand).Assembly);
            _ = services.AddMediatR(typeof(Artifact.Application.Commands.Reports.SaveReportRDTCommand).Assembly);

            _ = services.AddMediatR(typeof(Artifact.Application.Commands.Reports.SaveReportRADCommand).Assembly);

            _ = services.AddMediatR(typeof(Artifact.Application.Commands.Reports.SaveReportFPCCommand).Assembly);
            _ = services.AddMediatR(typeof(Artifact.Application.Commands.Reports.SaveReportRODCommand).Assembly);

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            _ = services.AddMicrosoftIdentityWebApiAuthentication(Configuration);

            _ = services.AddControllers(options =>
            {
                AuthorizationPolicy policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim("unique_name")
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            _ = services.AddSwaggerGen(c =>
            {
                // add JWT Authentication
                OpenApiSecurityScheme securityScheme = new()
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SPL.Reports.Api", Version = "v1" });
            });

            _ = services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            services.AddPersistenceInfrastructure(Configuration);
            services.ConfigureDependencies();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                _ = app.UseDeveloperExceptionPage();

            }
            _ = app.UseSwagger();
            _ = app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SPL.Reports.Api v1"));
            _ = app.UseHttpsRedirection();

            _ = app.UseRouting();
            _ = app.UseAuthentication();
            _ = app.UseAuthorization();

            _ = app.UseEndpoints(endpoints => _ = endpoints.MapControllers());

            //  app.InitEntityFramework();

        }

        public static class CustomExtensionMethods
        {
            //public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
            //{
            //    //SQL Server - BAAN Structure
            //    //services.AddDbContext<Data.OracleDatabase>(options => options.UseSqlServer(configuration["connectionString"]));

            //    //Oracle
            //    services.AddDbContext<Domain.IUnitOfWork, SIDCO.>(options => options.UseOracle(configuration["connectionString"], sqlOptions => sqlOptions.UseOracleSQLCompatibility("11")));

            //    return services;
            //}

            //public static void InitEntityFramework(this IApplicationBuilder app)
            //{
            //    using IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            //    //SQL Server - BAAN Structure
            //    //using var database = serviceScope.ServiceProvider.GetRequiredService<Data.OracleDatabase>();

            //    //Oracle
            //    Domain.IUnitOfWork database = serviceScope.ServiceProvider.GetRequiredService<Domain.IUnitOfWork>();

            //    database.Connection.Open();
            //    database.Connection.Close();
            //}

            //public static IServiceCollection AddAppServices(this IServiceCollection services)
            //{
            //    //SQL Server - BAAN Structure
            //    //services.AddTransient<Services.IItemService>(services => new Services.Oracle.ItemService(services.GetService<Data.OracleDatabase>()));

            //    //Oracle
            //    //services.AddTransient<Services.IItemService>(services => new Services.Oracle.ItemService(services.GetService<Oracle.OracleDatabase>()));

            //    return services;
            //}
        }
    }
}
