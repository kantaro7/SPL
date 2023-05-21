using MediatR;

using SPL.Domain;
using SPL.Domain.SPL.Artifact.ArtifactDesign;
using SPL.Domain.SPL.Artifact.Nozzles;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Queries.Nozzles
{
    public class GetRecordNozzleInformationQuery : IRequest<ApiResponse<NozzlesByDesign>>
    {
        public GetRecordNozzleInformationQuery(string pNroSerie)
        {
            NroSerie = pNroSerie;
        }
        #region Constructor

        public string NroSerie { get; }

      

        #endregion

    }
}
