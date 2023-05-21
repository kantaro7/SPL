namespace SPL.Configuration.Application.Queries.Configuration
{
    using System;
    using System.Collections.Generic;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Configuration;

    public class GetStabilizationQuery : IRequest<ApiResponse<List<StabilizationData>>>
    {
        public GetStabilizationQuery(string pNroSerie, bool? pStatus, bool? pStabilized)
        {

            this.NroSerie = pNroSerie;
            this.Status = pStatus;
            this.Stabilized = pStabilized;
        }
        #region Constructor
        public string NroSerie { get; }
        public bool? Status { get; }
        public bool? Stabilized { get; }
 
        #endregion
    }
}
