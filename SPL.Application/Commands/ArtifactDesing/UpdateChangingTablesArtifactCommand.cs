using MediatR;
using SPL.Domain.SPL.Artifact.ArtifactDesign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Commands.Artifactdesign
{
    public class UpdateChangingTablesArtifactCommand : IRequest<long>
    {
        public UpdateChangingTablesArtifactCommand(AllChangingTablesArtifact pData)
        {
            Data = pData;

        }
        #region Constructor

        public AllChangingTablesArtifact Data { get; }

        #endregion

    }
}
