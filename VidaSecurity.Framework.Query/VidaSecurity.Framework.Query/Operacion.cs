// Decompiled with JetBrains decompiler
// Type: VidaSecurity.Framework.Query.Operacion
// Assembly: VidaSecurity.Framework.Query, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9DD9E185-F5E8-47E5-8AC0-48BCE51D0C82
// Assembly location: D:\Nueva carpeta (2)\VidaSecurity.Framework.Query.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.SqlClient;
using VidaSecurity.Framework.Query.Transferencia;

namespace VidaSecurity.Framework.Query
{
    public class Operacion : IDisposable
    {
        private OracleType tiporetorno = OracleType.Cursor;
        private const int TimeOut = 3600;
        private const string Desconocido = "DESCONOCIDO";
        private OracleConnection fwconexion;
        private static string cadenaConexion;
        private int timeout;
        private OracleTransaction fwtransaccion;

        [Obsolete("NO USAR", true)]
        public Operacion()
        {
        }

        public Operacion(string cadenaconexion)
        {
            Operacion.cadenaConexion = cadenaconexion;
            this.timeout = 3600;
            this.tiporetorno = OracleType.Cursor;
            this.Abrir();
        }

        public Operacion(string cadenaconexion, int timeout)
        {
            Operacion.cadenaConexion = cadenaconexion;
            this.timeout = timeout;
            this.tiporetorno = OracleType.Cursor;
            this.Abrir();
        }

        public void Dispose()
        {
            try
            {
                this.fwconexion.Close();
                this.fwconexion.Dispose();
            }
            catch
            {
            }
            GC.Collect();
        }

        public void Ejecutar(string nombreprocedimiento, bool esconsulta, object[] paramin, object[] paramout, out object retorno, out DataTable cursor)
        {
            retorno = (object)null;
            cursor = new DataTable();
            try
            {
                if (this.fwconexion.State != ConnectionState.Open)
                    this.Abrir();
                if (this.fwconexion.State != ConnectionState.Open)
                    throw new Exception("La conexion se encuentra cerrada.");
                using (OracleCommand comando = this.ObtenerComando(nombreprocedimiento))
                {
                    this.tiporetorno = this.ObtenerParametros(paramin, comando.Parameters);
                    this.EjecutarComando(comando, esconsulta, this.tiporetorno, out cursor, out retorno);
                    this.ObtenerParametrosSalida(comando.Parameters, this.tiporetorno, ref retorno, paramout);
                    comando.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void Ejecutar(Operacion.QueryType tiposp, string nombreprocedimiento, ref List<OracleParameter> coleccionparametros, out object retorno, out DataTable cursor)
        {
            retorno = (object)null;
            cursor = new DataTable();
            try
            {
                if (this.fwconexion.State != ConnectionState.Open)
                    this.Abrir();
                if (this.fwconexion.State != ConnectionState.Open)
                    throw new Exception("La conexion se encuentra cerrada.");
                using (OracleCommand command = this.fwconexion.CreateCommand())
                {
                    command.CommandText = nombreprocedimiento;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = this.timeout;
                    command.Parameters.Clear();
                    foreach (OracleParameter oracleParameter in coleccionparametros)
                        command.Parameters.Add(oracleParameter);
                    switch (tiposp)
                    {
                        case Operacion.QueryType.NonQuery:
                            command.ExecuteNonQuery();
                            break;
                        case Operacion.QueryType.Scalar:
                            retorno = command.ExecuteScalar();
                            break;
                        case Operacion.QueryType.Reader:
                            cursor.Load((IDataReader)command.ExecuteReader());
                            break;
                    }
                    coleccionparametros.Clear();
                    foreach (OracleParameter parameter in (DbParameterCollection)command.Parameters)
                        coleccionparametros.Add(parameter);
                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        internal void Abrir()
        {
            this.fwconexion = new OracleConnection(Operacion.cadenaConexion);
            this.fwconexion.Open();
        }

        internal OracleCommand ObtenerComando(string nombreprocedimiento)
        {
            OracleCommand command = this.fwconexion.CreateCommand();
            command.CommandText = nombreprocedimiento;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = this.timeout;
            OracleCommandBuilder.DeriveParameters(command);
            return command;
        }

        internal OracleType ObtenerParametros(object[] parametrosentrada, OracleParameterCollection coleccionparametros)
        {
            OracleType oracleType = OracleType.Cursor;
            int index = 0;
            try
            {
                foreach (OracleParameter coleccionparametro in (DbParameterCollection)coleccionparametros)
                {
                    switch (coleccionparametro.Direction)
                    {
                        case ParameterDirection.Input:
                            if (parametrosentrada.GetValue(index) == null)
                                coleccionparametro.Value = (object)DBNull.Value;
                            else
                                coleccionparametro.Value = parametrosentrada.GetValue(index);
                            ++index;
                            continue;
                        case ParameterDirection.Output:
                            coleccionparametro.Value = (object)DBNull.Value;
                            continue;
                        case ParameterDirection.InputOutput:
                            if (parametrosentrada.GetValue(index) == null)
                                coleccionparametro.Value = (object)DBNull.Value;
                            else
                                coleccionparametro.Value = parametrosentrada.GetValue(index);
                            ++index;
                            continue;
                        case ParameterDirection.ReturnValue:
                            oracleType = coleccionparametro.OracleType;
                            coleccionparametro.Value = (object)DBNull.Value;
                            continue;
                        default:
                            coleccionparametro.Value = (object)DBNull.Value;
                            continue;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return oracleType;
        }

        internal void EjecutarComando(OracleCommand comando, bool esconsulta, OracleType tiporetorno, out DataTable cursor, out object retorno)
        {
            cursor = new DataTable();
            retorno = (object)null;
            if (esconsulta)
            {
                if (tiporetorno.Equals((object)OracleType.Cursor))
                    cursor.Load((IDataReader)comando.ExecuteReader());
                retorno = comando.ExecuteScalar();
            }
            comando.ExecuteNonQuery();
        }

        internal void ObtenerParametrosSalida(OracleParameterCollection coleccionparametros, OracleType tiporetorno, ref object retorno, object[] paramout)
        {
            int index = 0;
            foreach (OracleParameter coleccionparametro in (DbParameterCollection)coleccionparametros)
            {
                switch (coleccionparametro.Direction)
                {
                    case ParameterDirection.Output:
                        paramout[index] = this.EstablecerParametro(paramout[index], coleccionparametro.Value);
                        ++index;
                        continue;
                    case ParameterDirection.InputOutput:
                        paramout[index] = this.EstablecerParametro(paramout[index], coleccionparametro.Value);
                        ++index;
                        continue;
                    case ParameterDirection.ReturnValue:
                        if (!tiporetorno.Equals((object)OracleType.Cursor))
                        {
                            retorno = this.EstablecerParametro(retorno, coleccionparametro.Value);
                            continue;
                        }
                        continue;
                    default:
                        continue;
                }
            }
        }

        internal object EstablecerParametro(object paramout, object valor)
        {
            try
            {
                if (valor.Equals((object)null) || valor.Equals((object)DBNull.Value))
                    return (object)null;
                Type type = valor.GetType();
                if (!type.Equals(typeof(DataTable)) && !type.Equals(typeof(IDataReader)) && (!type.Equals(typeof(OracleDataReader)) && !type.Equals(typeof(SqlDataReader))))
                    return valor;
                paramout = (object)new DataTable();
                ((DataTable)paramout).Load((IDataReader)valor);
                return paramout;
            }
            catch
            {
                return valor;
            }
        }

        public static void EjecutarAccion(string cadenaconexion, string nombreprocedimiento, object[] paramin, object[] paramout, out object retorno)
        {
            retorno = (object)null;
            DataTable cursor = new DataTable();
            using (Operacion operacion = new Operacion(cadenaconexion))
                operacion.Ejecutar(nombreprocedimiento, false, paramin, paramout, out retorno, out cursor);
        }

        public static void EjecutarConsulta(string cadenaconexion, string nombreprocedimiento, object[] paramin, object[] paramout, out object retorno, out DataTable cursor)
        {
            retorno = (object)null;
            cursor = new DataTable();
            using (Operacion operacion = new Operacion(cadenaconexion))
                operacion.Ejecutar(nombreprocedimiento, true, paramin, paramout, out retorno, out cursor);
        }

        public static T EjecutarEscalar<T>(string cadenaconexion, string nombreprocedimiento, object[] paramin, object[] paramout)
        {
            object retorno = (object)null;
            DataTable cursor = new DataTable();
            using (Operacion operacion = new Operacion(cadenaconexion))
                operacion.Ejecutar(nombreprocedimiento, false, paramin, paramout, out retorno, out cursor);
            return (T)Convert.ChangeType(retorno, typeof(T));
        }

        public static RetornoComando EjecutarComando(bool esOracle, string cadenaconexion, string comandoSql)
        {
            DataTable tabla = new DataTable();
            DbConnection dbConnection = !esOracle ? (DbConnection)new SqlConnection(cadenaconexion) : (DbConnection)new OracleConnection(cadenaconexion);
            if (dbConnection.State != ConnectionState.Open)
                dbConnection.Open();
            if (dbConnection.State != ConnectionState.Open)
                throw new Exception("La conexion se encuentra cerrada.");
            try
            {
                using (DbCommand command = dbConnection.CreateCommand())
                {
                    command.CommandText = comandoSql;
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 3600;
                    command.Parameters.Clear();
                    tabla.Load((IDataReader)command.ExecuteReader());
                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ha ocurrido un error al ejecutar la consulta: {0}", (object)ex.Message), ex);
            }
            return Operacion.AnalizaRetorno(tabla);
        }

        public static string EjecutarComandoJson(bool esOracle, string cadenaconexion, string comandoSql)
        {
            return JsonConvert.SerializeObject((object)Operacion.EjecutarComando(esOracle, cadenaconexion, comandoSql), (Formatting)1);
        }

        internal static RetornoComando AnalizaRetorno(DataTable tabla)
        {
            List<string> cabeceras = new List<string>();
            DataTable dataTable = new DataTable();
            foreach (DataColumn column in (InternalDataCollectionBase)tabla.Columns)
                cabeceras.Add(column.Caption);
            dataTable = tabla.Copy();
            return new RetornoComando(cabeceras, tabla);
        }

        internal static string ObtenerCadena()
        {
            return Operacion.cadenaConexion;
        }

        public enum QueryType
        {
            NonQuery,
            Scalar,
            Reader,
        }
    }
}
