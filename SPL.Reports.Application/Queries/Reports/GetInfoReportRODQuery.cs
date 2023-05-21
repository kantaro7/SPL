using MediatR;

using SPL.Domain;
using SPL.Domain.SPL.Reports;
using SPL.Domain.SPL.Reports.ROD;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Reports.Application.Queries.Reports

{
    public class GetInfoReportRODQuery : IRequest<ApiResponse<RODTestsGeneral>>
    {
        public GetInfoReportRODQuery(string pNroSerie, string pKeyTest, string pTestConnection, string pWindingMaterial, string pUnitOfMeasurement, bool pResult)
        {

            this.NroSerie = pNroSerie;
            this.KeyTest = pKeyTest;
            this.TestConnection = pTestConnection;
            this.WindingMaterial = pWindingMaterial;
            this.UnitOfMeasurement = pUnitOfMeasurement;
            this.Result = pResult;

        }
        #region Constructor


        public string NroSerie { get; }
        public string KeyTest { get; }
        public string TestConnection { get; }
        public string WindingMaterial { get; }
        public string UnitOfMeasurement { get; }
        public bool Result { get; }
        #endregion

    }

    public class GetAQuery : IRequest<ApiResponse<IEnumerable<PCIParameters>>>
    {
        #region Constructor

        public GetAQuery(
            string pNoSerie,
            string pWindingMaterial,
            string pCapacity,
            string pAtPositions,
            string pBtPositions,
            string pTerPositions,
            bool pIsAT,
            bool pIsBT,
            bool pIsTer)
        {
            this.NoSerie = pNoSerie;
            this.WindingMaterial = pWindingMaterial;
            this.Capacity = pCapacity;
            this.AtPositions = pAtPositions == "0" ? string.Empty : pAtPositions;
            this.BtPositions = pBtPositions == "0" ? string.Empty : pBtPositions;
            this.TerPositions = pTerPositions == "0" ? string.Empty : pTerPositions;
            this.IsAT = pIsAT;
            this.IsBT = pIsBT;
            this.IsTer = pIsTer;
        }

        #endregion

        #region Properties

        public string NoSerie { get; }
        
        public string WindingMaterial { get; }

        public string Capacity { get; }

        public string AtPositions { get; }

        public string BtPositions { get; }

        public string TerPositions { get; }
        
        public bool IsBT { get; }
        
        public bool IsAT { get; }
        
        public bool IsTer { get; }

        #endregion
    }
}