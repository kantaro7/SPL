
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Artifact.PlateTension
{
    public interface IPlateTensionInfrastructure
    {

        Task<IEnumerable<PlateTension>> getPlateTension(string Unit, string pTypeVoltage);
        public Task<long> savePlateTension(List<PlateTension> pList, bool pStatusDelete);
    }
}
