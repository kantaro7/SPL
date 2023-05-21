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

    public class CheckInfoRODHandler : IRequestHandler<CheckInfoRODQuery, ApiResponse<CheckInfoROD>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public CheckInfoRODHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<CheckInfoROD>> Handle(CheckInfoRODQuery request, CancellationToken cancellationToken)
        {
            try
            {

                CheckInfoROD result = await this._infrastructure.CheckInfoROD(request.NoSerie, request.WindingMaterial ,request.AtPositions,request.BtPositions,request.TerPositions,request.IsAT,request.IsBT,request.IsTer);

                if (result.Error == "ERROR")
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
