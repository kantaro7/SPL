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
   public class UpdateWarrantiesArtifactHandler : IRequestHandler<UpdateWarrantiesArtifactCommand, long>
    {

        private readonly IArtifactdesignInfrastructure _infrastructure;

        public UpdateWarrantiesArtifactHandler(IArtifactdesignInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<long> Handle(UpdateWarrantiesArtifactCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Data.OrderCode))
                return -1;

            var result = await _infrastructure.UpdateWarrantiesArtifact(request.Data);

            return result;
        }

        #endregion
    }
}
