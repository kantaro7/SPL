namespace SPL.Security.Api
{
    using MediatR;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;

    using SPL.Security.Api.ApiConfiguration;

    using SPL.Security.Infrastructure;

    using System;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Identity.Web;
    using SPL.Security.Application.Queries.Security;
    using System.IdentityModel.Tokens.Jwt;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.AspNetCore.Authentication.JwtBearer;

    public class Startup
    {
        public Startup(IConfiguration Security) => this.Security = Security;

        public IConfiguration Security { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _ = services.AddControllers();
            _ = services.AddMediatR(typeof(GetProfilesQuery).Assembly);


            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddMicrosoftIdentityWebApiAuthentication(Security);

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

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SPL.Security.Api", Version = "v1" });
            });
            

            services.AddPersistenceInfrastructure(this.Security);
            services.ConfigureDependencies();


           





            //services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            //services.AddControllers().AddJsonOptions(options =>
            //    options.JsonSerializerOptions.Converters.Add(new TimeSpanToStringConverter()));
            //services.AddSwaggerGen(options =>
            //{
            //    options.MapType(typeof(TimeSpan), () => new OpenApiSchema
            //    {
            //        Type = "string",
            //        Example = new OpenApiString("00:00:00")
            //    });
            //});


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                _ = app.UseDeveloperExceptionPage();

            }
            _ = app.UseSwagger();
            _ = app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SPL.Security.Api v1"));
            _ = app.UseHttpsRedirection();

            _ = app.UseRouting();
            _ = app.UseAuthentication();
            _ = app.UseAuthorization();

            _ = app.UseEndpoints(endpoints => _ = endpoints.MapControllers());


            //  app.InitEntityFramework();

        }

        public static class CustomExtensionMethods
        {
            //public static IServiceCollection AddDatabase(this IServiceCollection services, ISecurity Security)
            //{
            //    //SQL Server - BAAN Structure
            //    //services.AddDbContext<Data.OracleDatabase>(options => options.UseSqlServer(Security["connectionString"]));

            //    //Oracle
            //    services.AddDbContext<Domain.IUnitOfWork, SIDCO.>(options => options.UseOracle(Security["connectionString"], sqlOptions => sqlOptions.UseOracleSQLCompatibility("11")));

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
