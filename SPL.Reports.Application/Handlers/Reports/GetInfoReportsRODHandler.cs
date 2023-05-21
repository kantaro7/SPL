namespace SPL.Reports.Application.Handlers.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Domain.SPL.Reports.ROD;
    using SPL.Reports.Application.Queries.Reports;

    public class GetInfoReportsRODHandler : IRequestHandler<GetInfoReportRODQuery, ApiResponse<RODTestsGeneral>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public GetInfoReportsRODHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<RODTestsGeneral>> Handle(GetInfoReportRODQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.KeyTest))
                {
                    return new ApiResponse<RODTestsGeneral>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Prueba es requerido",
                        Structure = null
                    };
                }

                RODTestsGeneral result = await this._infrastructure.GetInfoRODReport(request.NroSerie, request.KeyTest, request.TestConnection, request.WindingMaterial, request.UnitOfMeasurement, request.Result);

                return new ApiResponse<RODTestsGeneral>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = result == null ? "No se encontraron resultados" : "Se encontraron resultados",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<RODTestsGeneral>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = ex.Message,
                    Structure = null
                };
            }
        }
        #endregion
    }

    public class GetAHandler : IRequestHandler<GetAQuery, ApiResponse<IEnumerable<PCIParameters>>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public GetAHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods

        public async Task<ApiResponse<IEnumerable<PCIParameters>>> Handle(GetAQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<int> capacities = request.Capacity.Split(',').Select(int.Parse).ToList();

                IEnumerable<PCIParameters> testParameters = await this._infrastructure.AAsync(
                    request.NoSerie,
                    request.WindingMaterial,
                    capacities,
                    request.AtPositions,
                    request.BtPositions,
                    request.TerPositions,
                    request.IsAT,
                    request.IsBT,
                    request.IsTer);

                return new ApiResponse<IEnumerable<PCIParameters>>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = !testParameters.Any() ? "No se encontraron resultados" : "Se encontraron resultados",
                    Structure = testParameters
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<PCIParameters>>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = ex.Message,
                    Structure = Enumerable.Empty<PCIParameters>()
                };
            }
        }
        #endregion
    }
}
