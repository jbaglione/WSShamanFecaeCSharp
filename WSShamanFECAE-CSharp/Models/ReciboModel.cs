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

        //public ReciboModel(DataTable dt, DataTable dtImp)
        //{
        //    NroComprobante = "000123";//
        //    FecDocumento = "14/02/19"; //modFechas.NtoD(Convert.ToInt64(dt.Rows[0]["FecDocumento"])).ToString().Substring(0, 10);
        //    Razon = "Jonathan Paul";// dt.Rows[0]["Razon"].ToString();
        //    Domicilio = "Antartida Argentina 5858, José C. Paz";//dt.Rows[0]["Domicilio"].ToString();
        //    IVA = "Consumidor Final";//dt.Rows[0]["IVA"].ToString();
        //    Cuit = "20-3425909-0";//dt.Rows[0]["Cuit"].ToString();

        //    Importes = new List<ImporteItem>();
        //    ImportesPorTipo = new List<ImportesPorTipo>();

        //    //Hidden default text.
        //    for (int vIdx = 0; vIdx <= 11; vIdx++)
        //    {
        //        Importes.Add(new ImporteItem());
        //    }
        //    //Hidden default text.
        //    for (int vIdx = 0; vIdx <= 6; vIdx++)
        //    {
        //        ImportesPorTipo.Add(new ImportesPorTipo());
        //    }

        //    for (int vIdx = 0; vIdx <= (dtImp.Rows.Count - 1); vIdx++)
        //    {
        //        //ImporteItem imp = new ImporteItem();
        //        //imp.Fecha = "14/02/19"; //modFechas.NtoD(Convert.ToInt64(dtImp.Rows[vIdx]["Fecha"])).ToString().Substring(0, 10);
        //        //imp.NroFactura = vIdx.ToString(); //dtImp.Rows[vIdx]["NroFactura"].ToString();
        //        //imp.Importe = (vIdx * 10).ToString();//dtImp.Rows[vIdx]["Importe"].ToString();
        //        //Importes.Add(imp);
        //        Importes[vIdx].Fecha = "14/02/19"; //modFechas.NtoD(Convert.ToInt64(dtImp.Rows[vIdx]["Fecha"])).ToString().Substring(0, 10);
        //        Importes[vIdx].NroFactura = vIdx.ToString(); //dtImp.Rows[vIdx]["NroFactura"].ToString();
        //        Importes[vIdx].Importe = (vIdx * 10).ToString();//dtImp.Rows[vIdx]["Importe"].ToString();
        //    }

        //    for (int vIdx = 0; vIdx <= (dtImp.Rows.Count - 1); vIdx++)
        //    {
        //        //ImporteItem imp = new ImporteItem();
        //        //imp.Fecha = "14/02/19"; //modFechas.NtoD(Convert.ToInt64(dtImp.Rows[vIdx]["Fecha"])).ToString().Substring(0, 10);
        //        //imp.NroFactura = vIdx.ToString(); //dtImp.Rows[vIdx]["NroFactura"].ToString();
        //        //imp.Importe = (vIdx * 10).ToString();//dtImp.Rows[vIdx]["Importe"].ToString();
        //        //Importes.Add(imp);
        //        ImportesPorTipo[vIdx].Efectivo = "$500"; //modFechas.NtoD(Convert.ToInt64(dtImp.Rows[vIdx]["Fecha"])).ToString().Substring(0, 10);
        //        ImportesPorTipo[vIdx].Cheque = "$1001"; //dtImp.Rows[vIdx]["NroFactura"].ToString();
        //        ImportesPorTipo[vIdx].Documento = "0";//dtImp.Rows[vIdx]["Importe"].ToString();
        //    }

        //    ImporteDescripcion = "Jonathan Paul";// dt.Rows[0]["Razon"].ToString();

        //    //Importe en letras.
        //    NumerosLetra numLetras = new NumerosLetra();
        //    ImporteDescripcion = numLetras.enletras("1744,25"); //dt.Rows[0]["ImporteDescripcion"].ToString();
        //    Concepto = "Desarrollo de Software"; //dt.Rows[0]["Concepto"].ToString();
        //    FormaPago = "Efectivo"; //dt.Rows[0]["FormaPago"].ToString();
        //    TotalIportes = "$ 1744,25";
        //    TotalSon = "$ 2744,25";

        //    string firma = dt.Rows[0]["Firma"].ToString();
        //    if (!string.IsNullOrEmpty(firma) && firma != "\0")
        //    {
        //        Firma = ByteArrayToImage(Convert.FromBase64String(firma.Replace("data:image/png;base64,", "")));
        //    }
        //    CamposPersonalizados = new List<string>
        //        {
        //            "FecDocumento", "IVA", "Cuit", "Domicilio", "Razon", "da_Firma", "NroComprobante",
        //            "impImporte10", "impNroFactura8", "impFecha8", "impImporte9", "impNroFactura7",
        //            "impFecha7", "impImporte8", "impNroFactura6", "impFecha6", "impImporte6", "impNroFactura5",
        //            "impFecha5", "impImporte4", "impNroFactura4", "impFecha4", "impImporte3", "impNroFactura3",
        //            "impFecha3", "impImporte2", "impNroFactura2", "impFecha2", "impFecha1", "impNroFactura1",
        //            "impImporte1", "ConceptoPart2", "ConceptoPart1", "ImporteDescripcionPart2", "ImporteDescripcionPart1",
        //            "efectivo6", "efectivo5", "cheque5", "cheque6", "documento6", "documento5", "documento7", "cheque7",
        //            "efectivo7", "TotalIportes", "TotalSon", "impImporte7", "impNroFactura12", "impFecha12", "impImporte5",
        //            "impNroFactura11", "impFecha11", "efectivo4", "cheque4", "documento4", "impFecha9", "impNroFactura9",
        //            "ImpImporte11", "impFecha10", "impNroFactura10", "ImpImporte12", "documento1", "documento2", "documento3",
        //            "cheque3", "cheque2", "cheque1", "efectivo1", "efectivo2", "efectivo3" };
        //}
        private Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

    }
}