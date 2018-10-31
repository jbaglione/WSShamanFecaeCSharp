using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using ShamanClases;
using DevExpress.XtraReports.UI;
using System.Web.Configuration;
using System;
using WSShamanFECAE_CSharp.Properties;
using WSShamanFECAE_CSharp.report;
using static ShamanClases.modDeclares;
using System.Web;
using System.IO;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Drawing;

namespace WSShamanFECAE_CSharp
{
    //  To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    //  <System.Web.Script.Services.ScriptService()> _
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class WSShamanFECAE : WebService
    {

        public enum Certificado : int
        {
            crtArba = 0,
            crtAgip = 1,
            crtGanancias = 2,
            crtIVA = 3,
            crtCajaPrevisional = 4,
            crtContratoVenta = 4,
        }

        public enum ContratoVenta : int
        {
            cvTarjetaDeCredito,
            cvDebitoAutomatico,
            cvTransferenciaBancaria,
            cvPagoMisCuentas
        }

        [WebMethod()]
        public byte[] GetPDF_Cache(decimal pDocId, decimal pUsrId)
        {
            ShamanClases.PanelC.Conexion objConexion = new ShamanClases.PanelC.Conexion();
            MemoryStream vRet = null;

            NameValueCollection appSettings = WebConfigurationManager.AppSettings;

            if (objConexion.Iniciar(appSettings.Get("cacheServer"), int.Parse(appSettings.Get("cachePort")), appSettings.Get("cacheNameSpace"), appSettings.Get("cacheShamanAplicacion"), appSettings.Get("cacheShamanUser"), int.Parse(appSettings.Get("cacheShamanCentro")), true))
            {
                // ----> Obtengo documento
                ShamanClases.VentasC.ClientesDocumentos objComprobante = new ShamanClases.VentasC.ClientesDocumentos();
                DataView vDataView = new DataView();
                MemoryStream vStream = new MemoryStream();
                if (objComprobante.prepareToPrint(objConexion.PID, pDocId, ref vDataView, ref vStream))
                {
                    repImagenDocumento objReport = new repImagenDocumento();
                    objReport.DataSource = vDataView;
                    objReport.LoadLayout(vStream);
                    vRet = new MemoryStream();
                    objReport.ExportToPdf(vRet);
                    // ----> Marco descarga
                    ShamanClases.VentasC.CliDocAutomatizacion objMarcas = new ShamanClases.VentasC.CliDocAutomatizacion();
                    objMarcas.SetAutomatizacion(pDocId, hCliDocAutomatizacion.hDownload, 1, pUsrId, HttpContext.Current.Request.UserHostAddress, "JOB");
                }

                objComprobante = null;
                objConexion.Cerrar(objConexion.PID);
            }

            objConexion = null;
            if (!(vRet == null))
            {
                return vRet.ToArray();
            }
            else
            {
                return null;
            }

        }

        [WebMethod()]
        public byte[] GetReportsIncidente_Cache(decimal pInc, decimal pUsrId)
        {
            ShamanClases.PanelC.Conexion objConexion = new ShamanClases.PanelC.Conexion();
            MemoryStream vRet = null;
            NameValueCollection appSettings = WebConfigurationManager.AppSettings;

            if (objConexion.Iniciar(appSettings.Get("cacheServer"), int.Parse(appSettings.Get("cachePort")), appSettings.Get("cacheNameSpace"), appSettings.Get("cacheShamanAplicacion"), appSettings.Get("cacheShamanUser"), int.Parse(appSettings.Get("cacheShamanCentro")), true))
            {
                // ----> Obtengo documento
                ShamanClases.VentasC.IncOrdenesControl objOrdenes = new ShamanClases.VentasC.IncOrdenesControl();
                DataTable dtOrds = objOrdenes.GetOrdenesIncidente(pInc);
                objOrdenes = null;
                objConexion.Cerrar(objConexion.PID);
            }

            objConexion = null;
            if (!(vRet == null))
            {
                return vRet.ToArray();
            }
            else
            {
                return null;
            }

        }

        [WebMethod()]
        public DataTable GetCuentaCorriene(decimal pUsr)
        {
            ShamanClases.PanelC.Conexion objConexion = new ShamanClases.PanelC.Conexion();
            // ---> Conecto a Cache
            NameValueCollection appSettings = WebConfigurationManager.AppSettings;

            if (objConexion.Iniciar(appSettings.Get("cacheServer"), int.Parse(appSettings.Get("cachePort")), appSettings.Get("cacheNameSpace"), appSettings.Get("cacheShamanAplicacion"), appSettings.Get("cacheShamanUser"), int.Parse(appSettings.Get("cacheShamanCentro")), true))
            {
                // ---> Conecto a Tango
                ShamanClases.VentasC.Tango objTango = new ShamanClases.VentasC.Tango();
                SqlConnection cnn = objTango.UpTangoEngine(Convert.ToDecimal(appSettings.Get("tangoEmpresaId")));//double.Parse
                if (!(cnn == null))
                {
                    // -----> Obengo lista de proveedores
                    ShamanClases.VentasC.ProveedoresDocumentos objProveedores = new ShamanClases.VentasC.ProveedoresDocumentos();
                    DataTable dtPrv = objProveedores.GetProveedoresByUsuarioExtranet(pUsr);
                    if ((dtPrv.Rows.Count > 0))
                    {
                        DataTable dtOps = objTango.GetOPs(cnn, dtPrv);
                        decimal vPreId = objProveedores.GetPrestadorIdByUsuarioExtranet(pUsr);
                        string vMedId = objProveedores.GetMedicoIdByUsuarioExtranet(pUsr);
                        int vIdx;
                        dtOps.Columns["Referencias"].ReadOnly = false;
                        dtOps.Columns["TipoComprobante"].ReadOnly = false;
                        dtOps.Columns["NroComprobante"].ReadOnly = false;
                        for (vIdx = 0; (vIdx
                                    <= (dtOps.Rows.Count - 1)); vIdx++)
                        {
                            dtOps.Rows[vIdx]["Referencias"] = objProveedores.GetReferenciasOP(vPreId, vMedId, dtOps.Rows[vIdx]["NroOrdenPago"].ToString());
                            if (DBNull.Value == dtOps.Rows[vIdx]["TipoComprobante"])
                            {
                                dtOps.Rows[vIdx]["TipoComprobante"] = "";
                            }

                            if (DBNull.Value == dtOps.Rows[vIdx]["NroComprobante"])
                            {
                                dtOps.Rows[vIdx]["NroComprobante"] = "";
                            }

                            if (((dtOps.Rows[vIdx]["TipoComprobante"].ToString() == "")
                                        && (dtOps.Rows[vIdx]["NroComprobante"].ToString() == "")))
                            {
                                DataTable dtDocs = objProveedores.GetComprobantesOrdenesPago(dtOps.Rows[vIdx]["NroOrdenPago"].ToString(), dtOps.Rows[vIdx]["TipoComprobante"].ToString(), dtOps.Rows[vIdx]["NroComprobante"].ToString());
                                int vDocIdx;
                                for (vDocIdx = 0; (vDocIdx
                                            <= (dtDocs.Rows.Count - 1)); vDocIdx++)
                                {
                                    if ((vDocIdx == 0))
                                    {
                                        dtOps.Rows[vIdx]["TipoComprobante"] = dtDocs.Rows[vDocIdx]["TipoComprobante"];
                                        dtOps.Rows[vIdx]["NroComprobante"] = dtDocs.Rows[vDocIdx]["NroComprobante"];
                                    }
                                    else
                                    {

                                        DataRow dtRow = dtOps.NewRow();
                                        int vCol;
                                        for (vCol = 0; (vCol
                                                    <= (dtOps.Columns.Count - 1)); vCol++)
                                        {
                                            dtRow[vCol] = dtOps.Rows[vIdx][vCol];
                                        }

                                        dtRow["TipoComprobante"] = dtDocs.Rows[vDocIdx]["TipoComprobante"];
                                        dtRow["NroComprobante"] = dtDocs.Rows[vDocIdx]["NroComprobante"];
                                        dtOps.Rows.Add(dtRow);
                                    }

                                }

                            }

                        }

                        cnn.Close();
                        objConexion.Cerrar(objConexion.PID, true);
                        dtOps.TableName = "OrdenesPago";
                        DataView dtView = new DataView(dtOps);
                        dtView.Sort = "NroOrdenPago DESC, TipoComprobante, NroComprobante";
                        return dtView.ToTable();
                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }

        }

        [WebMethod()]
        public byte[] GetCertificadoRetencion_Tango(string pNroOp, Certificado pRetId)
        {

            try
            {
                ShamanClases.PanelC.Conexion objConexion = new ShamanClases.PanelC.Conexion();
                // ---> Conecto a Cache

                NameValueCollection appSettings = WebConfigurationManager.AppSettings;

                if (objConexion.Iniciar(appSettings.Get("cacheServer"), int.Parse(appSettings.Get("cachePort")), appSettings.Get("cacheNameSpace"), appSettings.Get("cacheShamanAplicacion"), appSettings.Get("cacheShamanUser"), int.Parse(appSettings.Get("cacheShamanCentro")), true))
                {
                    ShamanClases.VentasC.Tango objTango = new ShamanClases.VentasC.Tango();
                    MemoryStream vRet = null;
                    SqlConnection cnn = objTango.UpTangoEngine(Convert.ToDecimal(appSettings.Get("tangoEmpresaId")), false);
                    if (!(cnn == null))
                    {
                        DataTable dt;
                        switch (pRetId)
                        {
                            case Certificado.crtArba:
                                dt = objTango.GetCertificadoRetencion(cnn, pNroOp, ShamanClases.VentasC.Tango.Certificado.crtArba);
                                using (repCertificadoIIBB objReport = new repCertificadoIIBB())
                                {
                                    objReport.DataSource = new DataView(dt);
                                    BindSection(objReport.grpCertificado, objReport.DataSource);
                                    objReport.Impuesto.Text = "Comprobante de Retención IIBB Pcia. Bs.As.";
                                    vRet = new MemoryStream();
                                    objReport.ExportToPdf(vRet);
                                }
                                break;
                            case Certificado.crtAgip:
                                dt = objTango.GetCertificadoRetencion(cnn, pNroOp, ShamanClases.VentasC.Tango.Certificado.crtAgip);
                                using (repCertificadoIIBB objReport = new repCertificadoIIBB())
                                {
                                    objReport.DataSource = new DataView(dt);
                                    this.BindSection(objReport.grpCertificado, objReport.DataSource);
                                    objReport.Impuesto.Text = "Comprobante de Retención IIBB Ciudad de Bs. As.";
                                    vRet = new MemoryStream();
                                    objReport.ExportToPdf(vRet);
                                }
                                break;
                            case Certificado.crtGanancias:
                                dt = objTango.GetCertificadoRetencion(cnn, pNroOp, ShamanClases.VentasC.Tango.Certificado.crtGanancias);
                                using (repCertificadoGanancias objReport = new repCertificadoGanancias())
                                {
                                    objReport.DataSource = new DataView(dt);
                                    BindSection(objReport.grpCertificado, objReport.DataSource);
                                    objReport.Impuesto.Text = "Comprobante de Retención de Ganancias";
                                    vRet = new MemoryStream();
                                    objReport.ExportToPdf(vRet);
                                }
                                break;
                            case Certificado.crtIVA:
                                dt = objTango.GetCertificadoRetencion(cnn, pNroOp, ShamanClases.VentasC.Tango.Certificado.crtIVA);
                                using (repCertificadoGanancias objReport = new repCertificadoGanancias())
                                {
                                    objReport.DataSource = new DataView(dt);
                                    BindSection(objReport.grpCertificado, objReport.DataSource);
                                    objReport.Impuesto.Text = "Comprobante de Retención de IVA";
                                    vRet = new MemoryStream();
                                    objReport.ExportToPdf(vRet);
                                }
                                break;

                            case Certificado.crtCajaPrevisional:
                                dt = objTango.GetCertificadoCajaPrevisional(cnn, pNroOp);
                                using (repCertificadoCaja objReport = new repCertificadoCaja())
                                {
                                    objReport.DataSource = new DataView(dt);
                                    BindSection(objReport.grpCertificado, objReport.DataSource);
                                    vRet = new MemoryStream();
                                    objReport.ExportToPdf(vRet);
                                }

                                break;
                        }
                        cnn.Close();
                        objConexion.Cerrar(objConexion.PID, true);
                        if (!(vRet == null))
                        {
                            return vRet.ToArray();
                        }
                        else
                        {
                            return System.Text.Encoding.Unicode.GetBytes("No pudo armarse el PDF");
                        }

                    }
                    else
                    {
                        return System.Text.Encoding.Unicode.GetBytes("No hay conexión a Tango");
                    }

                }
                else
                {
                    return System.Text.Encoding.Unicode.GetBytes("No hay conexión a Cache");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Retorna el contrato en un Array de byte, correspondiente a un pdf.
        /// </summary>
        /// <param name="pClienteId"></param>
        /// <param name="oTipoContrato"></param>
        /// <returns></returns>
        [WebMethod()]
        public byte[] GetContratoVenta(string pClienteId)
        {
             ShamanClases.PanelC.Conexion objConexion = new ShamanClases.PanelC.Conexion();

            try
            {
                NameValueCollection appSettings = WebConfigurationManager.AppSettings;

                if (objConexion.Iniciar(appSettings.Get("cacheServer"), int.Parse(appSettings.Get("cachePort")), appSettings.Get("cacheNameSpace"), appSettings.Get("cacheShamanAplicacion"), appSettings.Get("cacheShamanUser"), int.Parse(appSettings.Get("cacheShamanCentro")), true))
                {
                    MemoryStream vRet = null;
                    VentasC.ContratosClientes objContratosClientes = new VentasC.ContratosClientes();
                    DataView vDataView = new DataView();
                    MemoryStream vStream = new MemoryStream();

                    DataTable dt = objContratosClientes.GetContratoVenta(pClienteId);

                    switch (dt.Rows[0]["da_FormaPago"].ToString())
                    {
                        case "TARJETA DE CREDITO":
                            using (repContratoVenta_tc objReport = new repContratoVenta_tc())
                            {
                                objReport.DataSource = new DataView(dt);
                                BindSection(objReport.grpContrato, objReport.DataSource, PersonalizarCamposTarjeta(objReport, dt));
                                vRet = new MemoryStream();
                                objReport.ExportToPdf(vRet);
                            }
                            break;
                        case "DEBITO AUTOMATICO":
                            using (repContratoVenta_da objReport = new repContratoVenta_da())
                            {
                                objReport.DataSource = new DataView(dt);
                                BindSection(objReport.grpContrato, objReport.DataSource, PersonalizarCamposDebitoAutomatico(objReport, dt));
                                vRet = new MemoryStream();
                                objReport.ExportToPdf(vRet);
                            }
                            break;
                        case "TRANSFERENCIA":
                            using (repContratoVenta_tb objReport = new repContratoVenta_tb())
                            {
                                objReport.DataSource = new DataView(dt);
                                BindSection(objReport.grpContrato, objReport.DataSource, PersonalizarCamposTransferencia(objReport, dt));
                                vRet = new MemoryStream();
                                objReport.ExportToPdf(vRet);
                            }
                            break;
                        default:
                            using (repContratoVenta objReport = new repContratoVenta())
                            {
                                objReport.DataSource = new DataView(dt);
                                BindSection(objReport.grpContrato, objReport.DataSource, PersonalizarCamposDebitoEfectivo(objReport, dt));
                                vRet = new MemoryStream();
                                objReport.ExportToPdf(vRet);
                            }
                            break;
                            //case ContratoVenta.cvTransferenciaBancaria:
                            //    //using (repCertificadoGanancias objReport = new repCertificadoGanancias())
                            //    //{
                            //    //    objReport.DataSource = new DataView(dt);
                            //    //    BindSection(objReport.grpCertificado, objReport.DataSource);
                            //    //    objReport.Impuesto.Text = "Comprobante de Retención de Ganancias";
                            //    //    vRet = new MemoryStream();
                            //    //    objReport.ExportToPdf(vRet);
                            //    //}
                            //    break;
                            //case ContratoVenta.cvPagoMisCuentas:
                            //    //using (repCertificadoGanancias objReport = new repCertificadoGanancias())
                            //    //{
                            //    //    objReport.DataSource = new DataView(dt);
                            //    //    BindSection(objReport.grpCertificado, objReport.DataSource);
                            //    //    objReport.Impuesto.Text = "Comprobante de Retención de IVA";
                            //    //    vRet = new MemoryStream();
                            //    //    objReport.ExportToPdf(vRet);
                            //    //}
                            //    break;
                    }

                    objConexion.Cerrar(objConexion.PID, true);

                    return vRet == null? System.Text.Encoding.Unicode.GetBytes("No pudo armarse el PDF"): vRet.ToArray();
                }
                else
                {
                    return System.Text.Encoding.Unicode.GetBytes("No hay conexión a Cache");
                }
            }
            catch (Exception ex)
            {
                objConexion.Cerrar(objConexion.PID, true);
                throw ex;
            }
        }

        private List<string> PersonalizarCamposTarjeta(repContratoVenta_tc objReport, DataTable dt)
        {
            CamposPersonalidosBasico cpb = new CamposPersonalidosBasico(dt);
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

        private List<string> PersonalizarCamposDebitoAutomatico(repContratoVenta_da objReport, DataTable dt)
        {
            CamposPersonalidosBasico cpb = new CamposPersonalidosBasico(dt);
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

        private List<string> PersonalizarCamposDebitoEfectivo(repContratoVenta objReport, DataTable dt)
        {
            CamposPersonalidosBasico cpb = new CamposPersonalidosBasico(dt);
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

        private List<string> PersonalizarCamposTransferencia(repContratoVenta_tb objReport, DataTable dt)
        {
            CamposPersonalidosBasico cpb = new CamposPersonalidosBasico(dt);
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

        //private List<string> PersonalizarCampos(repContratoVenta_tc objReport, DataTable dt)
        //{
        //    objReport.sdb_FechaIngreso.Text = modFechas.NtoD(Convert.ToInt64(dt.Rows[0]["sdb_FechaIngreso"])).ToString().Substring(0, 10);
        //    objReport.aclaracion.Text = dt.Rows[0]["cc_NombreCompleto"].ToString();
        //    objReport.aclaracion2.Text = dt.Rows[0]["cc_NombreCompleto"].ToString();
        //    //Entre Calles.
        //    string calle1 = dt.Rows[0]["ap_EntreCalle1"].ToString() == "\0" ? "" : dt.Rows[0]["ap_EntreCalle1"].ToString();
        //    string calle2 = dt.Rows[0]["ap_EntreCalle2"].ToString() == "\0" ? "" : dt.Rows[0]["ap_EntreCalle2"].ToString();
        //    objReport.ap_EntreCalles.Text = string.IsNullOrEmpty(calle2) ? calle1 : calle1 + " Y " + calle2;
        //    //tc_Vencimiento
        //    long periodo = Convert.ToInt64(dt.Rows[0]["tc_Vencimiento"]);
        //    objReport.tc_Vencimiento.Text = periodo > 12010 ? modFechas.GetPeriodo(periodo) : "";
        //    //Importe en letras.
        //    NumerosLetra numLetras = new NumerosLetra();
        //    objReport.da_ImporteMensualDescripcion_c.Text = "( PESOS " + numLetras.enletras(dt.Rows[0]["da_ImporteMensual"].ToString()) + ")";
        //    string firma = dt.Rows[0]["Firma"].ToString();
        //    if(!string.IsNullOrEmpty(firma) && firma != "\0")
        //    {
        //        objReport.da_Firma.Image = ByteArrayToImage(Convert.FromBase64String(firma.Replace("data:image/png;base64,", "")));
        //        objReport.da_Firma.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
        //        objReport.da_Firma2.Image = objReport.da_Firma.Image;
        //        objReport.da_Firma2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
        //    }
        //    return new List<string> { "sdb_fechaingreso", "aclaracion", "aclaracion2", "ap_entrecalles", "tc_vencimiento", "da_importemensualdescripcion_c" };
        //}

        //private void xrPictureBox1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    XRPictureBox xrBox = sender as XRPictureBox;
        //    string base64String = this.GetCurrentColumnValue("SomeFieldName") as string;
        //    Image img = ByteArrayToImage(Convert.FromBase64String(base64String));
        //    xrBox.Image = img;
        //}

        private void BindSection(Band pBand, object pSou, List<string> pCamposPersonalizados = null)
        {
            //XRControl xr;
            if (pCamposPersonalizados == null)
            {
                foreach (XRControl xr in pBand.Controls)
                    if ((xr.GetType().Name == "XRLabel"))
                        if (xr.Name.Substring(0, 2).ToLower() != "xr")
                            xr.DataBindings.Add("Text", pSou, xr.Name);
            }
            else
            {
                foreach (XRControl xr in pBand.Controls)
                    if ((xr.GetType().Name == "XRLabel"))
                        if (xr.Name.Substring(0, 2).ToLower() != "xr" && !pCamposPersonalizados.Contains(xr.Name.ToLower()))
                            xr.DataBindings.Add("Text", pSou, xr.Name);
            }
        }

    }
    public class CamposPersonalidosBasico
    {
        public string FechaIngreso { get; set; }
        public string Aclaracion { get; set; }
        public string EntreCalles { get; set; }
        public string ImporteMensualDescripcion { get; set; }
        public Image Firma { get; set; }
        public List<string> CamposPersonalizados { get; set; }
        public CamposPersonalidosBasico(DataTable dt)
        {
            FechaIngreso = modFechas.NtoD(Convert.ToInt64(dt.Rows[0]["sdb_FechaIngreso"])).ToString().Substring(0, 10);
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