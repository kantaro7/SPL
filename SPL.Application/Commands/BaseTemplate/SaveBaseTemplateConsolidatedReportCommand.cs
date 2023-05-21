using MediatR;

using SPL.Domain;
using SPL.Domain.SPL.Artifact.ArtifactDesign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Commands.BaseTemplate
{
    public class SaveBaseTemplateConsolidatedReportCommand : IRequest<ApiResponse<long>>
    {
        public SaveBaseTemplateConsolidatedReportCommand(Domain.SPL.Artifact.BaseTemplate.BaseTemplateConsolidatedReport pData)
        {
            Data = pData;
        }

        #region Constructor

        public Domain.SPL.Artifact.BaseTemplate.BaseTemplateConsolidatedReport Data { get; }
    

        #endregion

    }
}
