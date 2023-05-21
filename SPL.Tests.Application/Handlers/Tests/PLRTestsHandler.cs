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
    using SPL.Domain.SPL.Tests.PLR;
    using SPL.Tests.Application.Commands.Tests;

    public class PLRTestsHandler : IRequestHandler<PLRTestsCommand, ApiResponse<ResultPLRTests>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public PLRTestsHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<ResultPLRTests>> Handle(PLRTestsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ResultPLRTests result = new();

                PLRTests ObjTests = new();

                List<PLRTestsDetails> ResultPCITestsDetails = new();
                List<decimal> datosDesv = new List<decimal>();
                List<decimal> datosTen = new List<decimal>();

                if (request.Data.KeyTest.ToUpper().Equals("RAC"))
                {

                    List<ErrorColumns> listErrors = new();

               
              
                ObjTests = request.Data;

                if (request.Data == null)
                {
                    return new ApiResponse<ResultPLRTests>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Faltan datos para realizar el cálculo",
                        Structure = null
                    };
                }

                for (int details = 0; details < request.Data.PLRTestsDetails.Count; details++)
                {
                   
                        request.Data.PLRTestsDetails[details].Reactance = Math.Round((request.Data.PLRTestsDetails[details].Tension * 1000) / request.Data.PLRTestsDetails[details].Current, 3);

                         decimal PorcentajeDesviación = Math.Round( (request.Data.PLRTestsDetails[details].Reactance / request.Data.Rldnt) - 1,4);
                        request.Data.PLRTestsDetails[details].PorcD = PorcentajeDesviación;
                          datosDesv.Add(PorcentajeDesviación);
                          datosTen.Add(Math.Abs(request.Data.NominalVoltage - request.Data.PLRTestsDetails[details].Tension));
                    }

             
           
                decimal min = datosTen.Min();
                int minPos = datosTen.IndexOf(min);

                request.Data.PorcDeviationNV = datosDesv[minPos] * 100;
                result.PLRTests = request.Data;
                result.Results = listErrors.ToList();
                }
                else {
                    return new ApiResponse<ResultPLRTests>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "La prueba para tiempo no tiene aplica validación de datos",
                        Structure = null
                    };
                }
                return new ApiResponse<ResultPLRTests>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = "Resultado exitoso",
                    Structure = result
                };
            }
            catch (Exception ex)
             {
            return new ApiResponse<ResultPLRTests>()
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
