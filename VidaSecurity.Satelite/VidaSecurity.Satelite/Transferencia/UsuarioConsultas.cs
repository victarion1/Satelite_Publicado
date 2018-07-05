using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using VidaSecurity.Satelite.Util;

namespace VidaSecurity.Satelite.Transferencia
{
    public class UsuarioConsultas
    {
        public int Id { set; get; }

        public string Origen { set; get; }

        public string OrigenTexto { set; get; }

        public string Consulta { set; get; }

        public string Parametro { set; get; }

        public string Perfil { set; get; }

        public UsuarioConsultas(DataRow fila)
        {
            this.Id = Dato.GetDato<int>(fila["ID".ToString()]);
            this.Origen = Dato.GetDato<string>(fila["ORIGEN".ToString()]);
            this.OrigenTexto = Dato.GetDato<string>(fila["ORIGEN_TEXTO".ToString()]);
            this.Consulta = Dato.GetDato<string>(fila["CONSULTA".ToString()]);
            this.Parametro = Dato.GetDato<string>(fila["PARAMETRO".ToString()]);
            this.Perfil = Dato.GetDato<string>(fila["PERFIL".ToString()]);
        }
    }
}
