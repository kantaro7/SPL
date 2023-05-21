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
    public class GetColumsConfigRADQuery : IRequest<ApiResponse<List<ColumnTitleRADReports>>>
    {
        public GetColumsConfigRADQuery(string pTypeUnit, string pLenguage, string pThirdWinding)
        {
         
            this.TypeUnit = pTypeUnit;
            this.ThirdWinding = pThirdWinding;
            this.Lenguage = pLenguage;

        }
        #region Constructor

     
        public string TypeUnit { get; }
        public string ThirdWinding { get; }
        public string Lenguage { get; }
        #endregion

    }
}
