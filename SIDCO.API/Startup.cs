using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Serilog;
using SIDCO.API.ApiConfiguration;
using SIDCO.Infrastructure;

using System.IdentityModel.Tokens.Jwt;

namespace SIDCO.API
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

            services.AddControllers();

            services.AddMediatR(typeof(Application.Queries.Artifactdesign.GetArtifactDesignQuery).Assembly);
            services.AddMediatR(typeof(Application.Queries.Artifactdesign.GetLenguagesQuery).Assembly);

            services.AddMediatR(typeof(Application.Queries.Artifactdesign.GetAngularDisplacementQuery).Assembly);
            services.AddMediatR(typeof(Application.Queries.Artifactdesign.GetApplicableRuleQuery).Assembly);
            services.AddMediatR(typeof(Application.Queries.Artifactdesign.GetApplicationsQuery).Assembly);
            services.AddMediatR(typeof(Application.Queries.Artifactdesign.GetTypeTransformersQuery).Assembly);

            services.AddMediatR(typeof(Application.Queries.Artifactdesign.GetConnectionTypesQuery).Assembly);
            services.AddMediatR(typeof(Application.Queries.Artifactdesign.GetOperativeConditionsQuery).Assembly);


            services.AddSingleton(Log.Logger);
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddMicrosoftIdentityWebApiAuthentication(Configuration);

            services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim("unique_name")
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });



            services.AddSwaggerGen(c =>
            {
                // add JWT Authentication
                var securityScheme = new OpenApiSecurityScheme
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

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SIDCO.API", Version = "v1" });
            });



        

          
            // services.AddAppServices();
            services.AddPersistenceInfrastructure(Configuration);
           // services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
   
            services.ConfigureDependencies();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
              
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SIDCO.API v1"));
            app.UseHttpsRedirection();

            app.UseRouting();
            _ = app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //  app.InitEntityFramework();

        }
    }

        public static class CustomExtensionMethods
        {
        //public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddDbContext<SIDCOContext>(opciones => opciones.UseOracle(configuration.GetConnectionString("OracleConnection"), b => b.MigrationsAssembly((SIDCOContext).Assembly.FullName).UseOracleSQLCompatibility("12")));
        //    return services;

        //    services.AddDbContext<Domain.IUnitOfWork, SIDCOContext>(options => options.UseOracle(configuration["OracleConnection"], sqlOptions => sqlOptions.UseOracleSQLCompatibility("11")));
        //    return services;
        //}

        //public static void InitEntityFramework(this IApplicationBuilder app)
        //{
        //    using IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

        //    Oracle
        //    Domain.IUnitOfWork database = serviceScope.ServiceProvider.GetRequiredService<Domain.IUnitOfWork>();

        //    database.Connection.Open();
        //    database.Connection.Close();
        //}

        //public static IServiceCollection AddAppServices(this IServiceCollection services)
        //{
        //    SQL Server -BAAN Structure
        //    services.AddTransient<Services.IItemService>(services => new Services.Oracle.ItemService(services.GetService<Data.OracleDatabase>()));

        //    Oracle
        //    services.AddTransient<Services.IItemService>(services => new Services.Oracle.ItemService(services.GetService<Oracle.OracleDatabase>()));

        //    return services;
        //}
    }
    }

