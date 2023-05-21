namespace SPL.Reports.Infrastructure
{

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using SPL.Reports.Infrastructure.Entities;

    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration) => services.AddDbContext<dbReportsSPLContext>(o => o.UseSqlServer(configuration.GetConnectionString("SqlConnection")));
    }
}
