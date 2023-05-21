namespace SPL.Tests.Application.Handlers.Tests
{
    using MediatR;

   
    using SPL.Domain;
    using SPL.Domain.SPL.Tests;
    using SPL.Tests.Application.Commands.Tests;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class RDTTestsHandler : IRequestHandler<RDTTestsCommand, ApiResponse<ResultRDTTestsDetails>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public RDTTestsHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<ResultRDTTestsDetails>> Handle(RDTTestsCommand request, CancellationToken cancellationToken)
        {
            try
            {

                List<decimal> nominalValues = new() { };

                List<decimal> desvicacionAValues = new() { };
                List<decimal> desvicacionBValues = new() { };
                List<decimal> desvicacionCValues = new() { };

                List<decimal> allTensions = new() { };
                List<decimal> unitTensions = new() { };

                ErrorColumns errors;
                List<ErrorColumns> listErrors = new();

                int count = request.Data.Tensions.Count;

                //Lista de todos
                for (int i = 0; i < count; i++)
                {
                    switch (request.Data.AllTension)
                    {
                        case "AT":
                            allTensions.Add(request.Data.AngularDisplacement.H_wye == 1
                               ? request.Data.Tensions[i] / Convert.ToDecimal(Math.Sqrt(3))
                               : request.Data.Tensions[i]);
                            break;
                        case "BT":
                            allTensions.Add(request.Data.AngularDisplacement.X_wye == 1
                                ? request.Data.Tensions[i] / Convert.ToDecimal(Math.Sqrt(3))
                                : request.Data.Tensions[i]);
                            break;
                        case "Ter":
                            allTensions.Add( request.Data.AngularDisplacement.T_wye == 1
                                ? request.Data.Tensions[i] / Convert.ToDecimal(Math.Sqrt(3))
                                : request.Data.Tensions[i]);
                            break;
                        default:
                            allTensions[i] = 0;
                            break;
                    }
                }
                //Lista de unico
                switch (request.Data.UnitTension)
                {
                    case "AT":
                        unitTensions.Add(request.Data.AngularDisplacement.H_wye == 1
                            ? request.Data.UnitValue / Convert.ToDecimal(Math.Sqrt(3))
                            : request.Data.UnitValue);
                        break;
                    case "BT":
                        unitTensions.Add( request.Data.AngularDisplacement.X_wye == 1
                            ? request.Data.UnitValue / Convert.ToDecimal(Math.Sqrt(3))
                            : request.Data.UnitValue);
                        break;
                    case "Ter":
                        unitTensions.Add( request.Data.AngularDisplacement.T_wye == 1
                            ? request.Data.UnitValue / Convert.ToDecimal(Math.Sqrt(3))
                            : request.Data.UnitValue);
                        break;
                    default:
                        unitTensions.Add(1);
                        break;
                }

                for (int i = 1; i < count; i++)
                {
                    unitTensions.Add(unitTensions[0]);
                }

                //Lista de valor nominal

                nominalValues = request.Data.AllTension.Equals("AT")
                    ? allTensions.Select(allTension => allTension / unitTensions.First()).ToList()
                    : request.Data.AllTension.Equals("BT")
                        ? request.Data.UnitTension.Equals("AT")
                                            ? allTensions.Select(allTension => unitTensions.First() / allTension).ToList()
                                            : allTensions.Select(allTension => allTension / unitTensions.First()).ToList()
                        : allTensions.Select(allTension => unitTensions.First() / allTension).ToList();

                //Lista desviacion A B y C
                for (int i = 0; i < count; i++)
                {
                    desvicacionAValues.Add(((request.Data.Columns[0].Values[i] / nominalValues[i]) - 1) * 100);
                    desvicacionBValues.Add(((request.Data.Columns[1].Values[i] / nominalValues[i]) - 1) * 100);
                    desvicacionCValues.Add(((request.Data.Columns[2].Values[i] / nominalValues[i]) - 1) * 100);
                }

                decimal dMaxColumna;

                dMaxColumna = CalculaMaxColumna(desvicacionAValues.ToArray());
                dMaxColumna = Math.Max(dMaxColumna, CalculaMaxColumna(desvicacionBValues.ToArray()));
                dMaxColumna = Math.Max(dMaxColumna, CalculaMaxColumna(desvicacionCValues.ToArray()));

                if (dMaxColumna > request.Data.AcceptanceValue)
                {
                    errors = new ErrorColumns(0, 0, "Para los valores del porcentaje de desviación de las tres columnas (Desv1, Desv2 y Desv3) el valor máximo obtenido es mayor al valor de aceptación");
                    listErrors.Add(errors);
                }

                ResultRDTTestsDetails result = new() {MessageErrors = listErrors, NominalValue = nominalValues, DeviationPhasesA = desvicacionAValues , DeviationPhasesB = desvicacionBValues , DeviationPhasesC = desvicacionCValues, HVVolts = allTensions, LVVolts = unitTensions };

   

                return new ApiResponse<ResultRDTTestsDetails>()
                {
                    Code =   (int)ResponsesID.exitoso,
                    Description = "Resultado exitoso",
                    Structure = result
                };

             
            }
            catch (Exception ex)
            {

                return new ApiResponse<ResultRDTTestsDetails>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = ex.Message,
                    Structure = null
                };

            }
        }

        public static decimal CalculaMaxColumna(decimal[] Columna)
        {
            decimal dMaxColumna = 0;
            if (Columna != null)
            {
                for (int i = 0; i < Columna.Length; i++)
                {
                    dMaxColumna = Math.Max(dMaxColumna, Columna[i]);
                }
            }
            return dMaxColumna;
        }
        #endregion
    }
}
