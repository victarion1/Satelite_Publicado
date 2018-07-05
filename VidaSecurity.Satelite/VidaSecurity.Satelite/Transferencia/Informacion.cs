using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidaSecurity.Satelite.Transferencia
{
    public class Informacion
    {
        private string v;
        private string empty;

        public bool Error { set; get; }

        public string ErrorMensaje { set; get; }

        public string Respuesta { set; get; }

        public Informacion()
        {
            this.Error = false;
            this.ErrorMensaje = string.Empty;
            this.Respuesta = string.Empty;
        }

        public Informacion(bool error, string errorMensaje, string respuesta)
        {
            this.Error = error;
            this.ErrorMensaje = errorMensaje;
            this.Respuesta = respuesta;
        }

        public Informacion(string v, string empty)
        {
            this.v = v;
            this.empty = empty;
        }
    }
}
