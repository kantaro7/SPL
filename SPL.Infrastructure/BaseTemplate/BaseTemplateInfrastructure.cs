using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using SPL.Artifact.Infrastructure.Entities;
using SPL.Domain;
using SPL.Domain.SPL.Artifact.BaseTemplate;
using SPL.Domain.SPL.Artifact.PlateTension;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Artifact.Infrastructure.BaseTemplate
{
    public class BaseTemplateInfrastructure : IBaseTemplateInfrastructure
    {
        private readonly dbDevMigSPLContext _dbContext;
        private readonly IMapper _Mapper;

        public BaseTemplateInfrastructure(IMapper Map, dbDevMigSPLContext dbContext)
        {
            this._Mapper = Map;
            _dbContext = dbContext;
        }

        #region Methods

        public Task<long> saveBaseTemplate(SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate pData)
        {

            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {

                var BaseTemplate = _dbContext.SplPlantillaBases.AsNoTracking().FirstOrDefault(CE => CE.TipoReporte.Equals(pData.TipoReporte) && CE.ClavePrueba.Equals(pData.ClavePrueba) && CE.ClaveIdioma.Equals(pData.ClaveIdioma) && CE.ColumnasConfigurables == pData.ColumnasConfigurables);

                //pData.Plantilla = pData.Plantilla[(pData.Plantilla.IndexOf(",") + 1)..];

                SplPlantillaBase data = new SplPlantillaBase()
                {
                    TipoReporte = pData.TipoReporte,
                    ClavePrueba = pData.ClavePrueba,
                    ClaveIdioma = pData.ClaveIdioma,
                    //Plantilla = Convert.FromBase64String(pData.Plantilla),
                    Plantilla = pData.Plantilla,
                    ColumnasConfigurables = pData.ColumnasConfigurables,
                    Creadopor = BaseTemplate is null ? pData.Creadopor : BaseTemplate.Creadopor,
                    Fechacreacion = BaseTemplate is null ? DateTime.Now : BaseTemplate.Fechacreacion,
                    Modificadopor = BaseTemplate is null ? string.Empty : pData.Modificadopor,
                    Fechamodificacion = BaseTemplate is null ? null : DateTime.Now
                };

                if (BaseTemplate is null)
                    _dbContext.SplPlantillaBases.Add(data);
                else
                    _dbContext.SplPlantillaBases.Update(data);

                _dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }

            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, nameof(saveBaseTemplate));

            }
        }

        public Task<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate> GetBaseTemplate(string pTypeReport, string pKeyTest, string pkeyLanguage, int pNroColumnas)

        {
            return Task.FromResult(this._Mapper.Map<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate>(_dbContext.SplPlantillaBases.AsNoTracking().FirstOrDefault(CE => CE.TipoReporte.Equals(pTypeReport) && CE.ClavePrueba.Equals(pKeyTest) && CE.ClaveIdioma.Equals(pkeyLanguage) && CE.ColumnasConfigurables==pNroColumnas)));
        }

        public Task<long> saveBaseTemplateConsolidatedReport(BaseTemplateConsolidatedReport pData)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {

                var BaseTemplate = _dbContext.SplRepConsolidados.AsNoTracking().FirstOrDefault(CE => CE.Idioma.Equals(pData.ClaveIdioma));

                //pData.Plantilla = pData.Plantilla[(pData.Plantilla.IndexOf(",") + 1)..];

                SplRepConsolidado data = new SplRepConsolidado()
                {
                    Idioma = pData.ClaveIdioma,
                    Archivo = pData.Plantilla,
                    Creadopor = BaseTemplate is null ? pData.Creadopor : BaseTemplate.Creadopor,
                    Fechacreacion = BaseTemplate is null ? DateTime.Now : BaseTemplate.Fechacreacion,
                    Modificadopor = BaseTemplate is null ? string.Empty : pData.Modificadopor,
                    Fechamodificacion = BaseTemplate is null ? null : DateTime.Now
                };

                if (BaseTemplate is null)
                    _dbContext.SplRepConsolidados.Add(data);
                else
                    _dbContext.SplRepConsolidados.Update(data);

                _dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }

            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, nameof(saveBaseTemplate));

            }
        }

        public Task<BaseTemplateConsolidatedReport> GetBaseTemplateConsolidatedReport(string pkeyLanguage)
        {
            return Task.FromResult(this._Mapper.Map<BaseTemplateConsolidatedReport>(_dbContext.SplRepConsolidados.AsNoTracking().FirstOrDefault(CE => CE.Idioma.Equals(pkeyLanguage))));

        }

        #endregion

    }
}
