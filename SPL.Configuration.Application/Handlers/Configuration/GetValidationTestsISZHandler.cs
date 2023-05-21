﻿namespace SPL.Configuration.Application.Handlers.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Configuration.Application.Queries.Configuration;
    using SPL.Domain;
    using SPL.Domain.SPL.Configuration;
    using SPL.Domain.SPL.Tests;


    public class GetValidationTestsISZHandler : IRequestHandler<GetValidationTestsISZ, ApiResponse<List<ValidationTestsIsz>>>
    {

        private readonly IConfigurationInfrastructure _infrastructure;

        public GetValidationTestsISZHandler(IConfigurationInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<ApiResponse<List<ValidationTestsIsz>>> Handle(GetValidationTestsISZ request, CancellationToken cancellationToken)
        {

            try
            {
               

                List<ValidationTestsIsz> result = await this._infrastructure.GetValidationTestsISZ();

                return new ApiResponse<List<ValidationTestsIsz>>()
                {
                    Code = result == null ? (int)ResponsesID.fallido : result.Count <= 0 ? (int)ResponsesID.fallido : (int)ResponsesID.exitoso,
                    Description = result == null ? "No se encontraron resultados" : result.Count <= 0 ? "No se encontraron resultados" : "",
                    Structure = result
                };
            }
            catch (Exception ex)
            {

                return new ApiResponse<List<ValidationTestsIsz>>()
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
