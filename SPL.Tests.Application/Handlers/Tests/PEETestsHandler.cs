namespace SPL.Tests.Application.Handlers.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports.PCE;
    using SPL.Domain.SPL.Tests;
    using SPL.Domain.SPL.Tests.FPB;
    using SPL.Domain.SPL.Tests.FPC;

    using SPL.Domain.SPL.Tests.PCI;
    using SPL.Domain.SPL.Tests.PEE;
    using SPL.Domain.SPL.Tests.PLR;
    using SPL.Tests.Application.Commands.Tests;

    public class PEETestsHandler : IRequestHandler<PEETestsCommand, ApiResponse<ResultPEETests>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public PEETestsHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<ResultPEETests>> Handle(PEETestsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Data == null)
                {
                    return new ApiResponse<ResultPEETests>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Faltan datos para realizar el cálculo",
                        Structure = null
                    };
                }

                ResultPEETests result = new() { PEETests = request.Data, Results = new()};

                foreach (PEETestsDetails item in request.Data.PEETestsDetails)
                {
                    if (item.PowerKW > item.Kwaux_gar)
                    {
                        result.Results.Add(new ErrorColumns( 3, request.Data.PEETestsDetails .IndexOf(item)+1, $"La Potencia en {item.CoolingType} es mayor al valor de la garantía de potencia ({item.Kwaux_gar})" ));
                    }
                }
                
                return new ApiResponse<ResultPEETests>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = "Resultado exitoso",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultPEETests>()
                {
                    Code = (int)ResponsesID.exception,
                    Description = ex.Message,
                    Structure = null
                };
            }

          
        }
      
        #endregion
    }
}
