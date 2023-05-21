using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPL.Configuration.Infrastructure.Entities;

namespace SPL.Configuration.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

        #if RELEASE
                    services.AddDbContext<dbDevMigSPLContext>(o => o.UseSqlServer(configuration.GetConnectionString("SqlConnection")));
        #else
                    services.AddDbContext<dbDevMigSPLContext>(o => o.UseSqlServer(configuration.GetConnectionString("SqlConnection")));
        #endif

        }
    }
}
