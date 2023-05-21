using MediatR;

using SPL.Domain;
using SPL.Domain.SPL.Artifact.ArtifactDesign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Commands.Artifactdesign
{
    public class SaveResistDesignCommand : IRequest<ApiResponse<long>>
    {
        public SaveResistDesignCommand(List<ResistDesign> pData)
        {
            Data = pData;

        }
        #region Constructor

        public List<ResistDesign> Data { get; }

        #endregion

    }
}
