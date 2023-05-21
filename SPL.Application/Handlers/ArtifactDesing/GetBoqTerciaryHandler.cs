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
    public class GetBoqTerciaryHandler : IRequestHandler<GetBoqTerciaryQuery, decimal>
    {

        private readonly IArtifactdesignInfrastructure _infrastructure;

        public GetBoqTerciaryHandler(IArtifactdesignInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<decimal> Handle(GetBoqTerciaryQuery request, CancellationToken cancellationToken)
        {
           return  await _infrastructure.GetBoqTerciary(request.NroOrden);

        }

        
        #endregion
    }
}
