namespace SPL.Artifact.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;

    using SPL.Artifact.Infrastructure.Entities;
    using SPL.Domain;
    using SPL.Domain.SPL.Artifact.PlateTension;

    public class PlateTensionInfrastructure : IPlateTensionInfrastructure
    {
        private readonly dbDevMigSPLContext _dbContext;
        private readonly IMapper _Mapper;

        public PlateTensionInfrastructure(IMapper Map, dbDevMigSPLContext dbContext)
        {
            this._Mapper = Map;
            this._dbContext = dbContext;
        }

        #region Methods

        public Task<IEnumerable<PlateTension>> getPlateTension(string Unit, string pTypeVoltage) => pTypeVoltage == "-1"
                ? Task.FromResult(this._Mapper.Map<IEnumerable<PlateTension>>(this._dbContext.SplTensionPlacas.AsNoTracking().Where(CE => CE.Unidad == Unit).OrderBy(c => c.Orden).ToList()))
                : Task.FromResult(this._Mapper.Map<IEnumerable<PlateTension>>(this._dbContext.SplTensionPlacas.AsNoTracking().Where(CE => CE.Unidad == Unit && CE.TipoTension == pTypeVoltage).OrderBy(c => c.Orden).ToList()));

        public Task<long> savePlateTension(List<PlateTension> pList, bool pStatusDelete)
        {

            using IDbContextTransaction transaction = this._dbContext.Database.BeginTransaction();
            try
            {
                List<SplTensionPlaca> PlateTensions = this._dbContext.SplTensionPlacas.AsNoTracking().Where(CE => CE.Unidad == pList.FirstOrDefault().Unidad).ToList();
                bool modified = false;
                string creadoPor = "";
                DateTime fechaCreacion = new DateTime();
                if (PlateTensions.Count() > 0 && pStatusDelete)
                {
                    creadoPor = PlateTensions[0].Creadopor;
                    fechaCreacion = PlateTensions[0].Fechacreacion;
                    modified = true;
                    this._dbContext.SplTensionPlacas.RemoveRange(PlateTensions);
                    _ = this._dbContext.SaveChanges();
                    PlateTensions.Clear();
                }

                if (PlateTensions.Count() > 0)
                {
                    this._dbContext.SplTensionPlacas.RemoveRange(PlateTensions);

                    foreach (PlateTension item in pList)
                    {
                        item.Creadopor = PlateTensions.FirstOrDefault().Creadopor;
                        item.Fechacreacion = PlateTensions.FirstOrDefault().Fechacreacion;
                        item.Fechamodificacion = pList.FirstOrDefault().Fechamodificacion;
                        item.Modificadopor = pList.FirstOrDefault().Modificadopor;
                    }
                    this._dbContext.SplTensionPlacas.AddRange(this._Mapper.Map<IEnumerable<SplTensionPlaca>>(pList));
                    _ = this._dbContext.SaveChanges();
                }
                else
                {
                    foreach (PlateTension item in pList)
                    {
                        if (modified)
                        {
                            item.Creadopor = creadoPor;
                            item.Fechacreacion = fechaCreacion;
                        }
                        else
                        {
                            item.Creadopor = item.Creadopor;
                            item.Fechacreacion = DateTime.Now;

                            item.Fechamodificacion = null;
                            item.Modificadopor = null;
                        }
                        
                    }
                    this._dbContext.SplTensionPlacas.AddRange(this._Mapper.Map<IEnumerable<SplTensionPlaca>>(pList));
                    _ = this._dbContext.SaveChanges();
                }

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }

            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, nameof(savePlateTension));

            }
        }
        #endregion

    }
}
