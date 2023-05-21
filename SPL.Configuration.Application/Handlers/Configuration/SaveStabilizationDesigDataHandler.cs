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
  

    public class SaveStabilizationDesignDataHandler : IRequestHandler<SaveStabilizationDesignDataCommand, ApiResponse<long>>
    {

        private readonly IConfigurationInfrastructure _infrastructure;

        public SaveStabilizationDesignDataHandler(IConfigurationInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<long>> Handle(SaveStabilizationDesignDataCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //if (request.Data == null)
                //{

                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "Estructura incorrecta",
                //        Structure = -1
                //    };
                //}

                //if (request.Data.IdMarca <= 0)
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "Marca es requerido",
                //        Structure = -1
                //    };
                //}

                //if (!int.TryParse(request.Data.IdTipo.ToString(), out _))
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "Tipo no debe poseer decimales",
                //        Structure = -1
                //    };
                //}

                //if (Math.Round(request.Data.IdTipo) > 99999)
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "Tipo no puede ser mayor a 5 caracteres",
                //        Structure = -1
                //    };
                //}

                //if (string.IsNullOrEmpty(request.Data.Descripcion))
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "Descripción es requerido",
                //        Structure = -1
                //    };
                //}

                //if (request.Data.Descripcion.Length > 128)
                //{
                //    return new ApiResponse<long>()
                //    {
                //        Code = (int)ResponsesID.fallido,
                //        Description = "Descripción de la marca no puede excederse de 128 caracteres",
                //        Structure = -1
                //    };
                //}

                var result = await _infrastructure.saveStabilizationDesignData(request.Data);

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
