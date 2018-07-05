using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using VidaSecurity.Satelite.Util;

namespace VidaSecurity.Satelite.Transferencia
{
    internal class InfoBd
    {
        public bool EsOracle { set; get; }

        public string Cadena { set; get; }

        public InfoBd(DataRow consulta)
        {
            this.EsOracle = Dato.GetDato<bool>(consulta["NOMBRE_BD".ToString()]);
            this.Cadena = Dato.GetDato<string>(consulta["CONEXION".ToString()]);
        }
    }
}
