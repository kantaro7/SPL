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
    using SPL.Reports.Application.Queries.Reports;

    public class CheckInfoPCEHandler : IRequestHandler<CheckInfoPCEQuery, ApiResponse<CheckInfoROD>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public CheckInfoPCEHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<CheckInfoROD>> Handle(CheckInfoPCEQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<int> capacities = request.Capacity.Split(',').Select(int.Parse).ToList();

                CheckInfoROD result = await this._infrastructure.CheckInfoPCE(request.NoSerie, capacities, request.AtPositions,request.BtPositions,request.TerPositions,request.IsAT,request.IsBT,request.IsTer);
                if(result is null)
                {
                    return new ApiResponse<CheckInfoROD>()
                    {
                        Code = -1,
                        Description = "Error en validacion de información de ROD y PCI",
                        Structure = null
                    };

                }

                if(result.Error == "ERROR")
                {
                    return new ApiResponse<CheckInfoROD>()
                    {
                        Code = -1,
                        Description = result.Message,
                        Structure = null
                    };
                }
                else
                {
                    return new ApiResponse<CheckInfoROD>()
                    {
                        Code = 1,
                        Description = "",
                        Structure = result
                    };
                }

                
            }
            catch (Exception ex)
            {
                return new ApiResponse<CheckInfoROD>()
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
