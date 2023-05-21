﻿namespace SPL.Reports.Application.Handlers.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Reports.Application.Queries.Reports;

    public class GetConsolidatedReportHandler : IRequestHandler<GetConsolidatedReportQuery, ApiResponse<List<ConsolidatedReport>>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public GetConsolidatedReportHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<ConsolidatedReport>>> Handle(GetConsolidatedReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.NoSerie))
                {
                    return new ApiResponse<List<ConsolidatedReport>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El numero de serie es requerido",
                        Structure = null
                    };
                }

                if (string.IsNullOrEmpty(request.KeyLenguage))
                {
                    return new ApiResponse<List<ConsolidatedReport>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El idioma es requerido",
                        Structure = null
                    };
                }

                List<ConsolidatedReport> result = await this._infrastructure.GetConsolidatedReport(request.NoSerie, request.KeyLenguage);

                return new ApiResponse<List<ConsolidatedReport>>()
                {
                    Code = result == null ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result == null ? "No se encontraron resultados" : "",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ConsolidatedReport>>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = ex.Message,
                    Structure = null
                };
            }
        }
        #endregion
    }
}
