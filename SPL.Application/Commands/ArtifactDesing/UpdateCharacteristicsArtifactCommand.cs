using MediatR;
using SPL.Domain.SPL.Artifact.ArtifactDesign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Commands.Artifactdesign
{
    public class UpdateCharacteristicsArtifactCommand : IRequest<long>
    {
        public UpdateCharacteristicsArtifactCommand(AllCharacteristicsArtifact pData)
        {
            Data = pData;
       

        }
        #region Constructor

        public AllCharacteristicsArtifact Data { get; }

        #endregion

    }
}
