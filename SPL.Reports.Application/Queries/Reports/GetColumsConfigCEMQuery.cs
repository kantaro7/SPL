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
    public class GetColumsConfigCEMQuery : IRequest<ApiResponse<List<ColumnTitleCEMReports>>>
    {
        public GetColumsConfigCEMQuery(decimal pTypeTrafoId, string pKeyLenguage, string pPos, string pPosSecundary , string pNoSerieNormal)
        {
         
            this.TypeTrafoId = pTypeTrafoId;
    
            this.KeyLenguage = pKeyLenguage;
            this.NoSerieNormal = pNoSerieNormal;
            this.PosSecundary = pPosSecundary;
            this.Pos = pPos;

        }
        #region Constructor

     
        public decimal TypeTrafoId { get; }
        public string KeyLenguage { get; }
        public string Pos { get; }
        public string PosSecundary { get; }
        public string NoSerieNormal { get; }
        #endregion

    }
}
