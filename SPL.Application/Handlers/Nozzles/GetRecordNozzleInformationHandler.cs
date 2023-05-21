using MediatR;

using SPL.Artifact.Application.Common;
using SPL.Artifact.Application.Queries.BaseTemplate;
using SPL.Artifact.Application.Queries.Nozzles;
using SPL.Domain;
using SPL.Domain.SPL.Artifact.ArtifactDesign;
using SPL.Domain.SPL.Artifact.BaseTemplate;
using SPL.Domain.SPL.Artifact.Nozzles;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Handlers.Nozzles
{
    public class GetRecordNozzleInformationHandler : IRequestHandler<GetRecordNozzleInformationQuery, ApiResponse<NozzlesByDesign>>
    {

        private readonly IArtifactdesignInfrastructure _infrastructureDesign;
        private readonly INozzlesInfrastructure _infrastructureNozzles;

        public GetRecordNozzleInformationHandler(IArtifactdesignInfrastructure infrastructureDesign, INozzlesInfrastructure infrastructureNozzles)
        {
            _infrastructureDesign = infrastructureDesign;
            _infrastructureNozzles = infrastructureNozzles;
        }

        #region Methods
        public async Task<ApiResponse<NozzlesByDesign>> Handle(GetRecordNozzleInformationQuery request, CancellationToken cancellationToken)
        {

            try
            {
                NozzlesByDesign result = new();
                string[] aparato = request.NroSerie.Split('-');

                if (string.IsNullOrEmpty(aparato[0].Trim()))
                {

                    return new ApiResponse<NozzlesByDesign>()
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
                    return new ApiResponse<NozzlesByDesign>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El número de serie no puede excederse de 55 caracteres",
                        Structure = null
                    };
                }
                if (valFormat)
                {

                    return new ApiResponse<NozzlesByDesign>()
                    {
                        Code = (int)ResponsesID.fallido,
                        Description = "El número de serie solo debe contener letras, números y guion medio",
                        Structure = null
                    };
                }

                List<NozzlesArtifact> Info = await _infrastructureDesign.GetSplInfoaparatoBoqs(aparato[0]);

                if(Info.Count == 0)
                {
                    return new ApiResponse<NozzlesByDesign>()
                    {
                        Code = result == null ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                        Description = "El aparato no cuenta con información de las boquillas",
                        Structure = result
                    };
                }

                result.TotalQuantity = 0;

                Info.ForEach(x => result.TotalQuantity += Convert.ToInt32(x.Qty ?? 0));

                result.NozzleInformation = await _infrastructureNozzles.GetRecordNozzleInformation(request.NroSerie);

                return new ApiResponse<NozzlesByDesign>()
                {
                    Code = (int)ResponsesID.exitoso,
                    Description = "Datos obtenidos por exito",
                    Structure = result
                };
            }
            catch (Exception ex)
            {

                return new ApiResponse<NozzlesByDesign>()
                {
                    Code = (int)(ResponsesID.fallido),
                    Description = ex.Message,
                    Structure = null
                };

            }
        }
    }

        #endregion
    
}

