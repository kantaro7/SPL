using MediatR;
using SPL.Domain.SPL.Artifact.ArtifactDesign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Commands.Artifactdesign
{
    public class UpdateRulesArtifactCommand : IRequest<long>
    {
        public UpdateRulesArtifactCommand(List<RulesArtifact> pData)
        {
            Data = pData;

        }
        #region Constructor

        public List<RulesArtifact> Data { get; }

        #endregion

    }
}