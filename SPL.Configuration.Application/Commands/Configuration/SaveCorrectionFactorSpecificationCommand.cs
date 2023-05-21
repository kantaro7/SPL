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
    public class SaveCorrectionFactorSpecificationCommand : IRequest<ApiResponse<long>>
    {
        public SaveCorrectionFactorSpecificationCommand(CorrectionFactorSpecification pData)
        {
            Data = pData;

        }
        #region Constructor

        public CorrectionFactorSpecification Data { get; }

        #endregion

    }
}
