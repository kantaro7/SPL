using MediatR;

using SPL.Artifact.Application.Queries.Artifactdesign;

using SPL.Domain.SPL.Artifact.ArtifactDesign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Handlers.Artifactdesign
{
    public class GetVoltageKVQueryHandler : IRequestHandler<GetVoltageKVQuery, VoltageKV>
    {

        private readonly IArtifactdesignInfrastructure _infrastructure;

        public GetVoltageKVQueryHandler(IArtifactdesignInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<VoltageKV> Handle(GetVoltageKVQuery request, CancellationToken cancellationToken)
        {
           
               VoltageKV obj = new VoltageKV();
                var result =  await _infrastructure.GetInfoCarLocal(request.NroSerie);

                    obj.tensionkvaltatension1 = result.Mvaf1Voltage1;
                    obj.tensionkvaltatension2 = result.Mvaf1Voltage2;

                    obj.tensionkvbajatension1 = result.Mvaf2Voltage1;
                    obj.tensionkvbajatension2 = result.Mvaf2Voltage2;

                    obj.tensionkvsegundabaja1 = result.Mvaf3Voltage1;
                    obj.tensionkvsegundabaja2 = result.Mvaf3Voltage2;

                    obj.tensionkvterciario1 = result.Mvaf4Voltage1;
                    obj.tensionkvterciario2 = result.Mvaf4Voltage2;

                    obj.tensionkvaltatension3 = result.Mvaf1Voltage3;
                    obj.tensionkvaltatension4 = result.Mvaf1Voltage4;

                    obj.tensionkvbajatension3 = result.Mvaf2Voltage3;
                    obj.tensionkvbajatension4 = result.Mvaf2Voltage4;

                    obj.tensionkvsegundabaja3 = result.Mvaf3Voltage3;
                    obj.tensionkvsegundabaja4 = result.Mvaf3Voltage4;

                    obj.tensionkvterciario3 = result.Mvaf3Voltage4;
                    obj.tensionkvterciario4 = result.Mvaf4Voltage4;

             

            return obj;
        }

        
        #endregion
    }
}
