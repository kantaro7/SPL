using MediatR;

using SPL.Domain;
using SPL.Domain.SPL.Artifact.ArtifactDesign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Queries.Artifactdesign
{
    public class GetResistDesignQuery : IRequest<ApiResponse<List<ResistDesign>>>
    {
        public GetResistDesignQuery(string nroSerie, string unitOfMeasurement, string testConnection, decimal temperature, string idSection, decimal order)
        {
            NroSerie = nroSerie;
            UnitOfMeasurement = unitOfMeasurement;
            TestConnection = testConnection;
            Temperature = temperature;
            IdSection = idSection;
            Order = order;
        }
        #region Constructor

        public string NroSerie { get; }
        public string UnitOfMeasurement { get; }
        public string TestConnection { get; }
        public decimal Temperature { get; }
        public string IdSection { get; }
        public decimal Order { get; }

      

        #endregion

    }
}
