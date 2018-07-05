using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System;

namespace VidaSecurity.Satelite.Util
{
    internal static class Dato
    {
        public static T GetDato<T>(object valor)
        {
            if (valor == null || valor == DBNull.Value)
                return default(T);
            return (T)Convert.ChangeType(valor, typeof(T));
        }
    }
}
