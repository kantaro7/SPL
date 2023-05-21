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
    public class SaveStabilizationDesignDataCommand : IRequest<ApiResponse<long>>
    {
        public SaveStabilizationDesignDataCommand(StabilizationDesignData pData)
        {
            Data = pData;

        }
        #region Constructor

        public StabilizationDesignData Data { get; }

        #endregion

    }
}
