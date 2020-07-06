using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSShamanFECAE_CSharp.Models.RecetaModels
{
    public class RecetaModel
    {
        public decimal Id { get; set; }
        public decimal NroReceta { get; set; }
        public ClientesRecetas Cliente { get; set; }
        public string NroAfiliado { get; set; }
        public ClientesPlanes Plan { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public short flgFueraPadron { get; set; }

        public DateTime FecReceta { get; set; }
        public string FecHorCreacion { get; set; }
        public string Observaciones { get; set; }

        public string Medico { get; set; }

        public string MatriculaNacional { get; set; }
        public string MatriculaProvincial { get; set; }
        public string MatriculaOtra { get; set; }

        public string Matricula
        {
            get
            {
                if(!string.IsNullOrEmpty(MatriculaNacional))
                    return string.Format("M.N. {0}", MatriculaNacional);
                else if (!string.IsNullOrEmpty(MatriculaProvincial))
                    return string.Format("M.P. {0}", MatriculaProvincial);
                else if (!string.IsNullOrEmpty(MatriculaOtra))
                    return string.Format("M.Nro. {0}", MatriculaOtra);
                return "";
            }
        }

        public decimal UsuarioExtranetEmisorId { get; set; }


        public List<MedicamentosModel> Medicamentos { get; set; }
    }
}