namespace SPL.Reports.Application.Handlers.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Reports.Application.Queries.Reports;

    public class GetPDFReportsHandler : IRequestHandler<GetPDFReportQuery, ApiResponse<ReportPDF>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public GetPDFReportsHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<ReportPDF>> Handle(GetPDFReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.TypeReport))
                {
                    return new ApiResponse<ReportPDF>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El tipo de reporte es requerido",
                        Structure = null
                    };
                }

                if (request.TypeReport.Trim().Length > 3)
                {
                    return new ApiResponse<ReportPDF>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor tipo de reporte no puede ser mayor a 3 caracteres",
                        Structure = null
                    };
                }

                ReportPDF result = await this._infrastructure.GetPDFReport(request.Code,request.TypeReport);

                return new ApiResponse<ReportPDF>()
                {
                    Code =  (int)ResponsesID.exitoso,
                    Description = result == null ? "No se encontraron resultados" : "Se encontraron resultados",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ReportPDF>()
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
