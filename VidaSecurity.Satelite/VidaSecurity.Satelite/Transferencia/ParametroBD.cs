using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using VidaSecurity.Satelite.Util;

namespace VidaSecurity.Satelite.Transferencia
{
    public class ParametroBD
    {
        public string Nombre { set; get; }

        public string TipoDato { set; get; }

        public string TipoAyuda { set; get; }

        public string AyudaValores { set; get; }

        public ParametroBD(DataRow parametros)
        {
            this.Nombre = Dato.GetDato<string>(parametros["nombre".ToString()]);
            this.TipoDato = Dato.GetDato<string>(parametros["tipo_parametro".ToString()]);
            this.TipoAyuda = Dato.GetDato<string>(parametros["tipo_ayuda".ToString()]);
            this.AyudaValores = Dato.GetDato<string>(parametros["ayuda_valores".ToString()]);
        }
    }
}
