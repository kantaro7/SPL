using MediatR;

using SPL.Domain;
using SPL.Domain.SPL.Artifact.ArtifactDesign;
using SPL.Domain.SPL.Artifact.Nozzles;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Commands.Nozzles
{
    public class SaveRecordNozzleInformationCommand : IRequest<ApiResponse<long>>
    {
        public SaveRecordNozzleInformationCommand(NozzlesByDesign pData)
        {
            Data = pData;
   

        }
        #region Constructor

        public NozzlesByDesign Data { get; }
     

        #endregion

    }
}
