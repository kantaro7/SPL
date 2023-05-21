using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using SPL.Artifact.Api.ApiConfiguration;

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SPL.Artifact.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using System.IdentityModel.Tokens.Jwt;

namespace SPL.Artifact.Api
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
           
            services.AddMediatR(typeof(Application.Queries.Artifactdesign.CheckNumberOrderQuery).Assembly);

            services.AddMediatR(typeof(Application.Queries.Artifactdesign.GetBillNeutroQuery).Assembly);
            services.AddMediatR(typeof(Application.Queries.Artifactdesign.GetConnectionTypesQuery).Assembly);
            services.AddMediatR(typeof(Application.Queries.Artifactdesign.GetDerivationsQuery).Assembly);
            services.AddMediatR(typeof(Application.Queries.Artifactdesign.GetNBAIBilKvQuery).Assembly);
            services.AddMediatR(typeof(Application.Queries.Artifactdesign.GetTapsQuery).Assembly);
            services.AddMediatR(typeof(Application.Queries.Artifactdesign.GetVoltageKVQuery).Assembly);

          
            services.AddMediatR(typeof(Application.Commands.Artifactdesign.SaveArtifactDesignCommand).Assembly);

            services.AddMediatR(typeof(Application.Commands.Artifactdesign.UpdateChangingTablesArtifactCommand).Assembly);
            services.AddMediatR(typeof(Application.Commands.Artifactdesign.UpdateCharacteristicsArtifactCommand).Assembly);
            services.AddMediatR(typeof(Application.Commands.Artifactdesign.UpdateGeneralArtifactCommand).Assembly);
            services.AddMediatR(typeof(Application.Commands.Artifactdesign.UpdateLabTestsArtifactCommand).Assembly);
            services.AddMediatR(typeof(Application.Commands.Artifactdesign.UpdateLightningRodArtifactCommand).Assembly);
            services.AddMediatR(typeof(Application.Commands.Artifactdesign.UpdateNozzlesArtifactCommand).Assembly);
            services.AddMediatR(typeof(Application.Commands.Artifactdesign.UpdateRulesArtifactCommand).Assembly);
            services.AddMediatR(typeof(Application.Commands.Artifactdesign.UpdateWarrantiesArtifactCommand).Assembly);

            #region PlaneTension 
            services.AddMediatR(typeof(Application.Queries.PlateTension.GetTapBaanQuery).Assembly);
            services.AddMediatR(typeof(Application.Queries.PlateTension.GetCharacterisQuery).Assembly);
            services.AddMediatR(typeof(Application.Queries.PlateTension.GetPlateTensionQuery).Assembly);
            services.AddMediatR(typeof(Application.Commands.PlateTension.SavePlateTensionCommand).Assembly);
            #endregion

            #region BaseTemplate 
            services.AddMediatR(typeof(Application.Commands.BaseTemplate.SaveBaseTemplateCommand).Assembly);
            services.AddMediatR(typeof(Application.Commands.BaseTemplate.SaveBaseTemplateConsolidatedReportCommand).Assembly);
            services.AddMediatR(typeof(Application.Queries.BaseTemplate.GetBaseTemplateQuery).Assembly);
            services.AddMediatR(typeof(Application.Queries.BaseTemplate.GetBaseTemplateConsolidatedReportQuery).Assembly);
            #endregion

            services.AddMediatR(typeof(Application.Commands.Nozzles.SaveRecordNozzleInformationCommand).Assembly);
            services.AddMediatR(typeof(Application.Queries.Nozzles.GetRecordNozzleInformationQuery).Assembly);

            services.AddMediatR(typeof(Application.Queries.Artifactdesign.GetResistDesignQuery).Assembly);
            services.AddMediatR(typeof(Application.Commands.Artifactdesign.SaveResistDesignCommand).Assembly);

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SPL.Artifact.Api", Version = "v1" });
            //    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); //This line
            //});


            //services.AddHttpClient();



            //services.AddMicrosoftIdentityWebApiAuthentication(Configuration)
            //    .EnableTokenAcquisitionToCallDownstreamApi()
            //    .AddInMemoryTokenCaches();

            //services.AddControllers(options =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        // .RequireClaim("email") // disabled this to test with users that have no email (no license added)
            //        .Build();
            //    options.Filters.Add(new AuthorizeFilter(policy));
            //});

            //services.AddSwaggerGen(c =>
            //{
            //    // add JWT Authentication
            //    var securityScheme = new OpenApiSecurityScheme
            //    {
            //        Name = "JWT Authentication",
            //        Description = "Enter JWT Bearer token **_only_**",
            //        In = ParameterLocation.Header,
            //        Type = SecuritySchemeType.Http,
            //        Scheme = "bearer", // must be lower case
            //        BearerFormat = "JWT",
            //        Reference = new OpenApiReference
            //        {
            //            Id = JwtBearerDefaults.AuthenticationScheme,
            //            Type = ReferenceType.SecurityScheme
            //        }
            //    };
            //    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            //    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            //    {
            //        {securityScheme, new string[] { }}
            //    });

            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SPL.Artifact.Api", Version = "v1" });
            //});


            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            //services.AddMicrosoftIdentityWebApiAuthentication(Configuration);

            //services.AddControllers(options =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .RequireClaim("unique_name")
            //        .Build();
            //    options.Filters.Add(new AuthorizeFilter(policy));
            //});




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

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SPL.Artifact.Api", Version = "v1" });
            });




         

            services.AddPersistenceInfrastructure(Configuration);
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
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SPL.Artifact.Api v1"));
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
         
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
