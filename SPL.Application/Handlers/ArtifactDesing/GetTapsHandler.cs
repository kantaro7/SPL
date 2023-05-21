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
    public class GetTapsHandler : IRequestHandler<GetTapsQuery, Taps>
    {

        private readonly IArtifactdesignInfrastructure _infrastructure;

        public GetTapsHandler(IArtifactdesignInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<Taps> Handle(GetTapsQuery request, CancellationToken cancellationToken)
        {
          
                    Taps obj = new Taps();
                    var result =  await _infrastructure.GetInfoCarLocal(request.NroSerie);

                    obj.tapsaltatension = result.Mvaf1Taps;
                    obj.tapsbajatension = result.Mvaf2Taps;
                    obj.tapssegundabaja = result.Mvaf3Taps;
                    obj.tapsterciario = result.Mvaf4Taps;

            return obj;
        }

        
        #endregion
    }
}
