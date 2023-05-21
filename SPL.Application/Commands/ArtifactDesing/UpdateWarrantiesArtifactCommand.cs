using MediatR;
using SPL.Domain.SPL.Artifact.ArtifactDesign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Commands.Artifactdesign
{
    public class UpdateWarrantiesArtifactCommand : IRequest<long>
    {
        public UpdateWarrantiesArtifactCommand(WarrantiesArtifact pData)
        {
            Data = pData;

        }
        #region Constructor

        public WarrantiesArtifact Data { get; }

        #endregion

    }
}