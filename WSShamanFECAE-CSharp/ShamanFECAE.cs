using System.Data;
using System.Data.SqlClient;
using ShamanClases;
using DevExpress.XtraReports.UI;
using System.Web.Configuration;
using System;
using WSShamanFECAE_CSharp.report;
using static ShamanClases.modDeclares;
using System.Web;

using System.Collections.Specialized;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using WSShamanFECAE_CSharp.Models;
using WSShamanFECAE_CSharp.Enums;

namespace WSShamanFECAE_CSharp
{
    public class ShamanFECAE
    {
        private NameValueCollection appSettings = WebConfigurationManager.AppSettings;

        public byte[] GetPDF_Cache(decimal pDocId, decimal pUsrId)
        {
            ShamanClases.PanelC.Conexion objConexion = new ShamanClases.PanelC.Conexion();
            MemoryStream vRet = null;

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

        public byte[] GetReportsIncidente_Cache(decimal pInc, decimal pUsrId)
        {
            ShamanClases.PanelC.Conexion objConexion = new ShamanClases.PanelC.Conexion();
            MemoryStream vRet = null;

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

        public DataTable GetCuentaCorriene(decimal pUsr)
        {
            ShamanClases.PanelC.Conexion objConexion = new ShamanClases.PanelC.Conexion();
            // ---> Conecto a Cache

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
                    if (dtPrv.Rows.Count > 0)
                    {
                        DataTable dtOps = objTango.GetOPs(cnn, dtPrv);
                        decimal vPreId = objProveedores.GetPrestadorIdByUsuarioExtranet(pUsr);
                        string vMedId = objProveedores.GetMedicoIdByUsuarioExtranet(pUsr);
                        int vIdx;
                        dtOps.Columns["Referencias"].ReadOnly = false;
                        dtOps.Columns["TipoComprobante"].ReadOnly = false;
                        dtOps.Columns["NroComprobante"].ReadOnly = false;
                        for (vIdx = 0; vIdx <= (dtOps.Rows.Count - 1); vIdx++)
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

                            if ((dtOps.Rows[vIdx]["TipoComprobante"].ToString() == "")
                                        && (dtOps.Rows[vIdx]["NroComprobante"].ToString() == ""))
                            {
                                DataTable dtDocs = objProveedores.GetComprobantesOrdenesPago(dtOps.Rows[vIdx]["NroOrdenPago"].ToString(), dtOps.Rows[vIdx]["TipoComprobante"].ToString(), dtOps.Rows[vIdx]["NroComprobante"].ToString());
                                int vDocIdx;
                                for (vDocIdx = 0; vDocIdx <= (dtDocs.Rows.Count - 1); vDocIdx++)
                                {
                                    if (vDocIdx == 0)
                                    {
                                        dtOps.Rows[vIdx]["TipoComprobante"] = dtDocs.Rows[vDocIdx]["TipoComprobante"];
                                        dtOps.Rows[vIdx]["NroComprobante"] = dtDocs.Rows[vDocIdx]["NroComprobante"];
                                    }
                                    else
                                    {
                                        DataRow dtRow = dtOps.NewRow();
                                        int vCol;
                                        for (vCol = 0; vCol <= (dtOps.Columns.Count - 1); vCol++)
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
                        return null;
                }
                else
                    return null;
            }
            else
                return null;

        }

        public byte[] GetCertificadoRetencion_Tango(string pNroOp, Certificado pRetId)
        {

            try
            {

                ShamanClases.PanelC.Conexion objConexion = new ShamanClases.PanelC.Conexion();
                // ---> Conecto a Cache

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
                                    BindSection(objReport.lblCajaSub, objReport.DataSource);
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

        public byte[] GetContratoVenta(string pClienteId)
        {
            ShamanClases.PanelC.Conexion objConexion = new ShamanClases.PanelC.Conexion();

            try
            {
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

                    return vRet == null ? System.Text.Encoding.Unicode.GetBytes("No pudo armarse el PDF") : vRet.ToArray();
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
        public byte[] GetComprobante(int documentoId)
        {
            MemoryStream vRet = null;
            DataView vDataView = new DataView();
            MemoryStream vStream = new MemoryStream();

            try
            {
                ConnectionStringCache connectionString = GetConnectionString();
                VentasC.ClientesDocumentos objClientesDocumentos = new VentasC.ClientesDocumentos(connectionString);
                ReciboModel rm = objClientesDocumentos.GetDesignerRecibo<ReciboModel>(documentoId);
                if (rm != null)
                {
                    //TODO: Clase y metodo de verdad
                    DataTable dtImp1 = new DataTable();// objClientesDocumentos.GetDTDesigner(documentoId, "D");
                    DataTable dtImp2 = new DataTable(); //objClientesDocumentos.GetDTDesigner(documentoId, "D");

                    using (repRecibo objReport = new repRecibo())
                    {
                        //objReport.DataSource = new DataView(dt);
                        PersonalizarCamposRecibo(objReport, rm, dtImp1, dtImp2);
                        //BindSection(objReport.grpContrato, objReport.DataSource, PersonalizarCamposRecibo(objReport, dt, dtImp1, dtImp2));
                        vRet = new MemoryStream();
                        objReport.ExportToPdf(vRet);
                    }

                    return vRet == null ? System.Text.Encoding.Unicode.GetBytes("No pudo armarse el PDF") : vRet.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return System.Text.Encoding.Unicode.GetBytes("No pudo armarse el PDF");
        }

        private static ConnectionStringCache GetConnectionString()
        {
            NameValueCollection appSettings = WebConfigurationManager.AppSettings;

            ConnectionStringCache connectionString = new ConnectionStringCache
            {
                Server = appSettings.Get("cacheServer"),
                Port = appSettings.Get("cachePort"),
                Namespace = appSettings.Get("cacheNameSpace"),
                Aplicacion = appSettings.Get("cacheShamanAplicacion"),
                User = appSettings.Get("cacheShamanUser"),
                Centro = appSettings.Get("cacheShamanCentro"),
                Password = appSettings.Get("Password"),
                UserID = appSettings.Get("UserID")
            };
            return connectionString;
        }

        private List<string> PersonalizarCamposTarjeta(repContratoVenta_tc objReport, DataTable dt)
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

        private List<string> PersonalizarCamposRecibo(repRecibo objReport, ReciboModel rm, DataTable dtImp, DataTable dtImp2)
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

        private List<string> PersonalizarCamposDebitoAutomatico(repContratoVenta_da objReport, DataTable dt)
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

        private List<string> PersonalizarCamposDebitoEfectivo(repContratoVenta objReport, DataTable dt)
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

        private List<string> PersonalizarCamposTransferencia(repContratoVenta_tb objReport, DataTable dt)
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

        public string GetPuntosEnPoligono(float pLat, float pLon, string pTip)
        {
            string GetPuntosEnPoligono = "";
            ShamanClases.PanelC.Conexion objConexion = new ShamanClases.PanelC.Conexion();
            bool vNeedClose = false;
            try
            {
                ShamanClases.CompuMapC.Zonificaciones objZonas = new ShamanClases.CompuMapC.Zonificaciones();
                if (objConexion.Iniciar(appSettings.Get("cacheServer"), int.Parse(appSettings.Get("cachePort")), appSettings.Get("cacheNameSpace"), appSettings.Get("cacheShamanAplicacion"), appSettings.Get("cacheShamanUser"), int.Parse(appSettings.Get("cacheShamanCentro")), true))
                {
                    vNeedClose = true;
                    string vDev = objZonas.GetPoligonosInPoint(pLat, pLon, pTip, true);
                    GetPuntosEnPoligono = vDev;
                }
                else
                {
                    GetPuntosEnPoligono = "Sin conexión";
                }

                if (vNeedClose)
                {
                    objConexion.Cerrar(objConexion.PID, true);
                }

            }
            catch (Exception ex)
            {
                objConexion.Cerrar(objConexion.PID, true);
                GetPuntosEnPoligono = ex.Message;
            }
            return GetPuntosEnPoligono;
        }
    }
}