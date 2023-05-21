
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Artifact.Nozzles
{
    public interface INozzlesInfrastructure
    {
        public Task<List<RecordNozzleInformation>> GetRecordNozzleInformation(string nroSerie);
        public Task<long> saveRecordNozzleInformation(NozzlesByDesign pData);
    }
}
