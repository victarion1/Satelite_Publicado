using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FrameWork.Data;

namespace VidaSecurity.Satelite.Util
{
    internal static class Constante
    {
        public static List<Constante.conexiones> ListaConexiones
        {
           
            get
            {
                return new List<Constante.conexiones>()
        {
          new Constante.conexiones()
          {
              
            CadenaEncriptada = "",
            Cadena = "driver={SQL Server};server=kronos;database=satelite;uid=satelite;pwd=satelite"
          }
        };
            }
        }

        internal struct conexiones
        {
            public string CadenaEncriptada;
            public string Cadena;
        }
    }
}
