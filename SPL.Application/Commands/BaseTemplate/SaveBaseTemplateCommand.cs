using MediatR;

using SPL.Domain;
using SPL.Domain.SPL.Artifact.ArtifactDesign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Commands.BaseTemplate
{
    public class SaveBaseTemplateCommand : IRequest<ApiResponse<long>>
    {
        public SaveBaseTemplateCommand(SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate pData)
        {
            Data = pData;
        }

        #region Constructor

        public SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate Data { get; }
    

        #endregion

    }
}
