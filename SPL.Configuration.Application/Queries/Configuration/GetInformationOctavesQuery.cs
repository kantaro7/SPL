namespace SPL.Configuration.Application.Queries.Configuration
{
    using System;
    using System.Collections.Generic;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Configuration;

    public class GetInformationOctavesQuery : IRequest<ApiResponse<List<InformationOctaves>>>
    {
        public GetInformationOctavesQuery(string pNroSerie, string pTypeInformation, string pDateData)
        {
            this.NroSerie = pNroSerie;
            this.TypeInformation = pTypeInformation;
            this.DateData = pDateData;
  
        }
        #region Constructor
        public string NroSerie { get; }
        public string TypeInformation { get; }
        public string DateData { get; }

        #endregion
    }
}
