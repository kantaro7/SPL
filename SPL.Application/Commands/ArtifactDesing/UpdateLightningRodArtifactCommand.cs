using MediatR;
using SPL.Domain.SPL.Artifact.ArtifactDesign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Commands.Artifactdesign
{
    public class UpdateLightningRodArtifactCommand : IRequest<long>
    {
        public UpdateLightningRodArtifactCommand(List<LightningRodArtifact> pData)
        {
            Data = pData;

        }
        #region Constructor

        public List<LightningRodArtifact> Data { get; }

        #endregion

    }
}
