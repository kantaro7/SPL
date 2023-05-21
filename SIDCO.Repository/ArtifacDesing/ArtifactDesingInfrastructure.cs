namespace SIDCO.Infrastructure.Artifacdesign
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using SIDCO.Domain.Artifactdesign;
    using SIDCO.Domain.Domain.Models;
    using SIDCO.Infrastructure.Common;
    using SIDCO.Infrastructure.Functions;

    public class ArtifactdesignInfrastructure : IArtifactdesignInfrastructure
    {

        private readonly dbDevSIDCOContext _dbContext;
        private readonly IMapper _Mapper;
        private readonly IConfiguration _configuration;
        // DcoMvaCharacteristic dcoMvaCharacteristics;
        private readonly ILogger<ArtifactdesignInfrastructure> _logger;
        public ArtifactdesignInfrastructure(IMapper Map, dbDevSIDCOContext dbContext, IConfiguration pConfiguration, ILogger<ArtifactdesignInfrastructure> logger)
        {
            this._logger = logger;
            this._Mapper = Map;
            this._dbContext = dbContext;
            this._configuration = pConfiguration;

        }

        #region Methods
        public async Task<InformationArtifact> GetGeneralArtifactdesign(string serial)
        {

            using (dbDevSIDCOContext _context = new())
            {

                try
                {

                }
                catch (Exception)
                {

                    throw;
                }

                #region localFunctions
                Task<FnGetInfoDatosGenerales> GetFnGetInfoDatosGenerales(string serial)
                {

                    List<SqlParameter> parametros = new()
                    {

                        //if (serial != )
                        Methods.CrearSqlParameter("@V_NO_SERIE", SqlDbType.VarChar, serial)
                    };

                    //EJECUTAR CONSULTA
                    DataSet ds = Methods.ejecutarConsultaOFuncion("SELECT * FROM [dbo].[FN_GET_INFO_DATOS_GENERALES](@V_NO_SERIE)", parametros, "DatosGenerales", _context.Database.GetConnectionString());

                    return Task.FromResult((ds.Tables["DatosGenerales"].Rows.Count > 0) ? new FnGetInfoDatosGenerales()
                    {
                        OrderCode = serial,
                        Aplicacion = ds.Tables["DatosGenerales"].Rows[0]["Aplicacion"].ToString(),
                        Applicationid = Convert.ToInt32(ds.Tables["DatosGenerales"].Rows[0]["Applicationid"].ToString()),
                        Cliente = ds.Tables["DatosGenerales"].Rows[0]["Cliente"].ToString(),
                        Norma_Aplicable = ds.Tables["DatosGenerales"].Rows[0]["Norma_Aplicable"].ToString(),
                        Standardid = Convert.ToInt32(ds.Tables["DatosGenerales"].Rows[0]["Standardid"].ToString()),
                        Tipo = ds.Tables["DatosGenerales"].Rows[0]["Tipo"].ToString(),
                        Typetrafoid = Convert.ToInt32(ds.Tables["DatosGenerales"].Rows[0]["Typetrafoid"].ToString())

                    } : new FnGetInfoDatosGenerales()); ;

                }
                Task<FnGetInfoDatosGenerales2> GetFnGetInfoDatosGenerales2(string serial)
                {

                    List<SqlParameter> parametros = new()
                    {

                        //if (serial != )
                        Methods.CrearSqlParameter("@V_NO_SERIE", SqlDbType.VarChar, serial)
                    };

                    //EJECUTAR CONSULTA
                    DataSet ds = Methods.ejecutarConsultaOFuncion("SELECT * FROM [dbo].[FN_GET_INFO_DATOS_GENERALES_2](@V_NO_SERIE)", parametros, "DatosGenerales2", _context.Database.GetConnectionString());

                    return Task.FromResult((ds.Tables["DatosGenerales2"].Rows.Count > 0) ? new FnGetInfoDatosGenerales2()
                    {
                        Altitud = ds.Tables["DatosGenerales2"].Rows[0]["Altitud"].ToString(),
                        Altitud_F_M = ds.Tables["DatosGenerales2"].Rows[0]["Altitud_F_M"].ToString(),
                        Fases = ds.Tables["DatosGenerales2"].Rows[0]["Fases"].ToString(),
                        Frec = ds.Tables["DatosGenerales2"].Rows[0]["Frec"].ToString(),

                    } : new FnGetInfoDatosGenerales2());

                }
                Task<FnGetInfoReqEsp> GetFnGetInfoReqEsp(string serial)
                {

                    List<SqlParameter> parametros = new()
                    {

                        //if (serial != )
                        Methods.CrearSqlParameter("@V_NO_SERIE", SqlDbType.VarChar, serial)
                    };

                    //EJECUTAR CONSULTA
                    DataSet ds = Methods.ejecutarConsultaOFuncion("SELECT * FROM [dbo].[FN_GET_INFO_REQ_ESP](@V_NO_SERIE)", parametros, "InfoReqEsp", _context.Database.GetConnectionString());

                    return Task.FromResult((ds.Tables["InfoReqEsp"].Rows.Count > 0) ? new FnGetInfoReqEsp()
                    {
                        DESPLAZAMIENTO_ANGULAR = ds.Tables["InfoReqEsp"].Rows[0]["DESPLAZAMIENTO_ANGULAR"].ToString(),
                        Polarity_id = string.IsNullOrEmpty(ds.Tables["InfoReqEsp"].Rows[0]["Polarity_id"].ToString()) ? 0 : Convert.ToInt32(ds.Tables["InfoReqEsp"].Rows[0]["Polarity_id"].ToString()),
                        Polarity_other = ds.Tables["InfoReqEsp"].Rows[0]["Polarity_other"].ToString(),

                    } : new FnGetInfoReqEsp());

                }
                Task<FunGetLenguageSidco> GetFunGetLenguageSidco(string serial)
                {

                    List<SqlParameter> parametros = new()
                    {

                        //if (serial != )
                        Methods.CrearSqlParameter("@V_NO_SERIE", SqlDbType.VarChar, serial)
                    };

                    //EJECUTAR CONSULTA
                    DataSet ds = Methods.ejecutarConsultaOFuncion("SELECT * FROM [dbo].[FN_GET_IDIOMASIDCO](@V_NO_SERIE)", parametros, "LenguageSidco", _context.Database.GetConnectionString());

                    return Task.FromResult((ds.Tables["LenguageSidco"].Rows.Count > 0) ? new FunGetLenguageSidco()
                    {
                        LENGUAJE = ds.Tables["LenguageSidco"].Rows[0]["LENGUAJE"].ToString(),
                        Lenguaje_id = Convert.ToInt32(ds.Tables["LenguageSidco"].Rows[0]["Language_id"].ToString()),
                        PO_NUMBER = ds.Tables["LenguageSidco"].Rows[0]["PO_NUMBER"].ToString(),

                    } : new FunGetLenguageSidco());

                }

                Task<List<NozzlesArtifact>> GetSidcoInfoaparatoBoqs(string serial)
                {

                    //LISTA DE PARÁMETROS
                    List<SqlParameter> parametros = new()
                    {

                        //if (serial != )
                        Methods.CrearSqlParameter("@V_NO_SERIE", SqlDbType.VarChar, serial)
                    };

                    //EJECUTAR CONSULTA
                    DataSet ds = Methods.ejecutarConsultaOFuncion("SELECT * FROM[dbo].[FN_GET_BOQUILLAS](@V_NO_SERIE)", parametros, "boquillas", _context.Database.GetConnectionString());

                    //decimal.Round(result.HasValue && Double.IsNaN(result.Value) ? 0 : Convert.ToDecimal(result), 3)

                    this._logger.LogInformation("pase por GetSidcoInfoaparatoBoqs");

                    return Task.FromResult(ds.Tables["boquillas"].AsEnumerable().Select(result => new NozzlesArtifact()
                    {
                        ColumnTitle = result["COLUMN_TITLE"].ToString(),
                        ColumnTypeId = string.IsNullOrEmpty(result["COLUMN_TYPE_ID"].ToString()) ? null : Convert.ToDecimal(result["COLUMN_TYPE_ID"].ToString()),
                        OrderIndex = string.IsNullOrEmpty(result["ORDER_INDEX"].ToString()) ? null : Convert.ToDecimal(result["ORDER_INDEX"].ToString()),
                        Qty = string.IsNullOrEmpty(result["QTY"].ToString()) ? null : Convert.ToDecimal(result["QTY"].ToString()),
                        VoltageClass = string.IsNullOrEmpty(result["VOLTAGE_CLASS"].ToString()) ? null : decimal.Round(Convert.ToDecimal(result["VOLTAGE_CLASS"].ToString()), 3),
                        BilClass = string.IsNullOrEmpty(result["BIL_CLASS"].ToString()) ? null : Convert.ToDecimal(result["BIL_CLASS"].ToString()),
                        CurrentAmps = string.IsNullOrEmpty(result["CURRENT_AMPS"].ToString()) ? null : Convert.ToDecimal(result["CURRENT_AMPS"].ToString()),
                        CurrentAmpsReq = string.IsNullOrEmpty(result["CURRENT_AMPS_REQ"].ToString()) ? null : Convert.ToDecimal(result["CURRENT_AMPS_REQ"].ToString())

                    }).ToList());

                }

                Task<List<CharacteristicsArtifact>> GetSidcoInfoaparatoCars(string serial)
                {
                    //LISTA DE PARÁMETROS
                    List<SqlParameter> parametros = new()
                    {

                        //if (serial != )
                        Methods.CrearSqlParameter("@V_NO_SERIE", SqlDbType.VarChar, serial)
                    };

                    //EJECUTAR CONSULTA
                    DataSet ds = Methods.ejecutarConsultaOFuncion("SELECT * FROM [dbo].[FN_GET_TIPO_ENFTO](@V_NO_SERIE)", parametros, "Cars", _context.Database.GetConnectionString());

                    return Task.FromResult(ds.Tables["Cars"].AsEnumerable().Select(result => new CharacteristicsArtifact()
                    {
                        CoolingType = result["COOLING_TYPE"].ToString(),
                        OverElevation = string.IsNullOrEmpty(result["OVER_ELEVATION"].ToString()) ? null : Convert.ToDecimal(result["OVER_ELEVATION"].ToString()),
                        Hstr = string.IsNullOrEmpty(result["HSTR"].ToString()) ? null : Convert.ToDecimal(result["HSTR"].ToString()),
                        DevAwr = string.IsNullOrEmpty(result["DEV_AWR"].ToString()) ? null : Convert.ToDecimal(result["DEV_AWR"].ToString()),
                        Mvaf1 = string.IsNullOrEmpty(result["MVAF1"].ToString()) ? null : Convert.ToDecimal(result["MVAF1"].ToString()),
                        Mvaf2 = string.IsNullOrEmpty(result["MVAF2"].ToString()) ? null : Convert.ToDecimal(result["MVAF2"].ToString()),
                        Mvaf3 = string.IsNullOrEmpty(result["MVAF3"].ToString()) ? null : Convert.ToDecimal(result["MVAF3"].ToString()),
                        Mvaf4 = string.IsNullOrEmpty(result["MVAF4"].ToString()) ? null : Convert.ToDecimal(result["MVAF4"].ToString()),
                        OrderCode = result["ORDER_CODE"].ToString(),

                    }).ToList());

                }

                Task<VoltageKV> GetSidcoVoltageKV(string serial)
                {

                    List<SqlParameter> parametros = new()
                    {

                        //if (serial != )
                        Methods.CrearSqlParameter("@V_NO_SERIE", SqlDbType.VarChar, serial)
                    };

                    //EJECUTAR CONSULTA
                    DataSet ds = Methods.ejecutarConsultaOFuncion("SELECT * FROM [dbo].[FN_GET_TENSION_KV](@V_NO_SERIE)", parametros, "voltageKV", _context.Database.GetConnectionString());

                    return Task.FromResult((ds.Tables["voltageKV"].Rows.Count > 0) ? new VoltageKV()
                    {
                        tensionkvaltatension1 = string.IsNullOrEmpty(ds.Tables["voltageKV"].Rows[0]["MVAF1_VOLTAGE1"].ToString()) ? null : Convert.ToDecimal(ds.Tables["voltageKV"].Rows[0]["MVAF1_VOLTAGE1"].ToString()),

                        tensionkvaltatension2 = string.IsNullOrEmpty(ds.Tables["voltageKV"].Rows[0]["MVAF1_VOLTAGE2"].ToString()) ? null : Convert.ToDecimal(ds.Tables["voltageKV"].Rows[0]["MVAF1_VOLTAGE2"].ToString()),

                        tensionkvbajatension1 = string.IsNullOrEmpty(ds.Tables["voltageKV"].Rows[0]["MVAF2_VOLTAGE1"].ToString()) ? null : Convert.ToDecimal(ds.Tables["voltageKV"].Rows[0]["MVAF2_VOLTAGE1"].ToString()),

                        tensionkvbajatension2 = string.IsNullOrEmpty(ds.Tables["voltageKV"].Rows[0]["MVAF2_VOLTAGE2"].ToString()) ? null : Convert.ToDecimal(ds.Tables["voltageKV"].Rows[0]["MVAF2_VOLTAGE2"].ToString()),

                        tensionkvsegundabaja1 = string.IsNullOrEmpty(ds.Tables["voltageKV"].Rows[0]["MVAF3_VOLTAGE1"].ToString()) ? null : Convert.ToDecimal(ds.Tables["voltageKV"].Rows[0]["MVAF3_VOLTAGE1"].ToString()),

                        tensionkvsegundabaja2 = string.IsNullOrEmpty(ds.Tables["voltageKV"].Rows[0]["MVAF3_VOLTAGE2"].ToString()) ? null : Convert.ToDecimal(ds.Tables["voltageKV"].Rows[0]["MVAF3_VOLTAGE2"].ToString()),

                        tensionkvterciario1 = string.IsNullOrEmpty(ds.Tables["voltageKV"].Rows[0]["MVAF4_VOLTAGE1"].ToString()) ? null : Convert.ToDecimal(ds.Tables["voltageKV"].Rows[0]["MVAF4_VOLTAGE1"].ToString()),

                        tensionkvterciario2 = string.IsNullOrEmpty(ds.Tables["voltageKV"].Rows[0]["MVAF4_VOLTAGE2"].ToString()) ? null : Convert.ToDecimal(ds.Tables["voltageKV"].Rows[0]["MVAF4_VOLTAGE2"].ToString()),

                        tensionkvaltatension3 = string.IsNullOrEmpty(ds.Tables["voltageKV"].Rows[0]["MVAF1_VOLTAGE3"].ToString()) ? null : Convert.ToDecimal(ds.Tables["voltageKV"].Rows[0]["MVAF1_VOLTAGE3"].ToString()),

                        tensionkvaltatension4 = string.IsNullOrEmpty(ds.Tables["voltageKV"].Rows[0]["MVAF1_VOLTAGE4"].ToString()) ? null : Convert.ToDecimal(ds.Tables["voltageKV"].Rows[0]["MVAF1_VOLTAGE4"].ToString()),

                        tensionkvbajatension3 = string.IsNullOrEmpty(ds.Tables["voltageKV"].Rows[0]["MVAF2_VOLTAGE3"].ToString()) ? null : Convert.ToDecimal(ds.Tables["voltageKV"].Rows[0]["MVAF2_VOLTAGE3"].ToString()),

                        tensionkvbajatension4 = string.IsNullOrEmpty(ds.Tables["voltageKV"].Rows[0]["MVAF2_VOLTAGE4"].ToString()) ? null : Convert.ToDecimal(ds.Tables["voltageKV"].Rows[0]["MVAF2_VOLTAGE4"].ToString()),

                        tensionkvsegundabaja3 = string.IsNullOrEmpty(ds.Tables["voltageKV"].Rows[0]["MVAF3_VOLTAGE3"].ToString()) ? null : Convert.ToDecimal(ds.Tables["voltageKV"].Rows[0]["MVAF3_VOLTAGE3"].ToString()),

                        tensionkvsegundabaja4 = string.IsNullOrEmpty(ds.Tables["voltageKV"].Rows[0]["MVAF3_VOLTAGE4"].ToString()) ? null : Convert.ToDecimal(ds.Tables["voltageKV"].Rows[0]["MVAF3_VOLTAGE4"].ToString()),

                        tensionkvterciario3 = string.IsNullOrEmpty(ds.Tables["voltageKV"].Rows[0]["MVAF3_VOLTAGE4"].ToString()) ? null : Convert.ToDecimal(ds.Tables["voltageKV"].Rows[0]["MVAF3_VOLTAGE4"].ToString()),

                        tensionkvterciario4 = string.IsNullOrEmpty(ds.Tables["voltageKV"].Rows[0]["MVAF4_VOLTAGE4"].ToString()) ? null : Convert.ToDecimal(ds.Tables["voltageKV"].Rows[0]["MVAF4_VOLTAGE4"].ToString()),

                    } : new VoltageKV());

                }

                Task<NBAIBilKv> GetSidcoNBAIKV(string serial)
                {

                    List<SqlParameter> parametros = new()
                    {

                        //if (serial != )
                        Methods.CrearSqlParameter("@V_NO_SERIE", SqlDbType.VarChar, serial)
                    };

                    //EJECUTAR CONSULTA
                    DataSet ds = Methods.ejecutarConsultaOFuncion("SELECT * FROM [dbo].[FN_GET_NBAI_BIL_KV](@V_NO_SERIE)", parametros, "NBAIBILKV", _context.Database.GetConnectionString());

                    return Task.FromResult((ds.Tables["NBAIBILKV"].Rows.Count > 0) ? new NBAIBilKv()
                    {
                        nbaialtatension = string.IsNullOrEmpty(ds.Tables["NBAIBILKV"].Rows[0]["MVAF1_NBAI1"].ToString()) ? null : Convert.ToDecimal(ds.Tables["NBAIBILKV"].Rows[0]["MVAF1_NBAI1"].ToString()),

                        nbaibajatension = string.IsNullOrEmpty(ds.Tables["NBAIBILKV"].Rows[0]["MVAF2_NBAI1"].ToString()) ? null : Convert.ToDecimal(ds.Tables["NBAIBILKV"].Rows[0]["MVAF2_NBAI1"].ToString()),

                        nbaisegundabaja = string.IsNullOrEmpty(ds.Tables["NBAIBILKV"].Rows[0]["MVAF3_NBAI1"].ToString()) ? null : Convert.ToDecimal(ds.Tables["NBAIBILKV"].Rows[0]["MVAF3_NBAI1"].ToString()),

                        nabaitercera = string.IsNullOrEmpty(ds.Tables["NBAIBILKV"].Rows[0]["MVAF4_NBAI1"].ToString()) ? null : Convert.ToDecimal(ds.Tables["NBAIBILKV"].Rows[0]["MVAF4_NBAI1"].ToString()),

                    } : new NBAIBilKv());

                }

                Task<ConnectionTypes> GetSidcoConnectionTypes(string serial)
                {

                    List<SqlParameter> parametros = new()
                    {

                        //if (serial != )
                        Methods.CrearSqlParameter("@V_NO_SERIE", SqlDbType.VarChar, serial)
                    };

                    //EJECUTAR CONSULTA
                    DataSet ds = Methods.ejecutarConsultaOFuncion("SELECT * FROM [dbo].[FN_GET_TIPO_CONEXION](@V_NO_SERIE)", parametros, "ConnectionTypes", _context.Database.GetConnectionString());

                    return Task.FromResult((ds.Tables["ConnectionTypes"].Rows.Count > 0) ? new ConnectionTypes()
                    {
                        otraconexionaltatension = ds.Tables["ConnectionTypes"].Rows[0]["MVAF1_CONNECTION_OTHER"].ToString(),
                        otraconexionbajatension = ds.Tables["ConnectionTypes"].Rows[0]["MVAF2_CONNECTION_OTHER"].ToString(),

                        otraconexionsegundabaja = ds.Tables["ConnectionTypes"].Rows[0]["MVAF3_CONNECTION_OTHER"].ToString(),
                        otraconexiontercera = ds.Tables["ConnectionTypes"].Rows[0]["MVAF4_CONNECTION_OTHER"].ToString(),

                        idconexionaltatension = string.IsNullOrEmpty(ds.Tables["ConnectionTypes"].Rows[0]["MVAF1_CONNECTION_ID"].ToString()) ? null : Convert.ToInt32(ds.Tables["ConnectionTypes"].Rows[0]["MVAF1_CONNECTION_ID"].ToString()),

                        idconexionbajatension = string.IsNullOrEmpty(ds.Tables["ConnectionTypes"].Rows[0]["MVAF2_CONNECTION_ID"].ToString()) ? null : Convert.ToInt32(ds.Tables["ConnectionTypes"].Rows[0]["MVAF2_CONNECTION_ID"].ToString()),

                        idconexionsegundabaja = string.IsNullOrEmpty(ds.Tables["ConnectionTypes"].Rows[0]["MVAF3_CONNECTION_ID"].ToString()) ? null : Convert.ToInt32(ds.Tables["ConnectionTypes"].Rows[0]["MVAF3_CONNECTION_ID"].ToString()),

                        idconexiontercera = string.IsNullOrEmpty(ds.Tables["ConnectionTypes"].Rows[0]["MVAF4_CONNECTION_ID"].ToString()) ? null : Convert.ToInt32(ds.Tables["ConnectionTypes"].Rows[0]["MVAF4_CONNECTION_ID"].ToString()),

                    } : new ConnectionTypes());

                }
                Task<Derivations> GetSidcoDerivations(string serial)
                {

                    List<SqlParameter> parametros = new()
                    {

                        //if (serial != )
                        Methods.CrearSqlParameter("@V_NO_SERIE", SqlDbType.VarChar, serial)
                    };

                    //EJECUTAR CONSULTA
                    DataSet ds = Methods.ejecutarConsultaOFuncion("SELECT * FROM [dbo].[FN_GET_DERIVACIONES](@V_NO_SERIE)", parametros, "Derivations", _context.Database.GetConnectionString());

                    return Task.FromResult((ds.Tables["Derivations"].Rows.Count > 0) ? new Derivations()
                    {
                        tipoderivacionaltatension = ds.Tables["Derivations"].Rows[0]["RCBN_FCBN1"].ToString(),

                        valorderivacionupaltatension = string.IsNullOrEmpty(ds.Tables["Derivations"].Rows[0]["MVAF1_DER_UP"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Derivations"].Rows[0]["MVAF1_DER_UP"].ToString()),

                        valorderivaciondownaltatension = string.IsNullOrEmpty(ds.Tables["Derivations"].Rows[0]["MVAF1_DER_DOWN"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Derivations"].Rows[0]["MVAF1_DER_DOWN"].ToString()),

                        tipoderivacionaltatension_2 = ds.Tables["Derivations"].Rows[0]["RCBN_FCBN1_2"].ToString(),

                        valorderivacionupaltatension_2 = string.IsNullOrEmpty(ds.Tables["Derivations"].Rows[0]["MVAF1_DER_UP_2"].ToString()) ? null : Convert.ToInt32(ds.Tables["Derivations"].Rows[0]["MVAF1_DER_UP_2"].ToString()),

                        valorderivaciondownaltatension_2 = string.IsNullOrEmpty(ds.Tables["Derivations"].Rows[0]["MVAF1_DER_DOWN_2"].ToString()) ? null : Convert.ToInt32(ds.Tables["Derivations"].Rows[0]["MVAF1_DER_DOWN_2"].ToString()),

                        tipoderivacionbajatension = ds.Tables["Derivations"].Rows[0]["RCBN_FCBN2"].ToString(),

                        valorderivacionupbajatension = string.IsNullOrEmpty(ds.Tables["Derivations"].Rows[0]["MVAF2_DER_UP"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Derivations"].Rows[0]["MVAF2_DER_UP"].ToString()),

                        valorderivaciondownbajatension = string.IsNullOrEmpty(ds.Tables["Derivations"].Rows[0]["MVAF2_DER_DOWN"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Derivations"].Rows[0]["MVAF2_DER_DOWN"].ToString()),

                        tipoderivacionsegundatension = ds.Tables["Derivations"].Rows[0]["RCBN_FCBN3"].ToString(),

                        valorderivacionupsegundatension = string.IsNullOrEmpty(ds.Tables["Derivations"].Rows[0]["MVAF3_DER_UP"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Derivations"].Rows[0]["MVAF3_DER_UP"].ToString()),
                        valorderivaciondownsegundatension = string.IsNullOrEmpty(ds.Tables["Derivations"].Rows[0]["MVAF3_DER_DOWN"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Derivations"].Rows[0]["MVAF3_DER_DOWN"].ToString()),

                        tipoderivacionterceratension = ds.Tables["Derivations"].Rows[0]["RCBN_FCBN4"].ToString(),

                        valorderivacionupterceratension = string.IsNullOrEmpty(ds.Tables["Derivations"].Rows[0]["MVAF4_DER_UP"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Derivations"].Rows[0]["MVAF4_DER_UP"].ToString()),

                        valorderivaciondownterceratension = string.IsNullOrEmpty(ds.Tables["Derivations"].Rows[0]["MVAF4_DER_DOWN"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Derivations"].Rows[0]["MVAF4_DER_DOWN"].ToString()),

                        //idConexionEquivalente = Convert.ToInt32(ds.Tables["Derivations"].Rows[0]["MVAF1_CONNECTION_ID"].ToString()),
                        //idConexionEquivalente2 = Convert.ToInt32(ds.Tables["Derivations"].Rows[0]["MVAF2_CONNECTION_ID"].ToString()),
                        //idConexionEquivalente3 = Convert.ToInt32(ds.Tables["Derivations"].Rows[0]["MVAF3_CONNECTION_ID"].ToString()),
                        //idConexionEquivalente4 = Convert.ToInt32(ds.Tables["Derivations"].Rows[0]["MVAF4_CONNECTION_ID"].ToString()),

                    } : new Derivations());

                }
                Task<Taps> GetSidcoTaps(string serial)
                {
                    List<SqlParameter> parametros = new()
                    {

                        //if (serial != )
                        Methods.CrearSqlParameter("@V_NO_SERIE", SqlDbType.VarChar, serial)
                    };

                    //EJECUTAR CONSULTA
                    DataSet ds = Methods.ejecutarConsultaOFuncion("SELECT * FROM [dbo].[FN_GET_TAPS] (@V_NO_SERIE)", parametros, "Taps", _context.Database.GetConnectionString());

                    return Task.FromResult((ds.Tables["Taps"].Rows.Count > 0) ? new Taps()
                    {
                        tapsaltatension = string.IsNullOrEmpty(ds.Tables["Taps"].Rows[0]["MVAF1_TAPS"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Taps"].Rows[0]["MVAF1_TAPS"].ToString()),

                        tapsbajatension = string.IsNullOrEmpty(ds.Tables["Taps"].Rows[0]["MVAF2_TAPS"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Taps"].Rows[0]["MVAF2_TAPS"].ToString()),

                        tapssegundabaja = string.IsNullOrEmpty(ds.Tables["Taps"].Rows[0]["MVAF3_TAPS"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Taps"].Rows[0]["MVAF3_TAPS"].ToString()),

                        tapsterciario = string.IsNullOrEmpty(ds.Tables["Taps"].Rows[0]["MVAF4_TAPS"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Taps"].Rows[0]["MVAF4_TAPS"].ToString()),

                    } : new Taps());

                }
                Task<NBAINeutro> GetSidcoBillNeutro(string serial)
                {

                    List<SqlParameter> parametros = new()
                    {

                        //if (serial != )
                        Methods.CrearSqlParameter("@V_NO_SERIE", SqlDbType.VarChar, serial)
                    };

                    //EJECUTAR CONSULTA
                    DataSet ds = Methods.ejecutarConsultaOFuncion("SELECT * FROM [dbo].[FN_GET_NBAI_NEUTRO] (@V_NO_SERIE)", parametros, "BillNeutro", _context.Database.GetConnectionString());

                    return Task.FromResult((ds.Tables["BillNeutro"].Rows.Count > 0) ? new NBAINeutro()
                    {
                        valornbaineutroaltatension = string.IsNullOrEmpty(ds.Tables["BillNeutro"].Rows[0]["MVAF1_NBAI_NEUTRO"].ToString()) ? null : Convert.ToDecimal(ds.Tables["BillNeutro"].Rows[0]["MVAF1_NBAI_NEUTRO"].ToString()),

                        valornbaineutrobajatension = string.IsNullOrEmpty(ds.Tables["BillNeutro"].Rows[0]["MVAF2_NBAI_NEUTRO"].ToString()) ? null : Convert.ToDecimal(ds.Tables["BillNeutro"].Rows[0]["MVAF2_NBAI_NEUTRO"].ToString()),

                        valornbaineutrosegundabaja = string.IsNullOrEmpty(ds.Tables["BillNeutro"].Rows[0]["MVAF3_NBAI_NEUTRO"].ToString()) ? null : Convert.ToDecimal(ds.Tables["BillNeutro"].Rows[0]["MVAF3_NBAI_NEUTRO"].ToString()),

                        valornbaineutrotercera = string.IsNullOrEmpty(ds.Tables["BillNeutro"].Rows[0]["MVAF4_NBAI_NEUTRO"].ToString()) ? null : Convert.ToDecimal(ds.Tables["BillNeutro"].Rows[0]["MVAF4_NBAI_NEUTRO"].ToString()),

                    } : new NBAINeutro());

                }

                Task<List<ChangingTablesArtifact>> GetSidcoChangingTables(string serial)
                {

                    //LISTA DE PARÁMETROS
                    List<SqlParameter> parametros = new()
                    {

                        //if (serial != )
                        Methods.CrearSqlParameter("@V_NO_SERIE", SqlDbType.VarChar, serial)
                    };

                    //EJECUTAR CONSULTA
                    DataSet ds = Methods.ejecutarConsultaOFuncion("SELECT * FROM [dbo].[FN_GET_INFOAPARATO_CAM] (@V_NO_SERIE)", parametros, "changingTables", _context.Database.GetConnectionString());

                    return Task.FromResult(ds.Tables["changingTables"].AsEnumerable().Select(result => new ChangingTablesArtifact()
                    {
                        ColumnTypeId = string.IsNullOrEmpty(result["COLUMN_TYPE_ID"].ToString()) ? 0 : Convert.ToDecimal(result["COLUMN_TYPE_ID"].ToString()),

                        ColumnTitle = result["COLUMN_TITLE"].ToString(),

                        OrderIndex = string.IsNullOrEmpty(result["ORDER_INDEX"].ToString()) ? null : Convert.ToDecimal(result["ORDER_INDEX"].ToString()),

                        OperationId = string.IsNullOrEmpty(result["OPERATION_ID"].ToString()) ? null : Convert.ToDecimal(result["OPERATION_ID"].ToString()),

                        FlagRcbnFcbn = result["FLAG_RCBN_FCBN"].ToString(),

                        DerivId = string.IsNullOrEmpty(result["DERIV_ID"].ToString()) ? null : Convert.ToDecimal(result["DERIV_ID"].ToString()),

                        DerivOther = result["DERIV_OTHER"].ToString(),

                        DerivId2 = string.IsNullOrEmpty(result["DERIV_ID2"].ToString()) ? null : Convert.ToDecimal(result["DERIV_ID2"].ToString()),

                        Deriv2Other = result["DERIV2_OTHER"].ToString(),

                        Taps = string.IsNullOrEmpty(result["TAPS"].ToString()) ? null : Convert.ToDecimal(result["TAPS"].ToString()),

                    }).ToList());

                }

                Task<WarrantiesArtifact> GetSidcoInfoaparatoGars(string serial)
                {
                    List<SqlParameter> parametros = new()
                    {

                        //if (serial != )
                        Methods.CrearSqlParameter("@V_NO_SERIE", SqlDbType.VarChar, serial)
                    };

                    //EJECUTAR CONSULTA
                    DataSet ds = Methods.ejecutarConsultaOFuncion("SELECT * FROM [dbo].[FN_GET_GARANTIAS] (@V_NO_SERIE)", parametros, "Warranties", _context.Database.GetConnectionString());

                    return Task.FromResult((ds.Tables["Warranties"].Rows.Count > 0) ? new WarrantiesArtifact()
                    {
                        //OrderCode = ds.Tables["Warranties"].Rows[0]["MVAF1_NBAI_NEUTRO"].ToString(),
                        Iexc100 = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["IEXC_100"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["IEXC_100"].ToString()),

                        Iexc110 = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["IEXC_110"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["IEXC_110"].ToString()),

                        Kwfe100 = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["KWFE_100"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["KWFE_100"].ToString()),

                        Kwfe110 = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["KWFE_110"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["KWFE_110"].ToString()),

                        TolerancyKwfe = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["TOLERANCY_KWFE"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["TOLERANCY_KWFE"].ToString()),

                        KwcuMva = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["KWCU_MVA"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["KWCU_MVA"].ToString()),

                        KwcuKv = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["KWCU_KV"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["KWCU_KV"].ToString()),

                        Kwcu = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["KWCU"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["KWCU"].ToString()),

                        TolerancyKwCu = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["TOLERANCY_KW_CU"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["TOLERANCY_KW_CU"].ToString()),

                        Kwaux2 = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["KWAUX_2"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["KWAUX_2"].ToString()),

                        Kwaux3 = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["KWAUX_3"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["KWAUX_3"].ToString()),

                        Kwaux4 = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["KWAUX_4"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["KWAUX_4"].ToString()),

                        Kwaux1 = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["KWAUX_1"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["KWAUX_1"].ToString()),

                        TolerancyKwAux = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["TOLERANCY_KW_AUX"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["TOLERANCY_KW_AUX"].ToString()),

                        Kwtot100 = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["KWTOT_100"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["KWTOT_100"].ToString()),

                        Kwtot110 = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["KWTOT_110"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["KWTOT_110"].ToString()),

                        TolerancyKwtot = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["TOLERANCY_KWTOT"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["TOLERANCY_KWTOT"].ToString()),

                        ZPositiveMva = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["Z_POSITIVE_MVA"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["Z_POSITIVE_MVA"].ToString()),

                        ZPositiveHx = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["Z_POSITIVE_HX"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["Z_POSITIVE_HX"].ToString()),

                        ZPositiveHy = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["Z_POSITIVE_HY"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["Z_POSITIVE_HY"].ToString()),

                        ZPositiveXy = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["Z_POSITIVE_XY"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["Z_POSITIVE_XY"].ToString()),

                        TolerancyZpositive = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["TOLERANCY_ZPOSITIVE"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["TOLERANCY_ZPOSITIVE"].ToString()),

                        TolerancyZpositive2 = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["TOLERANCY_Zpositive2"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["TOLERANCY_Zpositive2"].ToString()),

                        NoiseOa = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["NOISE_OA"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["NOISE_OA"].ToString()),

                        NoiseFa1 = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["NOISE_FA1"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["NOISE_FA1"].ToString()),

                        NoiseFa2 = string.IsNullOrEmpty(ds.Tables["Warranties"].Rows[0]["NOISE_FA2"].ToString()) ? null : Convert.ToDecimal(ds.Tables["Warranties"].Rows[0]["NOISE_FA2"].ToString())

                    } : new WarrantiesArtifact());

                }

                Task<List<LightningRodArtifact>> GetSidcoLightningRod(string serial)
                {

                    //LISTA DE PARÁMETROS
                    List<SqlParameter> parametros = new()
                    {

                        //if (serial != )
                        Methods.CrearSqlParameter("@V_NO_SERIE", SqlDbType.VarChar, serial)
                    };

                    //EJECUTAR CONSULTA
                    DataSet ds = Methods.ejecutarConsultaOFuncion("SELECT * FROM [dbo].[FN_GET_INFOAPARATO_APAR] (@V_NO_SERIE)", parametros, "LightningRod", _context.Database.GetConnectionString());

                    return Task.FromResult(ds.Tables["LightningRod"].AsEnumerable().Select(result => new LightningRodArtifact()
                    {
                        ColumnTypeId = string.IsNullOrEmpty(result["COLUMN_TYPE_ID"].ToString()) ? 0 : Convert.ToDecimal(result["COLUMN_TYPE_ID"].ToString()),

                        ColumnTitle = result["COLUMN_TITLE"].ToString(),

                        OrderIndex = string.IsNullOrEmpty(result["ORDER_INDEX"].ToString()) ? null : Convert.ToDecimal(result["ORDER_INDEX"].ToString()),

                        Qty = string.IsNullOrEmpty(result["QTY"].ToString()) ? null : decimal.TryParse(result["QTY"].ToString(), out decimal qyt) ? qyt : 0,

                    }).ToList());

                }

                #endregion

                Task<LabTestsArtifact> GetSidcoLabTests(string serial)
                {

                    List<SqlParameter> parametros = new()
                    {

                        //if (serial != )
                        Methods.CrearSqlParameter("@V_NO_SERIE", SqlDbType.VarChar, serial)
                    };

                    //EJECUTAR CONSULTA
                    DataSet ds = Methods.ejecutarConsultaOFuncion("SELECT * FROM [dbo].[FN_GET_INFOAPARATO_LAB]  (@V_NO_SERIE)", parametros, "LabTests", _context.Database.GetConnectionString());

                    return Task.FromResult((ds.Tables["LabTests"].Rows.Count > 0) ? new LabTestsArtifact()
                    {
                        TextTestDielectric = ds.Tables["LabTests"].Rows[0]["TEXT_TEST_DIELECTRIC"].ToString(),
                        TextTestPrototype = ds.Tables["LabTests"].Rows[0]["TEXT_TEST_PROTOTYPE"].ToString(),
                        TextTestRoutine = ds.Tables["LabTests"].Rows[0]["TEXT_TEST_ROUTINE"].ToString()

                    } : new LabTestsArtifact());

                }

                Task<FnGetInfoDatosGenerales> TaskGen1 = GetFnGetInfoDatosGenerales(serial);
                Task<FnGetInfoDatosGenerales2> TaskGen2 = GetFnGetInfoDatosGenerales2(serial);
                Task<FnGetInfoReqEsp> TaskGen3 = GetFnGetInfoReqEsp(serial);
                Task<FunGetLenguageSidco> TaskGen4 = GetFunGetLenguageSidco(serial);

                Task<List<NozzlesArtifact>> TaskBoq = GetSidcoInfoaparatoBoqs(serial);
                Task<List<CharacteristicsArtifact>> TaskCar = GetSidcoInfoaparatoCars(serial);
                Task<VoltageKV> TaskVoltageKV = GetSidcoVoltageKV(serial);
                Task<NBAIBilKv> TaskNBAIKV = GetSidcoNBAIKV(serial);
                Task<ConnectionTypes> TaskConnectionTypes = GetSidcoConnectionTypes(serial);
                Task<Derivations> TaskDerivatios = GetSidcoDerivations(serial);
                Task<Taps> TaskTaps = GetSidcoTaps(serial);
                Task<NBAINeutro> TaskBillNeutro = GetSidcoBillNeutro(serial);
                Task<List<ChangingTablesArtifact>> TaskChangingTables = GetSidcoChangingTables(serial);
                Task<WarrantiesArtifact> TaskWarranties = GetSidcoInfoaparatoGars(serial);
                Task<List<LightningRodArtifact>> TaskLightningRod = GetSidcoLightningRod(serial);

                Task<LabTestsArtifact> TaskLabTests = GetSidcoLabTests(serial);

                List<Task> tasks1 = new()
                { TaskGen1, TaskGen2, TaskGen3, TaskGen4 , TaskLabTests,
                TaskBoq, TaskTaps, TaskBillNeutro,
                };

                List<Task> tasks2 = new()
                {
                    TaskCar, TaskNBAIKV,
                TaskConnectionTypes, TaskDerivatios, TaskLightningRod  };

                List<Task> tasks3 = new() { TaskChangingTables, TaskWarranties, TaskVoltageKV };

                //if (TaskRules!=null)
                //{
                //    tasks1.Add(TaskRules);
                //}

                await Task.WhenAny(tasks1).Result;
                await Task.WhenAny(tasks2).Result;
                await Task.WhenAny(tasks3).Result;

                GeneralArtifact ObjectGeneral = new(TaskGen1.Result.OrderCode, TaskGen1.Result.Aplicacion, Convert.ToDecimal(TaskGen2.Result.Fases),
                    TaskGen1.Result.Cliente, Convert.ToDecimal(TaskGen2.Result.Frec), TaskGen4.Result.PO_NUMBER,
                    Convert.ToDecimal(TaskGen2.Result.Altitud), TaskGen2.Result.Altitud_F_M,
                    TaskGen1.Result.Typetrafoid, TaskGen1.Result.Tipo,
                    TaskGen1.Result.Applicationid,
                    TaskGen1.Result.Standardid,
                    TaskGen1.Result.Norma_Aplicable,
                    TaskGen4.Result.Lenguaje_id, TaskGen4.Result.LENGUAJE,
                    TaskGen3.Result.Polarity_id,
                    TaskGen3.Result.Polarity_other,
                    TaskGen3.Result.DESPLAZAMIENTO_ANGULAR,
                    "", DateTime.Now, "", DateTime.Now);

                double? result = 0, mvamaxTot = 0.0;

                foreach (NozzlesArtifact item in TaskBoq.Result)
                {

                    if (item.ColumnTitle.ToUpper() is "ALTA TENSIÓN" or "DEV SERIE")
                    {
                        mvamaxTot = this.ObtenerNumeroMaximoPoroDevanado(item.ColumnTitle, TaskCar.Result.ToList());
                        double? tension1 = Convert.ToDouble(TaskVoltageKV.Result.tensionkvaltatension1);
                        result = mvamaxTot / Math.Sqrt(3) / tension1 * 1000;
                        item.CorrienteUnidad = decimal.Round(result.HasValue && double.IsNaN(result.Value) ? 0 : Convert.ToDecimal(result), 3);

                    }
                    else
                    if (item.ColumnTitle.ToUpper() is "BAJA TENSIÓN 2")
                    {
                        mvamaxTot = this.ObtenerNumeroMaximoPoroDevanado(item.ColumnTitle, TaskCar.Result.ToList());
                        double? tension1 = Convert.ToDouble(TaskVoltageKV.Result.tensionkvsegundabaja1);
                        result = mvamaxTot / Math.Sqrt(3) / tension1 * 1000;
                        item.CorrienteUnidad = decimal.Round(result.HasValue && double.IsNaN(result.Value) ? 0 : Convert.ToDecimal(result), 3);
                    }

                    else

                    if (item.ColumnTitle.ToUpper() is "BAJA TENSIÓN" or "DEV COMÚN")
                    {

                        mvamaxTot = this.ObtenerNumeroMaximoPoroDevanado(item.ColumnTitle, TaskCar.Result.ToList());
                        double? tension1 = Convert.ToDouble(TaskVoltageKV.Result.tensionkvbajatension1);
                        result = mvamaxTot / Math.Sqrt(3) / tension1 * 1000;
                        item.CorrienteUnidad = decimal.Round(result.HasValue && double.IsNaN(result.Value) ? 0 : Convert.ToDecimal(result), 3);
                    }

                    else

                    if (item.ColumnTitle.ToUpper() is "TERC")
                    {

                        mvamaxTot = this.ObtenerNumeroMaximoPoroDevanado(item.ColumnTitle, TaskCar.Result.ToList());
                        double? tension1 = Convert.ToDouble(TaskVoltageKV.Result.tensionkvterciario1);
                        result = mvamaxTot / Math.Sqrt(3) / tension1 * 1000;
                        item.CorrienteUnidad = decimal.Round(result.HasValue && double.IsNaN(result.Value) ? 0 : Convert.ToDecimal(result), 3);
                    }
                    else
                    {
                        item.CorrienteUnidad = 0;
                    }
                };

                return new InformationArtifact(
                    ObjectGeneral,
                    TaskBoq.Result,
                    TaskChangingTables.Result.OrderBy(x => x.ColumnTypeId).ToList(),
                    EvalCoolingType(TaskCar.Result),
                    TaskLightningRod.Result,
                    null,
                    TaskWarranties.Result,
                    TaskLabTests.Result, TaskVoltageKV.Result,
                    TaskNBAIKV.Result,
                    TaskConnectionTypes.Result,
                    TaskDerivatios.Result,
                    TaskTaps.Result,
                    TaskBillNeutro.Result,
                    null
                    );

            }
        }

        public double? ObtenerNumeroMaximoPoroDevanado(string devanado, List<CharacteristicsArtifact> pList)
        {
            double? mva1 = 0.0, mva2 = 0.0, mva3 = 0.0, mva4 = 0.0, mvamaxTot = 0.0;
            double? mvamax1;
            double? mvamax2;
            if (devanado.ToUpper() is "ALTA TENSIÓN" or "DEV SERIE")
            {

                for (int i = 0; i < pList.Count; i++)//capacidad
                {
                    switch (i)
                    {
                        case 0:
                            mva1 = Convert.ToDouble(pList[i].Mvaf1);
                            break;
                        case 1:
                            mva2 = Convert.ToDouble(pList[i].Mvaf1);
                            break;
                        case 2:
                            mva3 = Convert.ToDouble(pList[i].Mvaf1);
                            break;
                        case 3:
                            mva4 = Convert.ToDouble(pList[i].Mvaf1);
                            break;
                    }
                }
                mvamax1 = Math.Max((double)mva1, (double)mva2);
                mvamax2 = Math.Max((double)mva3, (double)mva4);
                mvamaxTot = Math.Max((double)mvamax1, (double)mvamax2);

            }
            else
            if (devanado.ToUpper() is "BAJA TENSIÓN" or "DEV COMÚN")
            {
                for (int i = 0; i < pList.Count; i++)//capacidad
                {
                    switch (i)
                    {
                        case 0:
                            mva1 = Convert.ToDouble(pList[i].Mvaf2);
                            break;
                        case 1:
                            mva2 = Convert.ToDouble(pList[i].Mvaf2);
                            break;
                        case 2:
                            mva3 = Convert.ToDouble(pList[i].Mvaf2);
                            break;
                        case 3:
                            mva4 = Convert.ToDouble(pList[i].Mvaf2);
                            break;
                    }
                }
                mvamax1 = Math.Max((double)mva1, (double)mva2);
                mvamax2 = Math.Max((double)mva3, (double)mva4);
                mvamaxTot = Math.Max((double)mvamax1, (double)mvamax2);
            }
            else

            if (devanado.ToUpper() is "BAJA TENSIÓN 2")
            {
                for (int i = 0; i < pList.Count; i++)//capacidad
                {
                    switch (i)
                    {
                        case 0:
                            mva1 = Convert.ToDouble(pList[i].Mvaf3);
                            break;
                        case 1:
                            mva2 = Convert.ToDouble(pList[i].Mvaf3);
                            break;
                        case 2:
                            mva3 = Convert.ToDouble(pList[i].Mvaf3);
                            break;
                        case 3:
                            mva4 = Convert.ToDouble(pList[i].Mvaf3);
                            break;
                    }
                }
                mvamax1 = Math.Max((double)mva1, (double)mva2);
                mvamax2 = Math.Max((double)mva3, (double)mva4);
                mvamaxTot = Math.Max((double)mvamax1, (double)mvamax2);
            }
            else

            if (devanado.ToUpper() is "TERC")
            {
                for (int i = 0; i < pList.Count; i++)//capacidad
                {
                    switch (i)
                    {
                        case 0:
                            mva1 = Convert.ToDouble(pList[i].Mvaf4);
                            break;
                        case 1:
                            mva2 = Convert.ToDouble(pList[i].Mvaf4);
                            break;
                        case 2:
                            mva3 = Convert.ToDouble(pList[i].Mvaf4);
                            break;
                        case 3:
                            mva4 = Convert.ToDouble(pList[i].Mvaf4);
                            break;
                    }
                }
                mvamax1 = Math.Max((double)mva1, (double)mva2);
                mvamax2 = Math.Max((double)mva3, (double)mva4);
                mvamaxTot = Math.Max((double)mvamax1, (double)mvamax2);

            }

            return mvamaxTot;
        }

        private static List<CharacteristicsArtifact> EvalCoolingType(List<CharacteristicsArtifact> list)
        {
            List<CharacteristicsArtifact> result = new();
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

        private static List<CharacteristicsArtifact> renameCoolingType(List<CharacteristicsArtifact> list, bool onan)
        {
            string type = onan ? "ONAN" : "ONAF";
            List<CharacteristicsArtifact> types = list.FindAll(x => x.CoolingType.Contains(type));
            List<CharacteristicsArtifact> result = new();
            int count = 1;
            if (types.Count > 1)
            {
                while (types.Count > 0)
                {
                    decimal? min = types.Min(x => x.OverElevation);
                    if (types.Where(x => x.OverElevation == min).Count() > 1)
                    {
                        decimal? minMva = types.Min(x => x.Mvaf1);
                        CharacteristicsArtifact mini = types.Find(x => x.Mvaf1 == minMva);
                        _ = types.Remove(mini);

                        mini.CoolingType = $"{type}{count}";
                        result.Add(mini);

                    }
                    else
                    {
                        CharacteristicsArtifact mini = types.Find(x => x.OverElevation == min);
                        _ = types.Remove(mini);
                        mini.CoolingType = $"{type}{count}";
                        result.Add(mini);
                    }
                    count = 2;
                }
            }
            else if (types.Count == 1)
            {
                CharacteristicsArtifact unico = types.First();
                unico.CoolingType = type;
                result.Add(unico);
            }

            return result;
        }

        //public int GetCharField(decimal idOrder, string fieldName)
        //{
        //    string[] words = fieldName.Split('_');

        //    for (int i = 0; i < words.Length; i++)
        //    {
        //        words[i] = words[i].Substring(0, 1).ToUpper() + words[i][1..].ToLower();
        //    }
        //    string field = string.Join("", words);

        //    Type myType = typeof(DcoMvaCharacteristic);
        //    PropertyInfo myFieldInfo = myType.GetProperty(field);
        //    return myFieldInfo.GetValue(dcoMvaCharacteristics) is null ? -9999 : Convert.ToInt32(myFieldInfo.GetValue(dcoMvaCharacteristics));
        //}

        public Task<List<GeneralProperties>> GetIdiomasSidco() => Task.FromResult(this._Mapper.Map<List<GeneralProperties>>(this._dbContext.CoreCatalogs.Where(x => x.AttributeId == Convert.ToDecimal(Methods.Enums.GetIdiomasSidco)).ToList()));
        public Task<List<GeneralProperties>> GetTypesTransformers() => Task.FromResult(this._Mapper.Map<List<GeneralProperties>>(this._dbContext.CoreCatalogs.Where(x => x.AttributeId == Convert.ToDecimal(Methods.Enums.GetTypesTransformers)).ToList()));
        public Task<List<GeneralProperties>> GetApplications() => Task.FromResult(this._Mapper.Map<List<GeneralProperties>>(this._dbContext.CoreCatalogs.Where(x => x.AttributeId == Convert.ToDecimal(Methods.Enums.GetApplications)).ToList()));
        public Task<List<GeneralProperties>> GetApplicableRule() => Task.FromResult(this._Mapper.Map<List<GeneralProperties>>(this._dbContext.CoreCatalogs.Where(x => x.AttributeId == Convert.ToDecimal(Methods.Enums.GetApplicableRule)).ToList()));
        public Task<List<GeneralProperties>> GetAngularDisplacement() => Task.FromResult(this._Mapper.Map<List<GeneralProperties>>(this._dbContext.CoreCatalogs.Where(x => x.AttributeId == Convert.ToDecimal(Methods.Enums.GetAngularDisplacement)).ToList()));

        public Task<List<GeneralProperties>> GetOperativeConditions() => Task.FromResult(this._Mapper.Map<List<GeneralProperties>>(this._dbContext.CoreCatalogs.Where(x => x.AttributeId == Convert.ToDecimal(Methods.Enums.GetOperativeConditions)).ToList()));

        public Task<List<GeneralProperties>> GetConnectionTypes() => Task.FromResult(this._Mapper.Map<List<GeneralProperties>>(this._dbContext.CoreCatalogs.Where(x => x.AttributeId == Convert.ToDecimal(Methods.Enums.GetConnectionTypes)).ToList()));
        #endregion

    }
}
