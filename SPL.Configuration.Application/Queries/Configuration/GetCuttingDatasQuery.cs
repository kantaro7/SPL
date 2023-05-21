namespace SPL.Configuration.Application.Queries.Configuration
{
    using System;
    using System.Collections.Generic;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Configuration;

    public class GetCuttingDatasQuery : IRequest<ApiResponse<List<HeaderCuttingData>>>
    {
        public GetCuttingDatasQuery(string pNroSerie)
        {

            this.NroSerie = pNroSerie;

        }
        #region Constructor
        public string NroSerie { get; }
  
 
        #endregion
    }
}
