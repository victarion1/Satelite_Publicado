using FrameWork.Common;
using FrameWork.Data;
using FrameWork.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VidaSecurity.Framework.Query.Transferencia;
using VidaSecurity.Satelite.Transferencia;
using VidaSecurity.Satelite.Util;

namespace VidaSecurity.Satelite
{
    public class Operacion
    {
        private const string ConexionLog = "Framework";
        private const string CUP = "PortalSecurity";
        private const string NombreServicio = "Consulta Satelite";
        private const string Usuario = "UsrSatelite";

        public List<UsuarioConsultas> SateliteConsultaUsuario(string username)
        {
            List<UsuarioConsultas> usuarioConsultasList = new List<UsuarioConsultas>();
            try
            {
                object obj = (object)null;
                DataTable dataTable;
                Conexion.FWEjecutarConsulta(null,
                                            "SATELITE",
                                            "usp_ConsultarUsuarioSatelite",
                                            ConnectionBase.ProcedureType.Procedure,
                                            new object[] { username },
                                            new object[] { },
                                            out obj,
                                            out dataTable
                                            );

                foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                    usuarioConsultasList.Add(new UsuarioConsultas(row));
                return usuarioConsultasList;
            }
            catch (Exception ex)
            {
                throw new Exception("Error - " + ex.Message, ex);
            }
        }

        public List<Parametro> RetornaParametro(int idConsulta)
        {
            List<Parametro> parametroList = new List<Parametro>();
            foreach (ParametroBD parametroBd in this.ConsultaParametro(idConsulta))
            {
                if (parametroBd.TipoAyuda.ToUpper().Trim().Equals("LIST"))
                {
                    IEnumerable<ParDato> source = ((IEnumerable<string>)parametroBd.AyudaValores.Split(',')).Select<string, ParDato>((Func<string, ParDato>)(x => new ParDato()
                    {
                        Id = x,
                        Texto = x
                    }));

                    parametroList.Add(new Parametro()
                    {
                        Nombre = parametroBd.Nombre,
                        TipoDato = parametroBd.TipoDato,
                        EsLista = true,
                        Lista = source.ToList<ParDato>()
                    });

                }
                else if (parametroBd.TipoAyuda.ToUpper().Trim().Equals("QUERY"))
                    parametroList.Add(new Parametro()
                    {
                        Nombre = parametroBd.Nombre,
                        TipoDato = parametroBd.TipoDato,
                        EsLista = true,
                        Lista = this.ConsultaListaDatos(idConsulta, parametroBd.AyudaValores)
                    });
                else
                    parametroList.Add(new Parametro()
                    {
                        Nombre = parametroBd.Nombre,
                        TipoDato = parametroBd.TipoDato,
                        EsLista = false,
                        Lista = new List<ParDato>()
                    });
            }
            return parametroList;
        }

        private List<ParDato> ConsultaListaDatos(int idConsulta, string comando)
        {
            string str = this.ConsultaCadenaConexion(idConsulta);
            bool esOracle = !this.DeterminaBaseDato(str);
            RetornoComando retorno = VidaSecurity.Framework.Query.Operacion.EjecutarComando(esOracle, this.LimpiaCadena(str, esOracle), comando);
            List<ParDato> lista = new List<ParDato>();
            ParDato nvoDato = null;
            foreach (DataRow item in retorno.Sabana.Rows)
            {
                nvoDato = new ParDato();
                nvoDato.Id = item.ItemArray[0].ToString();
                nvoDato.Texto = item.ItemArray[1].ToString();
                lista.Add(nvoDato);
            }

            return lista;
        }

        private List<ParametroBD> ConsultaParametro(int idConsulta)
        {
            List<ParametroBD> parametroBdList = new List<ParametroBD>();
            try
            {
                object obj = (object)null;
                DataTable dataTable;
                Conexion.FWEjecutarConsulta(null, "SATELITE", "ups_ConsultaParametros", ConnectionBase.ProcedureType.Procedure, new object[]
                { idConsulta}, new object[] { }, out obj, out dataTable);
                foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                    parametroBdList.Add(new ParametroBD(row));
            }
            catch (Exception ex)
            {
                throw new Exception("Error - " + ex.Message, ex);
            }
            return parametroBdList;
        }

        public string ConsultaCadenaConexion(int idConsulta)
        {
            string str = string.Empty;
            try
            {
                object obj = (object)null;
                DataTable dataTable;
                Conexion.FWEjecutarConsulta(null, "SATELITE", "ups_ConsultaCadenaConexion", ConnectionBase.ProcedureType.Procedure, new object[]
                { idConsulta}, new object[] { }, out obj, out dataTable);
                if (dataTable.Rows.Count > 0)
                    return Dato.GetDato<string>((object)dataTable.Rows[0]["CONEXION"].ToString());
            }
            catch (Exception ex)
            {
                new FwLog("PortalSecurity").EscribeLog("Error", string.Format("Ha ocurrido un error obtener la cadena de conexión, el mensaje es : {0} ", (object)ex.Message));
                str = JsonConvert.SerializeObject((object)new
                {
                    resultado = false,
                    mensaje = ex
                });
            }
            return string.Empty;
        }

        public string ConsultaQuery(int idConsulta)
        {
            string str = string.Empty;
            try
            {
                object obj = (object)null;
                DataTable dataTable;
                Conexion.FWEjecutarConsulta(null, "SATELITE", "usp_DetalleConsulta", ConnectionBase.ProcedureType.Procedure, new object[]
                {idConsulta}, new object[] { }, out obj, out dataTable);
                if (dataTable.Rows.Count > 0)
                    return Dato.GetDato<string>(dataTable.Rows[0]["Query"]);
            }
            catch (Exception ex)
            {
                new FwLog("PortalSecurity").EscribeLog("Error", string.Format("Ha ocurrido un error al consultar la cadena de conexión, el mensaje es : {0} ", (object)ex.Message));
                str = JsonConvert.SerializeObject((object)new
                {
                    resultado = false,
                    mensaje = ex
                });
            }
            return string.Empty;
        }

        public RetornoComando EjecutarConsulta(int idConsulta, List<string> parametros)
        {
            string str = this.ConsultaCadenaConexion(idConsulta);
            string consulta = this.ConsultaQuery(idConsulta);
            string comandoSql = this.GeneraComando(idConsulta, consulta, parametros);
            bool esOracle = !this.DeterminaBaseDato(str);
            return VidaSecurity.Framework.Query.Operacion.EjecutarComando(esOracle, this.LimpiaCadena(str, esOracle), comandoSql);
        }

        private string LimpiaCadena(string cadena, bool esOracle)
        {
            string str1 = string.Empty;
            if (esOracle)
            {
                string str2 = cadena;
                char[] chArray = new char[1] { ';' };
                foreach (string str3 in str2.Split(chArray))
                {
                    if (!str3.ToUpper().StartsWith("PROVIDER"))
                        str1 = string.IsNullOrEmpty(str1) ? str3 : str1 + "; " + str3;
                }
                return str1;
            }
            string str4 = cadena;
            char[] chArray1 = new char[1] { ';' };
            foreach (string str2 in str4.Split(chArray1))
            {
                if (!str2.ToUpper().StartsWith("DRIVER"))
                    str1 = string.IsNullOrEmpty(str1) ? str2 : str1 + "; " + str2;
            }
            return str1;
        }

        private bool DeterminaBaseDato(string cadenaConexion)
        {
            return cadenaConexion.ToUpper().Contains("{SQL SERVER}");
        }

        private string GeneraComando(int idConsulta, string consulta, List<string> parametros)
        {
            foreach (Parametro parametro1 in this.RetornaParametro(idConsulta))
            {
                string newValue = string.Empty;
                foreach (string parametro2 in parametros)
                {
                    if (parametro2.StartsWith(parametro1.Id))
                        newValue = parametro2.Replace(parametro1.Id + "#", string.Empty);
                }
                string upper = parametro1.Nombre.ToUpper();
                consulta = consulta.ToUpper().Replace("@" + upper + "@", newValue);
            }
            return consulta;
        }
    }
}
