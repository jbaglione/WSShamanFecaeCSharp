using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace WSShamanFECAE_CSharp.Models
{
    public class ReciboModel
    {

        public string RazonSocialCompleta { get; set; }
        public string EmpresaDomicilio { get; set; }
        public string EmpresaCodigoPostal { get; set; }

        public string EmpresaSituacionIva { get; set; }
        public string EmpresaCUIT { get; set; }
        public string EmpresaNroIngresosBrutos { get; set; }
        public string EmpresaInicio { get; set; }

        public string NroComprobante { get; set; }
        public string FecDocumento { get; set; }
        public string RazonSocial { get; set; }
        public string Domicilio { get; set; }
        //public string SituacionIva { get; set; } //no queda claro si pertenece al cliente o paramedic
        public string Cuit { get; set; }

        public List<ImporteItem> Importes { get; set; }
        public List<ImportesPorTipo> ImportesPorTipo { get; set; }

        //public string TotalIportes { get; set; }
        //public string TotalSon { get; set; }
        public string Importe { get; set; }

        public string ImporteDescripcion { get; set; }
        public string Concepto { get; set; }
        public string FormaPago { get; set; }

        public Image Firma { get; set; }

        public List<string> CamposPersonalizados { get; set; }

        private Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

    }
}