namespace SPL.Reports.Application.Handlers.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Reports.Application.Common;
    using SPL.Reports.Application.Queries.Reports;

    public class GetGeneralTypesReportsHandler : IRequestHandler<GetGeneralTypesReportsQuery, ApiResponse<List<InfoGeneralTypesReports>>>
    {
        private readonly IReportsInfrastructure _infrastructure;

        public GetGeneralTypesReportsHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<InfoGeneralTypesReports>>> Handle(GetGeneralTypesReportsQuery request, CancellationToken cancellationToken)
        {

            try
            {
                if (string.IsNullOrEmpty(request.NroSerie))
                {
                    return new ApiResponse<List<InfoGeneralTypesReports>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El número de serie es requerido",
                        Structure = null
                    };
                }

                if (string.IsNullOrEmpty(request.TypeReport))
                {
                    return new ApiResponse<List<InfoGeneralTypesReports>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El tipo de reporte es requerido",
                        Structure = null
                    };
                }

                if (request.TypeReport.Trim().Length > 3)
                {
                    return new ApiResponse<List<InfoGeneralTypesReports>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El valor tipo de reporte no puede ser mayor a 3 caracteres",
                        Structure = null
                    };
                }

                bool valLongitud = Validations.validacion55Caracteres(request.NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(request.NroSerie.Trim());

                if (valLongitud)
                {
                    return new ApiResponse<List<InfoGeneralTypesReports>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El número de serie no puede excederse de 55 caracteres",
                        Structure = null
                    };
                }
                if (valFormat)
                {
                    return new ApiResponse<List<InfoGeneralTypesReports>>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El número de serie solo debe contener letras, números y guión medio",
                        Structure = null
                    };
                }

                List<InfoGeneralTypesReports> result = await this._infrastructure.GetResultDetailsReports(request.NroSerie, request.TypeReport);

                return new ApiResponse<List<InfoGeneralTypesReports>>()
                {
                    Code =  (int)ResponsesID.exitoso,
                    Description = result.Count <= 0 ? "No se encontraron resultados" : "Se encontraron resultados",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<InfoGeneralTypesReports>>()
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
