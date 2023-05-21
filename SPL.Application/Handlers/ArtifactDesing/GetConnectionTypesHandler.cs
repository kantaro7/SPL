using MediatR;

using SPL.Artifact.Application.Queries.Artifactdesign;
using SPL.Domain.SPL.Artifact.ArtifactDesign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Handlers.Artifactdesign
{
    public class GetConnectionTypesHandler : IRequestHandler<GetConnectionTypesQuery, ConnectionTypes>
    {

        private readonly IArtifactdesignInfrastructure _infrastructure;

        public GetConnectionTypesHandler(IArtifactdesignInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<ConnectionTypes> Handle(GetConnectionTypesQuery request, CancellationToken cancellationToken)
        {
       
                ConnectionTypes obj = new ConnectionTypes();
                var result =  await _infrastructure.GetInfoCarLocal(request.NroSerie);

    

         
                    obj.otraconexionaltatension = result.Mvaf1ConnectionOther;
                    obj.otraconexionbajatension = result.Mvaf2ConnectionOther;
                    obj.otraconexionsegundabaja = result.Mvaf3ConnectionOther;
                    obj.otraconexiontercera = result.Mvaf4ConnectionOther;
                 
             

            return obj;
        }

        
        #endregion
    }
}
