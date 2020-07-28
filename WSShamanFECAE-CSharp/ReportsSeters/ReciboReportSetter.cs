using System.Data.SqlClient;
using ShamanClases;
using DevExpress.XtraReports.UI;
using System.Web.Configuration;
using System;
using WSShamanFECAE_CSharp.report;
using static ShamanClases.modDeclares;
using System.Web;

using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Collections.Generic;

using WSShamanFECAE_CSharp.Models;

namespace WSShamanFECAE_CSharp.ReportsSeters
{
    public class ReciboReportSetter
    {
        public List<string> PersonalizarCamposRecibo(repRecibo objReport, ReciboModel rm, DataTable dtImportes, DataTable dtFormas, string importeDescripcion, string concepto)
        {
            //ReciboModel rm = new ReciboModel(dt, dtImp);
            objReport.RazonSocialCompleta.Text = rm.RazonSocialCompleta;
            objReport.EmpresaDomicilio.Text = rm.EmpresaDomicilio + "(" + rm.EmpresaCodigoPostal + ")";
            objReport.EmpresaSituacionIva.Text = rm.EmpresaSituacionIva;
            objReport.EmpresaCUIT.Text = rm.EmpresaCUIT;
            objReport.EmpresaNroIngresosBrutos.Text = rm.EmpresaNroIngresosBrutos;
            objReport.EmpresaInicio.Text = Convert.ToDateTime(rm.EmpresaInicio).ToShortDateString();


            objReport.NroComprobante.Text = rm.NroComprobante;
            objReport.FecDocumento.Text = modFechasCs.AnsiToDateString(rm.FecDocumento);
            objReport.Razon.Text = rm.RazonSocial;
            objReport.Domicilio.Text = rm.Domicilio;
            objReport.IVA.Text = rm.EmpresaSituacionIva;
            objReport.Cuit.Text = rm.Cuit;

            SetSubReportImportes(objReport, dtImportes);
            SetSubReportFormas(objReport, dtFormas, importeDescripcion, concepto);



            //if (rm.ImporteDescripcion != null  && rm.ImporteDescripcion.Length > 36)
            //{
            //    int cutIndex = rm.ImporteDescripcion.Substring(0, 36).LastIndexOf(' ');

            //    objReport.ImporteDescripcionPart1.Text = rm.ImporteDescripcion.Substring(0, cutIndex);
            //    objReport.ImporteDescripcionPart2.Text = rm.ImporteDescripcion.Substring(cutIndex, rm.ImporteDescripcion.Length - cutIndex);
            //}
            //else
            //{
            //    objReport.ImporteDescripcionPart1.Text = rm.ImporteDescripcion;
            //    objReport.ImporteDescripcionPart2.Text = "";
            //}

            //if (rm.Concepto != null && rm.Concepto.Length > 50)
            //{
            //    int cutIndex = rm.ImporteDescripcion.Substring(0, 50).LastIndexOf(' ');

            //    objReport.ConceptoPart1.Text = rm.Concepto.Substring(0, cutIndex);
            //    objReport.ConceptoPart2.Text = rm.Concepto.Substring(cutIndex, rm.ImporteDescripcion.Length - cutIndex);
            //}
            //else
            //{
            //    objReport.ConceptoPart1.Text = rm.Concepto;
            //    objReport.ConceptoPart2.Text = "";
            //}

            if (rm.Importes != null)
                SetImportes(objReport, rm.Importes);
            if (rm.ImportesPorTipo != null)
                SetImportesPorTipo(objReport, rm.ImportesPorTipo);

            //objReport.TotalIportes.Text = rm.TotalIportes;
            objReport.Importe.Text = "$ " + rm.Importe;

            objReport.da_Firma.Image = rm.Firma;
            objReport.da_Firma.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;

            //objReport.da_Firma2.Image = rm.Firma;
            //objReport.da_Firma2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            ////Personalizacion Tarjeta
            //long periodo = Convert.ToInt64(dt.Rows[0]["tc_Vencimiento"]);
            //objReport.tc_Vencimiento.Text = periodo > 12010 ? modFechas.GetPeriodo(periodo) : "";
            //rm.CamposPersonalizados.Add("tc_Vencimiento");
            //objReport.tc_TarjetaCredito.Text = dt.Rows[0]["tc_TarjetaCredito"].ToString();
            //rm.CamposPersonalizados.Add("tc_TarjetaCredito");

            return rm.CamposPersonalizados;
        }

        private void SetSubReportImportes(repRecibo objReport, DataTable dtImportes)
        {
            try
            {
                var bandsReport = objReport.Bands[BandKind.GroupHeader];
                XRSubreport detailReport = bandsReport.FindControl("xrSubreport1", true) as XRSubreport;
                XtraReport reportSource = detailReport.ReportSource as XtraReport;
                var bandsSubReport = reportSource.Bands[BandKind.Detail];
                XRTable table = bandsSubReport.FindControl("xrTable1", true) as XRTable;
                var realTable = (XRTable)((DevExpress.XtraPrinting.IBrickOwner)table).RealControl;

                float rowHeight = 25.0F;
                for (int i = 0; i <= (dtImportes.Rows.Count - 1); i++)
                {
                    XRTableRow rowObs = new XRTableRow();
                    rowObs.HeightF = rowHeight;
                    for (int j = 0; j <= 3; j++)
                    {
                        XRTableCell cell = new XRTableCell();
                        switch (j)
                        {
                            case 0:
                                cell.Text = Convert.ToDateTime(dtImportes.Rows[i]["FecDocumento"]).ToShortDateString();
                                cell.WidthF = 75;
                                cell.BorderWidth = 0.5F;
                                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                                break;
                            case 1:
                                cell.Text = dtImportes.Rows[i]["TipoComprobante"].ToString();
                                cell.WidthF = 50;
                                cell.BorderWidth = 0.5F;
                                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                                break;
                            case 2:
                                cell.Text = dtImportes.Rows[i]["NroComprobante"].ToString();
                                cell.WidthF = 100;
                                cell.BorderWidth = 0.5F;
                                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                                break;
                            case 3:
                                cell.Text = "$" + dtImportes.Rows[i]["Importe"].ToString();
                                cell.WidthF = 94;
                                cell.BorderWidth = 0.5F;
                                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                                break;
                        }
                        cell.Font = new Font("Tahoma", 8);
                        rowObs.Cells.Add(cell);
                    }
                    realTable.Rows.Add(rowObs);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetSubReportFormas(repRecibo objReport, DataTable dtFormas, string importeDescripcion, string concepto)
        {
            try
            {
                var bandsReport = objReport.Bands[BandKind.GroupHeader];
                XRSubreport detailReport = bandsReport.FindControl("xrSubreport2", true) as XRSubreport;
                XtraReport reportSource = detailReport.ReportSource as XtraReport;
                var bandsSubReport = reportSource.Bands[BandKind.Detail];


                XRLabel importeDescripcionPart1 = bandsSubReport.FindControl("importeDescripcionPart1", true) as XRLabel;
                XRLabel importeDescripcionPart2 = bandsSubReport.FindControl("importeDescripcionPart2", true) as XRLabel;
                XRLabel conceptoPart1 = bandsSubReport.FindControl("ConceptoPart1", true) as XRLabel;
                //XRLabel conceptoPart2 = bandsSubReport.FindControl("ConceptoPart2", true) as XRLabel;

                if (importeDescripcion.Length <= 30)
                {
                    importeDescripcionPart1.Text = importeDescripcion;
                    importeDescripcionPart2.Text = "";
                }
                else
                {
                    string part1 = importeDescripcion.Substring(0, 29);

                    importeDescripcionPart1.Text = importeDescripcion.Substring(29, 1) != " " ? part1 + "-" : part1;
                    importeDescripcionPart2.Text = importeDescripcion.Substring(29, importeDescripcion.Length - 29);
                }

                conceptoPart1.Text = concepto;

                XRTable table = bandsSubReport.FindControl("xrTable1", true) as XRTable;
                var realTable = (XRTable)((DevExpress.XtraPrinting.IBrickOwner)table).RealControl;

                float rowHeight = 25.0F;
                for (int i = 0; i <= (dtFormas.Rows.Count - 1); i++)
                {
                    XRTableRow rowObs = new XRTableRow();
                    rowObs.HeightF = rowHeight;
                    for (int j = 0; j <= 4; j++)
                    {
                        XRTableCell cell = new XRTableCell();
                        string formaDePago = dtFormas.Rows[i]["FormaDePago"].ToString();
                        switch (j)
                        {
                            case 0:
                                cell.Text = formaDePago;
                                cell.WidthF = 52.5F;
                                cell.BorderWidth = 0.5F;
                                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                                break;
                            case 1:
                                cell.Text = formaDePago == "EF" ? "" : dtFormas.Rows[i]["Banco"].ToString();
                                cell.WidthF = 90;
                                cell.BorderWidth = 0.5F;
                                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                                break;
                            case 2:
                                cell.Text = formaDePago != "CH" ? "" : dtFormas.Rows[i]["NroCheque"].ToString();
                                cell.WidthF = 80;
                                cell.BorderWidth = 0.5F;
                                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                                break;
                            case 3:
                                cell.Text = formaDePago != "CH" ? "" : Convert.ToDateTime(dtFormas.Rows[i]["FecCheque"]).ToShortDateString();
                                cell.WidthF = 75;
                                cell.BorderWidth = 0.5F;
                                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                                break;
                            case 4:
                                cell.Text = "$" + dtFormas.Rows[i]["Importe"].ToString();
                                cell.WidthF = 94;
                                cell.BorderWidth = 0.5F;
                                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                                break;
                        }
                        cell.Font = new Font("Tahoma", 8);
                        rowObs.Cells.Add(cell);
                    }
                    realTable.Rows.Add(rowObs);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void SetImportesPorTipo(repRecibo objReport, List<ImportesPorTipo> importesPorTipo)
        {
            int importesPorTipoCount = importesPorTipo.Count;
            //if (importesPorTipoCount > 0)
            //{
            //    objReport.efectivo1.Text = importesPorTipo[0].Efectivo;
            //    objReport.cheque1.Text = importesPorTipo[0].Cheque;
            //    objReport.documento1.Text = importesPorTipo[0].Documento;
            //}
            //if (importesPorTipoCount > 1)
            //{
            //    objReport.efectivo2.Text = importesPorTipo[1].Efectivo;
            //    objReport.cheque2.Text = importesPorTipo[1].Cheque;
            //    objReport.documento2.Text = importesPorTipo[1].Documento;
            //}
            //if (importesPorTipoCount > 2)
            //{
            //    objReport.efectivo3.Text = importesPorTipo[2].Efectivo;
            //    objReport.cheque3.Text = importesPorTipo[2].Cheque;
            //    objReport.documento3.Text = importesPorTipo[2].Documento;
            //}
            //if (importesPorTipoCount > 3)
            //{
            //    objReport.efectivo4.Text = importesPorTipo[3].Efectivo;
            //    objReport.cheque4.Text = importesPorTipo[3].Cheque;
            //    objReport.documento4.Text = importesPorTipo[3].Documento;
            //}
            //if (importesPorTipoCount > 4)
            //{
            //    objReport.efectivo5.Text = importesPorTipo[4].Efectivo;
            //    objReport.cheque5.Text = importesPorTipo[4].Cheque;
            //    objReport.documento5.Text = importesPorTipo[4].Documento;
            //}
            //if (importesPorTipoCount > 5)
            //{
            //    objReport.efectivo6.Text = importesPorTipo[5].Efectivo;
            //    objReport.cheque6.Text = importesPorTipo[5].Cheque;
            //    objReport.documento6.Text = importesPorTipo[5].Documento;
            //}
            //if (importesPorTipoCount > 6)
            //{
            //    objReport.efectivo7.Text = importesPorTipo[6].Efectivo;
            //    objReport.cheque7.Text = importesPorTipo[6].Cheque;
            //    objReport.documento7.Text = importesPorTipo[6].Documento;
            //}
        }

        private void SetImportes(repRecibo objReport, List<ImporteItem> importes)
        {
            int importesCount = importes.Count;
            //if (importesCount > 0)
            //{
            //    objReport.impFecha1.Text = importes[0].Fecha;
            //    objReport.impImporte1.Text = importes[0].Importe;
            //    objReport.impNroFactura1.Text = importes[0].NroFactura;
            //}
            //if (importesCount > 1)
            //{
            //    objReport.impFecha2.Text = importes[1].Fecha;
            //    objReport.impImporte2.Text = importes[1].Importe;
            //    objReport.impNroFactura2.Text = importes[1].NroFactura;
            //}
            //if (importesCount > 2)
            //{
            //    objReport.impFecha3.Text = importes[2].Fecha;
            //    objReport.impImporte3.Text = importes[2].Importe;
            //    objReport.impNroFactura3.Text = importes[2].NroFactura;
            //}
            //if (importesCount > 3)
            //{
            //    objReport.impFecha4.Text = importes[3].Fecha;
            //    objReport.impImporte4.Text = importes[3].Importe;
            //    objReport.impNroFactura4.Text = importes[3].NroFactura;
            //}
            //if (importesCount > 4)
            //{
            //    objReport.impFecha5.Text = importes[4].Fecha;
            //    objReport.impImporte5.Text = importes[4].Importe;
            //    objReport.impNroFactura5.Text = importes[4].NroFactura;
            //}
            //if (importesCount > 5)
            //{
            //    objReport.impFecha6.Text = importes[5].Fecha;
            //    objReport.impImporte6.Text = importes[5].Importe;
            //    objReport.impNroFactura6.Text = importes[5].NroFactura;
            //}
            //if (importesCount > 6)
            //{
            //    objReport.impFecha7.Text = importes[6].Fecha;
            //    objReport.impImporte7.Text = importes[6].Importe;
            //    objReport.impNroFactura7.Text = importes[6].NroFactura;
            //}
            //if (importesCount > 7)
            //{
            //    objReport.impFecha8.Text = importes[7].Fecha;
            //    objReport.impImporte8.Text = importes[7].Importe;
            //    objReport.impNroFactura8.Text = importes[7].NroFactura;
            //}
            //if (importesCount > 8)
            //{
            //    objReport.impFecha9.Text = importes[8].Fecha;
            //    objReport.impImporte9.Text = importes[8].Importe;
            //    objReport.impNroFactura9.Text = importes[8].NroFactura;
            //}
            //if (importesCount > 9)
            //{
            //    objReport.impFecha10.Text = importes[9].Fecha;
            //    objReport.impImporte10.Text = importes[9].Importe;
            //    objReport.impNroFactura10.Text = importes[9].NroFactura;
            //}
            //if (importesCount > 10)
            //{
            //    objReport.impFecha11.Text = importes[10].Fecha;
            //    objReport.ImpImporte11.Text = importes[10].Importe;
            //    objReport.impNroFactura11.Text = importes[10].NroFactura;
            //}
            //if (importesCount > 11)
            //{
            //    objReport.impFecha12.Text = importes[11].Fecha;
            //    objReport.ImpImporte12.Text = importes[11].Importe;
            //    objReport.impNroFactura12.Text = importes[11].NroFactura;
            //}
        }
    }
}