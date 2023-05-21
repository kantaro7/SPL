namespace SPL.Artifact.Infrastructure.Artifacdesign
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
    using SPL.Domain.SPL.Artifact.ArtifactDesign;
    using SPL.Domain.SPL.Security;

    public class ArtifactdesignInfrastructure : IArtifactdesignInfrastructure
    {

        private readonly dbDevMigSPLContext _dbContext;
        private readonly IMapper _Mapper;

        public ArtifactdesignInfrastructure(IMapper Map, dbDevMigSPLContext dbContext)
        {
            this._Mapper = Map;
            this._dbContext = dbContext;
        }

        #region Methods

        public async Task<InformationArtifact> GetGeneralArtifactdesign(string serial)

        {
            //var b = this._dbContext.SplInfoaparatoDgs.Where(x => x.OrderCode == "G3814-01").FirstOrDefault();
            //var b2 = this._dbContext.SplInfoaparatoCaps.Where(x => x.OrderCode == "G3814-01").FirstOrDefault();
            //var b3 = this._dbContext.SplInfoaparatoAprs.Where(x => x.OrderCode == "G3814-01").FirstOrDefault();
            //var b4 = this._dbContext.SplInfoaparatoGars.Where(x => x.OrderCode == "G3814-01").FirstOrDefault();
            //var b5 = this._dbContext.SplInfoaparatoLabs.Where(x => x.OrderCode == "G3814-01").FirstOrDefault();
            //var b6 = this._dbContext.SplInfoaparatoNors.Where(x => x.OrderCode == "G3814-01").FirstOrDefault();

            #region localFunctions
            Task<SplInfoaparatoDg> GetSplInfoaparatoDgs(string serial) => Task.FromResult(this._dbContext.SplInfoaparatoDgs.AsNoTracking().FirstOrDefault(CE => CE.OrderCode == serial));
            Task<IEnumerable<SplInfoaparatoCap>> GetSplInfoaparatoCaps(string serial) => Task.FromResult(this._dbContext.SplInfoaparatoCaps.AsNoTracking().Where(CE => CE.OrderCode == serial).AsEnumerable());
            Task<SplInfoaparatoGar> GetSplInfoaparatoGars(string serial) => Task.FromResult(this._dbContext.SplInfoaparatoGars.AsNoTracking().FirstOrDefault(CE => CE.OrderCode == serial));
            Task<IEnumerable<SplInfoaparatoApr>> GetSplInfoaparatoAprs(string serial) => Task.FromResult(this._dbContext.SplInfoaparatoAprs.AsNoTracking().Where(CE => CE.OrderCode == serial).AsEnumerable());
            Task<IEnumerable<SplInfoaparatoCam>> GetSplInfoaparatoCams(string serial) => Task.FromResult(this._dbContext.SplInfoaparatoCams.AsNoTracking().Where(CE => CE.OrderCode == serial).AsEnumerable());
            Task<SplInfoaparatoLab> GetSplInfoaparatoLabs(string serial) => Task.FromResult(this._dbContext.SplInfoaparatoLabs.AsNoTracking().FirstOrDefault(CE => CE.OrderCode == serial));
            Task<IEnumerable<SplInfoaparatoNor>> GetSplInfoaparatoNors(string serial) => Task.FromResult(this._dbContext.SplInfoaparatoNors.AsNoTracking().Where(CE => CE.OrderCode == serial).AsEnumerable());
            #endregion
            //return this._Mapper.Map<GeneralArtifact>(result);
            Task<SplInfoaparatoDg> TaskGen = GetSplInfoaparatoDgs(serial);
            Task<List<NozzlesArtifact>> TaskNozz = this.GetSplInfoaparatoBoqs(serial);
            Task<IEnumerable<SplInfoaparatoCap>> TaskChar = GetSplInfoaparatoCaps(serial);
            Task<SplInfoaparatoGar> TaskWar = GetSplInfoaparatoGars(serial);
            Task<IEnumerable<SplInfoaparatoApr>> TaskLig = GetSplInfoaparatoAprs(serial);
            Task<IEnumerable<SplInfoaparatoCam>> TaskCam = GetSplInfoaparatoCams(serial);
            Task<SplInfoaparatoLab> TaskLab = GetSplInfoaparatoLabs(serial);
            Task<IEnumerable<SplInfoaparatoNor>> TaskNor = GetSplInfoaparatoNors(serial);

            List<Task> generalArtifactTasks = new() { TaskGen, TaskNozz, TaskChar, TaskWar, TaskLig, TaskCam, TaskLab, TaskNor };

            await Task.WhenAny(generalArtifactTasks).Result;

            return new InformationArtifact(
                this._Mapper.Map<GeneralArtifact>(TaskGen.Result),
                TaskNozz.Result,
                this._Mapper.Map<List<ChangingTablesArtifact>>(TaskCam.Result),
                this._Mapper.Map<List<CharacteristicsArtifact>>(TaskChar.Result.ToList()),
                this._Mapper.Map<List<LightningRodArtifact>>(TaskLig.Result),
                this._Mapper.Map<List<RulesArtifact>>(TaskNor.Result),
                this._Mapper.Map<WarrantiesArtifact>(TaskWar.Result),
                this._Mapper.Map<LabTestsArtifact>(TaskLab.Result));
        }

        public Task<List<NozzlesArtifact>> GetSplInfoaparatoBoqs(string serial) => Task.FromResult(this._Mapper.Map<List<NozzlesArtifact>>(this._dbContext.SplInfoaparatoBoqs.AsNoTracking().Where(CE => CE.OrderCode == serial).AsEnumerable()));

        public Task<bool> CheckOrderNumber(string serial) => Task.FromResult(this._dbContext.SplInfoaparatoDgs.Where(CE => CE.OrderCode == serial).ToList().Exists(x => x.OrderCode == serial));

        public Task<long> SaveInformationArtifact(InformationArtifact pData)
        {

            using IDbContextTransaction transaction = this._dbContext.Database.BeginTransaction();
            try
            {
                if (pData.GeneralArtifact != null)
                {
                    _ = this._dbContext.SplInfoaparatoDgs.Add(this._Mapper.Map<SplInfoaparatoDg>(pData.GeneralArtifact));
                    _ = this._dbContext.SplInfoaparatoCars.Add(this._Mapper.Map<SplInfoaparatoCar>(new InfoCarLocal(true, pData.GeneralArtifact.OrderCode, pData.VoltageKV, pData.NBAI, pData.Connections, pData.Derivations, pData.Taps, pData.NBAINeutro, pData.GeneralArtifact.Creadopor)));
                }

                if (pData.NozzlesArtifact != null)
                {
                    this._dbContext.SplInfoaparatoBoqs.AddRange(this._Mapper.Map<IEnumerable<SplInfoaparatoBoq>>(pData.NozzlesArtifact));
                }

                if (pData.CharacteristicsArtifact != null)
                {

                    foreach (CharacteristicsArtifact item in pData.CharacteristicsArtifact)
                    {
                        if (item.DevAwr == 0)
                        {
                            item.DevAwr = null;
                        }

                        if (item.Hstr == 0)
                        {
                            item.Hstr = null;
                        }
                    }

                    this._dbContext.SplInfoaparatoCaps.AddRange(this._Mapper.Map<IEnumerable<SplInfoaparatoCap>>(pData.CharacteristicsArtifact.OrderBy(x => x.OverElevation)));
                }

                if (pData.WarrantiesArtifact != null)
                {
                    _ = this._dbContext.SplInfoaparatoGars.Add(this._Mapper.Map<SplInfoaparatoGar>(pData.WarrantiesArtifact));

                }

                if (pData.LightningRodArtifact != null)
                {
                    this._dbContext.SplInfoaparatoAprs.AddRange(this._Mapper.Map<IEnumerable<SplInfoaparatoApr>>(pData.LightningRodArtifact));
                }

                if (pData.ChangingTablesArtifact != null)
                {
                    this._dbContext.SplInfoaparatoCams.AddRange(this._Mapper.Map<IEnumerable<SplInfoaparatoCam>>(pData.ChangingTablesArtifact));
                }

                if (pData.Tapbaan != null)
                {
                    _ = this._dbContext.SplInfoaparatoTaps.Add(this._Mapper.Map<SplInfoaparatoTap>(pData.Tapbaan));
                }

                if (pData.LabTestsArtifact != null)
                {
                    _ = this._dbContext.SplInfoaparatoLabs.Add(this._Mapper.Map<SplInfoaparatoLab>(pData.LabTestsArtifact));
                }

                if (pData.RulesArtifact != null)
                {
                    this._dbContext.SplInfoaparatoNors.AddRange(this._Mapper.Map<IEnumerable<SplInfoaparatoNor>>(pData.RulesArtifact));
                }

                _ = this._dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }

            catch (Exception)
            {
                transaction.Rollback();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Error));

            }
        }

        public LabTestsArtifact LabTestsArtifact { get; set; }

        public Task<long> UpdategeneralArtifac(GeneralArtifact pData)
        {

            using IDbContextTransaction transaction = this._dbContext.Database.BeginTransaction();
            try
            {

                SplInfoaparatoDg generalArtifact = this._dbContext.SplInfoaparatoDgs.AsNoTracking().FirstOrDefault(CE => CE.OrderCode == pData.OrderCode);
                _ = generalArtifact is null
                    ? this._dbContext.SplInfoaparatoDgs.Add(this._Mapper.Map<SplInfoaparatoDg>(pData))
                    : this._dbContext.SplInfoaparatoDgs.Update(this._Mapper.Map<SplInfoaparatoDg>(pData));

                _ = this._dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }

            catch (Exception)
            {
                transaction.Rollback();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Error));

            }
        }
        public Task<long> UpdateNozzlesArtifact(List<NozzlesArtifact> pData)
        {

            using IDbContextTransaction transaction = this._dbContext.Database.BeginTransaction();
            try
            {
                IEnumerable<SplInfoaparatoBoq> boq = this._dbContext.SplInfoaparatoBoqs.AsNoTracking().Where(CE => CE.OrderCode == pData.FirstOrDefault().OrderCode);

                this._dbContext.SplInfoaparatoBoqs.RemoveRange(boq);
                _ = this._dbContext.SaveChanges();

                this._dbContext.SplInfoaparatoBoqs.AddRange(this._Mapper.Map<IEnumerable<SplInfoaparatoBoq>>(pData));
                _ = this._dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }

            catch (Exception)
            {
                transaction.Rollback();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Error));

            }
        }
        public Task<long> UpdateChangingTablesArtifact(AllChangingTablesArtifact pData)
        {

            using IDbContextTransaction transaction = this._dbContext.Database.BeginTransaction();
            try
            {

                if (pData.Changetables.Count() != 0 && pData.Changetables is not null)
                {
                    IEnumerable<SplInfoaparatoCam> changingTable = this._dbContext.SplInfoaparatoCams.AsNoTracking().Where(CE => CE.OrderCode == pData.Changetables.FirstOrDefault().OrderCode);

                    this._dbContext.SplInfoaparatoCams.RemoveRange(changingTable);
                    _ = this._dbContext.SaveChanges();

                    this._dbContext.SplInfoaparatoCams.AddRange(this._Mapper.Map<IEnumerable<SplInfoaparatoCam>>(pData.Changetables));
                    _ = this._dbContext.SaveChanges();

                }

                SplInfoaparatoTap taps = this._dbContext.SplInfoaparatoTaps.AsNoTracking().FirstOrDefault(CE => CE.OrderCode == pData.Changetables.FirstOrDefault().OrderCode);

                _ = taps is null
                    ? this._dbContext.SplInfoaparatoTaps.Add(this._Mapper.Map<SplInfoaparatoTap>(pData.Tapbaan))
                    : this._dbContext.SplInfoaparatoTaps.Update(this._Mapper.Map<SplInfoaparatoTap>(pData.Tapbaan));

                _ = this._dbContext.SaveChanges();
                transaction.Commit();

                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }

            catch (Exception)
            {
                transaction.Rollback();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Error));

            }
        }
        public Task<long> UpdateCharacteristicsArtifact(AllCharacteristicsArtifact pData)
        {

            using IDbContextTransaction transaction = this._dbContext.Database.BeginTransaction();
            try
            {
                string modificadoPor = "";
                foreach (CharacteristicsArtifact item in pData.ListEnfriamientos)
                {
                    if (item.DevAwr == 0)
                    {
                        item.DevAwr = null;
                    }

                    if (item.Hstr == 0)
                    {
                        item.Hstr = null;
                    }
                    modificadoPor = item.Modificadopor;
                }
                SplInfoaparatoCar Objcharacteris = this._dbContext.SplInfoaparatoCars.AsNoTracking().FirstOrDefault(CE => CE.OrderCode == pData.CodOrden);
                String creadoPor = (Objcharacteris is null)?modificadoPor:Objcharacteris.Creadopor;
                _ = Objcharacteris is null
                    ? this._dbContext.SplInfoaparatoCars.Add(this._Mapper.Map<SplInfoaparatoCar>(new InfoCarLocal(true, pData.CodOrden, pData.VoltageKV, pData.NBAI, pData.Connections, pData.Derivations, pData.Taps, pData.NBAINeutro, creadoPor)))
                    : this._dbContext.SplInfoaparatoCars.Update(this._Mapper.Map<SplInfoaparatoCar>(new InfoCarLocal(false, pData.CodOrden, pData.VoltageKV, pData.NBAI, pData.Connections, pData.Derivations, pData.Taps, pData.NBAINeutro, Objcharacteris.Fechacreacion, creadoPor, modificadoPor)));

                IEnumerable<SplInfoaparatoCap> characteris = this._dbContext.SplInfoaparatoCaps.AsNoTracking().Where(CE => CE.OrderCode == pData.CodOrden);

                this._dbContext.SplInfoaparatoCaps.RemoveRange(characteris);
                _ = this._dbContext.SaveChanges();

                this._dbContext.SplInfoaparatoCaps.AddRange(EvalCoolingType(this._Mapper.Map<IEnumerable<SplInfoaparatoCap>>(pData.ListEnfriamientos).ToList()));
                _ = this._dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }

            catch (Exception)
            {
                transaction.Rollback();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Error));

            }
        }
        public Task<long> UpdateLightningRodArtifact(List<LightningRodArtifact> pData)
        {

            using IDbContextTransaction transaction = this._dbContext.Database.BeginTransaction();
            try
            {

                IEnumerable<SplInfoaparatoApr> apartarayos = this._dbContext.SplInfoaparatoAprs.AsNoTracking().Where(CE => CE.OrderCode == pData.FirstOrDefault().OrderCode);

                this._dbContext.SplInfoaparatoAprs.RemoveRange(apartarayos);
                _ = this._dbContext.SaveChanges();

                this._dbContext.SplInfoaparatoAprs.AddRange(this._Mapper.Map<IEnumerable<SplInfoaparatoApr>>(pData));
                _ = this._dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }

            catch (Exception)
            {
                transaction.Rollback();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Error));

            }
        }
        public Task<long> UpdateRulesArtifact(List<RulesArtifact> pData)
        {

            using IDbContextTransaction transaction = this._dbContext.Database.BeginTransaction();
            try
            {

                IEnumerable<SplInfoaparatoNor> normas = this._dbContext.SplInfoaparatoNors.AsNoTracking().Where(CE => CE.OrderCode == pData.FirstOrDefault().OrderCode);

                this._dbContext.SplInfoaparatoNors.RemoveRange(normas);
                _ = this._dbContext.SaveChanges();

                this._dbContext.SplInfoaparatoNors.AddRange(this._Mapper.Map<IEnumerable<SplInfoaparatoNor>>(pData));

                _ = this._dbContext.SaveChanges();

                transaction.Commit();

                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }

            catch (Exception)
            {
                transaction.Rollback();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Error));

            }
        }

        public Task<long> UpdateWarrantiesArtifact(WarrantiesArtifact pData)
        {

            using IDbContextTransaction transaction = this._dbContext.Database.BeginTransaction();
            try
            {

                SplInfoaparatoGar garantias = this._dbContext.SplInfoaparatoGars.AsNoTracking().FirstOrDefault(CE => CE.OrderCode == pData.OrderCode);
                _ = garantias is null
                    ? this._dbContext.SplInfoaparatoGars.Add(this._Mapper.Map<SplInfoaparatoGar>(pData))
                    : this._dbContext.SplInfoaparatoGars.Update(this._Mapper.Map<SplInfoaparatoGar>(pData));

                _ = this._dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }

            catch (Exception)
            {
                transaction.Rollback();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Error));

            }
        }
        public Task<long> UpdateLabTestsArtifact(LabTestsArtifact pData)
        {

            using IDbContextTransaction transaction = this._dbContext.Database.BeginTransaction();
            try
            {

                SplInfoaparatoLab pruebas = this._dbContext.SplInfoaparatoLabs.AsNoTracking().FirstOrDefault(CE => CE.OrderCode == pData.OrderCode);
                _ = pruebas is null
                    ? this._dbContext.SplInfoaparatoLabs.Add(this._Mapper.Map<SplInfoaparatoLab>(pData))
                    : this._dbContext.SplInfoaparatoLabs.Update(this._Mapper.Map<SplInfoaparatoLab>(pData));

                _ = this._dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }

            catch (Exception)
            {
                transaction.Rollback();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Error));

            }
        }

        public Task<InfoCarLocal> GetInfoCarLocal(string nroSerie) => Task.FromResult(this._Mapper.Map<InfoCarLocal>(this._dbContext.SplInfoaparatoCars.FirstOrDefault(x => x.OrderCode == nroSerie)));

        public Task<TapBaan> GetTapBaan(string nroSerie) => Task.FromResult(this._Mapper.Map<TapBaan>(this._dbContext.SplInfoaparatoTaps.FirstOrDefault(x => x.OrderCode == nroSerie)));

        #endregion

        private static IEnumerable<SplInfoaparatoCap> EvalCoolingType(List<SplInfoaparatoCap> list)
        {
            List<SplInfoaparatoCap> result = new();
            if (list.Count > 0)
            {
                result.AddRange(renameCoolingType(list, true));
                result.AddRange(renameCoolingType(list, false));
            }
            for (int i = 0; i < result.Count; i++)
            {
                result[i].Secuencia = i;
            }
            return result;
        }

        private static List<SplInfoaparatoCap> renameCoolingType(List<SplInfoaparatoCap> list, bool onan)
        {
            string type = onan ? "ONAN" : "ONAF";
            List<SplInfoaparatoCap> types = list.FindAll(x => x.CoolingType.Contains(type));
            List<SplInfoaparatoCap> result = new();
            int count = 1;
            if (types.Count > 1)
            {
                while (types.Count > 0)
                {
                    decimal? min = types.Min(x => x.OverElevation);
                    if (types.Where(x => x.OverElevation == min).Count() > 1)
                    {
                        decimal? minMva = types.Min(x => x.Mvaf1);
                        SplInfoaparatoCap mini = types.Find(x => x.Mvaf1 == minMva);
                        _ = types.Remove(mini);

                        mini.CoolingType = $"{type}{count}";
                        result.Add(mini);

                    }
                    else
                    {
                        SplInfoaparatoCap mini = types.Find(x => x.OverElevation == min);
                        _ = types.Remove(mini);
                        mini.CoolingType = $"{type}{count}";
                        result.Add(mini);
                    }
                    count = 2;
                }
            }
            else if (types.Count == 1)
            {
                SplInfoaparatoCap unico = types.First();
                unico.CoolingType = type;
                result.Add(unico);
            }

            return result;
        }

        public Task<List<ResistDesign>> GetResistDesign(string nroSerie, string unitOfMeasurement, string testConnection, decimal temperature, string idSection, decimal order)
        {
            try
            {
                return Task.FromResult(this._Mapper.Map<List<ResistDesign>>(this._dbContext.SplResistDisenos.Where(x => x.NoSerie == nroSerie && x.UnidadMedida.Equals(unitOfMeasurement) && x.ConexionPrueba.Equals(testConnection) && x.Temperatura == temperature && (x.IdSeccion.Equals(idSection) || idSection.Equals("-1")) && (x.Orden == order || order == -1))));

            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<long> SaveResistDesign(List<ResistDesign> pData)
        {
            using IDbContextTransaction transaction = this._dbContext.Database.BeginTransaction();
            try
            {
                var conexPrueba = pData.Select(x => x.ConexionPrueba).Distinct().ToList();
                // var a = this._dbContext.SplResistDisenos.ToList();
                List<SplResistDiseno> data = this._dbContext.SplResistDisenos.AsNoTracking().Where(x => x.NoSerie == pData.FirstOrDefault().NoSerie && conexPrueba.Contains(x.ConexionPrueba) && x.Temperatura == pData.FirstOrDefault().Temperatura && (x.UnidadMedida.ToUpper().Trim() == "OHMS" || x.UnidadMedida.ToUpper().Trim() == "MILIOHMS")).ToList();

               

                //if (data.Count() > 0)
                //{
                //    foreach (ResistDesign item in pData)
                //    {

                //        item.Creadopor = data.FirstOrDefault().Creadopor;
                //        item.Fechacreacion = data.FirstOrDefault().Fechacreacion;
                //        item.Fechamodificacion = DateTime.Now;
                //        item.Modificadopor = item.Modificadopor;
                //    }
                //    this._dbContext.SplResistDisenos.UpdateRange(this._Mapper.Map<IEnumerable<SplResistDiseno>>(pData));
                //    _ = this._dbContext.SaveChanges();
                //}
                //else
                //{
                //foreach (ResistDesign item in pData)
                //    {
                //       // item.Creadopor =  item.Creadopor;
                //        item.Fechacreacion = DateTime.Now;
                //    }

                    if (data.Count() > 0)
                    {
                        pData.ForEach(x => x.Creadopor = data[0].Creadopor);
                        pData.ForEach(x => x.Fechacreacion = data[0].Fechacreacion);
                        _dbContext.SplResistDisenos.RemoveRange(data);
                        _dbContext.SaveChanges();
                        data.Clear();
                    }
                    else
                    {
                        pData.ForEach(x => x.Modificadopor = null);
                        pData.ForEach(x => x.Fechamodificacion = null);
                    }

                this._dbContext.SplResistDisenos.AddRange(this._Mapper.Map<IEnumerable<SplResistDiseno>>(pData));
                    _ = this._dbContext.SaveChanges();
                //}

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);

            }
        }

        public Task<List<ResistDesign>> GetResistDesignCustom(string nroSerie, string unitOfMeasurement, string testConnection, decimal temperature, string idSection, decimal order)
        {
            try
            {
                var generalResult = this._Mapper.Map<List<ResistDesign>>(this._dbContext.SplResistDisenos.Where(x => x.NoSerie == nroSerie));

                if(unitOfMeasurement != "-1")
                {
                    generalResult = generalResult.Where(x => x.UnidadMedida.Trim().ToLower() == unitOfMeasurement.Trim().ToLower()).ToList();
                }

                if(testConnection != "-1")
                {
                    generalResult = generalResult.Where(x => x.ConexionPrueba.Trim().ToLower() == testConnection.Trim().ToLower()).ToList();
                }

                if (temperature != -1)
                {
                    generalResult = generalResult.Where(x => x.Temperatura == temperature).ToList();
                }

                if (idSection != "-1")
                {
                    generalResult = generalResult.Where(x => x.IdSeccion.Trim().ToUpper() == idSection.Trim().ToUpper()).ToList();
                }

                return Task.FromResult(generalResult);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task<decimal> GetBoqTerciary(string serial)
        {
            decimal qty = 0;
            try
            {

                var boqquilla = await this._dbContext.SplInfoaparatoBoqs.Where(x => x.OrderCode == serial && x.ColumnTitle.Contains("Terc")).FirstOrDefaultAsync();

                if(boqquilla == null)
                {
                    return qty;
                }
                else
                {
                    if(boqquilla?.Qty == null)
                    {
                        return qty;
                    }
                    else
                    {
                        return (decimal)boqquilla?.Qty;
                    }
                }

            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
