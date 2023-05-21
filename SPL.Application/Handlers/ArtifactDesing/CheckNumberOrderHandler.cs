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
    public class CheckNumberOrderHandler : IRequestHandler<CheckNumberOrderQuery, bool>
    {

        private readonly IArtifactdesignInfrastructure _infrastructure;

        public CheckNumberOrderHandler(IArtifactdesignInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<bool> Handle(CheckNumberOrderQuery request, CancellationToken cancellationToken)
        {
           return  await _infrastructure.CheckOrderNumber(request.NroOrden);

        }

        
        #endregion
    }
}
