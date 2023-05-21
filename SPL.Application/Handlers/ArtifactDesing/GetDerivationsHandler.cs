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
    public class GetDerivationsHandler : IRequestHandler<GetDerivationsQuery, Derivations>
    {

        private readonly IArtifactdesignInfrastructure _infrastructure;

        public GetDerivationsHandler(IArtifactdesignInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
        }

        #region Methods
        public async Task<Derivations> Handle(GetDerivationsQuery request, CancellationToken cancellationToken)
        {
      
                 Derivations obj = new Derivations();
                var result =  await _infrastructure.GetInfoCarLocal(request.NroSerie);

          
       
                    obj.valorderivacionupaltatension = result.Mvaf1DerUp;
                    obj.valorderivaciondownaltatension = result.Mvaf1DerDown;
                    obj.valorderivacionupbajatension = result.Mvaf2DerUp;
                    obj.valorderivaciondownbajatension = result.Mvaf2DerDown;

                    obj.valorderivacionupsegundatension = result.Mvaf3DerUp;
                    obj.valorderivaciondownsegundatension = result.Mvaf3DerDown;

                    obj.valorderivacionupterceratension = result.Mvaf4DerUp;
                    obj.valorderivaciondownterceratension = result.Mvaf4DerDown;
             

            return obj;
        }

        
        #endregion
    }
}
