﻿namespace SPL.Security.Application.Handlers.Security
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Domain.SPL.Security;
    using SPL.Security.Application.Commands.Security;

    public class DeleteAssignmentProfilesUsersHandler : IRequestHandler<DeleteAssignmentProfilesUsersCommand, ApiResponse<long>>
    {

        private readonly ISecurityInfrastructure _infrastructure;

        public DeleteAssignmentProfilesUsersHandler(ISecurityInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<long>> Handle(DeleteAssignmentProfilesUsersCommand request, CancellationToken cancellationToken)
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

              


                long result = await this._infrastructure.deleteAssignmentProfilesUsers(request.Data) ;

                return new ApiResponse<long>()
                {
                    Code = result == Enums.EnumsGen.Error ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result == Enums.EnumsGen.Error ? "Hubo un error al eliminar los datos" : "Datos eliminados exitosamente",
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

            catch (Exception ex)
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

