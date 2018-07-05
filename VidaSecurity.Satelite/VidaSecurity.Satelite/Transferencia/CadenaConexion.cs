using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

using System.Data;
using VidaSecurity.Satelite.Util;

namespace VidaSecurity.Satelite.Transferencia
{
    public class CadenaConexion
    {
        public int IdBd { set; get; }

        public string NombreBd { set; get; }

        public string Conexion { set; get; }

        public CadenaConexion(DataRow cadena)
        {
            this.IdBd = Dato.GetDato<int>(cadena["ID_BD".ToString()]);
            this.NombreBd = Dato.GetDato<string>(cadena["NOMBRE_BD".ToString()]);
            this.Conexion = Dato.GetDato<string>(cadena["CONEXION".ToString()]);
        }
    }
}
