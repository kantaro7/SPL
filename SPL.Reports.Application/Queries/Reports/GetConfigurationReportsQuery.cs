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
    public class GetConfigurationReportsQuery : IRequest<ApiResponse<List<ConfigurationReports>>>
    {
        public GetConfigurationReportsQuery(string pTypeReport, string pKeyTest, int pNumberColumns)
        {
            
            this.TypeReport = pTypeReport;
            this.KeyTest = pKeyTest;
            this.NumberColumns = pNumberColumns;

        }
        #region Constructor

        public string KeyTest { get; }
        public int NumberColumns { get; }
        public string TypeReport { get; }
        #endregion
    }
}