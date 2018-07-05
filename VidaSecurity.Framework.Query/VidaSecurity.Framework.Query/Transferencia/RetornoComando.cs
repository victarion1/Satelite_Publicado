// Decompiled with JetBrains decompiler
// Type: VidaSecurity.Framework.Query.Transferencia.RetornoComando
// Assembly: VidaSecurity.Framework.Query, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9DD9E185-F5E8-47E5-8AC0-48BCE51D0C82
// Assembly location: D:\Nueva carpeta (2)\VidaSecurity.Framework.Query.dll

using System.Collections.Generic;
using System.Data;

namespace VidaSecurity.Framework.Query.Transferencia
{
    public class RetornoComando
    {
        private List<string> cabeceras;
        private DataTable sabana;

        public RetornoComando(List<string> cabeceras, DataTable sabana)
        {
            this.cabeceras = cabeceras;
            this.sabana = sabana;
        }

        public List<string> Cabeceras
        {
            get
            {
                return this.cabeceras;
            }
            set
            {
                this.cabeceras = value;
            }
        }

        public DataTable Sabana
        {
            get
            {
                return this.sabana;
            }
            set
            {
                this.sabana = value;
            }
        }
    }
}
