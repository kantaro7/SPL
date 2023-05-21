using MediatR;
using SIDCO.Application.Queries.Artifactdesign;
using SIDCO.Domain.Artifactdesign;
using SIDCO.Domain.Domain.Models;
using SIDCO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SIDCO.Application.Handlers.Artifactdesign
{
    public class GetTypeTransformersHandler : IRequestHandler<GetTypeTransformersQuery, List<GeneralProperties>>
    {

        private readonly IArtifactdesignInfrastructure _infrastructure;

        public GetTypeTransformersHandler(IArtifactdesignInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<List<GeneralProperties>> Handle(GetTypeTransformersQuery request, CancellationToken cancellationToken)
        {
            return await _infrastructure.GetTypesTransformers();

    
        }

        
        #endregion
    }
}
