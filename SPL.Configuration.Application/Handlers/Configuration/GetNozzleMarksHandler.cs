namespace SPL.Configuration.Application.Handlers.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Configuration.Application.Queries.Configuration;
    using SPL.Domain;
    using SPL.Domain.SPL.Configuration;
    using SPL.Domain.SPL.Masters;
  

    public class GetNozzleMarksHandler : IRequestHandler<GetNozzleMarksQuery, ApiResponse<List<NozzleMarks>>>
    {

        private readonly IConfigurationInfrastructure _infrastructure;

        public GetNozzleMarksHandler(IConfigurationInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<NozzleMarks>>> Handle(GetNozzleMarksQuery request, CancellationToken cancellationToken)
        {
            try
            {
              

                List<NozzleMarks> result = await this._infrastructure.GetNozzleMarks(request.IdMark,request.Status);

                return new ApiResponse<List<NozzleMarks>>()
                {
                    Code = result is null ? (int)ResponsesID.fallido : result.Count <= 0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result is null ? "No se encontraron resultados" : result.Count <= 0 ? "No se encontraron resultados" : "",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<NozzleMarks>>()
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
