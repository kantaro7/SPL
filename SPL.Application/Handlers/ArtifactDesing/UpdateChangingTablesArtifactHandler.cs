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
    public class UpdateChangingTablesArtifactHandler : IRequestHandler<UpdateChangingTablesArtifactCommand, long>
    {

        private readonly IArtifactdesignInfrastructure _infrastructure;

        public UpdateChangingTablesArtifactHandler(IArtifactdesignInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<long> Handle(UpdateChangingTablesArtifactCommand request, CancellationToken cancellationToken)
        {
     

            var result = await _infrastructure.UpdateChangingTablesArtifact(request.Data);

            return result;
        }

        #endregion
    }
}

