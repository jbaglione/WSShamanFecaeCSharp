using System;
using System.Drawing;
using System.Collections.Generic;

using DevExpress.XtraReports.UI;

using WSShamanFECAE_CSharp.report;
using WSShamanFECAE_CSharp.Models.RecetaModels;


namespace WSShamanFECAE_CSharp.ReportsSeters
{
    public class RecetaReportSetter
    {
        public void PersonalizarCamposReceta(repReceta objReport, RecetaModel rm)
        {
            objReport.nroReceta.Text = rm.NroReceta.ToString("0000000000");
            //ReciboModel rm = new ReciboModel(dt, dtImp);
            objReport.Paciente.Text = rm.Nombre;
            objReport.Entidad.Text = rm.Cliente.ClienteId;
            objReport.plan.Text = rm.Plan.Id;
            objReport.NroAfiliado.Text = rm.NroAfiliado;
            objReport.Diagnostico.Text = rm.Diagnostico;
            objReport.TratamientoProlongado.Text = rm.flgTratamientoProlongado == 1 ? "SI" : "NO";
            objReport.Fecha.Text = rm.FecReceta.ToShortDateString();
            objReport.medico.Text = rm.Medico;
            objReport.Matricula.Text = rm.Matricula;


            SetSubReportMedicamentosText(objReport, rm.Medicamentos);

        }

        private void SetSubReportMedicamentos(repReceta objReport, List<MedicamentosModel> medicamentos)
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
                foreach (var med in medicamentos)
                {
                    XRTableRow rowObs = new XRTableRow();
                    rowObs.HeightF = rowHeight;
                    for (int j = 0; j <= 4; j++)
                    {
                        XRTableCell cell = new XRTableCell();
                        switch (j)
                        {
                            case 0:
                                cell.Text = med.Nombre;
                                cell.WidthF = 80;
                                cell.BorderWidth = 0.5F;
                                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                                break;
                            case 1:
                                cell.Text = med.Droga;
                                cell.WidthF = 75;
                                cell.BorderWidth = 0.5F;
                                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                                break;
                            case 2:
                                cell.Text = med.Presentacion;
                                cell.WidthF = 90;
                                cell.BorderWidth = 0.5F;
                                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                                break;
                            case 3:
                                cell.Text = med.Observaciones;
                                cell.WidthF = 90;
                                cell.BorderWidth = 0.5F;
                                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                                break;
                            case 4:
                                cell.Text = med.Cantidad.ToString() + " ";
                                cell.WidthF = 20;
                                cell.BorderWidth = 0.5F;
                                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                                break;
                        }
                        cell.Font = new Font("Arial", 8);
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

        private void SetSubReportMedicamentosText(repReceta objReport, List<MedicamentosModel> medicamentos)
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
                foreach (var med in medicamentos)
                {
                    XRTableRow rowObs = new XRTableRow();
                    rowObs.HeightF = rowHeight;
                    for (int j = 0; j <= 1; j++)
                    {
                        XRTableCell cell = new XRTableCell();
                        switch (j)
                        {
                            case 0:
                                cell.Text = med.MedicamentoFull;
                                cell.WidthF = 330;
                                cell.BorderWidth = 0F;
                                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                                cell.Multiline = true;
                                break;
                            case 1:
                                cell.Text = string.Format("X {0}", med.Cantidad);
                                cell.WidthF = 20;
                                cell.BorderWidth = 0F;
                                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                                break;
                        }
                        cell.Font = new Font("Arial", 8);
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
    }
}