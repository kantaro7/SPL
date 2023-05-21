namespace SPL.Reports.Infrastructure.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;
    using Microsoft.Data.SqlClient;

    public static class Methods
    {
        public static DataSet EjecutarConsultaOFuncion(string pConsulta, List<SqlParameter> pParametros, string pNombreTablaDS, string pCadenaConexion)
        {
            SqlConnection conexion = new(pCadenaConexion);
            SqlCommand consultarCMD = new(pConsulta, conexion);
            if (pParametros != null)
                pParametros.ForEach(parametro => consultarCMD.Parameters.Add(parametro));
            SqlDataAdapter consultarADP = new(consultarCMD);
            DataSet ds = new();
            _ = consultarADP.Fill(ds, pNombreTablaDS);
            consultarCMD.CommandTimeout = 9000000;
            consultarCMD.Dispose();
            consultarADP.Dispose();

            return ds;
        }

        public static IEnumerable<T> EjecutarSP<T>(string pQuery, SqlParameter[] pParametros, string pCadenaConexion) where T : new()
        {
            try
            {


                SqlConnection conexion = new(pCadenaConexion);

                using (SqlCommand command = new SqlCommand(pQuery, conexion))
                {

                    if (pParametros != null)
                    {
                        command.Parameters.AddRange(pParametros);
                        command.CommandTimeout = 120;
                    }
                    command.CommandType = CommandType.StoredProcedure;

                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    var resultRequest = MapEntity<T>(reader);
                    reader.Close();
                    if (command.Connection.State == ConnectionState.Open)
                        command.Connection.Close();
                    return resultRequest;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
        }

        public static SqlParameter CrearSqlParameter(string nombre, SqlDbType tipo, object valor, int size = 0) => (size == 0) ? new SqlParameter(nombre, tipo) { Value = valor, Direction = ParameterDirection.Input } :
                                 new SqlParameter(nombre, tipo, size) { Value = valor, Direction = ParameterDirection.Input };

        public static IEnumerable<T> MapEntity<T>(IDataReader dr) where T : new()
        {
            Type businessEntityType = typeof(T);
            List<T> entitys = new List<T>();
            Hashtable hashtable = new Hashtable();
            PropertyInfo[] properties = businessEntityType.GetProperties();
            foreach (PropertyInfo info in properties)
            {
                hashtable[info.Name.ToUpper()] = info;
            }
            while (dr.Read())
            {
                T newObject = new T();
                for (int index = 0; index < dr.FieldCount; index++)
                {
                    PropertyInfo info = (PropertyInfo)
                                        hashtable[dr.GetName(index).ToUpper()];
                    if ((info != null) && info.CanWrite)
                    {
                        info.SetValue(newObject, dr.GetValue(index) is DBNull ? null : dr.GetValue(index), null);
                    }
                }
                entitys.Add(newObject);
            }
            dr.Close();
            return entitys;
        }

    }
}
