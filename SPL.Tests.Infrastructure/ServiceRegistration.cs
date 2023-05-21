namespace SPL.Tests.Infrastructure
{

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration) => services.AddDbContext<dbTestsSPLContext>(o => o.UseSqlServer(configuration.GetConnectionString("SqlConnection")));
    }
}
