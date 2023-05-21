using MediatR;
using SPL.Domain.SPL.Artifact.ArtifactDesign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Commands.Artifactdesign
{
    public class UpdateGeneralArtifactCommand : IRequest<long>
    {
        public UpdateGeneralArtifactCommand(GeneralArtifact pData)
        {
            Data = pData;

        }
        #region Constructor

        public GeneralArtifact Data { get; }

        #endregion

    }
}
