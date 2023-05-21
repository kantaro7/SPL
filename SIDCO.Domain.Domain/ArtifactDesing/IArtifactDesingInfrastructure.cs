using SIDCO.Domain.Domain.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SIDCO.Domain.Artifactdesign
{
    public interface IArtifactdesignInfrastructure
    {
        public Task<List<GeneralProperties>> GetIdiomasSidco();

        public  Task<List<GeneralProperties>> GetTypesTransformers();

        public  Task<List<GeneralProperties>> GetApplications();

        public  Task<List<GeneralProperties>> GetApplicableRule();

        public  Task<List<GeneralProperties>> GetAngularDisplacement();

        public Task<InformationArtifact> GetGeneralArtifactdesign(String serial);

        public Task<List<GeneralProperties>> GetOperativeConditions();

        public Task<List<GeneralProperties>> GetConnectionTypes();

    }
}
