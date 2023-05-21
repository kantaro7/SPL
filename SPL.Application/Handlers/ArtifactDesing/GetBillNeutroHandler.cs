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
    public class GetBillNeutroHandler : IRequestHandler<GetBillNeutroQuery, NBAINeutro>
    {

        private readonly IArtifactdesignInfrastructure _infrastructure;

        public GetBillNeutroHandler(IArtifactdesignInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<NBAINeutro> Handle(GetBillNeutroQuery request, CancellationToken cancellationToken)
        {
               
                
                 var result =  await _infrastructure.GetInfoCarLocal(request.NroSerie);

            return new NBAINeutro
            {
                valornbaineutroaltatension = result.Mvaf1NbaiNeutro,
                valornbaineutrobajatension = result.Mvaf2NbaiNeutro,
                valornbaineutrosegundabaja = result.Mvaf3NbaiNeutro,
                valornbaineutrotercera = result.Mvaf4NbaiNeutro
            };
        }

        
        #endregion
    }
}
