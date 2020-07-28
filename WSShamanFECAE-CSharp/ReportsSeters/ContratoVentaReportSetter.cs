using System;
using System.Data;
using System.Collections.Generic;

using ShamanClases;
using WSShamanFECAE_CSharp.report;
using WSShamanFECAE_CSharp.Models;

namespace WSShamanFECAE_CSharp.ReportsSeters
{
    public class ContratoVentaReportSetter
    {
        public List<string> PersonalizarCamposTarjeta(repContratoVenta_tc objReport, DataTable dt)
        {
            CamposPersonalizadosBasico cpb = new CamposPersonalizadosBasico(dt);
            objReport.sdb_FechaIngreso.Text = cpb.FechaIngreso;
            objReport.aclaracion.Text = cpb.Aclaracion;
            objReport.aclaracion2.Text = cpb.Aclaracion;
            objReport.ap_EntreCalles.Text = cpb.EntreCalles;
            objReport.da_ImporteMensualDescripcion_c.Text = cpb.ImporteMensualDescripcion;
            objReport.da_Firma.Image = cpb.Firma;
            objReport.da_Firma.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            objReport.da_Firma2.Image = cpb.Firma;
            objReport.da_Firma2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;

            //Personalizacion Tarjeta
            long periodo = Convert.ToInt64(dt.Rows[0]["tc_Vencimiento"]);
            objReport.tc_Vencimiento.Text = periodo > 12010 ? modFechas.GetPeriodo(periodo) : "";
            cpb.CamposPersonalizados.Add("tc_Vencimiento");
            objReport.tc_TarjetaCredito.Text = dt.Rows[0]["tc_TarjetaCredito"].ToString();
            cpb.CamposPersonalizados.Add("tc_TarjetaCredito");

            return cpb.CamposPersonalizados;
        }
        public List<string> PersonalizarCamposDebitoAutomatico(repContratoVenta_da objReport, DataTable dt)
        {
            CamposPersonalizadosBasico cpb = new CamposPersonalizadosBasico(dt);
            objReport.sdb_FechaIngreso.Text = cpb.FechaIngreso;
            objReport.aclaracion.Text = cpb.Aclaracion;
            objReport.aclaracion2.Text = cpb.Aclaracion;
            objReport.ap_EntreCalles.Text = cpb.EntreCalles;
            objReport.da_ImporteMensualDescripcion_c.Text = cpb.ImporteMensualDescripcion;
            objReport.da_Firma.Image = cpb.Firma;
            objReport.da_Firma.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            objReport.da_Firma2.Image = cpb.Firma;
            objReport.da_Firma2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;

            ////Personalizacion Debito En Cuenta
            //objReport.dc_NombreTitular.Text = dt.Rows[0]["dc_NombreTitular"].ToString();
            //objReport.dc_TipoCuenta.Text = periodo > 12010 ? modFechas.GetPeriodo(periodo) : "";
            //objReport.tc_.Text = 
            //cpb.CamposPersonalizados.Add("tc_Vencimiento");

            return cpb.CamposPersonalizados;
        }

        public List<string> PersonalizarCamposDebitoEfectivo(repContratoVenta objReport, DataTable dt)
        {
            CamposPersonalizadosBasico cpb = new CamposPersonalizadosBasico(dt);
            objReport.sdb_FechaIngreso.Text = cpb.FechaIngreso;
            objReport.aclaracion.Text = cpb.Aclaracion;
            objReport.aclaracion2.Text = cpb.Aclaracion;
            objReport.ap_EntreCalles.Text = cpb.EntreCalles;
            objReport.da_ImporteMensualDescripcion_c.Text = cpb.ImporteMensualDescripcion;
            objReport.da_Firma.Image = cpb.Firma;
            objReport.da_Firma.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            objReport.da_Firma2.Image = cpb.Firma;
            objReport.da_Firma2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;

            ////Personalizacion Debito En Cuenta
            //objReport.dc_NombreTitular.Text = dt.Rows[0]["dc_NombreTitular"].ToString();
            //objReport.dc_TipoCuenta.Text = periodo > 12010 ? modFechas.GetPeriodo(periodo) : "";
            //objReport.tc_.Text = 
            //cpb.CamposPersonalizados.Add("tc_Vencimiento");

            return cpb.CamposPersonalizados;
        }

        public List<string> PersonalizarCamposTransferencia(repContratoVenta_tb objReport, DataTable dt)
        {
            CamposPersonalizadosBasico cpb = new CamposPersonalizadosBasico(dt);
            objReport.sdb_FechaIngreso.Text = cpb.FechaIngreso;
            objReport.aclaracion.Text = cpb.Aclaracion;
            objReport.aclaracion2.Text = cpb.Aclaracion;
            objReport.ap_EntreCalles.Text = cpb.EntreCalles;
            objReport.da_ImporteMensualDescripcion_c.Text = cpb.ImporteMensualDescripcion;
            objReport.da_Firma.Image = cpb.Firma;
            objReport.da_Firma.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            objReport.da_Firma2.Image = cpb.Firma;
            objReport.da_Firma2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;

            ////Personalizacion Debito En Cuenta
            //objReport.dc_NombreTitular.Text = dt.Rows[0]["dc_NombreTitular"].ToString();
            //objReport.dc_TipoCuenta.Text = periodo > 12010 ? modFechas.GetPeriodo(periodo) : "";
            //objReport.tc_.Text = 
            //cpb.CamposPersonalizados.Add("tc_Vencimiento");

            return cpb.CamposPersonalizados;
        }
    }
}