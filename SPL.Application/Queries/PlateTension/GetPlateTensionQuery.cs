using MediatR;

using SPL.Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Queries.PlateTension
{
    public class GetPlateTensionQuery : IRequest<ApiResponse<List<Domain.SPL.Artifact.PlateTension.PlateTension>>>
    {
        public GetPlateTensionQuery(string pUnit, string pTypeVoltage)
        {
            Unit = pUnit;
            TypeVoltage = pTypeVoltage;
        }
        #region Constructor

        public string Unit { get; }

        public string TypeVoltage { get; }

        #endregion

    }
}
