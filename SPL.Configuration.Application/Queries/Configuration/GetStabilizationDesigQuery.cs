namespace SPL.Configuration.Application.Queries.Configuration
{
    using System;
    using System.Collections.Generic;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Configuration;

    public class GetStabilizationDesignQuery : IRequest<ApiResponse<StabilizationDesignData>>
    {
        public GetStabilizationDesignQuery(string pNroSerie )
        {

            this.NroSerie = pNroSerie;
        }
        #region Constructor
        public string NroSerie { get; }
        #endregion
    }
}
