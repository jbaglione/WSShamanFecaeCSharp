using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSShamanFECAE_CSharp.Models.RecetaModels
{
    public class MedicamentosModel
    {
        public decimal Id { get; set; }
        public int NroRenglon { get; set; }
        public string AbreviaturaId { get; set; }
        public string Nombre { get; set; }
        public string Droga { get; set; }
        public string DrogaId { get; set; }
        public string Presentacion { get; set; }
        public int Cantidad { get; set; }
        public string Observaciones { get; set; }
        public int flgNomenclado { get; set; }

        public string MedicamentoFull
        {
            get
            {
                var medFull = string.Format("{0}, {1}{2}{3}{4}", Nombre, Presentacion, Environment.NewLine, Droga, Environment.NewLine);
                if(!string.IsNullOrEmpty(Observaciones))
                    medFull += string.Format("{0}{1}", Observaciones, Environment.NewLine);

                return medFull + Environment.NewLine;
            }
        }
    }
}