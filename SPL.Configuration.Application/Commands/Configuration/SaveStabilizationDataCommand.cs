using MediatR;

using SPL.Domain;
using SPL.Domain.SPL.Artifact.ArtifactDesign;
using SPL.Domain.SPL.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Configuration.Application.Commands.Configuration
{
    public class SaveStabilizationDataCommand : IRequest<ApiResponse<long>>
    {
        public SaveStabilizationDataCommand(StabilizationData pData)
        {
            Data = pData;

        }
        #region Constructor

        public StabilizationData Data { get; }

        #endregion

    }
}
