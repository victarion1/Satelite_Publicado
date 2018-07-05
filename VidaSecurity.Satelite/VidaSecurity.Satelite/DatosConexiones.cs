using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidaSecurity.Satelite
{
    public static class DatosConexiones
    {
        public const string Conexion = "SATELITE";

        internal static class Procedimiento
        {
            public const string SpConsultarUsuarioSatelite = "usp_ConsultarUsuarioSatelite";
            public const string SpConsultaParametros = "ups_ConsultaParametros";
            public const string SpDetalleConsulta = "usp_DetalleConsulta";
            public const string SpConsultaCadenaConexion = "ups_ConsultaCadenaConexion";
        }
    }
}
