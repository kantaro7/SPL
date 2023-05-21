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
    public class UpdateGeneralArtifactHandler : IRequestHandler<UpdateGeneralArtifactCommand, long>
    {

        private readonly IArtifactdesignInfrastructure _infrastructure;

        public UpdateGeneralArtifactHandler(IArtifactdesignInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<long> Handle(UpdateGeneralArtifactCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Data.OrderCode))
                return -1;

            var result = await _infrastructure.UpdategeneralArtifac(request.Data);

            return result;
        }

        #endregion
    }
}
