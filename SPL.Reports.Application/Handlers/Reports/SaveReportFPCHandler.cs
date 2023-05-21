namespace SPL.Reports.Application.Handlers.Reports
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using SPL.Artifact.Application.Commands.Reports;
    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Domain.SPL.Tests;
    using SPL.Reports.Application.Common;

    public class SaveReportFPCHandler : IRequestHandler<SaveReportFPCCommand, ApiResponse<long>>
    {

        private readonly IReportsInfrastructure _infrastructure;

        public SaveReportFPCHandler(IReportsInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<long>> Handle(SaveReportFPCCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Data == null)
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Estructura incorrecta",
                        Structure = -1
                    };
                }

                foreach (Domain.SPL.Reports.FPC.FPCTests item in request.Data.Data)
                {
                    int position = request.Data.Data.IndexOf(item);
                    if (item.Date == null)
                    {
                        return new ApiResponse<long>()
                        {
                            Code = (int)ResponsesID.fallido,
                            Description = "El campo Fecha de la prueba es requerido, en la sección nro. "+ (position + 1),
                            Structure = -1
                        };
                    }
                    //if (item.Date > DateTime.Now)
                    //{
                    //    return new ApiResponse<long>()
                    //    {
                    //        Code = (int)ResponsesID.fallido,
                    //        Description = "El campo Fecha de la prueba no puede ser mayor a la fecha actual, en la sección nro. " + (position + 1),
                    //        Structure = -1
                    //    };
                    //}

                    //if (item.Date > DateTime.Now)
                    //{
                    //    return new ApiResponse<long>()
                    //    {
                    //        Code = (int)ResponsesID.fallido,
                    //        Description = "El campo Fecha de la prueba no puede ser mayor a la fecha actual, en la sección nro. " + (position + 1),
                    //        Structure = -1
                    //    };
                    //}
                }

                long result = await this._infrastructure.SaveInfoFPCReport(request.Data);

                return new ApiResponse<long>()
                {
                    Code = result == Enums.EnumsGen.Error ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result == Enums.EnumsGen.Error ? "Hubo un error al guardar los datos" : "Datos guardados exitosamente",
                    Structure = result
                };
            }
            catch (DbUpdateException ex)
            {
               
                return new ApiResponse<long>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = ex.Message,
                    Structure = Enums.EnumsGen.Error
                };
            }

        }
        #endregion
    }
}

