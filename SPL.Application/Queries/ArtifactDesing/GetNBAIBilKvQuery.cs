﻿using MediatR;

using SPL.Domain.SPL.Artifact.ArtifactDesign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Queries.Artifactdesign
{
    public class GetNBAIBilKvQuery : IRequest<NBAIBilKv>
    {
        public GetNBAIBilKvQuery(string pNroSerie)
        {
            NroSerie = pNroSerie;
        }
        #region Constructor

        public string NroSerie { get; }

      

        #endregion

    }
}
