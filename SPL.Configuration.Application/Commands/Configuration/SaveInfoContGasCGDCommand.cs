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
    public class SaveInfoContGasCGDCommand : IRequest<ApiResponse<long>>
    {
        public SaveInfoContGasCGDCommand(ContGasCGD pData)
        {
            Data = pData;

        }
        #region Constructor

        public ContGasCGD Data { get; }

        #endregion

    }
}
