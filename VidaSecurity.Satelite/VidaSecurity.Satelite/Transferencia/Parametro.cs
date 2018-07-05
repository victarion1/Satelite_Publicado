using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.Generic;

namespace VidaSecurity.Satelite.Transferencia
{
    public class Parametro
    {
        public string Nombre { set; get; }

        public string TipoDato { set; get; }

        public bool EsLista { set; get; }

        public List<ParDato> Lista { set; get; }

        public string Id
        {
            get
            {
                return string.Format("id{0}", (object)this.Nombre.ToLower().Trim().Replace(" ", string.Empty).Replace("/", string.Empty).Replace("_", string.Empty).Replace("-", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty));
            }
        }
    }
}
