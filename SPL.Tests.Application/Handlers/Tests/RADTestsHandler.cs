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

    public class RADTestsHandler : IRequestHandler<RADTestsCommand, ApiResponse<ResultRADTests>>
    {

        private readonly ITestsInfrastructure _infrastructure;

        public RADTestsHandler(ITestsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<ResultRADTests>> Handle(RADTestsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ResultRADTests result = new() { results = new List<ResultRADTestsDetails>() };

                result.results.Add(this.calculateTable(request.Data.Columns, request.Data.AcceptanceValue, request.Data.Times));

                if (request.Data.Columns2 != null)
                {
                    if (request.Data.Columns2.Count > 0)
                    {
                        result.results.Add(this.calculateTable(request.Data.Columns2, request.Data.AcceptanceValue, request.Data.Times));
                    }
                }

                return new ApiResponse<ResultRADTests>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = "Resultado exitoso",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultRADTests>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = ex.Message,
                    Structure = null
                };
            }
        }

        public ResultRADTestsDetails calculateTable(List<Column> Columns, decimal AcceptanceValue, List<string> ListTime)
        {
            ResultRADTestsDetails result = new();
            ErrorColumns errors;
            List<ErrorColumns> listErrors = new();
            List<decimal> listValuesAbs = new();
            List<decimal> listValuesPol = new();

            for (int i = 0; i < Columns.Count; i++)
            {
                for (int j = 1; j < Columns[i].Values.Count; j++)
                {
                    if (Columns[i].Values[j] < Columns[i].Values[j - 1])
                    {
                        errors = new ErrorColumns(i, j, "Error en valores de columna nro. " + i + " el valor en la posición " + j + "  es menor al enterior: " + Columns[i].Values[j - 1]);
                        listErrors.Add(errors);
                    }
                }

                if (Columns[i].Values[Columns[i].Values.Count() - 1] < AcceptanceValue)
                {
                    errors = new ErrorColumns(i, Columns[i].Values.Count(), "Error en valores de columna nro. " + i + " el último valor debe ser mayor al valor de aceptación");
                    listErrors.Add(errors);
                }

                if (Columns[i].Values[ListTime.IndexOf(Enums.RADTime.c_15sec)] == 0)
                {
                    errors = new ErrorColumns(0, 0, "Para el cálculo de índice de absorción se está dividiendo entre 0 en la fila 15 Sec.");
                    listErrors.Add(errors);
                }
                else
                {
                    listValuesAbs.Add(Columns[i].Values[ListTime.IndexOf(Enums.RADTime.c_60sec)] / Columns[i].Values[ListTime.IndexOf(Enums.RADTime.c_15sec)]);
                }

                if (Columns[i].Values[ListTime.IndexOf(Enums.RADTime.c_60sec)] == 0)
                {
                    errors = new ErrorColumns(0, 0, "Para el cálculo de índice de polarización se está dividiendo entre 0 en la fila 60 Sec. ");
                    listErrors.Add(errors);
                }
                else
                {
                    listValuesPol.Add(Columns[i].Values[ListTime.IndexOf(Enums.RADTime.c_10min)] / Columns[i].Values[ListTime.IndexOf(Enums.RADTime.c_60sec)]);
                }
            }

            result.AbsorptionIndexs = listValuesAbs;
            result.PolarizationIndexs = listValuesPol;
            result.MessageErrors = listErrors;
            return result;
        }
        #endregion
    }
}
