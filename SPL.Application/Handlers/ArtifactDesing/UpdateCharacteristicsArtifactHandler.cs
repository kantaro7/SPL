using MediatR;

using SPL.Artifact.Application.Commands.Artifactdesign;
using SPL.Domain.SPL.Artifact.ArtifactDesign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Handlers.Artifactdesign
{
   public  class UpdateCharacteristicsArtifactHandler : IRequestHandler<UpdateCharacteristicsArtifactCommand, long>
    {

        private readonly IArtifactdesignInfrastructure _infrastructure;

        public UpdateCharacteristicsArtifactHandler(IArtifactdesignInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<long> Handle(UpdateCharacteristicsArtifactCommand request, CancellationToken cancellationToken)
        {
           
            
           var result = await _infrastructure.UpdateCharacteristicsArtifact(request.Data);

            return result;
        }

        #endregion
    }
}
