using MediatR;

using SPL.Domain.SPL.Artifact.ArtifactDesign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Commands.Artifactdesign
{
    public class SaveArtifactDesignCommand : IRequest<long>
    {
        public SaveArtifactDesignCommand(InformationArtifact pData)
        {
            Data = pData;

        }
        #region Constructor

        public InformationArtifact Data { get; }

        #endregion

    }
}
