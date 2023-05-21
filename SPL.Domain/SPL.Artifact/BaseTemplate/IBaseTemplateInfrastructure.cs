
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Artifact.BaseTemplate
{
    public interface IBaseTemplateInfrastructure
    {
        public Task<long> saveBaseTemplate(BaseTemplate pData);
        public Task<long> saveBaseTemplateConsolidatedReport(BaseTemplateConsolidatedReport pData);
        public Task<BaseTemplate> GetBaseTemplate(string pTypeReport, string pKeyTest, string pkeyLanguage, int pNroColumnas);
        public Task<BaseTemplateConsolidatedReport> GetBaseTemplateConsolidatedReport(string pkeyLanguage);
    }
}
