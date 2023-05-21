using MediatR;

using SPL.Domain;
using SPL.Domain.SPL.Artifact.ArtifactDesign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Commands.PlateTension
{
    public class SavePlateTensionCommand : IRequest<ApiResponse<long>>
    {
        public SavePlateTensionCommand(List<Domain.SPL.Artifact.PlateTension.PlateTension> pData, bool pStatusDelete)
        {
            Data = pData;
            StatusDelete = pStatusDelete;

        }
        #region Constructor

        public List<Domain.SPL.Artifact.PlateTension.PlateTension> Data { get; }
        public bool StatusDelete { get; }

        #endregion

    }
}
