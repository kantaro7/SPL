using MediatR;

using SPL.Domain;
using SPL.Domain.SPL.Reports;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Reports.Application.Queries.Reports

{
    public class GetColumsConfigRDTQuery : IRequest<ApiResponse<List<ColumnTitleRDTReports>>>
    {
        public GetColumsConfigRDTQuery(string pKeyTest, string pDAngular, string pRule, string pLenguage)
        {
          
            this.KeyTests = pKeyTest;
            this.DAngular = pDAngular;
            this.Rule = pRule;
            this.Lenguage = pLenguage;

        }
        #region Constructor

        public string KeyTests { get; }
        public string DAngular { get; }
        public string Rule { get; }
        public string Lenguage { get; }
        #endregion

    }
}
