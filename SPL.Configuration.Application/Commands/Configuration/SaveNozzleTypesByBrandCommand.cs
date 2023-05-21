﻿using MediatR;

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
    public class SaveNozzleTypesByBrandCommand : IRequest<ApiResponse<long>>
    {
        public SaveNozzleTypesByBrandCommand(TypesNozzleMarks pData)
        {
            Data = pData;

        }
        #region Constructor

        public TypesNozzleMarks Data { get; }

        #endregion

    }
}
