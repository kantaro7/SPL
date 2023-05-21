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
    public class GetNBAIBilKvHandler : IRequestHandler<GetNBAIBilKvQuery, NBAIBilKv>
    {

        private readonly IArtifactdesignInfrastructure _infrastructure;

        public GetNBAIBilKvHandler(IArtifactdesignInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<NBAIBilKv> Handle(GetNBAIBilKvQuery request, CancellationToken cancellationToken)
        {
              
                NBAIBilKv obj = new NBAIBilKv();
                var result =  await _infrastructure.GetInfoCarLocal(request.NroSerie);

          
                    obj.nbaialtatension = result.Mvaf1Nbai1;
                    obj.nbaibajatension = result.Mvaf2Nbai1;
                    obj.nbaisegundabaja = result.Mvaf3Nbai1;
                    obj.nabaitercera = result.Mvaf4Nbai1;
           
               
            return obj;
        }

        
        #endregion
    }
}
