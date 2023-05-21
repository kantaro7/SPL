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
    using SPL.Domain.SPL.Tests.ROD;
    using SPL.Tests.Application.Commands.Tests;

    public class RODTestsHandler : IRequestHandler<RODTestsCommand, ApiResponse<ResultRODTests>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public RODTestsHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<ResultRODTests>> Handle(RODTestsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<ErrorColumns> listErrors = new();

                ResultRODTests result = new();

                List<RODTests> ResultRODTests = new();
                RODTests ObjRODTests = new();

                List<RODTestsDetails> ResultRODTestsDetails = new();

                ResultRODTests = request.Data.ToList();
                List<decimal> listPorc_Desv = new();
                List<decimal> listPorc_DesvxDesign = new();
                for (int i = 0; i < request.Data.Count; i++)
                {
                    listPorc_Desv = new();
                    listPorc_DesvxDesign = new();
                    decimal CorrectionFactor20 = request.Data[i].WindingMaterial.ToUpper().Equals("COBRE") ? (Convert.ToDecimal(234.5) + 20) / (Convert.ToDecimal(234.5) + request.Data[i].Temperature) : ((225 + 20) / (225 + request.Data[i].Temperature));
                    request.Data[i].FactorCor20 = CorrectionFactor20;

                    decimal CorrectionFactorTemp = request.Data[i].WindingMaterial.ToUpper().Equals("COBRE") ? (Convert.ToDecimal(234.5) + request.Data[i].BoostTemperature) / (Convert.ToDecimal(234.5) + request.Data[i].Temperature) : (225 + request.Data[i].BoostTemperature) / (225 + request.Data[i].Temperature);
                    request.Data[i].FactorCorSE = CorrectionFactorTemp;

                    ResultRODTestsDetails = new List<RODTestsDetails>();

                    for (int r = 0; r < request.Data[i].RODTestsDetails.Count; r++)
                    {

                        request.Data[i].RODTestsDetails[r].AverageResistance = (request.Data[i].RODTestsDetails[r].TerminalH1 + request.Data[i].RODTestsDetails[r].TerminalH2 + request.Data[i].RODTestsDetails[r].TerminalH3) / 3;

                        decimal Correccion_20 = request.Data[i].RODTestsDetails[r].AverageResistance * CorrectionFactor20;
                        request.Data[i].RODTestsDetails[r].PercentageA = Correccion_20;

                        decimal Correccion_SE = request.Data[i].RODTestsDetails[r].AverageResistance * CorrectionFactorTemp;
                        request.Data[i].RODTestsDetails[r].PercentageB = Correccion_SE;

                        List<decimal> Termiales = new()
                        {
                            request.Data[i].RODTestsDetails[r].TerminalH1,
                            request.Data[i].RODTestsDetails[r].TerminalH2,
                            request.Data[i].RODTestsDetails[r].TerminalH3
                        };

                        decimal minimoTerminal = Termiales.Min();
                        decimal maximoTerminal = Termiales.Max();

                        decimal PorcDesv = Math.Round( ((maximoTerminal / minimoTerminal) - 1) * 100,2);
                        request.Data[i].RODTestsDetails[r].Desv = PorcDesv;
                        listPorc_Desv.Add(PorcDesv);

                        List<decimal> Correccion20ResisD = new()
                        {
                            Correccion_20,
                            request.Data[i].RODTestsDetails[r].ResistDesigns.Resistencia
                        };

                        decimal minimoCorreccion20ResisD = Correccion20ResisD.Min();
                        decimal maximoCorreccion20ResisD = Correccion20ResisD.Max();
                        decimal PorcDesvDesign = ((maximoCorreccion20ResisD / minimoCorreccion20ResisD) - 1) * 100;
                        request.Data[i].RODTestsDetails[r].DesvDesign = PorcDesvDesign;
                        listPorc_DesvxDesign.Add(PorcDesvDesign);

                    }

                    decimal maxPorc_Desv = listPorc_Desv.Max();
                    request.Data[i].MaxPorc_Desv = Math.Round(maxPorc_Desv,2);
                    decimal maxPorc_DesvxDesign = listPorc_DesvxDesign.Max();
                    request.Data[i].MaxPorc_DesvxDesign = Math.Round(maxPorc_DesvxDesign,2);
                    decimal minPorc_DesvxDesign = listPorc_DesvxDesign.Min();
                    request.Data[i].MinPorc_DesvxDesign = Math.Round(minPorc_DesvxDesign,2);

                    if (maxPorc_Desv > request.Data[i].ValorAcepPhases)
                    {
                        listErrors.Add(new ErrorColumns(i, i, "El valor de Porcentaje de desviación máximo es mayor al valor de aceptacion para fases " + maxPorc_Desv + " mayor a " + request.Data[i].ValorAcepPhases));
                    }

                    if (maxPorc_DesvxDesign > request.Data[i].ValorAcMaDesign)
                    {
                        listErrors.Add(new ErrorColumns(i, i, "El valor de Porcentaje de desviación de diseño máximo es mayor al valor de aceptacion máximo de diseño " + maxPorc_DesvxDesign + " mayor a " + request.Data[i].ValorAcMaDesign));
                    }

                    if (minPorc_DesvxDesign < request.Data[i].AcMiDesignValue)
                    {
                        listErrors.Add(new ErrorColumns(i, i, "El valor de Porcentaje de desviación de diseño mínimo es menor al valor de aceptacion mínimo de diseño " + maxPorc_DesvxDesign + " menor que " + request.Data[i].AcMiDesignValue));
                    }
                }
                ResultRODTests = request.Data.ToList();
                result.RODTests = ResultRODTests;
                result.Results = listErrors.ToList();

                return new ApiResponse<ResultRODTests>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = "Resultado exitoso",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultRODTests>()
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
