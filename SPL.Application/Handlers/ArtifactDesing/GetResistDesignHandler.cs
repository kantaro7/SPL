using MediatR;

using SPL.Artifact.Application.Common;
using SPL.Artifact.Application.Queries.Artifactdesign;
using SPL.Domain;
using SPL.Domain.SPL.Artifact.ArtifactDesign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Handlers.Artifactdesign
{
    public class GetResistDesignHandler : IRequestHandler<GetResistDesignQuery, ApiResponse<List<ResistDesign>>>
    {

        private readonly IArtifactdesignInfrastructure _infrastructure;

        public GetResistDesignHandler(IArtifactdesignInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<ApiResponse<List<ResistDesign>>> Handle(GetResistDesignQuery request, CancellationToken cancellationToken)
        {

            string[] aparato = request.NroSerie.Split('-');

            if (string.IsNullOrEmpty(aparato[0].Trim()))
            {

                return new ApiResponse<List<ResistDesign>>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = "El número de serie es requerido",
                    Structure = null
                };
            }

            bool valLongitud = Validations.validacion55Caracteres(aparato[0].Trim());
            bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(aparato[0].Trim());

            if (valLongitud)
            {
                return new ApiResponse<List<ResistDesign>>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = "El número de serie no puede excederse de 55 caracteres",
                    Structure = null
                };
            }
            if (valFormat)
            {

                return new ApiResponse<List<ResistDesign>>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = "El número de serie solo debe contener letras, números y guion medio",
                    Structure = null
                };
            }

            if (string.IsNullOrEmpty(request.UnitOfMeasurement.Trim()))
            {

                return new ApiResponse<List<ResistDesign>>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = "Unidad de medida es requerido",
                    Structure = null
                };
            }

            if (string.IsNullOrEmpty(request.TestConnection.Trim()))
            {

                return new ApiResponse<List<ResistDesign>>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = "Conexión de Prueba es requerido",
                    Structure = null
                };
            }

            if (request.Temperature <= 0)
            {

                return new ApiResponse<List<ResistDesign>>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = "Temperatura es requerido",
                    Structure = null
                };
            }

            int[] result = CommonMethods.cantDigitsPoint(Convert.ToDouble(request.Temperature)); 

            if (result[0] > 3 || result[1] > 1)
            {

                return new ApiResponse<List<ResistDesign>>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = "La temperatura debe ser mayor a cero considerando 3 enteros con 1 decimal",
                    Structure = null
                };
            }

            var infoArtifact = await _infrastructure.GetGeneralArtifactdesign(aparato[0]);
            
            if (infoArtifact.GeneralArtifact == null)
            {
                return new ApiResponse<List<ResistDesign>>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = "El aparato no cuenta con información general",
                    Structure = null
                };
            }
            if (infoArtifact.ChangingTablesArtifact.Count == 0)
            {
                return new ApiResponse<List<ResistDesign>>()
                {
                    Code = (int)ResponsesID.fallido,
                    Description = "El aparato no cuenta con información de cambiadores",
                    Structure = null
                };
            }
            var data = await _infrastructure.GetResistDesign(request.NroSerie,request.UnitOfMeasurement, request.TestConnection, request.Temperature, request.IdSection, request.Order);

            return new ApiResponse<List<ResistDesign>> ()
            {
                Code = (int)ResponsesID.exitoso,
                Description = "Datos obtenidos de forma exitosa",
                Structure = data
            };
        }

        
        #endregion
    }
}
