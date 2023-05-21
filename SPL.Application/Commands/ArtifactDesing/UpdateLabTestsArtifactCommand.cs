
using MediatR;
using SPL.Domain.SPL.Artifact.ArtifactDesign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Commands.Artifactdesign
{
    public class UpdateLabTestsArtifactCommand : IRequest<long>
    {
        public UpdateLabTestsArtifactCommand(LabTestsArtifact pData)
        {
            Data = pData;

        }
        #region Constructor

        public LabTestsArtifact Data { get; }

        #endregion

    }
}
