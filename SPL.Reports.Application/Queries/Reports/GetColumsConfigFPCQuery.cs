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
    public class GetColumsConfigFPCQuery : IRequest<ApiResponse<List<ColumnTitleFPCReports>>>
    {
        public GetColumsConfigFPCQuery(string pTypeUnit, string pLenguage)
        {
         
            this.TypeUnit = pTypeUnit;
    
            this.Lenguage = pLenguage;

        }
        #region Constructor

     
        public string TypeUnit { get; }
        public string Lenguage { get; }
        #endregion

    }
}
