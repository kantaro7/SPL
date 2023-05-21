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
    public class GetBaseTemplateQuery : IRequest<ApiResponse<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate>>
    {
        public GetBaseTemplateQuery(string pTypeReport, string pKeyTest,string pKeyLanguage, int pNroColumns)
        {
            TypeReport = pTypeReport;
            KeyTest = pKeyTest;
            KeyLanguage = pKeyLanguage;
            NroColumns = pNroColumns;
        }

        #region Constructor

        public string TypeReport { get; }
        public string KeyTest { get; }
        public string KeyLanguage { get; }
        public int NroColumns { get; }
    

        #endregion

    }
}
