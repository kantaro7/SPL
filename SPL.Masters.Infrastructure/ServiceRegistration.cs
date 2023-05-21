namespace SPL.Masters.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration) => services.AddDbContext<dbQAMigSPLContext>(o => o.UseSqlServer(configuration.GetConnectionString("SqlConnection")));
    }
}
