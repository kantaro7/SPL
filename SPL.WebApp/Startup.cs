namespace SPL.WebApp
{
    using AutoMapper;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Identity.Web;
    using Microsoft.Identity.Web.TokenCacheProviders.Distributed;
    using Microsoft.Identity.Web.UI;

    using Newtonsoft.Json.Serialization;

    using Serilog;

    using SPL.WebApp.Domain.SecurityApis;
    using SPL.WebApp.Domain.Services;
    using SPL.WebApp.Domain.Services.FPC;
    using SPL.WebApp.Domain.Services.Imp;
    using SPL.WebApp.Domain.Services.Imp.ETD;
    using SPL.WebApp.Domain.Services.Imp.FPC;
    using SPL.WebApp.Domain.Services.Imp.ISZ;
    using SPL.WebApp.Domain.Services.Imp.PCE;
    using SPL.WebApp.Domain.Services.Imp.PEE;
    using SPL.WebApp.Domain.Services.Imp.PIR;
    using SPL.WebApp.Domain.Services.Imp.RAN;
    using SPL.WebApp.Domain.Services.Imp.ROD;
    using SPL.WebApp.Domain.Services.Imp.TAP;
    using SPL.WebApp.Domain.Services.Imp.TDP;
    using SPL.WebApp.Domain.Services.ProfileSecurity;
    using SPL.WebApp.Mapper;
    using SPL.WebApp.Services;
    using SPL.WebApp.Validations;

    using System;
    using System.Security.Claims;

    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment CurrentEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //Add cors
            //_ = services.AddCors(options => options.AddPolicy(name: "SPL-Migration",
            //                        builder => _ = builder.WithOrigins("https://localhost:44308", "https://localhost:5001",
            //                                                "https://qa-spl-webapp.azurewebsites.net",
            //                                                "http://qa-spl-webapp.azurewebsites.net")));

            _ = services.AddControllers();

            string[] initialScopes = Configuration.GetValue<string>("CallApi:ScopeForAccessTokenBasic")?.Split(' ');

            _ = services.AddMicrosoftIdentityWebAppAuthentication(Configuration).EnableTokenAcquisitionToCallDownstreamApi(initialScopes).AddDistributedTokenCaches().AddMicrosoftGraph();

            _ = services.Configure<MsalDistributedTokenCacheAdapterOptions>(options =>
            {
                // Optional: Disable the L1 cache in apps that don't use session affinity
                //                 by setting DisableL1Cache to 'false'.
                options.DisableL1Cache = false;

                // Or limit the memory (by default, this is 500 MB)
                options.L1CacheOptions.SizeLimit = 1024 * 1024 * 1024; // 1 GB

                // You can choose if you encrypt or not encrypt the cache
                options.Encrypt = false;

                // And you can set eviction policies for the distributed
                // cache.
                options.SlidingExpiration = TimeSpan.FromHours(1);
            });

            _ = services.AddDistributedMemoryCache();

      

            if (CurrentEnvironment.IsDevelopment() || CurrentEnvironment.IsEnvironment("Local"))
            {
                _ = services.AddSingleton<IDistributedCache, LocalFileTokenCache>();
            }
            else
            {
                _ = services.AddSingleton<IDistributedCache, MemoryDistributedCache>();
            }

            //services.AddRazorPages().AddMvcOptions(options =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .Build();
            //    options.Filters.Add(new AuthorizeFilter(policy));
            //}).AddMicrosoftIdentityUI();

            _ = services.AddMvc(sharedOptions =>
            {
                AuthorizationPolicy policity = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

                sharedOptions.Filters.Add(new AuthorizeFilter(policity));

            });

            //     services.AddAuthentication(sharedOptions =>
            // {
            //     sharedOptions.DefaultScheme = Microsoft.Identity.Web.Constants.Bearer;
            //     sharedOptions.DefaultChallengeScheme = Microsoft.Identity.Web.Constants.Bearer;
            // })
            //.AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

            _ = services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
         
            // Add framework services.
            _ = services
                .AddControllersWithViews()
                // Maintain property names during serialization. See:
                // https://github.com/aspnet/Announcements/issues/194
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            _ = services.AddControllers()
        .AddJsonOptions(options =>
             options.JsonSerializerOptions.Converters.Add(new CustomConverter())

);

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>
            //    {
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
            //    options.SlidingExpiration = true;
            //    options.AccessDeniedPath = "/Forbidden/";
            //    });

            #region Automapper

            MapperConfiguration config = new(cfg => cfg.AddProfile(new ConfigureAutoMapper()));
            IMapper mapper = config.CreateMapper();
            _ = services.AddSingleton(mapper);

            #endregion

            // Add Kendo UI services to the services container
            _ = services.AddKendo();
            _ = services.AddHttpClient<IArtifactClientService, ArtifactClientService>();
            _ = services.AddHttpClient<IMasterHttpClientService, MasterHttpClientService>();
            _ = services.AddHttpClient<ISidcoClientService, SidcoClientService>();
            _ = services.AddHttpClient<IConfigurationClientService, ConfigurationClientService>();
            _ = services.AddTransient<IArtifactRecordService, ArtifactRecordService>();
            _ = services.AddTransient<IPlateTensionService, PlateTensionService>();
            _ = services.AddTransient<IReportClientService, ReportClientService>();
            _ = services.AddTransient<ITestClientService, TestClientService>();
            _ = services.AddTransient<IRadClientService, RadClientService>();
            _ = services.AddTransient<IRdtClientService, RdtClientService>();
            _ = services.AddTransient<IFpcClientService, FpcClientService>();
            _ = services.AddTransient<IRodClientService, RodClientService>();
            _ = services.AddTransient<IGatewayClientService, GatewayClientService>();
            _ = services.AddTransient<ICorrectionFactorService, CorrectionFactorService>();
            _ = services.AddTransient<IRadService, RadService>();
            _ = services.AddTransient<IRdtService, RdtService>();
            _ = services.AddTransient<IPciService, PciService>();
            _ = services.AddTransient<IPciClientService, PciClientService>();
            _ = services.AddTransient<IPlrService, PlrService>();
            _ = services.AddTransient<IPlrClientService, PlrClientService>();
            _ = services.AddTransient<IRdtService, RdtService>();
            _ = services.AddTransient<IFpcService, FpcService>();
            _ = services.AddTransient<IRodService, RodService>();
            _ = services.AddTransient<IRanClientService, RanClientService>();
            _ = services.AddTransient<IRanService, RanService>();
            _ = services.AddTransient<INozzleMarkService, NozzleMarkService>();
            _ = services.AddTransient<INozzleBrandTypeClientService, NozzleBrandTypeClientService>();
            _ = services.AddHttpClient<IResistanceTwentyDegreesClientServices, ResistanceTwentyDegreesClientServices>();
            _ = services.AddTransient<INozzleInformationService, NozzleInformationService>();
            _ = services.AddTransient<IFpbClientService, FpbClientService>();
            _ = services.AddTransient<IFpbService, FpbService>();
            _ = services.AddHttpClient<IRctClientService, RctClientService>();
            _ = services.AddTransient<IRctService, RctService>();
            _ = services.AddHttpClient<IPceClientService, PceClientService>();
            _ = services.AddTransient<IPceService, PceService>();
            _ = services.AddHttpClient<IPrdClientService, PrdClientService>();
            _ = services.AddTransient<IPrdService, PrdService>();
            _ = services.AddHttpClient<IPeeClientService, PeeClientService>();
            _ = services.AddTransient<IPeeService, PeeService>();
            _ = services.AddHttpClient<IRyeClientService, RyeClientService>();
            _ = services.AddTransient<IRyeService, RyeService>();
            _ = services.AddTransient<IIszClientService, IszClientService>();
            _ = services.AddTransient<IIszService, IszService>();
            _ = services.AddTransient<IPirClientService, PirClientService>();
            _ = services.AddTransient<IPirService, PirService>();
            _ = services.AddTransient<IPimClientService, PimClientService>();
            _ = services.AddTransient<IPimService, PimService>();
            _ = services.AddTransient<ITinClientService, TinClientService>();
            _ = services.AddTransient<ITinService, TinService>();
            _ = services.AddTransient<ITapClientService, TapClientService>();
            _ = services.AddTransient<ITapService, TapService>();
            _ = services.AddTransient<ICemClientService, CemClientService>();
            _ = services.AddTransient<ICemService, CemService>();
            _ = services.AddTransient<ITdpClientService, TdpClientService>();
            _ = services.AddTransient<ITdpService, TdpService>();
            _ = services.AddTransient<ICgdClientService, CgdClientService>();
            _ = services.AddTransient<ICgdService, CgdService>();
            _ = services.AddTransient<IRddClientService, RddClientService>();
            _ = services.AddTransient<IRddService, RddService>();
            _ = services.AddTransient<IBpcClientService, BpcClientService>();
            _ = services.AddTransient<IBpcService, BpcService>();
            _ = services.AddTransient<IArfClientService, ArfClientService>();
            _ = services.AddTransient<IArfService, ArfService>();
            _ = services.AddTransient<IFpaClientService, FpaClientService>();
            _ = services.AddTransient<IFpaService, FpaService>();          
            _ = services.AddTransient<IIndClientService, IndClientService>();
            _ = services.AddTransient<IIndService, IndService>();
            _ = services.AddTransient<IInformationOctavesService, InformationOctavesService>();
            _ = services.AddTransient<IETDClientService, ETDClientService>();
            _ = services.AddTransient<IEtdService, EtdService>();
            _ = services.AddTransient<INraClientService, NraClientService>();
            _ = services.AddTransient<INraService, NraService>();
            _ = services.AddTransient<IDprService, DprService>();
            _ = services.AddTransient<IDprClientService, DprClientService>();
            _ = services.AddTransient<IProfileSecurityService, ProfileSecurityService>();

            _ = services.AddTransient<IValidateAccesApis, ValidateAccesApis>();
            _ = services.AddTransient<ICustomClaimsPrincipalFactory, CustomClaimsPrincipalFactory>();
            _  =services.AddSingleton(Log.Logger);

            //services.AddTransient<IClaimsTransformation, ClaimsTransformation>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Local"))
            {
                _ = app.UseDeveloperExceptionPage();
                //_ = app.UseBrowserLink();
            }
            else
            {
                _ = app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                _ = app.UseHsts();
            }
            _ = app.UseHttpsRedirection();
            _ = app.UseStaticFiles();

            _ = app.UseRouting();

            //_ = app.UseCors("SPL-Migration");

            _ = app.UseCookiePolicy();
            _ = app.UseAuthorization();
            _ = app.UseAuthentication();

            _ = app.UseEndpoints(endpoints => _ = endpoints.MapControllerRoute(
                      name: "default",
                      pattern: "{controller=Home}/{action=Index}/{id?}"));
        }
    }
}