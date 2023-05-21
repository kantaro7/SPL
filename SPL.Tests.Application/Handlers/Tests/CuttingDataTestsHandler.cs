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
    using SPL.Tests.Application.Commands.Tests;

    public class CuttingDataTestsHandler : IRequestHandler<CuttingDataCommand, ApiResponse<ResultCuttingDataTests>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public CuttingDataTestsHandler(ITestsInfrastructure infrastructure) => _infrastructure = infrastructure;

        #region Methods
        public Task<ApiResponse<ResultCuttingDataTests>> Handle(CuttingDataCommand request, CancellationToken cancellationToken)
        {
            List<ErrorColumns> listErrors = new();

            ResultCuttingDataTests result = new();
            //return null;
            try
            {
                if (request.Data == null)
                {
                    return Task.FromResult(new ApiResponse<ResultCuttingDataTests>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Faltan datos para realizar el cálculo",
                        Structure = null
                    });
                }

                int LimitStabilización = 5;
                decimal restTempPromAceite, sumTempPromAceite, resTempPromAlímit, sumTempPromAlímit, sumX, sumY, sumX2, sumX3, sumX4, sumX2Y, sumY2, sumXY, sumYP, sumXYP, sumYP2;
                restTempPromAceite = sumTempPromAceite = resTempPromAlímit = sumTempPromAlímit = sumX = sumY = sumX2 = sumX3 = sumX4 = sumX2Y = sumY2 = sumXY = sumYP = sumXYP = sumYP2 = 0;
                List<MatrixCuttingData> matrixCuttingDatas = new();
                List<MatrixReExpCuttingData> matrixReExpCuttingDatas = new();

                for (int r = 0; r < request.Data.SectionCuttingData.Count; r++)
                {
                    int pos = request.Data.SectionCuttingData.IndexOf(request.Data.SectionCuttingData[r]) + 1;
                    for (int j = 0; j < request.Data.SectionCuttingData[r].DetailCuttingData.Count; j++)
                    {
                        request.Data.SectionCuttingData[r].DetailCuttingData[j].TempR = (request.Data.SectionCuttingData[r].DetailCuttingData[j].Resistencia * ((request.Data.Constante + request.Data.SectionCuttingData[r].TempResistencia) / request.Data.SectionCuttingData[r].Resistencia)) - request.Data.Constante;

                        decimal x = ((Math.Round(request.Data.SectionCuttingData[r].DetailCuttingData[j].Tiempo, 0) * 60) + (Math.Abs(Math.Round(request.Data.SectionCuttingData[r].DetailCuttingData[j].Tiempo, 0) - request.Data.SectionCuttingData[r].DetailCuttingData[j].Tiempo) * 100)) / 10;

                        decimal y = request.Data.SectionCuttingData[r].DetailCuttingData[j].Resistencia;
                        decimal x2 = Convert.ToDecimal(Math.Pow(Convert.ToDouble(x), 2));
                        matrixCuttingDatas.Add(new MatrixCuttingData()
                        {
                            Table = pos,
                            X = x,
                            Time = request.Data.SectionCuttingData[r].DetailCuttingData[j].Tiempo,
                            Y = y,
                            X2 = x2,
                            X3 = Convert.ToDecimal(Math.Pow(Convert.ToDouble(x), 3)),
                            X4 = Convert.ToDecimal(Math.Pow(Convert.ToDouble(x), 4)),
                            XY = x * y,
                            X2Y = x2 * y,
                            Y2 = Convert.ToDecimal(Math.Pow(Convert.ToDouble(y), 2)),
                        });
                        decimal y2 = request.Data.SectionCuttingData[r].DetailCuttingData[j].TempR - request.Data.SectionCuttingData[r].TempAceiteProm;
                        decimal yp = Convert.ToDecimal(Math.Log(Convert.ToDouble(y2)));
                        matrixReExpCuttingDatas.Add(new MatrixReExpCuttingData()
                        {
                            Time = request.Data.SectionCuttingData[r].DetailCuttingData[j].Tiempo,
                            Table = pos,
                            X = x,
                            Y = y2,
                            YP = yp,
                            X2 = Convert.ToDecimal(Math.Pow(Convert.ToDouble(x), 2)),
                            YP2 = x * yp,
                            XYP = Convert.ToDecimal(Math.Pow(Convert.ToDouble(yp), 2)),
                        }); ;
                    }

                    sumX = matrixCuttingDatas.Where(x => x.Table == pos).Sum(x => x.X);
                    sumY = matrixCuttingDatas.Where(x => x.Table == pos).Sum(x => x.Y);
                    sumX2 = matrixCuttingDatas.Where(x => x.Table == pos).Sum(x => x.X2);
                    sumX3 = matrixCuttingDatas.Where(x => x.Table == pos).Sum(x => x.X3);
                    sumX4 = matrixCuttingDatas.Where(x => x.Table == pos).Sum(x => x.X4);
                    sumXY = matrixCuttingDatas.Where(x => x.Table == pos).Sum(x => x.XY);
                    sumX2Y = matrixCuttingDatas.Where(x => x.Table == pos).Sum(x => x.X2Y);
                    sumY2 = matrixCuttingDatas.Where(x => x.Table == pos).Sum(x => x.Y2);
                    sumYP = matrixReExpCuttingDatas.Where(x => x.Table == pos).Sum(x => x.YP);
                    sumXY = matrixReExpCuttingDatas.Where(x => x.Table == pos).Sum(x => x.XYP);
                    sumYP2 = matrixReExpCuttingDatas.Where(x => x.Table == pos).Sum(x => x.YP2);
                    decimal b = 0;
                    decimal c = 0;
                    decimal a = 0;
                    decimal r2 = 0;

                    int n = request.Data.SectionCuttingData[r].DetailCuttingData.Count();

                    decimal b1 = sumXY - (sumX * sumY / n);
                    decimal b2 = sumX4 - Convert.ToDecimal(Math.Pow(Convert.ToDouble(sumX2), 2) / n);
                    decimal b3 = sumX2Y - (sumX2 * sumY / n);
                    decimal b4 = sumX3 - (sumX2 * sumX / n);
                    decimal b5 = sumX2 - Convert.ToDecimal(Math.Pow(Convert.ToDouble(sumX), 2) / n);
                    decimal b6 = sumX4 - Convert.ToDecimal(Math.Pow(Convert.ToDouble(sumX2), 2) / n);
                    decimal b7 = Convert.ToDecimal(Math.Pow(Convert.ToDouble(sumX3 - (sumX2 * sumX / n)), 2));

                    b = ((b1 * b2) - (b3 * b4)) / ((b5 * b6) - b7);

                    decimal c1 = sumX2 - (Convert.ToDecimal(Math.Pow(Convert.ToDouble(sumX), 2)) / n);
                    decimal c2 = sumX2Y - (sumX2 * sumY / n);
                    decimal c3 = sumX3 - (sumX2 * sumX / n);
                    decimal c4 = sumXY - (sumX * sumY / n);
                    decimal c5 = sumX2 - Convert.ToDecimal(Math.Pow(Convert.ToDouble(sumX), 2) / n);
                    decimal c6 = sumX4 - Convert.ToDecimal(Math.Pow(Convert.ToDouble(sumX2), 2) / n);
                    decimal c7 = Convert.ToDecimal(Math.Pow(Convert.ToDouble(sumX3 - (sumX2 * sumX / n)), 2));

                    c = ((c1 * c2) - (c3 * c4)) / ((c5 * c6) - c7);

                    a = (sumY - (b * sumX) - (c * sumX2)) / n;

                    decimal r21 = b * (sumXY - (sumX * sumY / n));
                    decimal r22 = c * (sumX2Y - (sumX2 * sumY / n));
                    decimal r23 = sumY2 - Convert.ToDecimal(Math.Pow(Convert.ToDouble(sumY), 2) / n);

                    r2 = (r21 + r22) / r23;

                    decimal ResistenciaTiempoCero = Math.Round(a, 5);

                    decimal TemperaturaDevanado = Math.Round((ResistenciaTiempoCero * ((Convert.ToDecimal(234.5) + request.Data.SectionCuttingData[r].TempResistencia) / request.Data.SectionCuttingData[r].Resistencia)) - Convert.ToDecimal(234.5), 1);

                    decimal GradienteCobreAceite = Math.Round(TemperaturaDevanado - request.Data.SectionCuttingData[r].TempAceiteProm, 1);

                    decimal AWR = GradienteCobreAceite + request.Data.AOR;

                    decimal HSR = (GradienteCobreAceite * request.Data.SectionCuttingData[r].FactorK) + request.Data.AOR;

                    decimal HST = HSR + request.Data.AmbProm;

                    decimal d = ((n * sumXYP) - (sumX * sumYP)) / ((n * sumX2) - Convert.ToDecimal(Math.Pow(Convert.ToDouble(sumX), 2)));

                    decimal ap = (sumYP / matrixCuttingDatas.Where(x => x.Table == pos).Count()) - (d * (sumX / matrixCuttingDatas.Where(x => x.Table == pos).Count()));

                    decimal A = Convert.ToDecimal(Math.Exp(Convert.ToDouble(ap)));

                    decimal WindT = A + request.Data.SectionCuttingData[r].TempAceiteProm;

                    decimal ResistenciaTiempoCero2 = Math.Round(request.Data.SectionCuttingData[r].Resistencia * ((Convert.ToDecimal(234.5) + WindT) / (Convert.ToDecimal(234.5) + request.Data.SectionCuttingData[r].TempResistencia)), 5);

                    decimal TemperaturaDevanado2 = Math.Round((ResistenciaTiempoCero2 * ((Convert.ToDecimal(234.5) + request.Data.SectionCuttingData[r].TempResistencia) / request.Data.SectionCuttingData[r].Resistencia)) - Convert.ToDecimal(234.5), 1);

                    decimal GradienteCobreAceite2 = Math.Round(TemperaturaDevanado - request.Data.SectionCuttingData[r].TempAceiteProm, 1);

                    decimal AWR2 = GradienteCobreAceite + request.Data.AOR;

                    decimal HSR2 = (GradienteCobreAceite * request.Data.SectionCuttingData[r].FactorK) + request.Data.TOR
                      ;

                    decimal HST2 = HSR + request.Data.AmbProm;

                    //***Regresar datos

                    request.Data.SectionCuttingData[r].LimiteEst = LimitStabilización;
                    request.Data.SectionCuttingData[r].CapturaEn = (pos == 1) ? "AT" : (pos == 2) ? "BT" : "TER";
                    request.Data.SectionCuttingData[r].ResistZeroC = ResistenciaTiempoCero;
                    request.Data.SectionCuttingData[r].TempDevC = TemperaturaDevanado;
                    request.Data.SectionCuttingData[r].GradienteCaC = GradienteCobreAceite;

                    request.Data.SectionCuttingData[r].AwrC = AWR;
                    request.Data.SectionCuttingData[r].HsrC = HSR;
                    request.Data.SectionCuttingData[r].HstC = HST;
                    request.Data.SectionCuttingData[r].WindT = WindT;

                    request.Data.SectionCuttingData[r].ResistZeroE = ResistenciaTiempoCero2;
                    request.Data.SectionCuttingData[r].TempDevE = TemperaturaDevanado2;
                    request.Data.SectionCuttingData[r].GradienteCaE = GradienteCobreAceite2;
                    request.Data.SectionCuttingData[r].AwrE = AWR2;
                    request.Data.SectionCuttingData[r].HsrE = HSR2;
                    request.Data.SectionCuttingData[r].HstE = HST2;

                }

                sumTempPromAlímit = (request.Data.SectionCuttingData.Sum(x => x.TempAceiteProm) / request.Data.SectionCuttingData.Count()) + LimitStabilización;

                resTempPromAlímit = (request.Data.SectionCuttingData.Sum(x => x.TempAceiteProm) / request.Data.SectionCuttingData.Count()) - LimitStabilización;

                for (int r = 0; r < request.Data.SectionCuttingData.Count; r++)
                {
                    int pos = request.Data.SectionCuttingData.IndexOf(request.Data.SectionCuttingData[r]);
                    decimal lastValor = 0;
                    sumTempPromAceite = request.Data.SectionCuttingData[r].TempAceiteProm + LimitStabilización;
                    restTempPromAceite = request.Data.SectionCuttingData[r].TempAceiteProm - LimitStabilización;

                    lastValor = request.Data.SectionCuttingData[r].DetailCuttingData.Last().TempR;

                    if (lastValor < sumTempPromAceite && lastValor > restTempPromAceite)
                    {
                        listErrors.Add(new ErrorColumns(pos, request.Data.SectionCuttingData[r].DetailCuttingData.Count(), "Alarma. Detección de la alarma de estabilización"));
                    }
                }

                result.MessageErrors = listErrors;
                result.Data = request.Data;

                return Task.FromResult(new ApiResponse<ResultCuttingDataTests>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = "Resultado exitoso",
                    Structure = result
                });

            }
            catch (Exception ex)
            {
                return Task.FromResult(new ApiResponse<ResultCuttingDataTests>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = ex.Message,
                    Structure = null
                });
            }
        }

        #endregion

    }
}

