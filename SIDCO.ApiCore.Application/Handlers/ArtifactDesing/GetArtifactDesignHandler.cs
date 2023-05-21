using MediatR;
using SIDCO.Application.Queries.Artifactdesign;
using SIDCO.Domain.Artifactdesign;
using SIDCO.Domain.Domain.Models;
using SIDCO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SIDCO.Application.Handlers.Artifactdesign
{
    public class GetArtifactDesignHandler : IRequestHandler<GetArtifactDesignQuery, InformationArtifact>
    {

        private readonly IArtifactdesignInfrastructure _infrastructure;

        public GetArtifactDesignHandler(IArtifactdesignInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<InformationArtifact> Handle(GetArtifactDesignQuery request, CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(request.nroSerie))
            {
                return new InformationArtifact {GeneralArtifact = new GeneralArtifact { OrderCode = null} };

            }
            return await _infrastructure.GetGeneralArtifactdesign(request.nroSerie);
        }

        
        #endregion
    }
}
