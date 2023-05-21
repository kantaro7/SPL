using MediatR;
using SPL.Domain;
using SPL.Domain.SPL.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Reports.Application.Queries.Reports
{

    public class CheckInfoRODQuery : IRequest<ApiResponse<CheckInfoROD>>
    {
        public CheckInfoRODQuery(string pNoSerie, string pWindingMaterial, string pAtPositions, string pBtPositions, string pTerPositions,
            bool pIsAT, bool pIsBT, bool pIsTer)
        {

            this.NoSerie = pNoSerie;
            this.WindingMaterial = pWindingMaterial;


            this.AtPositions = pAtPositions == "0" ? string.Empty : pAtPositions;
            this.BtPositions = pBtPositions == "0" ? string.Empty : pBtPositions;
            this.TerPositions = pTerPositions == "0" ? string.Empty : pTerPositions;
            this.IsAT = pIsAT;
            this.IsBT = pIsBT;
            this.IsTer = pIsTer;

        }
        #region Constructor


        public string NoSerie { get; }
        public string WindingMaterial { get; }
        public string AtPositions { get; }
        public string BtPositions { get; }
        public string TerPositions { get; }
        public bool IsBT { get; }
        public bool IsAT { get; }
        public bool IsTer { get; }
        #endregion

    }
}
