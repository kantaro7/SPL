using MediatR;
using SPL.Domain.SPL.Artifact.ArtifactDesign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Commands.Artifactdesign
{
    public class UpdateNozzlesArtifactCommand : IRequest<long>
    {
        public UpdateNozzlesArtifactCommand(List<NozzlesArtifact> pData)
        {
            Data = pData;

        }
        #region Constructor

        public List<NozzlesArtifact> Data { get; }

        #endregion

    }
}