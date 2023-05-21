 namespace SPL.Tests.Application.Handlers.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests;
    using SPL.Domain.SPL.Tests.TAP;
    using SPL.Tests.Application.Commands.Tests;

    public class TAPTestsHandler : IRequestHandler<TAPTestsCommand, ApiResponse<ResultTAPTests>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public TAPTestsHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<ResultTAPTests>> Handle(TAPTestsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Data ==null)
                {
                    return new ApiResponse<ResultTAPTests>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Faltan datos para realizar el cálculo",
                        Structure = null
                    };
                }

                List<ErrorColumns> listErrors = new();
                ResultTAPTests result = new();
                TAPTests ResultTAPTests = new();
                ResultTAPTests = request.Data;

                for (int r = 0; r < request.Data.TAPTestsDetails.Count; r++)
                {

                    if (request.Data.TAPTestsDetails[r].Capacitance == 0)
                    {
                        return new ApiResponse<ResultTAPTests>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "Alguna capacitancia tiene como valor cero y provoca que de error por división entre cero",
                            Structure = null
                        };
                    }

                    decimal CalAmps =  60 * 2 * Convert.ToDecimal(Math.PI) * request.Data.Freacuency * request.Data.TAPTestsDetails[r].Capacitance * Convert.ToDecimal(Math.Pow(10,-9));
                    request.Data.TAPTestsDetails[r].AmpCal = Math.Round(CalAmps, 3);

                    decimal CurrentPercentage = Math.Abs((request.Data.TAPTestsDetails[r].CurrentkV / CalAmps) - 1);
        
                    request.Data.TAPTestsDetails[r].CurrentPercentage = Math.Round(CurrentPercentage, 3);

                    if (request.Data.TAPTestsDetails[r].CurrentPercentage > request.Data.ValueAcep)
                    {
                        listErrors.Add(new ErrorColumns(1, 1, "El valor de aceptación: "+ request.Data.ValueAcep + " es menor al porcentaje de corriente: "+ request.Data.TAPTestsDetails[r].CurrentPercentage + " en la fila nro. "+ (r+1)));
                    }
                }

                result.TAPTests = ResultTAPTests;
             
                result.Results = listErrors.ToList();

                return new ApiResponse<ResultTAPTests>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = "Resultado exitoso",
                    Structure = result
                 };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultTAPTests>()
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
