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
    public class SaveArtifactDesignHandler : IRequestHandler<SaveArtifactDesignCommand, long>
    {

        private readonly IArtifactdesignInfrastructure _infrastructure;

        public SaveArtifactDesignHandler(IArtifactdesignInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<long> Handle(SaveArtifactDesignCommand request, CancellationToken cancellationToken)
        {
          

            var result = await _infrastructure.SaveInformationArtifact(request.Data);

            return result;
        }

        #endregion
    }
}

