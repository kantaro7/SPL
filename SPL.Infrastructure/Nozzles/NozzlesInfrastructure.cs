namespace SPL.Artifact.Infrastructure.Nozzles
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;

    using SPL.Artifact.Infrastructure;
    using SPL.Artifact.Infrastructure.Entities;
    using SPL.Domain;
    using SPL.Domain.SPL.Artifact.Nozzles;

    public class NozzlesInfrastructure : INozzlesInfrastructure
    {
        private readonly dbDevMigSPLContext _dbContext;
        private readonly IMapper _Mapper;

        public NozzlesInfrastructure(IMapper Map, dbDevMigSPLContext dbContext)
        {
            this._Mapper = Map;
            this._dbContext = dbContext;
        }

        #region Methods
        public Task<List<RecordNozzleInformation>> GetRecordNozzleInformation(string nroSerie)
        {
            List<SqlParameter> parametros = new();
            string a = this._dbContext.Database.GetConnectionString();
            DataSet ds = Methods.ejecutarConsultaOFuncion($"SELECT * FROM [dbo].[GET_INFO_APARATO_DET_BOQ]('{nroSerie}')", parametros, "Data", this._dbContext.Database.GetConnectionString());

            return Task.FromResult(ds.Tables["Data"].AsEnumerable().Select(result => new RecordNozzleInformation()
            {
                NoSerie = result["NO_SERIE"].ToString(),
                NoSerieBoq = result["NO_SERIE_BOQ"].ToString(),
                Orden = Convert.ToDecimal(result["ORDEN"].ToString()),
                Posicion = result["POSICION"].ToString(),
                IdMarca = Convert.ToDecimal(result["ID_MARCA"].ToString()),
                Marca = result["DESCRIPCION_MARCA"].ToString(),
                IdTipo = Convert.ToDecimal(result["ID_TIPO"].ToString()),
                Tipo = result["DESCRIPCION_TIPO"].ToString(),
                FactorPotencia = Convert.ToDecimal(result["FACTOR_POTENCIA"].ToString()),
                Capacitancia = Convert.ToDecimal(result["CAPACITANCIA"].ToString()),
                Corriente = Convert.ToDecimal(result["CORRIENTE"].ToString()),
                Voltaje = Convert.ToDecimal(result["VOLTAJE"].ToString()),
                Prueba = Convert.ToBoolean(result["PRUEBA"].ToString()),
                Creadopor = result["CREADOPOR"].ToString(),
                Modificadopor = result["MODIFICADOPOR"].ToString(),
                Fechacreacion = string.IsNullOrEmpty(result["FECHACREACION"].ToString()) ? new DateTime() : Convert.ToDateTime(result["FECHACREACION"].ToString()),
                Fechamodificacion = string.IsNullOrEmpty(result["FECHAMODIFICACION"].ToString()) ? new DateTime() : Convert.ToDateTime(result["FECHAMODIFICACION"].ToString()),
                FactorPotencia2 = Convert.ToDecimal(result["FACTOR_POTENCIA2"].ToString()),
                Capacitancia2 = Convert.ToDecimal(result["CAPACITANCIA2"].ToString()),

            }).OrderBy(x => x.Orden).ToList());
        }

        public Task<long> saveRecordNozzleInformation(NozzlesByDesign pData)
        {
            using IDbContextTransaction transaction = this._dbContext.Database.BeginTransaction();
            try
            {
                List<SplInfoaparatoBoqdet> newData = pData.NozzleInformation.Select(item => new SplInfoaparatoBoqdet() {
                    Capacitancia = item.Capacitancia,
                    Corriente = item.Corriente,
                    Modificadopor = item.Modificadopor,
                    Fechamodificacion = item.Fechamodificacion,
                    Creadopor = item.Creadopor,
                    FactorPotencia = item.FactorPotencia,
                    Fechacreacion = DateTime.Now,
                    IdMarca = item.IdMarca,
                    IdTipo = item.IdTipo,
                    NoSerie = item.NoSerie,
                    NoSerieBoq = item.NoSerieBoq,
                    Posicion = item.Posicion,
                    Prueba = item.Prueba,
                    Voltaje = item.Voltaje,
                    FactorPotencia2 = item.FactorPotencia2,
                    Capacitancia2 = item.Capacitancia2,
                }).ToList();

                //pData.NozzleInformation = pData.NozzleInformation.OrderBy(x => x.Orden).ToList();
                List<SplInfoaparatoBoqdet> data = this._dbContext.SplInfoaparatoBoqdets.AsNoTracking().Where(CE => CE.NoSerie.Equals(pData.NozzleInformation.FirstOrDefault().NoSerie)).ToList();
                if (data.Count > 0)
                {
                    newData.ForEach(x => x.Fechacreacion = data[0].Fechacreacion);
                    newData.ForEach(x => x.Creadopor = data[0].Creadopor);
                }
                this._dbContext.RemoveRange(data);
                _ = this._dbContext.SaveChanges();

                //IEnumerable<string> sNozzs = pData.NozzleInformation.Select(x => x.NoSerieBoq);

                //// Borrando registros
                //for (int i = 0; i < data.Count; i++)
                //{
                //    if (!sNozzs.Any(x => x.Equals(data[i].NoSerieBoq)))
                //    {
                //        data[i] = null;
                //    }
                //}

                //_ = data.RemoveAll(x => x is null);

                //List<SplInfoaparatoBoqdet> data2 = this._dbContext.SplInfoaparatoBoqdets.AsNoTracking().Where(CE => !CE.NoSerie.Equals(pData.NozzleInformation.FirstOrDefault().NoSerie)).ToList();

                //// Actualizando registros
                //foreach (SplInfoaparatoBoqdet item in data)
                //{
                //    if (pData.NozzleInformation.Exists(x => x.NoSerieBoq.Equals(item.NoSerieBoq)))
                //    {

                //        RecordNozzleInformation element = pData.NozzleInformation.FirstOrDefault(x => x.NoSerieBoq.Equals(item.NoSerieBoq));
                //        item.Posicion = element.Posicion;
                //        item.IdMarca = element.IdMarca;
                //        item.IdTipo = element.IdTipo;
                //        item.FactorPotencia = element.FactorPotencia;
                //        item.Capacitancia = element.Capacitancia;
                //        item.Corriente = element.Corriente;
                //        item.Voltaje = element.Voltaje;
                //        item.Prueba = element.Prueba;
                //        item.Modificadopor = element.Modificadopor;
                //        item.Fechamodificacion = DateTime.Now;
                //        item.FactorPotencia2 = element.FactorPotencia2;
                //        item.Capacitancia2 = element.Capacitancia2;
                //    }
                //}

                //// Agregando nuevos registros
                //foreach (RecordNozzleInformation item in pData.NozzleInformation)
                //{
                //    if (!data.Exists(x => x.NoSerieBoq.Equals(item.NoSerieBoq)))
                //    {
                //        if (!data2.Exists(X => X.NoSerieBoq.Equals(item.NoSerieBoq)))
                //        {
                //            data.Add(new SplInfoaparatoBoqdet()
                //            {
                //                Capacitancia = item.Capacitancia,
                //                Corriente = item.Corriente,
                //                Creadopor = item.Creadopor,
                //                FactorPotencia = item.FactorPotencia,
                //                Fechacreacion = DateTime.Now,
                //                IdMarca = item.IdMarca,
                //                IdTipo = item.IdTipo,
                //                NoSerie = item.NoSerie,
                //                NoSerieBoq = item.NoSerieBoq,
                //                Posicion = item.Posicion,
                //                Prueba = item.Prueba,
                //                Voltaje = item.Voltaje,
                //                FactorPotencia2 = item.FactorPotencia2,
                //                Capacitancia2 = item.Capacitancia2
                //            });
                //        }
                //        else
                //        {
                //            throw new ArgumentException("El número de boquilla: " + item.NoSerieBoq + " ya lo está usando otro aparato");
                //        }
                //    }
                //}

                //// Asignando consecutivo
                //int consecutivo = 1;
                //foreach (SplInfoaparatoBoqdet item in data)
                //{
                //    item.Orden = consecutivo;
                //    consecutivo++;
                //}
                //this._dbContext.AddRange(data);
                //_ = this._dbContext.SaveChanges();

                // Asignando consecutivo
                int consecutivo = 1;
                foreach (SplInfoaparatoBoqdet item in newData)
                {
                    item.Orden = consecutivo;
                    consecutivo++;
                }
                this._dbContext.AddRange(newData);
                _ = this._dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
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
