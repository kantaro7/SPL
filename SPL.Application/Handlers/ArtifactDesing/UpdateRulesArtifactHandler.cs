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
    public class UpdateRulesArtifactHandler : IRequestHandler<UpdateRulesArtifactCommand, long>
    {

        private readonly IArtifactdesignInfrastructure _infrastructure;

        public UpdateRulesArtifactHandler(IArtifactdesignInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<long> Handle(UpdateRulesArtifactCommand request, CancellationToken cancellationToken)
        {
           

            var result = await _infrastructure.UpdateRulesArtifact(request.Data);

            return result;
        }

        #endregion
    }
}
