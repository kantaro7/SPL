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
    using SPL.Domain.SPL.Tests.CGD;
    using SPL.Domain.SPL.Tests.FPB;
    using SPL.Domain.SPL.Tests.FPC;

    using SPL.Domain.SPL.Tests.PCI;
    using SPL.Domain.SPL.Tests.PLR;
    using SPL.Domain.SPL.Tests.RDD;
    using SPL.Tests.Application.Commands.Tests;

    public class RDDTestsHandler : IRequestHandler<RDDTestsCommand, ApiResponse<ResultRDDTests>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public RDDTestsHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<ResultRDDTests>> Handle(RDDTestsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ResultRDDTests result = new();

         

                List<RDDTestsDetails> ResultCGDTestsDetails = new();
                List<decimal> datosDesv = new();
                List<decimal> datosTen = new();

                ErrorColumns errors;
                List<ErrorColumns> listErrors = new();

        

                if (request.Data == null)
                {
                    return new ApiResponse<ResultRDDTests>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Faltan datos para realizar el cálculo",
                        Structure = null
                    };
                }
   
                for (int s = 0; s < request.Data.OutRDDTests.Count; s++)
                {
                    int pos = request.Data.OutRDDTests.IndexOf(request.Data.OutRDDTests[s]) + 1;

                    for (int details = 0; details < request.Data.OutRDDTests[s].RDDTestsDetails.Count; details++)
                    {
                        int posj = request.Data.OutRDDTests[s].RDDTestsDetails.IndexOf(request.Data.OutRDDTests[s].RDDTestsDetails[details]) + 1;

                        request.Data.OutRDDTests[s].RDDTestsDetails[details].Resistance = Math.Round(request.Data.OutRDDTests[s].RDDTestsDetails[details].LossesW / (request.Data.OutRDDTests[s].RDDTestsDetails[details].CurrentA * request.Data.OutRDDTests[s].RDDTestsDetails[details].CurrentA),3);

                        request.Data.OutRDDTests[s].RDDTestsDetails[details].Impedance = Math.Round(request.Data.OutRDDTests[s].RDDTestsDetails[details].AppliedVoltage / request.Data.OutRDDTests[s].RDDTestsDetails[details].CurrentA, 3);

                        request.Data.OutRDDTests[s].RDDTestsDetails[details].Reactance = Math.Round( Convert.ToDecimal( Math.Sqrt(  Math.Pow(Convert.ToDouble(request.Data.OutRDDTests[s].RDDTestsDetails[details].Impedance),  2) - Math.Pow(Convert.ToDouble(request.Data.OutRDDTests[s].RDDTestsDetails[details].Resistance), 2))),3);

                        request.Data.OutRDDTests[s].RDDTestsDetails[details].S3fV2 = Math.Round(Convert.ToDecimal(request.Data.Capacity * 1000)   / Convert.ToDecimal (Math.Pow(Convert.ToDouble(request.Data.VoltageEW), 2)),6);
                       
                        if (pos == 2)
                        {
                            decimal valor = request.Data.ConfigWinding.ToUpper().Equals("Delta - Estrella") ? decimal.Divide(1, 30) : decimal.Divide(1, 10);
                            request.Data.OutRDDTests[s].RDDTestsDetails[details].PorcX = Math.Round(valor * request.Data.OutRDDTests[s].RDDTestsDetails[details].Reactance * request.Data.OutRDDTests[s].RDDTestsDetails[details].S3fV2, 6);
                        }
                    }
                    if (pos == 1)
                    {
                        for (int y = 0; y < request.Data.OutRDDTests[s].RDDTestsDetails.Count; y++)
                        {
                            request.Data.OutRDDTests[s].RDDTestsDetails[y].PorcX = Math.Round(decimal.Divide(1,60) * request.Data.OutRDDTests[s].RDDTestsDetails.Sum(x => x.Reactance) * request.Data.OutRDDTests[s].RDDTestsDetails[y].S3fV2, 6);
                        }
                    }
                }

                decimal TporX = 1 - (request.Data.PorcJx / request.Data.OutRDDTests[0].RDDTestsDetails.FirstOrDefault().PorcX);
                request.Data.TporX = TporX;

                if (TporX > request.Data.ValueAcep)
                {
                    errors = new ErrorColumns(0, 0, "Error, el cálculo de %X ---> 1 - (%jX de los filtros / %X de la primera sección) :  " + TporX+ " es mayor al valor de aceptación "+ request.Data.ValueAcep);
                    listErrors.Add(errors);
                }

                result.RDDTestsGeneral = request.Data;
                result.Results = listErrors.ToList();
          
                return new ApiResponse<ResultRDDTests>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = "Resultado exitoso",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultRDDTests>()
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
