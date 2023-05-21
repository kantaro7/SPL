namespace SPL.Configuration.Application.Handlers.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Configuration.Application.Commands.Configuration;
    using SPL.Configuration.Application.Queries.Configuration;
    using SPL.Domain;
    using SPL.Domain.SPL.Configuration;
    using SPL.Domain.SPL.Masters;
  

    public class SaveCorrectionFactorSpecificationHandler : IRequestHandler<SaveCorrectionFactorSpecificationCommand, ApiResponse<long>>
    {

        private readonly IConfigurationInfrastructure _infrastructure;

        public SaveCorrectionFactorSpecificationHandler(IConfigurationInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<long>> Handle(SaveCorrectionFactorSpecificationCommand request, CancellationToken cancellationToken)
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

                if (string.IsNullOrEmpty(request.Data.Especificacion.Trim()))
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Especificación es requerido",
                        Structure = -1
                    };
                }

                if (request.Data.Temperatura < 0)
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Temperatura es requerido",
                        Structure = -1
                    };
                }

                if (!int.TryParse(request.Data.Temperatura.ToString(), out _))
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Temperatura no debe poseer decimales",
                        Structure = -1
                    };
                }

               

                if (Math.Round(request.Data.Temperatura) > 999)
                {
                    return new ApiResponse<long>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "Temperatura no puede ser mayor a 3 caracteres",
                        Structure = -1
                    };
                }

                //if (request.Data.FactorCorr <= 0)
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "Temperatura es requerido",
                //        Structure = -1
                //    };
                //}

                //if (!int.TryParse(request.Data.FactorCorr.ToString(), out _))
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "Temperatura no debe poseer decimales",
                //        Structure = -1
                //    };
                //}

                //if (Math.Round(request.Data.FactorCorr) > 999)
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "Temperatura no puede ser mayor a 3 caracteres",
                //        Structure = -1
                //    };
                //}

                var result = await _infrastructure.saveCorrectionFactorSpecification(request.Data);

                return new ApiResponse<long>()
                {
                    Code = result <= 0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result <= 0 ? "Hubo un error guardando los datos" : "Guardado con éxito",
                    Structure = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<long>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = ex.Message,
                    Structure = -1
                };
            }
        }
        #endregion
    }
}
