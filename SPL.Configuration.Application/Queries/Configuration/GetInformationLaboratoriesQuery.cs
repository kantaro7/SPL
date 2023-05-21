namespace SPL.Configuration.Application.Queries.Configuration
{
    using System;
    using System.Collections.Generic;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Configuration;

    public class GetInformationLaboratoriesQuery : IRequest<ApiResponse<List<InformationLaboratories>>>
    {
        public GetInformationLaboratoriesQuery()
        {


        }
        #region Constructor

        #endregion
    }
}
