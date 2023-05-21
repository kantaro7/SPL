using MediatR;

using SPL.Domain;
using SPL.Domain.SPL.Configuration;

using System.Collections.Generic;

namespace SPL.Configuration.Application.Commands.Configuration
{
    public class UpdateInformationOctavesCommand : IRequest<ApiResponse<long>>
    {
        public UpdateInformationOctavesCommand(List<InformationOctaves> pData)
        {
            Data = pData;

        }
        #region Constructor

        public List<InformationOctaves> Data { get; }

        #endregion

    }
}
