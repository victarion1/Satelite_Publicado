using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using VidaSecurity.Satelite.Util;

namespace VidaSecurity.Satelite.Transferencia
{
    public class Consulta
    {
        public int IdBaseDatos { set; get; }

        public string Query { set; get; }

        public string Conexion { set; get; }

        public Consulta(DataRow consulta)
        {
            this.IdBaseDatos = Dato.GetDato<int>(consulta["IdConsultaQuery"]);
            this.Query = Dato.GetDato<string>(consulta[nameof(Query)]);
            this.Conexion = Dato.GetDato<string>(consulta[nameof(Conexion)]);
        }
    }
}
