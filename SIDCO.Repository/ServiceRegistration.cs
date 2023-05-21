using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIDCO.Infrastructure
{
    public static class ServiceRegistration
    {
       
            public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
            {

              
            #if RELEASE
             services.AddDbContext<dbDevSIDCOContext>(o => o.UseSqlServer(configuration.GetConnectionString("SqlConnection")));
            #else
            services.AddDbContext<dbDevSIDCOContext>(o => o.UseSqlServer(configuration.GetConnectionString("SqlConnection")));
            #endif
        }

    }
}

