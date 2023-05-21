using MediatR;

using SPL.Domain;
using SPL.Domain.SPL.Artifact.ArtifactDesign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Queries.BaseTemplate
{
    public class GetBaseTemplateConsolidatedReportQuery : IRequest<ApiResponse<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplateConsolidatedReport>>
    {
        public GetBaseTemplateConsolidatedReportQuery(string pKeyLanguage)
        {
            KeyLanguage = pKeyLanguage;
        }

        #region Constructor

        public string KeyLanguage { get; }

        #endregion

    }
}
