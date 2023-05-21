namespace SPL.Masters.Infrastructure.Artifacdesign
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.EntityFrameworkCore;

    using SPL.Domain.SPL.Masters;
    using SPL.Masters.Infrastructure.Entities;

    public class MastersInfrastructure : IMastersInfrastructure
    {

        private readonly dbQAMigSPLContext _dbContext;
        private readonly IMapper _Mapper;

        public MastersInfrastructure(IMapper Map, dbQAMigSPLContext dbContext)
        {
            this._Mapper = Map;
            this._dbContext = dbContext;
        }
        #region Methods

        public Task<List<CatSidcoInformation>> GetCatSidcoInformation() => Task.FromResult(this._Mapper.Map<List<CatSidcoInformation>>(this._dbContext.SplCatsidcos.AsNoTracking().AsEnumerable()));
        public Task<List<CatSidcoOtherInformation>> GetCatSidcoOtherInformation() => Task.FromResult(this._Mapper.Map<List<CatSidcoOtherInformation>>(this._dbContext.SplCatsidcoOthers.AsNoTracking().AsEnumerable()));

        public Task<List<GeneralProperties>> GetUnitType() => Task.FromResult(this._dbContext.SplTipoUnidads.AsNoTracking().AsEnumerable().Select(item => new GeneralProperties(item.Clave, item.Descripcion)).ToList());

        public Task<List<GeneralProperties>> GetRulesEquivalents() => Task.FromResult(this._dbContext.SplNormas.AsNoTracking().AsEnumerable().Select(item => new GeneralProperties(item.Clave, item.Descripcion)).ToList());

        public Task<List<GeneralProperties>> GetLanguageEquivalents() => Task.FromResult(this._dbContext.SplIdiomas.AsNoTracking().AsEnumerable().Select(item => new GeneralProperties(item.ClaveIdioma, item.Descripcion)).ToList());

        public Task<List<GeneralProperties>> GetEquivalentsAngularDisplacement() => Task.FromResult(this._dbContext.SplDesplazamientoAngulars.AsNoTracking().AsEnumerable().Select(item => new GeneralProperties(item.Clave, item.Descripcion, item.HWye, item.XWye, item.TWye)).ToList());

        public Task<List<RulesRep>> GetRulesRep(string claveIdioma, string claveNorma) => Task.FromResult(this._Mapper.Map<List<RulesRep>>(this._dbContext.SplNormasreps.AsNoTracking().Where(item => item.ClaveNorma == claveNorma && item.ClaveIdioma == claveIdioma).ToList()));

        public Task<List<FileWeight>> GetConfigurationFiles(long module) => Task.FromResult(this._Mapper.Map<List<FileWeight>>(this._dbContext.PesoArchivos.AsNoTracking().Where(x => x.IdModulo == module)
                               .Include(x => x.ExtensionArchivoNavigation)
                               .Where(x => x.ExtensionArchivoNavigation.Active))
                               .ToList());

        public Task<List<GeneralProperties>> GetThirdWinding() => Task.FromResult(this._dbContext.SplTercerDevanadoTipos.AsNoTracking().AsEnumerable().Select(item => new GeneralProperties(item.Clave, item.Descripcion)).ToList());
        #endregion
    }
}
