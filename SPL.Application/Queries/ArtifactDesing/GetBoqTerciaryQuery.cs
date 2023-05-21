using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Queries.Artifactdesign
{
    public class GetBoqTerciaryQuery : IRequest<decimal>
    {
        public GetBoqTerciaryQuery(string pNroOrden)
        {
            NroOrden = pNroOrden;

        }
        #region Constructor

        public string NroOrden { get; }

      

        #endregion

    }
}
