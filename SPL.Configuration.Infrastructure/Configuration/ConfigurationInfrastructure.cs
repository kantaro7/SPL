namespace SPL.Configuration.Infrastructure.Configuration
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;

    using SPL.Configuration.Infrastructure.Common;
    using SPL.Configuration.Infrastructure.Entities;
    using SPL.Domain;
    using SPL.Domain.SPL.Configuration;

    public class ConfigurationInfrastructure : IConfigurationInfrastructure
    {
        private readonly dbDevMigSPLContext _dbContext;
        private readonly IMapper _Mapper;

        public ConfigurationInfrastructure(IMapper Map, dbDevMigSPLContext dbContext)
        {
            _Mapper = Map;
            _dbContext = dbContext;
        }

        #region Methods

        public Task<long> saveCorrectionFactorSpecification(CorrectionFactorSpecification pData)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                SplFactorcorFpc data = _dbContext.SplFactorcorFpcs.AsNoTracking().FirstOrDefault(CE => CE.Especificacion.Equals(pData.Especificacion) && CE.Temperatura == pData.Temperatura);

                _ = data is null
                    ? _dbContext.SplFactorcorFpcs.Add(_Mapper.Map<SplFactorcorFpc>(pData))
                    : _dbContext.SplFactorcorFpcs.Update(_Mapper.Map<SplFactorcorFpc>(pData));

                _ = _dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<long> deleteCorrectionFactorSpecification(CorrectionFactorSpecification pData)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                SplFactorcorFpc data = _dbContext.SplFactorcorFpcs.AsNoTracking().FirstOrDefault(CE => CE.Especificacion.Equals(pData.Especificacion) && CE.Temperatura == pData.Temperatura);
                _ = data != null
                    ? _dbContext.SplFactorcorFpcs.Remove(_Mapper.Map<SplFactorcorFpc>(pData))
                    : throw new ArgumentException("No se encuentra el registro");
                _ = _dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<List<CorrectionFactorSpecification>> GetCorrectionFactorSpecificationFPC(string pSpecification, decimal pTemperature, decimal pCorrectionFactor)
        {
            IEnumerable<CorrectionFactorSpecification> factors = _Mapper.Map<IEnumerable<CorrectionFactorSpecification>>(_dbContext.SplFactorcorFpcs.AsNoTracking().AsEnumerable());

            return Task.FromResult(factors.Where(x => (x.Especificacion.Equals(pSpecification) || pSpecification.Equals("-1")) && (x.Temperatura.Equals(pTemperature) || pTemperature == -1) && (x.FactorCorr.Equals(pCorrectionFactor) || pCorrectionFactor == -1)).ToList());
        }

        public Task<List<NozzleMarks>> GetNozzleMarks(long pIdMark, bool pStatus)
        {
            IEnumerable<NozzleMarks> marks = _Mapper.Map<List<NozzleMarks>>(_dbContext.SplMarcasBoqs.Where(x => x.Estatus == pStatus).AsNoTracking());
            return pIdMark != -1
                ? Task.FromResult(marks.Where(x => x.IdMarca == pIdMark).ToList())
                : Task.FromResult(marks.ToList());
        }

        public Task<List<TypesNozzleMarks>> GetTypeXMarksNozzle(long pIdMark, bool pStatus)
        {
            IEnumerable<TypesNozzleMarks> marks = _Mapper.Map<IEnumerable<TypesNozzleMarks>>(_dbContext.SplTiposxmarcaBoqs.Where(x => x.Estatus == pStatus).AsNoTracking());
            return pIdMark != -1
                ? Task.FromResult(marks.Where(x => x.IdMarca == pIdMark).ToList())
                : Task.FromResult(marks.ToList());
        }

        public Task<List<CorrectionFactorsXMarksXTypes>> GetCorrectionFactorsXMarksXTypes()
        {
            List<SqlParameter> parametros = new();

            //EJECUTAR CONSULTA
            DataSet ds = Methods.ejecutarConsultaOFuncion("SELECT * FROM [dbo].[GETFACTORCORFBP]()", parametros, "Data", _dbContext.Database.GetConnectionString());
            return Task.FromResult(ds.Tables["Data"].AsEnumerable().Select(result => new CorrectionFactorsXMarksXTypes()
            {

                IdMarca = Convert.ToDecimal(result["ID_MARCA"].ToString()),
                Marca = result["DESCRIPCION_MARCA"].ToString(),
                IdTipo = Convert.ToDecimal(result["ID_TIPO"].ToString()),
                Tipo = result["DESCRIPCION_TIPO"].ToString(),
                FactorCorr = Convert.ToDecimal(result["FACTOR_CORR"].ToString()),
                Temperatura = Convert.ToDecimal(result["TEMPERATURA"].ToString()),
                Creadopor = result["CREADOPOR"].ToString(),
                Modificadopor = result["MODIFICADOPOR"].ToString(),
                Fechacreacion = string.IsNullOrEmpty(result["FECHACREACION"].ToString()) ? new DateTime() : Convert.ToDateTime(result["FECHACREACION"].ToString()),
                Fechamodificacion = string.IsNullOrEmpty(result["FECHAMODIFICACION"].ToString()) ? null : Convert.ToDateTime(result["FECHAMODIFICACION"].ToString())

            }).ToList());
        }

        public Task<long> saveCorrectionFactorsXMarksXTypes(CorrectionFactorsXMarksXTypes pData)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                SplFactorcorFpb data = _dbContext.SplFactorcorFpbs.AsNoTracking().FirstOrDefault(CE => CE.IdMarca == pData.IdMarca && CE.Temperatura == pData.Temperatura && CE.IdTipo == pData.IdTipo);

                if (data is null)
                {
                    _ = _dbContext.SplFactorcorFpbs.Add(_Mapper.Map<SplFactorcorFpb>(pData));
                }
                else
                {
                    pData.Fechacreacion = data.Fechacreacion;
                    pData.Creadopor = data.Creadopor;
                    _ = _dbContext.SplFactorcorFpbs.Update(_Mapper.Map<SplFactorcorFpb>(pData));
                }

                _ = _dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }

            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<long> deleteCorrectionFactorsXMarksXTypes(CorrectionFactorsXMarksXTypes pData)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                SplFactorcorFpb data = _dbContext.SplFactorcorFpbs.AsNoTracking().FirstOrDefault(CE => CE.IdMarca == pData.IdMarca && CE.Temperatura == pData.Temperatura && CE.IdTipo == pData.IdTipo);

                _ = data != null
                    ? _dbContext.SplFactorcorFpbs.Remove(_Mapper.Map<SplFactorcorFpb>(pData))
                    : throw new ArgumentException("No se encuentra el registro");

                _ = _dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<CorrectionFactorsDesc> GetCorrectionFactorsDesc(string pSpecification, string pKeyLenguage)
        {
            try
            {
                return Task.FromResult(_Mapper.Map<CorrectionFactorsDesc>(_dbContext.SplDescFactorcors.AsNoTracking().FirstOrDefault(x => x.Especificacion.ToUpper().Equals(pSpecification.ToUpper()) && x.ClaveIdioma.ToUpper().Equals(pKeyLenguage.ToUpper()))));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<long> saveNozzleTypesByBrand(TypesNozzleMarks pData)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                decimal tipoCon = pData.IdTipo;
                string messageExiste = "La descripción del tipo de boquilla para la marca ya existe, favor de corregirla";

                if (pData.IdTipo == -1)
                {
                    tipoCon = _dbContext.SplTiposxmarcaBoqs.AsNoTracking().Where(CE => CE.IdMarca.Equals(pData.IdMarca)).Count() + 1;

                    SplTiposxmarcaBoq marca = _dbContext.SplTiposxmarcaBoqs.AsNoTracking().FirstOrDefault(CE => CE.IdMarca.Equals(pData.IdMarca) && CE.Descripcion.ToUpper().Equals(pData.Descripcion.ToUpper()));

                    if (marca != null)
                        throw new ArgumentException(messageExiste);

                    pData.IdTipo = tipoCon;
                    _ = _dbContext.SplTiposxmarcaBoqs.Add(_Mapper.Map<SplTiposxmarcaBoq>(pData));
                }
                else
                {

                    SplTiposxmarcaBoq marca = _dbContext.SplTiposxmarcaBoqs.AsNoTracking().FirstOrDefault(CE => CE.IdMarca.Equals(pData.IdMarca) && CE.IdTipo != pData.IdTipo && CE.Descripcion.ToUpper().Equals(pData.Descripcion.ToUpper()));
                    if (marca != null)
                        throw new ArgumentException(messageExiste);
                    _ = _dbContext.SplTiposxmarcaBoqs.Update(_Mapper.Map<SplTiposxmarcaBoq>(pData));
                }

                _ = _dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<long> deleteNozzleTypesByBrand(TypesNozzleMarks pData)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                SplTiposxmarcaBoq data = _dbContext.SplTiposxmarcaBoqs.AsNoTracking().FirstOrDefault(CE => CE.IdMarca.Equals(pData.IdMarca) && CE.IdTipo == pData.IdTipo);

                SplInfoaparatoBoqdet InfoaparatoBoqdet = _dbContext.SplInfoaparatoBoqdets.AsNoTracking().FirstOrDefault(CE => CE.IdTipo == pData.IdTipo && CE.IdMarca == pData.IdMarca);

                SplFactorcorFpb FactorcorFpb = _dbContext.SplFactorcorFpbs.AsNoTracking().FirstOrDefault(CE => CE.IdTipo == pData.IdTipo && CE.IdMarca == pData.IdMarca);

                SplInfoDetalleFpb InfoDetalleFpb = _dbContext.SplInfoDetalleFpbs.AsNoTracking().FirstOrDefault(CE => CE.IdTipo == pData.IdTipo && CE.IdMarca == pData.IdMarca);

                if (InfoaparatoBoqdet != null || FactorcorFpb != null || InfoDetalleFpb != null)
                {
                    throw new ArgumentException("El tipo de boquilla no puede ser eliminada ya que existen aparatos que la contienen o hay factor de corrección usándola o se uso en el reporte de factor de potencia y capacitancia");

                }

                _ = data != null
                    ? _dbContext.SplTiposxmarcaBoqs.Remove(_Mapper.Map<SplTiposxmarcaBoq>(pData))
                    : throw new ArgumentException("No se encuentra el registro");
                _ = _dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<List<TypesNozzleMarks>> GetNozzleTypesByBrand(long pIdMark)
        {
            IEnumerable<TypesNozzleMarks> marks = _Mapper.Map<IEnumerable<TypesNozzleMarks>>(_dbContext.SplTiposxmarcaBoqs.AsNoTracking());

            return pIdMark != -1
                ? Task.FromResult(marks.Where(x => x.IdMarca == pIdMark).ToList())
                : Task.FromResult(marks.ToList());
        }

        public Task<long> saveNozzleBrands(NozzleMarks pData)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                SplMarcasBoq data = _dbContext.SplMarcasBoqs.AsNoTracking().FirstOrDefault(CE => CE.IdMarca.Equals(pData.IdMarca));

                if (data is null)
                {
                    SplMarcasBoq marca = _dbContext.SplMarcasBoqs.AsNoTracking().FirstOrDefault(CE => CE.Descripcion.ToUpper().Equals(pData.Descripcion.ToUpper()));

                    if (marca != null)
                        throw new ArgumentException("La descripción ya existe, favor de corregirla");
                }
                else
                {
                    data.Estatus = pData.Estatus;
                    data.Descripcion = pData.Descripcion;
                    data.Fechamodificacion = pData.Fechamodificacion;
                    data.Modificadopor = pData.Modificadopor;
                }

                _ = data is null
                    ? _dbContext.SplMarcasBoqs.Add(_Mapper.Map<SplMarcasBoq>(pData))
                    : _dbContext.SplMarcasBoqs.Update(_Mapper.Map<SplMarcasBoq>(data));

                _ = _dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<long> deleteNozzleBrands(NozzleMarks pData)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {

                SplMarcasBoq data = _dbContext.SplMarcasBoqs.AsNoTracking().FirstOrDefault(CE => CE.IdMarca.Equals(pData.IdMarca));

                SplTiposxmarcaBoq InfoTypeMarksNozzle = _dbContext.SplTiposxmarcaBoqs.AsNoTracking().FirstOrDefault(CE => CE.IdMarca.Equals(pData.IdMarca));

                if (InfoTypeMarksNozzle != null)
                {
                    throw new ArgumentException("La marca no puede ser eliminada ya que tiene tipo de boquillas registradas");

                }

                _ = data != null
                    ? _dbContext.SplMarcasBoqs.Remove(_Mapper.Map<SplMarcasBoq>(pData))
                    : throw new ArgumentException("No se encuentra el registro");
                _ = _dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<List<NozzleMarks>> GetNozzleBrands(long pIdMark)
        {
            IEnumerable<NozzleMarks> marks = _Mapper.Map<IEnumerable<NozzleMarks>>(_dbContext.SplMarcasBoqs.AsNoTracking());

            return pIdMark != -1
                ? Task.FromResult(marks.Where(x => x.IdMarca == pIdMark).ToList())
                : Task.FromResult(marks.ToList());
        }

        public Task<List<ValidationTestsIsz>> GetValidationTestsISZ() => Task.FromResult(_Mapper.Map<List<ValidationTestsIsz>>(_dbContext.SplValidationTestsIszs.AsNoTracking().ToList()));

        public Task<List<ContGasCGD>> GetInfoContGasCGD(string pIdReport, string pKeyTests, string pTypeOil)
        {
            IEnumerable<ContGasCGD> marks = _Mapper.Map<IEnumerable<ContGasCGD>>(_dbContext.SplContgasCgds.AsNoTracking().ToList());

            if (pIdReport != "-1")
            {
                marks = marks.Where(item => item.TipoReporte.ToUpper().Equals(pIdReport.ToUpper()));
            }
            if (pKeyTests != "-1")
            {
                marks = marks.Where(item => item.ClavePrueba.ToUpper().Equals(pKeyTests.ToUpper()));
            }

            if (pTypeOil != "-1")
            {
                marks = marks.Where(item => item.TipoAceite.ToUpper().Equals(pTypeOil.ToUpper()));
            }

            return Task.FromResult(marks.ToList());
        }

        public Task<long> saveInfoContGasCGD(ContGasCGD pData)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                SplContgasCgd data = _dbContext.SplContgasCgds.AsNoTracking().FirstOrDefault(x => x.Id == pData.Id);
                _ = data is null
                    ? _dbContext.SplContgasCgds.Add(_Mapper.Map<SplContgasCgd>(pData))
                    : _dbContext.SplContgasCgds.Update(_Mapper.Map<SplContgasCgd>(pData));
                _ = _dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<long> deleteInfoContGasCGD(ContGasCGD pData)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                SplContgasCgd data = _dbContext.SplContgasCgds.AsNoTracking().FirstOrDefault(x => x.TipoReporte.ToUpper().Equals(pData.TipoReporte) && x.TipoAceite.ToUpper().Equals(pData.TipoAceite) && x.ClavePrueba.ToUpper().Equals(pData.ClavePrueba));
                _ = data != null
                    ? _dbContext.SplContgasCgds.Remove(_Mapper.Map<SplContgasCgd>(data))
                    : throw new ArgumentException("No se encuentra el registro");
                _ = _dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<List<InformationOctaves>> GetInformationOctaves(string pNroSerie, string pTypeInformation, string pDateData)
        {
            try
            {
                List<InformationOctaves> List = string.IsNullOrEmpty(pDateData)
                    ? _Mapper.Map<List<InformationOctaves>>(_dbContext.SplInfoOctavas.AsNoTracking().Where(x => x.NoSerie.ToUpper().Equals(pNroSerie.ToUpper())).ToList())
                    : _Mapper.Map<List<InformationOctaves>>(_dbContext.SplInfoOctavas.AsNoTracking().Where(x => x.NoSerie.ToUpper().Equals(pNroSerie.ToUpper()) && x.FechaDatos == Convert.ToDateTime(pDateData)).ToList());
                if (pTypeInformation != null)
                {
                    List = List.Where(x => x.TipoInfo.ToUpper().Equals(pTypeInformation)).ToList();
                }

                return Task.FromResult(List);
            }
            catch (DbUpdateException e)
            {
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar la obtención " + e.Message + sb.ToString() + e.InnerException.Message);
            }

            catch (Exception e)
            {

                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + e.InnerException?.Message);
            }
        }
        public Task<long> ImportInformationOctaves(List<InformationOctaves> pData)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var octaves = pData
                    .GroupBy(e => new { e.NoSerie, e.FechaDatos })
                    .OrderBy(e => e.Key.FechaDatos)
                    .FirstOrDefault(); /*Solo se carga información de un número de serie y una fecha*/

                if (octaves != null)
                {
                    var currentOctaves = _dbContext.SplInfoOctavas.AsNoTracking()
                        .Where(x =>
                            x.NoSerie.ToUpper().Equals(octaves.Key.NoSerie.ToUpper())
                            && x.FechaDatos == octaves.Key.FechaDatos)
                        .ToList();

                    foreach (var typeGroup in octaves.GroupBy(e => e.TipoInfo))
                    {
                        var currentTypeOctaves = currentOctaves
                            .Where(x => x.TipoInfo.ToUpper().Equals(typeGroup.Key.ToUpper()))
                            .ToArray();

                        foreach (var heights in typeGroup.GroupBy(e => e.Altura))
                        {
                            var currentTypeHeightOctaves = currentTypeOctaves.Where(e => e.Altura == heights.Key).ToArray();

                            if (currentTypeHeightOctaves.Any())
                            {
                                _dbContext.SplInfoOctavas.RemoveRange(currentTypeHeightOctaves);

                                /*Se actualiza la lista de octavas
                                 La idea es que la lista solo contenga las octavas que no se incluyeron en el archivo*/
                                foreach (var item in currentTypeHeightOctaves)
                                {
                                    currentOctaves.Remove(item);
                                }
                            }
                        }

                        List<SplInfoOctava> newData = _Mapper.Map<List<SplInfoOctava>>(typeGroup);
                        _dbContext.SplInfoOctavas.AddRange(newData);
                    }

                    /*TODO: Si existen octavas en base de datos que no estaban en el archivo (NoSerie,FechaDatos,Altura) se pueden eliminar.
                     _dbContext.SplInfoOctavas.RemoveRange(currentOctaves);*/

                    _ = _dbContext.SaveChanges();

                    transaction.Commit();
                }
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<long> UpdateInformationOctaves(List<InformationOctaves> pData)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();

            try
            {
                foreach (var octavesGroup in pData
                    .GroupBy(e => new { e.NoSerie, e.TipoInfo, e.FechaDatos, e.Altura })
                    .OrderBy(e => e.Key.FechaDatos))
                {
                    List<SplInfoOctava> currentOctaves = _dbContext.SplInfoOctavas
                        .Where(x =>
                            x.NoSerie.ToUpper().Equals(octavesGroup.Key.NoSerie.ToUpper())
                            && x.FechaDatos == octavesGroup.Key.FechaDatos
                            && x.Altura == octavesGroup.Key.Altura)
                        .ToList();

                    foreach (InformationOctaves updatedOctave in octavesGroup)
                    {
                        SplInfoOctava currentOctave = currentOctaves
                            .FirstOrDefault(e => e.NoSerie.ToUpper() == updatedOctave.NoSerie.ToUpper()
                                && e.TipoInfo == updatedOctave.TipoInfo
                                && e.FechaDatos == updatedOctave.FechaDatos
                                && e.Altura == updatedOctave.Altura
                                && e.Hora == updatedOctave.Hora);

                        if (currentOctave != null)
                        {
                            var modificadopor = updatedOctave.Modificadopor;
                            var fechamodificacion = updatedOctave.Fechamodificacion;

                            updatedOctave.Fechacreacion = currentOctave.Fechacreacion;
                            updatedOctave.Creadopor = currentOctave.Creadopor;
                            updatedOctave.Fechamodificacion = currentOctave.Fechamodificacion;
                            updatedOctave.Modificadopor = currentOctave.Modificadopor;

                            _Mapper.Map(updatedOctave, currentOctave);

                            if (_dbContext.Entry(currentOctave).State == EntityState.Modified)
                            {
                                currentOctave.Fechamodificacion = fechamodificacion;
                                currentOctave.Modificadopor = modificadopor;
                            }
                        }
                    }
                }

                _ = _dbContext.SaveChanges();

                transaction.Commit();

                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<StabilizationDesignData> GetStabilizationDesignData(string pNroSerie)
        {
            try
            {
                return Task.FromResult(_Mapper.Map<StabilizationDesignData>(_dbContext.SplInfoaparatoEsts.AsNoTracking().FirstOrDefault(x => x.NoSerie.ToUpper().Equals(pNroSerie.ToUpper()))));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        public Task<long> saveStabilizationDesignData(StabilizationDesignData pData)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            string g = _dbContext.Database.GetConnectionString();
            try
            {
                SplInfoaparatoEst data = _dbContext.SplInfoaparatoEsts.AsNoTracking().FirstOrDefault(x => x.NoSerie == pData.NoSerie);
                _ = data is null
                    ? _dbContext.SplInfoaparatoEsts.Add(_Mapper.Map<SplInfoaparatoEst>(pData))
                    : _dbContext.SplInfoaparatoEsts.Update(_Mapper.Map<SplInfoaparatoEst>(pData));
                _ = _dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);
            }
        }
        public Task<List<CorrectionFactorkWTypeCooling>> GetCorrectionFactorkWTypeCooling()
        {
            try
            {
                return Task.FromResult(_Mapper.Map<List<CorrectionFactorkWTypeCooling>>(_dbContext.SplFactorcorEtds.AsNoTracking().ToList().OrderBy(x => x.CoolingType)));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        public Task<long> saveCorrectionFactorkWTypeCooling(List<CorrectionFactorkWTypeCooling> pData)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                IEnumerable<SplFactorcorEtd> allRec = _dbContext.SplFactorcorEtds.AsNoTracking().AsEnumerable();
                _dbContext.SplFactorcorEtds.RemoveRange(allRec);
                _ = _dbContext.SaveChanges();
                _dbContext.SplFactorcorEtds.AddRange(_Mapper.Map<List<SplFactorcorEtd>>(pData));
                _ = _dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<List<StabilizationData>> GetStabilizationData(string pNroSerie, bool? pStatus, bool? pStabilized)
        {
            try
            {
                List<StabilizationData> listFilter = _Mapper.Map<List<StabilizationData>>(_dbContext.SplDatosgralEsts.AsNoTracking().ToList());
                if (!string.IsNullOrEmpty(pNroSerie))
                {
                    listFilter = listFilter.FindAll(x => x.NoSerie.ToUpper().Equals(pNroSerie.ToUpper()));
                }
                if (pStatus != null)
                {
                    listFilter = listFilter.FindAll(x => x.Estatus == pStatus);
                }

                foreach (StabilizationData item in listFilter)
                {
                    item.StabilizationDataDetails = _Mapper.Map<List<StabilizationDetailsData>>(_dbContext.SplDatosdetEsts.AsNoTracking().Where(x => x.IdReg == item.IdReg).ToList());
                    item.CantEstables = item.StabilizationDataDetails.Count(x => x.IdReg == item.IdReg && x.Estable);
                    item.CantInestables = item.StabilizationDataDetails.Count(x => x.IdReg == item.IdReg && !x.Estable);
                }

                if (pStabilized != null)
                {
                    listFilter = pStabilized == true
                        ? listFilter.FindAll(x => x.CantEstables > 0 && x.CantInestables < x.CantEstables)
                        : listFilter.FindAll(x => x.CantInestables > 0 && x.CantEstables < x.CantInestables);
                }

                return Task.FromResult(listFilter);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<long> saveStabilizationData(StabilizationData pData)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                SplDatosgralEst data = _dbContext.SplDatosgralEsts.AsNoTracking().FirstOrDefault(CE => CE.IdReg == pData.IdReg);

                SplDatosgralEst insertData = _Mapper.Map<SplDatosgralEst>(pData);

                _ = data is null ? _dbContext.SplDatosgralEsts.Add(insertData) : _dbContext.SplDatosgralEsts.Update(insertData);

                _ = _dbContext.SaveChanges();

                decimal idReg = insertData.IdReg;

                SplDatosdetEst dataDetails = _dbContext.SplDatosdetEsts.AsNoTracking().FirstOrDefault(CE => CE.IdReg == idReg);

                if (dataDetails != null)
                    _dbContext.SplDatosdetEsts.RemoveRange(dataDetails);

                foreach (StabilizationDetailsData item in pData.StabilizationDataDetails)
                {
                    item.IdReg = idReg;
                }

                _dbContext.SplDatosdetEsts.AddRange(_Mapper.Map<SplDatosdetEst>(pData.StabilizationDataDetails));
                _ = _dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<long> closeStabilizationData(string pNroSerie, decimal pIdReg)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                SplDatosgralEst data = _dbContext.SplDatosgralEsts.AsNoTracking().FirstOrDefault(CE => CE.IdReg == pIdReg && CE.NoSerie.ToUpper().Equals(pNroSerie));

                if (data != null)
                {
                    data.Estatus = false;
                    _ = _dbContext.SplDatosgralEsts.Update(data);
                }

                _ = _dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<List<InformationLaboratories>> GetInformationLaboratories()
        {
            try
            {
                return Task.FromResult(_Mapper.Map<List<InformationLaboratories>>(_dbContext.SplInfoLaboratorios.AsNoTracking().ToList()));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<List<HeaderCuttingData>> GetCuttingDatas(string pNroSerie)
        {
            try
            {

                List<HeaderCuttingData> dataGen = _Mapper.Map<List<HeaderCuttingData>>(_dbContext.SplCortegralEsts.AsNoTracking().Where(X => X.NoSerie.ToUpper().Equals(pNroSerie)).ToList());

                return Task.FromResult(dataGen);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<HeaderCuttingData> GetInfoHeaderCuttingData(decimal pIdCorte)
        {
            try
            {

                HeaderCuttingData dataGen = _Mapper.Map<HeaderCuttingData>(_dbContext.SplCortegralEsts.AsNoTracking().FirstOrDefault(X => X.IdCorte == pIdCorte));

                List<SectionCuttingData> dataSec = new();

                if (dataGen != null)
                {
                    dataSec = _Mapper.Map<List<SectionCuttingData>>(_dbContext.SplCorteseccEsts.AsNoTracking().Where(X => X.IdCorte == dataGen.IdCorte).ToList());
                    dataGen.SectionCuttingData = dataSec;
                }

                foreach (SectionCuttingData item in dataSec)
                {
                    item.DetailCuttingData = _Mapper.Map<List<DetailCuttingData>>(_dbContext.SplCortedetaEsts.AsNoTracking().Where(X => X.IdCorte == dataGen.IdCorte).ToList());
                    foreach (DetailCuttingData item2 in item.DetailCuttingData)
                    {
                        item2.TempR = (item2.Tiempo > 0 && item2.Resistencia > 0) ? (item2.Resistencia * ((dataGen.Constante + item.TempResistencia) / item.Resistencia)) - dataGen.Constante : item2.TempR;
                    }
                }
                return Task.FromResult(dataGen);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<long> saveCuttingData(HeaderCuttingData pData)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                if (pData.IdCorte > 0)
                {
                    SplCortegralEst dataGen = _dbContext.SplCortegralEsts.AsNoTracking().FirstOrDefault(X => X.IdCorte == pData.IdCorte);
                    List<SplCorteseccEst> dataSec = _dbContext.SplCorteseccEsts.AsNoTracking().Where(X => X.IdCorte == dataGen.IdCorte).ToList();
                    List<SplCortedetaEst> dataDet = _dbContext.SplCortedetaEsts.AsNoTracking().Where(X => X.IdCorte == dataGen.IdCorte).ToList();

                    _dbContext.SplCortedetaEsts.RemoveRange(dataDet);
                    _dbContext.SplCorteseccEsts.RemoveRange(dataSec);
                    _ = _dbContext.SplCortegralEsts.Remove(dataGen);
                    _ = _dbContext.SaveChanges();
                }

                SplCortegralEst insertData = _Mapper.Map<SplCortegralEst>(pData);
                _ = _dbContext.SplCortegralEsts.Add(insertData);
                _ = _dbContext.SaveChanges();

                List<SplCorteseccEst> insertDataSec = _Mapper.Map<List<SplCorteseccEst>>(pData.SectionCuttingData);
                insertDataSec.ForEach(x => x.IdCorte = insertData.IdCorte);
                _dbContext.SplCorteseccEsts.AddRange(insertDataSec);
                _ = _dbContext.SaveChanges();

                List<SplCortedetaEst> insertDataDet;

                foreach (SectionCuttingData item in pData.SectionCuttingData)
                {
                    insertDataDet = _Mapper.Map<List<SplCortedetaEst>>(item.DetailCuttingData);
                    insertDataDet.ForEach(x => x.IdCorte = insertData.IdCorte);
                    _dbContext.SplCortedetaEsts.AddRange(insertDataDet);
                    _ = _dbContext.SaveChanges();
                }
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(insertData.IdCorte));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);
            }
        }



        #endregion
    }
}
