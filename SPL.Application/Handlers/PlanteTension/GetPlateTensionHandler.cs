using MediatR;

using SPL.Artifact.Application.Queries.PlateTension;
using SPL.Domain;
using SPL.Domain.SPL.Artifact.PlateTension;
using SPL.Domain.SPL.Artifact;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Handlers.PlateTension
{
    public class GetPlateTensionHandler : IRequestHandler<GetPlateTensionQuery, ApiResponse<List<Domain.SPL.Artifact.PlateTension.PlateTension>>>
    {

        private readonly IPlateTensionInfrastructure _infrastructure;

        public GetPlateTensionHandler(IPlateTensionInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<ApiResponse<List<Domain.SPL.Artifact.PlateTension.PlateTension>>> Handle(GetPlateTensionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string[] unit = { "" };
        
                 if (!string.IsNullOrEmpty(request.Unit))
                    unit = request.Unit.Split('-');

                IEnumerable<Domain.SPL.Artifact.PlateTension.PlateTension> result = await _infrastructure.getPlateTension(request.Unit, request.TypeVoltage);

                if (result.Count() <=0 )
                   result = await _infrastructure.getPlateTension(unit[0], request.TypeVoltage);


                return new ApiResponse<List<Domain.SPL.Artifact.PlateTension.PlateTension>>()
                {
                    Code = result.Count() <=  0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result.Count() <= 0 ? "No se encontraron resultados" : "",
                    Structure = result.ToList()
                };
            }
            catch (Exception ex)
            {

                return new ApiResponse<List<Domain.SPL.Artifact.PlateTension.PlateTension>>()
                {
                    Code = (int)(ResponsesID.fallido),
                    Description = ex.Message,
                    Structure = null
                };

            }

        }

        #endregion
    }
}
