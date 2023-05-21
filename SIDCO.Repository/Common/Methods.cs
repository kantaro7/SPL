using Microsoft.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIDCO.Infrastructure.Common
{
   
    public static class Methods
    {
        public enum Enums
        {
            GetIdiomasSidco = 298,
            GetTypesTransformers = 3,
            GetApplications = 2,
            GetApplicableRule = 5,
            GetAngularDisplacement = 48,
            GetOperativeConditions = 270,
            GetConnectionTypes = 32,
            Active = 1,
            TopConditions = 9999,

        }
        public static DataSet ejecutarConsultaOFuncion(string pConsulta, List<SqlParameter> pParametros, string pNombreTablaDS, string pCadenaConexion)
        {
            SqlConnection conexion = new SqlConnection(pCadenaConexion);
            SqlCommand consultarCMD = new SqlCommand(pConsulta, conexion);
            if (pParametros != null)
                pParametros.ForEach(parametro => consultarCMD.Parameters.Add(parametro));
            SqlDataAdapter consultarADP = new SqlDataAdapter(consultarCMD);
            DataSet ds = new DataSet();
            consultarADP.Fill(ds, pNombreTablaDS);
            consultarCMD.CommandTimeout = 9000000;
            consultarCMD.Dispose();
            consultarADP.Dispose();
       
            return ds;
        }
        public static SqlParameter CrearSqlParameter(string nombre, SqlDbType tipo, object valor, int size = 0)
        {
            return (size == 0) ? new SqlParameter(nombre, tipo) { Value = valor, Direction = ParameterDirection.Input } :
                                 new SqlParameter(nombre, tipo, size) { Value = valor, Direction = ParameterDirection.Input };
        }

    }

}
