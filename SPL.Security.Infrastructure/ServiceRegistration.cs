using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Configuration;

namespace SPL.Security.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration Security)
        {

#if RELEASE
            services.AddDbContext<dbDevMigSPLContext>(o => o.UseSqlServer(Security.GetConnectionString("SqlConnection")));
#else
                    services.AddDbContext<dbDevMigSPLContext>(o => o.UseSqlServer(Security.GetConnectionString("SqlConnection")));
#endif

        }
    }
}
