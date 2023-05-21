using MediatR;

using SPL.Domain;
using SPL.Domain.SPL.Configuration;

using System.Collections.Generic;

namespace SPL.Configuration.Application.Commands.Configuration
{
    public class ImportInformationOctavesCommand : IRequest<ApiResponse<long>>
    {
        #region Constructor

        public ImportInformationOctavesCommand(List<InformationOctaves> pData)
        {
            Data = pData;

        }

        #endregion

        #region Properties

        public List<InformationOctaves> Data { get; }

        #endregion
    }
}