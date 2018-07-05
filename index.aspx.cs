using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using VidaSecurity.Satelite.Transferencia;



namespace Sitio
{
    [System.Web.Script.Services.ScriptService]
    public partial class index : FrameWork.Web.UI.BaseMenuPage
    {
        public class Menu
        {
            public bool EsUsuario;
            public string Nombre;
            public List<UsuarioConsultas> Lista;
        }

        public class Nodo
        {
            public string text;
            public List<Nodo> nodes;
            public List<string> tags;
            public int id;
        }

        private const string Mensaje = "Esto es una prueba y son las: {0}\nUsuario:{1}\nSession:{2}";
        private static string Session;
        private static string Usuario;

        private static int IdConsulta;


        protected void Page_Load(object sender, EventArgs e)
        {
            Session = this.CodigoSesion.ToString();
            Usuario = this.UserName.ToString();
            this.hf_username.Value = Usuario;
        }

        [System.Web.Services.WebMethod]
        public static string ConsultaMenu()
        {
            List<VidaSecurity.Satelite.Transferencia.UsuarioConsultas> lista = new List<UsuarioConsultas>();
            try
            {
                 FrameWork.Logging.LogEvento.FWEscribirLog("antes de SateliteConsultaUsuario: " + Usuario, "SATELITE_WEB", "Error", "SATELITE_WEB");
                lista = new VidaSecurity.Satelite.Operacion().SateliteConsultaUsuario(Usuario);
                 FrameWork.Logging.LogEvento.FWEscribirLog("despues de SateliteConsultaUsuario", "SATELITE_WEB", "Error", "SATELITE_WEB");


            }
            catch (Exception e)
            {
                FrameWork.Logging.LogEvento.FWEscribirLog(e.ToString(), "SATELITE_WEB", "Error", "SATELITE_WEB");

            }
            List<Nodo> listaMenu = new List<Nodo>();


            foreach (var nombre in from x in lista
                                   group x by x.Perfil into y
                                   select new { Nombre = y.Key })
            {
               
                Nodo tmp = new Nodo();
                tmp.text = nombre.Nombre;
                tmp.nodes = new List<Nodo>();

                foreach (UsuarioConsultas item in
                    from x in lista
                    where x.Perfil.Equals(nombre.Nombre)
                    orderby x.Consulta
                    select x
                )
                {
                    tmp.nodes.Add(new Nodo()
                    {
                        text = item.Consulta,
                        tags = new List<string>()
                        {
                            item.Id.ToString(),
                            item.Parametro
                        }
                    });
                }

                listaMenu.Add(tmp);
            }

            string mensaje = string.Format(Mensaje, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), Usuario, Session);
            return JsonConvert.SerializeObject(
                new
                {
                    resultado = true,
                    mensaje = mensaje,
                    lista = listaMenu
                });

        }

        [System.Web.Services.WebMethod]
        public static string ConsultaParametro(int idConsulta)
        {
            List<VidaSecurity.Satelite.Transferencia.Parametro> param =
                new VidaSecurity.Satelite.Operacion()
                    .RetornaParametro(idConsulta);

            return JsonConvert.SerializeObject(
                new
                {
                    resultado = true,
                    param = param
                });
        }

        [System.Web.Services.WebMethod]
        public static string EjecutarConsulta(int idConsulta, string parametros)
        {
              List<string> listaParametros = parametros.Split('|').ToList();
            //List<string> listaParametros = null;
                    
            try
            {
                VidaSecurity.Framework.Query.Transferencia.RetornoComando retorno = new VidaSecurity.Satelite.Operacion().EjecutarConsulta(idConsulta, listaParametros);

                return JsonConvert.SerializeObject(
                  new
                  {
                      resultado = true,
                      cabecera = retorno.Cabeceras,
                      sabana = retorno.Sabana.ToString()
                  });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(
                 new
                 {
                     resultado = false,
                     mensaje = ex.Message
                 });
            }
        }

        [System.Web.Services.WebMethod]
        public static string ConsultaConexion(int idConsulta)
        {
            return new VidaSecurity.Satelite.Operacion()
                    .ConsultaCadenaConexion(idConsulta);
        }
    }
}