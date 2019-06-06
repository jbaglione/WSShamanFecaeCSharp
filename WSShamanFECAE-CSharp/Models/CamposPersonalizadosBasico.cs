using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;

namespace WSShamanFECAE_CSharp.Models
{
    public class CamposPersonalizadosBasico
    {
        public string FechaIngreso { get; set; }
        public string Aclaracion { get; set; }
        public string EntreCalles { get; set; }
        public string ImporteMensualDescripcion { get; set; }
        public Image Firma { get; set; }
        public List<string> CamposPersonalizados { get; set; }
        public CamposPersonalizadosBasico(DataTable dt)
        {
            FechaIngreso = modFechasCs.NtoD(Convert.ToInt64(dt.Rows[0]["sdb_FechaIngreso"])).ToString().Substring(0, 10);
            Aclaracion = dt.Rows[0]["cc_NombreCompleto"].ToString();

            //Entre Calles.
            string calle1 = dt.Rows[0]["ap_EntreCalle1"].ToString() == "\0" ? "" : dt.Rows[0]["ap_EntreCalle1"].ToString();
            string calle2 = dt.Rows[0]["ap_EntreCalle2"].ToString() == "\0" ? "" : dt.Rows[0]["ap_EntreCalle2"].ToString();
            EntreCalles = string.IsNullOrEmpty(calle2) ? calle1 : calle1 + " Y " + calle2;

            //Importe en letras.
            NumerosLetra numLetras = new NumerosLetra();
            ImporteMensualDescripcion = "( PESOS " + numLetras.enletras(dt.Rows[0]["da_ImporteMensual"].ToString()) + ")";

            string firma = dt.Rows[0]["Firma"].ToString();
            if (!string.IsNullOrEmpty(firma) && firma != "\0")
            {
                Firma = ByteArrayToImage(Convert.FromBase64String(firma.Replace("data:image/png;base64,", "")));
            }
            CamposPersonalizados = new List<string> { "sdb_fechaingreso", "aclaracion", "aclaracion2", "ap_entrecalles", "tc_vencimiento", "da_importemensualdescripcion_c" };
        }
        private Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }

}