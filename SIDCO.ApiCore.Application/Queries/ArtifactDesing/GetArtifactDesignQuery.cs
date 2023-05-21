using MediatR;

using SIDCO.Domain.Artifactdesign;
using SIDCO.Domain.Domain.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SIDCO.Application.Queries.Artifactdesign
{
    public class GetArtifactDesignQuery : IRequest<InformationArtifact>
    {
        public GetArtifactDesignQuery(string pNroSerie)
        {
            nroSerie = pNroSerie;
      

        }
        #region Constructor

        public string nroSerie { get; }
   

        #endregion

    }
}
