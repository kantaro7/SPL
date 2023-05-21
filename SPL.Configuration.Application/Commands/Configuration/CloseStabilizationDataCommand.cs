using MediatR;

using SPL.Domain;
using SPL.Domain.SPL.Artifact.ArtifactDesign;
using SPL.Domain.SPL.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Configuration.Application.Commands.Configuration
{
    public class CloseStabilizationDataCommand : IRequest<ApiResponse<long>>
    {
        public CloseStabilizationDataCommand(string pNroSerie, decimal pIdReg)
        {
            NroSerie = pNroSerie;
            IdReg = pIdReg;

        }
        #region Constructor

        public string NroSerie { get; }
        public decimal IdReg { get; }


        #endregion

    }
}
