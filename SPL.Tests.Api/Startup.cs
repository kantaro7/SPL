namespace SPL.Tests.Api
{
    using MediatR;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Identity.Web;
    using Microsoft.OpenApi.Models;

    using SPL.Tests.Api.ApiConfiguration;
    using SPL.Tests.Infrastructure;

    using System.IdentityModel.Tokens.Jwt;

    public class Startup
    {
        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _ = services.AddControllers();
            _ = services.AddMediatR(typeof(Application.Queries.Tests.GetTestsQuery).Assembly);

            _ = services.AddMediatR(typeof(Application.Queries.Tests.GetNumNextRADQuery).Assembly);
            _ = services.AddMediatR(typeof(Application.Queries.Tests.GetNumNextRDTQuery).Assembly);

            _ = services.AddMediatR(typeof(Application.Commands.Tests.RADTestsCommand).Assembly);
            _ = services.AddMediatR(typeof(Application.Commands.Tests.RDTTestsCommand).Assembly);
            _ = services.AddMediatR(typeof(Application.Commands.Tests.RANTestsCommand).Assembly);
            _ = services.AddMediatR(typeof(Application.Commands.Tests.FPCTestsCommand).Assembly);
            _ = services.AddMediatR(typeof(Application.Commands.Tests.RODTestsCommand).Assembly);

            _ = services.AddMediatR(typeof(Application.Queries.Tests.GetNumNextFPCQuery).Assembly);
            _ = services.AddMediatR(typeof(Application.Queries.Tests.GetNumNextRODQuery).Assembly);
            _ = services.AddMediatR(typeof(Application.Queries.Tests.GetNumNextRANQuery).Assembly);

            _ = services.AddMediatR(typeof(Application.Queries.Tests.GetNumNextFPBQuery).Assembly);
            _ = services.AddMediatR(typeof(Application.Queries.Tests.GetNumNextRCTQuery).Assembly);


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

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SPL.Tests.Api", Version = "v1" });
            });
         

            services.AddPersistenceInfrastructure(this.Configuration);
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
            _ = app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SPL.Tests.Api v1"));
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
