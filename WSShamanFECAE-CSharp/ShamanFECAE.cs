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
using WSShamanFECAE_CSharp.Models.RecetaModels;
using DevExpress.XtraPrinting.Native;
using System.Text;
using System.Linq;
using InterSystems.Data.CacheTypes;
using System.Xml.Serialization;

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
                        //FIX Cambio en tango.
                        pNroOp = pNroOp.Length == 12 ? "0" + pNroOp : pNroOp;
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
            //DataView vDataView = new DataView();
            //MemoryStream vStream = new MemoryStream();

            try
            {
                ConnectionStringCache connectionString = GetConnectionString();
                VentasC.ClientesDocumentos objClientesDocumentos = new VentasC.ClientesDocumentos(connectionString);
                ReciboModel reciboGrales = objClientesDocumentos.GetDesignerRecibo<ReciboModel>(documentoId);
                if (reciboGrales != null)
                {
                    //TODO: Clase y metodo de verdad
                    DataTable dtImportes = objClientesDocumentos.GetImputacionesRecibo(documentoId); //  CreateDummyDataTable();
                    DataTable dtFormas = objClientesDocumentos.GetPagosYFormas(documentoId);

                    string periodo = dtImportes.Rows[0]["Periodo"].ToString();
                    string concepto = periodo.Substring(4, 2) + "/" + periodo.Substring(0, 4);

                    NumerosLetra nl = new NumerosLetra();
                    string importeDescripcion = nl.enletras(reciboGrales.Importe);

                    using (repRecibo objReport = new repRecibo())
                    {
                        //objReport.DataSource = new DataView(dt);
                        PersonalizarCamposRecibo(objReport, reciboGrales, dtImportes, dtFormas, importeDescripcion, concepto);
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
        public byte[] GetRecetaPdf(decimal pRecetaId, decimal pUsrId)
        {
            MemoryStream vRet = null;

            try
            {
                ConnectionStringCache connectionString = GetConnectionString();

                if (!(pRecetaId > 0))
                    return Encoding.Unicode.GetBytes("No pudo armarse el PDF");
                RecetaModel receta = new EmergencyC.Recetas(connectionString).GetRecetaByID<RecetaModel>(pRecetaId);
                receta.Medicamentos = new EmergencyC.RecetasMedicamentos(connectionString).GetByRecetaId<MedicamentosModel>(pRecetaId);
                string firma = new EmergencyC.Recetas(connectionString).GetFirmaByRecetaId(pRecetaId);

                //firma = "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";
                //firma = "Qk1GcQAAAAAAADYEAAAoAAAA0AcAAFgCAAABAAgAAQAAABBtAAC5DwAAvQ8AAAABAAAAAQAAAAAAAAEBAQACAgIAAwMDAAQEBAAFBQUABgYGAAcHBwAICAgACQkJAAoKCgALCwsADAwMAA0NDQAODg4ADw8PABAQEAAREREAEhISABMTEwAUFBQAFRUVABYWFgAXFxcAGBgYABkZGQAaGhoAGxsbABwcHAAdHR0AHh4eAB8fHwAgICAAISEhACIiIgAjIyMAJCQkACUlJQAmJiYAJycnACgoKAApKSkAKioqACsrKwAsLCwALS0tAC4uLgAvLy8AMDAwADExMQAyMjIAMzMzADQ0NAA1NTUANjY2ADc3NwA4ODgAOTk5ADo6OgA7OzsAPDw8AD09PQA+Pj4APz8/AEBAQABBQUEAQkJCAENDQwBEREQARUVFAEZGRgBHR0cASEhIAElJSQBKSkoAS0tLAExMTABNTU0ATk5OAE9PTwBQUFAAUVFRAFJSUgBTU1MAVFRUAFVVVQBWVlYAV1dXAFhYWABZWVkAWlpaAFtbWwBcXFwAXV1dAF5eXgBfX18AYGBgAGFhYQBiYmIAY2NjAGRkZABlZWUAZmZmAGdnZwBoaGgAaWlpAGpqagBra2sAbGxsAG1tbQBubm4Ab29vAHBwcABxcXEAcnJyAHNzcwB0dHQAdXV1AHZ2dgB3d3cAeHh4AHl5eQB6enoAe3t7AHx8fAB9fX0Afn5+AH9/fwCAgIAAgYGBAIKCggCDg4MAhISEAIWFhQCGhoYAh4eHAIiIiACJiYkAioqKAIuLiwCMjIwAjY2NAI6OjgCPj48AkJCQAJGRkQCSkpIAk5OTAJSUlACVlZUAlpaWAJeXlwCYmJgAmZmZAJqamgCbm5sAnJycAJ2dnQCenp4An5+fAKCgoAChoaEAoqKiAKOjowCkpKQApaWlAKampgCnp6cAqKioAKmpqQCqqqoAq6urAKysrACtra0Arq6uAK+vrwCwsLAAsbGxALKysgCzs7MAtLS0ALW1tQC2trYAt7e3ALi4uAC5ubkAurq6ALu7uwC8vLwAvb29AL6+vgC/v78AwMDAAMHBwQDCwsIAw8PDAMTExADFxcUAxsbGAMfHxwDIyMgAycnJAMrKygDLy8sAzMzMAM3NzQDOzs4Az8/PANDQ0ADR0dEA0tLSANPT0wDU1NQA1dXVANbW1gDX19cA2NjYANnZ2QDa2toA29vbANzc3ADd3d0A3t7eAN/f3wDg4OAA4eHhAOLi4gDj4+MA5OTkAOXl5QDm5uYA5+fnAOjo6ADp6ekA6urqAOvr6wDs7OwA7e3tAO7u7gDv7+8A8PDwAPHx8QDy8vIA8/PzAPT09AD19fUA9vb2APf39wD4+PgA+fn5APr6+gD7+/sA/Pz8AP39/QD+/v4A////AP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP//////////////////1/8AAP////8F/wEA/////////////9H/AAD/////BP8BAAH/AQD/////////////0P8AAP////8E/wEAAf8BAP/////////////Q/wAA/////wT/AQAB/wEA/////////////9D/AAD/////A/8BAAP/AQD/////////////z/8AAP////8D/wEAA/8BAP/////////////P/wAA/////wP/AQAD/wEA/////////////8//AAD/////A/8BAAP/AQD/////////////z/8AAP////8D/wEABP8BAP/////////////O/wAA/////wP/AQAE/wEA/////////////87/AAD/////A/8BAAT/AQD/////////////zv8AAP////8D/wEABf8BAP/////////////N/wAA/////wP/AQAF/wEA/////////////83/AAD/////A/8BAAX/AQD/////////////zf8AAP////8D/wEABv8BAP/////////////M/wAA/////wP/AQAG/wEA/////////////8z/AAD/////A/8BAAb/AQD/////////////zP8AAP////8D/wEAB/8BAP/////////////L/wAA/////wL/AQAI/wEA/////////////8v/AAD/////Av8BAAj/AQD/////////////y/8AAP////8C/wEACP8BAP/////////////L/wAA/////wL/AQAJ/wEA/////////////8r/AAD/////Av8BAAn/AQD/////////////yv8AAP////8C/wEACf8BAP/////////////K/wAA+P8BAP//CP8BAAn/AQD/////////////yv8AAPf/AgD//wj/AQAK/wEA/////////////8n/AAD3/wEAAf8BAP//B/8BAAr/AQD/////////////yf8AAPf/AQAB/wEA//8H/wEACv8BAP/////////////J/wAA9/8BAAH/AQD//wf/AQAK/wEA/////////////8n/AAD2/wEAA/8BAP//Bv8BAAv/AQD/////////////yP8AAPb/AQAD/wEA//8G/wEAC/8BAP/////////////I/wAA9v8BAAP/AQD//wb/AQAL/wEA/////////////8j/AAD2/wEAA/8BAP//Bv8BAAv/AQD/////////////yP8AAPb/AQAD/wEA//8G/wEADP8BAP/////////////H/wAA9v8BAAT/AQD//wX/AQAM/wEA/////////////8f/AAD2/wEABP8BAP//Bf8BAAz/AQD/////////////x/8AAPb/AQAE/wEA//8F/wEADP8BAP/////////////H/wAA9v8BAAT/AQD//wX/AQAN/wEA/////////////8b/AAD2/wEABf8BAP//BP8BAA3/AQD/////////////xv8AAPb/AQAF/wEA//8E/wEADf8BAP/////////////G/wAA9v8BAAX/AQD//wT/AQAN/wEA/////////////8b/AAD2/wEABf8BAP//BP8BAA7/AQD/////////////xf8AAPb/AQAF/wEA//8E/wEADv8BAP/////////////F/wAA9v8BAAX/AQD//wT/AQAO/wEA/////////////8X/AAD2/wEABv8BAP//A/8BAA7/AQD/////////////xf8AAPb/AQAG/wEA//8D/wEAD/8BAP/////////////E/wAA9v8BAAb/AQD//wP/AQAP/wEA/////////////8T/AAD2/wEABv8BAP//BP8BAA7/AQD/////////////xP8AAPX/AQAH/wEA//8E/wEADv8BAP/////////////E/wAA9f8BAAf/AQD//wT/AQAP/wEA/////////////8P/AAD1/wEACP8BAP//A/8BAA//AQD/////////////w/8AAPX/AQAI/wEA//8D/wEAD/8BAP/////////////D/wAA9f8BAAj/AQD//wP/AQAP/wEA/////////////8P/AAD1/wEACP8BAP//A/8BAA//AQD/////////////w/8AAPX/AQAI/wEA//8D/wEAEP8BAP/////////////C/wAA9f8BAAj/AQD//wP/AQAQ/wEA/////////////8L/AAD1/wEACP8BAP//A/8BABD/AQD/////////////wv8AAPX/AQAJ/wEAS/8BALX/AQAQ/wEA/////////////8L/AAD1/wEACf8BAEv/AgC0/wEAEP8BAP/////////////C/wAA9f8BAAn/AQBL/wIAtP8BABH/AQD/////////////wf8AAPX/AQAJ/wEAS/8CALT/AQAR/wEA/////////////8H/AAD1/wEACf8BAEr/AQAB/wEAtP8BABH/AQD/////////////wf8AAPX/AQAJ/wEASv8BAAH/AQC1/wEAEP8BAP/////////////B/wAA9f8BAAn/AQBK/wEAAf8BALX/AQAQ/wEA/////////////8H/AAD1/wEACf8BAEr/AQAB/wEAtf8BABD/AQD/////////////wf8AAPX/AQAK/wEASf8BAAH/AQC1/wEAEf8BAO3/AwD//////////8//AAD1/wEACv8BAEn/AQAB/wEAtf8BABH/AQDs/wEAA/8EAP//////////y/8AAPX/AQAK/wEASf8BAAH/AQC1/wEAEf8BAOv/AQAI/wIA///////////J/wAA9f8BAAr/AQBJ/wEAAf8BALX/AQAR/wEA6v8BAAv/AgD//////////8f/AAD1/wEACv8BAEn/AQAB/wEAtf8BABH/AQDq/wEADf8CAP//////////xf8AAPX/AQAK/wEASf8BAAH/AQC1/wEAEv8BAOn/AQAP/wIA///////////D/wAA9f8BAAr/AQBJ/wEAAf8BALX/AQAS/wEA6P8BABL/AQD//////////8L/AAD1/wEACv8BAEn/AQAC/wEAtP8BABL/AQDo/wEAE/8CAP//////////wP8AAPX/AQAK/wEASf8BAAL/AQC0/wEAEv8BAOj/AQAV/wEA//////////+//wAA9f8BAAr/AQBJ/wEAAv8BALX/AQAR/wEA6P8BABb/AQD//////////77/AAD1/wEACv8BAEn/AQAC/wEAtf8BABL/AQDn/wEAF/8CAP//////////vP8AAPX/AQAK/wEASf8BAAL/AQC1/wEAEv8BAOf/AQAZ/wEA//////////+7/wAA9f8BAAv/AQBH/wEAA/8BALX/AQAS/wEA6P8BABn/AQD//////////7r/AAD1/wEAC/8BAEf/AQAD/wEAtf8BABL/AQDo/wEAGv8BAP//////////uf8AAPX/AQAL/wEAR/8BAAP/AQC1/wEAEv8BAOj/AQAb/wEA//////////+4/wAA9f8BAAv/AQBH/wEAA/8BALX/AQAS/wEA6P8BABz/AQD//////////7f/AAD1/wEAC/8BAEf/AQAD/wEAtf8BABP/AQDo/wEAHP8BAP//////////tv8AAPX/AQAL/wEAR/8BAAP/AQC1/wEAE/8BAOj/AQAd/wEA//////////+1/wAA9f8BAAv/AQBH/wEAA/8BALX/AQAT/wEA6P8BAB7/AgD//////////7P/AAD1/wEAC/8BAEf/AQAD/wEAtf8BABP/AQDo/wEAIP8BAP//////////sv8AAPX/AQAL/wEAR/8BAAP/AQC2/wEAEv8BAOn/AQAg/wEA//////////+x/wAA9P8BAAz/AQBH/wEAA/8BALb/AQAS/wEA6f8BACH/AQD//////////7D/AAD0/wEADP8BAEf/AQAD/wEAtv8BABP/AQDo/wEAIv8BAP//////////r/8AAPT/AQAM/wEARv8BAAT/AQC2/wEAE/8BAOn/AQAi/wEA//////////+u/wAA9P8BAAz/AQBG/wEABP8BALb/AQAT/wEA6f8BACP/AQD//////////63/AAD0/wEADP8BAEb/AQAE/wEAtv8BABP/AQDq/wEAI/8BAP//////////rP8AAPT/AQAM/wEARv8BAAT/AQC2/wEAE/8BAOr/AQAk/wEA//////////+r/wAA9P8BAAz/AQBG/wEABP8BALb/AQAT/wEA6v8BACT/AQD//////////6v/AAD0/wEADP8BAEb/AQAE/wEAtv8BABP/AQDr/wEAJP8BAP//////////qv8AAPT/AQAM/wEARv8BAAT/AQC2/wEAFP8BAOr/AQAl/wEA//////////+p/wAA9P8BAA3/AQBF/wEAA/8BALj/AQAT/wEA6v8BACb/AQD//////////6j/AAD0/wEADf8BAEX/AQAD/wEAuP8BABP/AQDr/wEAJv8BAP//////////p/8AAPT/AQAN/wEARP8BAAT/AQC4/wEAE/8BAOv/AQAn/wEA//////////+m/wAA9P8BAA3/AQBE/wEABP8BALj/AQAT/wEA7P8BACf/AQD//////////6X/AAD0/wEADf8BAET/AQAE/wEAuP8BABP/AQDs/wEAJ/8BAP//////////pf8AAPT/AQAN/wEARP8BAAT/AQC4/wEAFP8BAOz/AQAn/wEA//////////+k/wAA9P8BAA3/AQBE/wEABP8BALj/AQAU/wEA7P8BACj/AQD//////////6P/AAD0/wEADf8BAET/AQAE/wEAuP8BABT/AQDt/wEAKP8BAP//////////ov8AAPT/AQAN/wEARP8BAAT/AQC4/wEAFP8BAO3/AQAp/wEA//////////+h/wAA9P8BAA3/AQBD/wEABf8BALj/AQAU/wEA7v8BACn/AQD//////////6D/AAD0/wEADf8BAEP/AQAF/wEAuf8BABP/AQDu/wEAKf8BAP//////////oP8AAPT/AQAN/wEAQ/8BAAX/AQC5/wEAFP8BAO7/AQAp/wEA//////////+f/wAA9P8BAA3/AQBD/wEABf8BALn/AQAU/wEA7v8BACr/AQD//////////57/AAD0/wEADf8BAEP/AQAF/wEAuf8BABT/AQDv/wEAKv8BAP//////////nf8AAPT/AQAN/wEAQv8BAAb/AQC5/wEAFP8BAPD/AQAq/wEA//////////+c/wAA9P8BAA3/AQBC/wEABv8BALn/AQAU/wEA8P8BACr/AQD//////////5z/AAD0/wEADf8BAEL/AQAF/wEAuv8BABT/AQDx/wEAKv8BAP//////////m/8AAPT/AQAN/wEAQv8BAAX/AQC6/wEAFP8BAPL/AQAq/wEA//////////+a/wAA9P8BAA3/AQBC/wEABf8BALr/AQAV/wEA8v8BACr/AQD//////////5n/AAD1/wEADP8BAEL/AQAF/wEAu/8BABT/AQDy/wEAK/8BAP//////////mP8AAPX/AQAN/wEAQP8BAAb/AQC7/wEAFP8BAPP/AQAr/wEA//////////+X/wAA9f8BAA3/AQBA/wEABv8BALv/AQAU/wEA9P8BACr/AQD//////////5f/AAD1/wEADf8BAED/AQAG/wEAu/8BABT/AQD0/wEAK/8BAP//////////lv8AAPX/AQAN/wEAQP8BAAb/AQC7/wEAFP8BAPX/AQAr/wEA//////////+V/wAA9f8BAA3/AQBA/wEABv8BALv/AQAU/wEA9v8BACv/AQD//////////5T/AAD1/wEADf8BAD//AQAH/wEAu/8BABX/AQD2/wEAKv8BAP//////////lP8AAPX/AQAN/wEAP/8BAAf/AQC8/wEAFP8BAPb/AQAr/wEA//////////+T/wAA9f8BAA3/AQA//wEAB/8BALz/AQAU/wEA9/8BACv/AQD//////////5L/AAD1/wEADf8BAD//AQAH/wEAvP8BABT/AQD4/wEAK/8BAP//////////kf8AAPX/AQAN/wEAP/8BAAf/AQC8/wEAFP8BAPn/AQAq/wEA//////////+R/wAA9f8BAA3/AQA+/wEACP8BALz/AQAU/wEA+v8BACr/AQD//////////5D/AAD1/wEADf8BAD7/AQAI/wEAvP8BABT/AQD7/wEAKv8BAP//////////j/8AAPX/AQAN/wEAPv8BAAj/AQC8/wEAFP8BAPz/AQAp/wEA//////////+P/wAA9f8BAA3/AQA+/wEACP8BALz/AQAV/wEA/P8BACn/AQD//////////47/AAD1/wEADf8BAD7/AQAI/wEAvf8BABT/AQD8/wEAKv8BAP//////////jf8AAPX/AQAN/wEAPv8BAAf/AQC+/wEAFP8BAP3/AQAq/wEA//////////+M/wAA9f8BAA3/AQA9/wEACP8BAL7/AQAU/wEA/v8BACn/AQD//////////4z/AAD1/wEADf8BAD3/AQAI/wEAvv8BABT/AQD//wEAKf8BAP//////////i/8AAPX/AQAN/wEAPf8BAAj/AQC+/wEAFP8BAP//Af8BACn/AQD//////////4r/AAD1/wEADf8BAD3/AQAI/wEAvv8BABT/AQD//wL/AQAp/wEA//////////+J/wAA9f8BAA3/AQA9/wEACP8BAL7/AQAV/wEA//8C/wEAKP8BAP//////////if8AAPX/AQAN/wEAPf8BAAj/AQC//wEAFP8BAP//A/8BACj/AQD//////////4j/AAD1/wEADP8BAD3/AQAJ/wEAv/8BABT/AQD//wT/AQAo/wEA//////////+H/wAA9f8BAAz/AQA9/wEACf8BAL//AQAU/wEAov8BAGH/AQAn/wEA//////////+H/wAA9f8BAAz/AQA9/wEACf8BAL//AQAU/wEAoP8CAAH/AQBh/wEAJ/8BAP//////////hv8AAPX/AQAM/wEAPf8BAAn/AQC//wEAFP8BAKD/AQAD/wEAYf8BACf/AQD//////////4X/AAD1/wEADP8BAD3/AQAJ/wEAv/8BABT/AQCf/wEABf8BAGH/AQAm/wEA//////////+F/wAA9f8BAAz/AQA9/wEACf8BAL//AQAV/wEAnv8BAAb/AQBh/wEAJv8BAP//////////hP8AAPX/AQAM/wEAPP8BAAr/AQDA/wEAFP8BAJ7/AQAG/wEAYv8BACb/AQD//////////4P/AAD1/wEADP8BADz/AQAK/wEAwP8BABT/AQCe/wEAB/8BAGL/AQAl/wEA//////////+D/wAA9f8BAAz/AQA8/wEACv8BAMD/AQAU/wEAnv8BAAj/AQBi/wEAJf8BAP//////////gv8AAPX/AQAM/wEAPP8BAAn/AQDB/wEAFP8BAJ3/AQAJ/wEAY/8BACX/AQD//////////4H/AAD1/wEADP8BADz/AQAJ/wEAwf8BABT/AQCd/wEACv8BAGP/AQAl/wEA//////////+A/wAA9f8BAAz/AQAb/woAFv8BAAr/AQDB/wEAFP8BAJ3/AQAK/wEAZP8BACT/AQD//////////4D/AAD1/wEADP8BABn/AgAK/wQAEv8BAAr/AQDB/wEAFf8BAJz/AQAL/wEAZP8BACT/AQD//////////3//AAD1/wEADP8BABf/AgAQ/wMAD/8BAAr/AQDB/wEAFf8BAJz/AQAL/wEAZf8BACT/AQD//////////37/AAD1/wEADP8BABX/AgAV/wIADf8BAAr/AQDC/wEAFP8BAJz/AQAM/wEAZf8CACL/AQD//////////37/AAD1/wEADP8BABT/AQAZ/wEADP8BAAr/AQDC/wEAFP8BAJz/AQAM/wEAZ/8BACL/AQD//////////33/AAD1/wEADP8BABP/AQAb/wIACf8BAAv/AQDC/wEAFP8BAJz/AQAN/wEAZ/8BACL/AQD//////////3z/AAD1/wEADP8BABL/AQAe/wEACP8BAAv/AQDC/wEAFP8BAJz/AQAN/wEAaP8BACH/AQD//////////3z/AAD1/wEADP8BABH/AQAg/wEAB/8BAAv/AQDC/wEAFP8BAJz/AQAO/wEAaP8BACH/AQD//////////3v/AAD1/wEADP8BABD/AQAi/wEABv8BAAv/AQDC/wEAFP8BAJz/AQAO/wEAaf8CACD/AQD//////////3r/AAD1/wEADP8BAA//AQAj/wEABv8BAAv/AQDC/wEAFf8BAJv/AQAP/wEAav8BAB//AQD//////////3r/AAD1/wEADP8BAA7/AQAl/wEABP8BAAv/AQDD/wEAFf8BAJv/AQAP/wEAa/8BAB//AQD//////////3n/AAD1/wEADP8BAA3/AQAn/wEAA/8BAAv/AQDE/wEAFP8BAJv/AQAQ/wEAa/8BAB7/AQD//////////3n/AAD1/wEADP8BAA3/AQAn/wEAA/8BAAv/AQDE/wEAFP8BAJv/AQAQ/wEAbP8CAB3/AQD//////////3j/AAD1/wEADP8BAAz/AQAp/wEAAv8BAAv/AQDE/wEAFP8BAJv/AQAQ/wEAbv8BABz/AQD//////////3j/AAD1/wEADP8BAAv/AQAr/wEAAf8BAAv/AQDE/wEAFP8BAJv/AQAR/wEAbv8BABz/AQD//////////3f/AAD1/wEADP8BAAr/AQAs/wEAAf8BAAv/AQDE/wEAFP8BAJv/AQAR/wEAb/8CABv/AQD//////////3b/AAD1/wEADP8BAAr/AQAt/wEADP8BAMT/AQAU/wEAm/8BABL/AQBw/wEAGv8BAP//////////dv8AAPX/AQAM/wEACf8BAC7/AQAM/wEAxP8BABX/AQAB/wUAlP8BABL/AQBx/wIAGf8BAP//////////df8AAPX/AQAM/wEACf8BAC7/AgAL/wEAxP8BABX/AgAF/wIAkv8BABL/AQBz/wEAGP8BAP//////////df8AAPX/AQAM/wEACP8BAC//AgAL/wEAxf8BABL/AwAI/wIAkP8BABP/AQBz/wIAF/8BAP//////////dP8AAPX/AQAM/wEACP8BAC//AQAB/wEACv8BAMX/AQAR/wEAAv8BAAr/AQCP/wEAE/8BAHX/AQAX/wEA//////////9z/wAA9f8BAAz/AQAH/wEAL/8BAAP/AQAI/wEAxv8BABH/AQAC/wEAC/8BAI7/AQAU/wEAdf8BABb/AQD//////////3P/AAD1/wEADP8BAAf/AQAv/wEAA/8BAAj/AQDG/wEAEP8BAAP/AQAL/wEAjv8BABT/AQB2/wIAFf8BAP//////////cv8AAPX/AQAM/wEABv8BADD/AQAE/wEAB/8BAMb/AQAP/wEABP8BAAz/AQCN/wEAFP8BAHj/AQAU/wEA//////////9y/wAA9f8BAAz/AQAG/wEAMP8BAAT/AQAH/wEAxv8BAA//AQAE/wEADf8BAIz/AQAV/wEAeP8CABP/AQD//////////3H/AAD1/wEADP8BAAb/AQAw/wEABf8BAAb/AQDG/wEADv8BAAb/AQAN/wEAi/8BABX/AQB6/wEAEv8BAP//////////cf8AAPX/AQAM/wEABf8BADD/AQAG/wEABv8BAMb/AQAO/wEABv8BAA3/AQCL/wEAFf8BAHv/AgAR/wEA//////////9w/wAA9v8BAAv/AQAF/wEAMP8BAAf/AQAF/wEAx/8BAAz/AQAH/wEADv8BAIr/AQAW/wEAfP8BABH/AQD//////////2//AAD2/wEAC/8BAAT/AQAx/wEAB/8BAAX/AQDH/wEADP8BAAf/AQAP/wEAif8BABb/AQB9/wIAD/8BAP//////////b/8AAPb/AQAL/wEABP8BADH/AQAI/wEABP8BAMf/AQAM/wEAB/8BAA//AQCK/wEAFv8BAH7/AgAO/wEA//////////9u/wAA9v8BAAv/AQAE/wEAMP8BAAn/AQAE/wEAx/8BAAv/AQAI/wEAEP8BAIn/AQAW/wEAgP8BAA3/AQD//////////27/AAD2/wEAC/8BAAT/AQAw/wEACf8BAAP/AQDI/wEAC/8BAAj/AQAR/wEAiP8BABb/AQCB/wIADP8BAP//////////bf8AAPb/AQAL/wEAA/8BADH/AQAK/wEAAv8BAMj/AQAL/wEACP8BABH/AQCI/wEAF/8BAIL/AgAK/wEA//////////9t/wAA9v8BAAv/AQAD/wEAMf8BAAr/AQAC/wEAyP8BAAr/AQAK/wEAEf8BAIf/AQAX/wEAhP8BAAr/AQD//////////2z/AAD2/wEAC/8BAAP/AQAw/wEADP8BAAH/AQDI/wEACv8BAAr/AQAS/wEAhv8BABf/AQCF/wIACP8BAP//////////bP8AAPb/AQAL/wEAA/8BADD/AQAM/wEAAf8BAMn/AQAJ/wEACv8BABL/AQCG/wEAGP8BAIb/AgAH/wEA//////////9r/wAA9v8BAAv/AQAD/wEAMP8BAAz/AQAB/wEAyf8BAAj/AQAL/wEAE/8BAIX/AQAY/wEAiP8CAAb/AQD//////////2r/AAD2/wEAC/8BAAL/AQAx/wEADf8CAMn/AQAI/wEAC/8BABP/AQCG/wEAGP8BAIn/AQAF/wEA//////////9q/wAA9v8BAAv/AQAC/wEAMP8BAA7/AgDJ/wEACP8BAAv/AQAU/wEAhf8BABj/AQCK/wIABP8BAP//////////af8AAPb/AQAL/wEAAv8BADD/AQAP/wEAyf8BAAf/AQAM/wEAFP8BAIX/AQAY/wEAjP8CAAL/AQD//////////2n/AAD2/wEAC/8BAAL/AQAw/wEAD/8BAMn/AQAH/wEADf8BABT/AQCE/wEAGf8BAI3/AQAC/wEA//////////9o/wAA9v8BAAv/AQAC/wEAMP8BAA7/AQAB/wEAyP8BAAf/AQAN/wEAFP8BAIT/AQAZ/wEAjv8DAP//////////aP8AAPb/AQAL/wEAAf8BADD/AQAP/wEAAf8BAMn/AQAF/wEADv8BABT/AQCE/wEAGf8BAJD/AgD//////////2f/AAD2/wEAC/8BAAH/AQAw/wEAD/8BAAH/AQDJ/wEABf8BAA7/AQAV/wEAg/8BABr/AQCQ/wMA//////////9l/wAA9v8BAAv/AQAB/wEAMP8BAA//AQAC/wEAJP8MAJj/AQAF/wEADv8BABX/AQCD/wEAGv8BAJH/AQAB/wIA//////////9j/wAA9v8BAAv/AQAB/wEAMP8BAA//AQAC/wEAH/8FAAz/BQCT/wEABf8BAA7/AQAV/wEAhP8BABn/AQCS/wEAAv8CAP//////////Yf8AAPb/AQAL/wEAAf8BAC//AQAQ/wEAA/8BABv/AwAW/wMAkP8BAAT/AQAP/wEAFv8BAIP/AQAa/wEAkf8BAAT/AgD//////////1//AAD2/wEAC/8BAAH/AQAv/wEAEP8BAAP/AQAZ/wIAHP8DAI3/AQAE/wEAEP8BABX/AQCD/wEAGv8BAJL/AQAF/wIA//////////9d/wAA9v8BAAv/AQAB/wEAL/8BABD/AQAE/wEAFv8CACH/AgCL/wEABP8BABD/AQAW/wEAgv8BABr/AQCS/wEAB/8CAP//////////W/8AAPb/AQAL/wEAAf8BAC//AQAQ/wEABP8BABT/AgAl/wIAiv8BAAP/AQAQ/wEAFv8BAIL/AQAb/wEAkv8BAAj/AgD//////////1n/AAD2/wEAC/8BAAH/AQAu/wEAEf8BAAX/AQAS/wEAKf8BAIn/AQAD/wEAEP8BABb/AQCC/wEAG/8BAJL/AQAK/wIA//////////9X/wAA9v8BAAv/AQAB/wEALv8BABH/AQAF/wEAEf8BACv/AgCH/wEAA/8BABD/AQAX/wEAHv8IAFv/AQAb/wEAKf8BAGn/AQAL/wIA//////////9V/wAA9v8BAAv/AQAB/wEALv8BABH/AQAG/wEADv8CAC7/AQCG/wEAAv8BABH/AQAX/wEAHP8CAAj/AgBa/wEAG/8BACf/AQAB/wEAaP8BAA3/AgD//////////1P/AAD2/wEAC/8CAC//AQAR/wEABv8BAA3/AQAx/wEAF/8JAGX/AQAC/wEAEv8BABf/AQAY/wMADP8CAFj/AQAb/wEAJv8BAAP/AQBo/wEADv8CAP//////////Uf8AAPb/AQAL/wIALv8BABH/AQAI/wEACv8CADP/AgAT/wIACf8CAGP/AQAC/wEAEv8BABf/AQAV/wMAEf8BAFf/AQAc/wEAJf8BAAT/AQBn/wEAEP8CAP//////////T/8AAPb/AQAL/wIALv8BABH/AQAI/wEACP8CADf/AQAR/wEADf8CAGH/AQAC/wEAEv8BABj/AQAS/wIAFf8BAFb/AQAc/wEAJf8BAAT/AQBo/wEAEf8CAP//////////Tf8AAPb/AQAL/wIALv8BABH/AQAJ/wEABf8CADr/AQAP/wEAEP8CAGD/AQAB/wEAEv8BABj/AQAQ/wIAGP8BAFX/AQAc/wEAJf8BAAX/AQBn/wEAE/8CAP//////////S/8AAPb/AQAL/wIALv8BABH/AQAK/wUAPf8CAA3/AQAS/wIAXv8BAAH/AQAS/wEAGP8BAA//AQAb/wIAIv8KACf/AQAd/wEAI/8BAAf/AQBn/wEAFP8CAP//////////Sf8AAPb/AQAL/wIALf8BABL/AQBO/wEAC/8BABX/AQBd/wEAAf8BABL/AQAZ/wEADf8BAB7/AQAe/wMACv8CACX/AQAd/wEAI/8BAAj/AQBm/wEAFv8CAP//////////R/8AAPb/AQAL/wIALf8BABL/AQBO/wEAC/8BABb/AQBc/wEAAf8BABL/AQAZ/wEADP8BACD/AQAa/wMAD/8CACT/AQAc/wEAI/8BAAj/AQBn/wEAF/8CAP//////////Rf8AAPb/AQAL/wIALf8BABL/AQBP/wEACf8BABj/AgBa/wEAAf8BABP/AQAZ/wEACv8BACH/AQAY/wIAFP8BACP/AQAd/wEAIv8BAAn/AQBm/wEAGf8DAP//////////Qv8AAPb/AQAL/wIALf8BABL/AQBP/wEACf8BABr/AQBZ/wIAFP8BABn/AQAJ/wEAI/8BABX/AgAX/wIAIf8BAB3/AQAi/wEACv8BAGb/AQAb/wIA//////////9A/wAA9/8BAAr/AgAs/wEAE/8BAFD/AQAH/wEAHP8BAFn/AQAU/wEAGv8BAAf/AQAl/wIAEf8CABv/AQAg/wEAHf8BACL/AQAL/wEAZf8BAB3/AgD//////////z7/AAD3/wEACv8CACz/AQAT/wEAUf8BAAb/AQAd/wIAV/8BABT/AQAa/wEABv8BACj/AgAN/wIAHv8BAB//AQAe/wEAIf8BAAv/AQBm/wEAHv8CAP//////////PP8AAPf/AQAK/wIALP8BABP/AQBR/wEABf8BACD/AQBW/wEAFP8BABv/AQAE/wEAK/8DAAj/AgAh/wEAHv8BAB7/AQAi/wEAC/8BAGX/AQAg/wIA//////////86/wAA9/8BAAr/AgAs/wEAE/8BAFL/AQAE/wEAIf8BAFX/AQAU/wEAG/8BAAP/AQAv/wgAJP8BAB7/AQAd/wEAIv8BAAv/AQBm/wEAIf8CAP//////////OP8AAPf/AQAK/wIAK/8BABT/AQBT/wEAA/8BACL/AgBT/wEAFP8BABv/AQAC/wEAXf8BAB3/AQAe/wEAIf8BAAz/AQBl/wEAI/8CAP//////////Nv8AAP//A/8CACv/AQAU/wEAVP8BAAL/AQAk/wEAUv8BABX/AQAb/wIAX/8BABz/AQAe/wEAIf8BAAz/AQBm/wEAJP8CAP//////////NP8AAP//A/8CACv/AQAU/wEAVP8BAAL/AQAl/wEAUf8BABX/AQAb/wEAYf8BABv/AQAe/wEAIf8BAA3/AQBl/wEAJv8CAP//////////Mv8AAP//A/8CACv/AQAU/wEAVf8BAAH/AQAm/wIAT/8CABT/AQAa/wIAYv8BABv/AQAd/wEAIf8BAA3/AQBl/wEAKP8CAP//////////MP8AAP//A/8CACr/AQAU/wEAVv8BAAH/AQAo/wEATv8CABT/AQAa/wEAAf8BAGH/AQAb/wEAHv8BACD/AQAO/wEAZf8BACn/AgD//////////y7/AAD//wP/AgAq/wEAFP8BAFf/AgAp/wIATP8CABT/AQAZ/wEAAv8BAGL/AQAa/wEAHv8BACD/AQAO/wEAZf8BACv/AgD//////////yz/AAD//wP/AQAB/wEAKf8BABT/AQCE/wEAS/8CABT/AQAY/wEAA/8BAGP/AQAZ/wEAHv8BACH/AQAO/wEAZf8BACz/AgD//////////yr/AAD//wP/AQAB/wEAKf8BABT/AQCF/wEASv8CABX/AQAW/wEABf8BAGP/AQAZ/wEAHv8BACD/AQAO/wEAZf8BAC7/AgD//////////yj/AAD//wP/AQAB/wEAKP8BABX/AQCG/wEASP8BAAH/AQAV/wEAFv8BAAX/AQBk/wEAGP8BAB7/AQAg/wEAD/8BAGX/AQAv/wIA//////////8m/wAA//8D/wEAAf8BACj/AQAV/wEAh/8BAEf/AQAB/wEAFf8BABX/AQAG/wEAZP8BABj/AQAe/wEAIP8BAA//AQBl/wEAMf8CAP//////////JP8AAP//A/8BAAH/AQAo/wEAFf8BAIj/AQBG/wEAAv8BABT/AQAU/wEACP8BAGT/AQAX/wEAH/8BAB//AQAP/wEAZv8BADL/AgD//////////yL/AAD//wP/AQAB/wEAKP8BABX/AQCJ/wEARf8BAAL/AQAU/wEAFP8BAAj/AQBl/wEAFv8BAB//AQAf/wEAEP8BAGX/AQA0/wIA//////////8g/wAA//8D/wEAAf8BACf/AQAW/wEAiv8CAEP/AQAC/wEAFP8BABP/AQAK/wEAZf8BABb/AQAe/wEAIP8BAA//AQBl/wEANv8CAP//////////Hv8AAP//A/8BAAH/AQAn/wEAFv8BAIz/AQBC/wEAAv8BABT/AQAT/wEACv8BAGX/AQAW/wEAHv8BACD/AQAQ/wEAZf8BADf/AgD//////////xz/AAD//wP/AQAB/wEAJ/8BABb/AQCN/wEAQf8BAAL/AQAV/wEAEf8BAAv/AQBm/wEAFf8BAB//AQAf/wEAEP8BAGX/AQA5/wIA//////////8a/wAA//8D/wEAAf8BACb/AQAX/wEAjv8CAD//AQAC/wEAFf8BABH/AQAM/wEAZv8BABT/AQAf/wEAH/8BABH/AQBl/wEAOv8CAP//////////GP8AAP//A/8BAAH/AQAm/wEAF/8BAJD/AQA+/wEAAv8BABX/AQAQ/wEADf8BAGb/AQAV/wEAHv8BACD/AQAQ/wEAZf8BADz/AgD//////////xb/AAD//wP/AQAB/wEAJv8BABf/AQCR/wEAPf8BAAP/AQAU/wEAEP8BAA3/AQBn/wEAFP8BAB//AQAf/wEAEf8BAGX/AQA9/wIA//////////8U/wAA//8D/wEAAf8BACb/AQAW/wEAk/8CADv/AQAD/wEAFP8BAA//AQAP/wEAZv8BABT/AQAf/wEAH/8BABH/AQBl/wEAP/8CAP//////////Ev8AAP//A/8BAAL/AQAk/wEAF/8BAJX/AQA6/wEAA/8BABT/AQAO/wEAEP8BAGf/AQAT/wEAH/8BAB//AQAS/wEAZf8BAED/AgD//////////xD/AAD//wP/AQAC/wEAJP8BABf/AQCW/wEAOf8BAAP/AQAU/wEADv8BABD/AQBo/wEAE/8BAB//AQAf/wEAEf8BAGX/AQBC/wIA//////////8O/wAA//8D/wEAAv8BACT/AQAX/wEAl/8BADj/AQAD/wEAFf8BAAz/AQAS/wEAZ/8BABP/AQAf/wEAH/8BABL/AQBl/wEAQ/8CAP//////////DP8AAP//A/8BAAL/AQAj/wEAGP8BAJj/AgA3/wEAAv8BABX/AQAM/wEAEv8BAGj/AQAS/wEAH/8BAB//AQAS/wEAZf8BAEX/AgD//////////wr/AAD//wP/AQAC/wEAI/8BABj/AQCa/wEANv8BAAL/AQAV/wEAC/8BABP/AQBp/wEAEf8BAB//AQAf/wEAEv8BAGb/AQBG/wIA//////////8I/wAA//8D/wEAAv8BACP/AQAY/wEAm/8BADX/AQAD/wEAFP8BAAv/AQAU/wEAaP8BABH/AQAg/wEAH/8BABL/AQBl/wEASP8CAP//////////Bv8AAP//A/8BAAL/AQAj/wEAGP8BAJz/AgAz/wEAA/8BABT/AQAK/wEAFf8BAGn/AQAR/wEAH/8BAB//AQAS/wEAZv8BAEn/AgD//////////wT/AAD//wP/AQAC/wEAIv8BABn/AQCe/wIAMf8BAAP/AQAU/wEACv8BABX/AQBq/wEAEP8BAB//AQAf/wEAE/8BAGX/AQBL/wEA//////////8D/wAA//8D/wEAA/8BACH/AQAZ/wEAoP8CAC//AQAD/wEAFf8BAAj/AQAX/wEAaf8BABD/AQAg/wEAH/8BABL/AQBl/wEATP8CAP//////////Af8AAP//A/8BAAP/AQAh/wEAGf8BAKL/BAAR/wQAFv8BAAP/AQAV/wEACP8BABf/AQBq/wEAD/8BACD/AQAf/wEAE/8BAGX/AQD//////////07/AAD//wP/AQAD/wEAIP8BABr/AQCm/xEABP8BABX/AQAD/wEAFf8BAAf/AQAY/wEAa/8BAA//AQAf/wEAH/8BABP/AQBl/wEA//////////9O/wAA//8D/wEAA/8BACD/AQAZ/wEAvf8BABT/AQAD/wEAFf8BAAf/AQAZ/wEAav8BAA//AQAf/wEAH/8BABP/AQBm/wEA//////////9N/wAA//8D/wEAA/8BACD/AQAZ/wEA0v8BAAT/AQAU/wEABv8BABr/AQBr/wEADv8BACD/AQAf/wEAE/8BAGX/AQD//////////03/AAD//wP/AQAD/wEAH/8BABr/AQDS/wEABP8BABT/AQAG/wEAG/8BAGr/AQAO/wEAIP8BAB//AQAT/wEAZv8BAP//////////TP8AAP//A/8BAAT/AQAe/wEAGv8BANL/AQAE/wEAFP8BAAX/AQAc/wEAa/8BAA7/AQAf/wEAH/8BABT/AQBl/wEA//////////9M/wAA//8D/wEABP8BAB7/AQAa/wEA0v8BAAT/AQAV/wEABP8BABz/AQBr/wEADv8BACD/AQAf/wEAE/8BAGX/AQD//////////0z/AAD//wP/AQAE/wEAHf8BABv/AQDT/wEAA/8BABX/AQAD/wEAHv8BAGv/AQAN/wEAIP8BAB//AQAU/wEAZf8BAP//////////S/8AAP//A/8BAAT/AQAd/wEAG/8BANP/AQAD/wEAFf8BAAP/AQAe/wEAa/8BAA3/AQAg/wEAH/8BABT/AQBl/wEA//////////9L/wAA//8D/wEABP8BAB3/AQAb/wEA0/8BAAT/AQAU/wEAAv8BAB//AQBs/wEADf8BACD/AQAf/wEAFP8BAGX/AQD//////////0r/AAD//wP/AQAF/wEAHP8BABv/AQDT/wEABP8BABT/AQAB/wEAIf8BAGv/AQAN/wEAIP8BAB//AQAU/wEAZf8BAP//////////Sv8AAP//A/8BAAX/AQAb/wEAHP8BANP/AQAE/wEAFP8BAAH/AQAh/wEAbP8BAAz/AQAg/wEAH/8BABT/AQBm/wEA//////////9J/wAA//8D/wEABf8BABv/AQAc/wEA0/8BAAT/AQAU/wIAIv8BAGz/AQAM/wEAIP8BACD/AQAU/wEAZf8BAP//////////Sf8AAP//A/8BAAX/AQAb/wEAHP8BANP/AQAE/wEAFP8CACP/AQBs/wEADP8BACD/AQAf/wEAFP8BAGX/AQD//////////0n/AAD//wP/AQAG/wEAGf8BABz/AQDU/wEABP8BABT/AgAj/wEAbP8BAAz/AQAg/wEAH/8BABX/AQBl/wEA//////////9I/wAA//8D/wEABv8BABn/AQAc/wEA1f8BAAP/AQAT/wEAAf8BACP/AQBs/wEADP8BACD/AQAf/wEAFf8BAGX/AQD//////////0j/AAD//wP/AQAG/wEAGf8BABz/AQDV/wEABP8BABH/AQAC/wEAJP8BAGz/AQAL/wEAIf8BAB//AQAU/wEAZv8BAP//////////R/8AAP//A/8BAAb/AQAY/wEAHf8BANX/AQAE/wEAEf8BAAL/AQAk/wEAbP8BAAz/AQAg/wEAH/8BABX/AQBl/wEA//////////9H/wAA//8D/wEAB/8BABf/AQAd/wEA1f8BAAT/AQAQ/wEAA/8BACT/AQBt/wEAC/8BACD/AQAf/wEAFf8BAGX/AQD//////////0f/AAD//wP/AQAH/wEAFv8BAB7/AQDW/wEAA/8BAA//AQAF/wEAJP8BAGz/AQAL/wEAIf8BAB//AQAV/wEAZf8BAP//////////Rv8AAP//A/8BAAj/AQAV/wEAHv8BANb/AQAD/wEADv8BAAb/AQAk/wEAbP8BAAv/AQAh/wEAH/8BABX/AQBl/wEA//////////9G/wAA//8D/wEACP8BABT/AQAf/wEA1v8BAAP/AQAN/wEAB/8BACT/AQBt/wEAC/8BACD/AQAf/wEAFv8BAGT/AQD//////////0b/AAD//wP/AQAI/wEAFP8BAB//AQDX/wEAAv8BAA3/AQAH/wEAJf8BAGz/AQAL/wEAIP8BACD/AQAV/wEAZf8BAP//////////Rf8AAP//A/8BAAn/AQAT/wEAH/8BANf/AQAD/wEAC/8BAAj/AQAl/wEAbf8BAAr/AQAh/wEAH/8BABX/AQBl/wEA//////////9F/wAA//8D/wEACf8BABL/AQAg/wEA2P8BAAL/AQAK/wEACf8BACX/AQBt/wEACv8BACH/AQAf/wEAFv8BAGX/AQD//////////0T/AAD//wP/AQAK/wEAEf8BAB//AQDZ/wEAAv8BAAj/AgAL/wEAJf8BAG3/AQAK/wEAIP8BACD/AQAV/wEAZf8BAP//////////RP8AAP//A/8BAAr/AQAQ/wEAIP8BANr/AQAB/wEABv8CAA3/AQAl/wEAbf8BAAr/AQAh/wEAH/8BABb/AQBk/wEA//////////9E/wAA//8D/wEAC/8BAA//AQAg/wEA2/8CAAT/AgAP/wEAJf8BAG3/AQAK/wEAIf8BAB//AQAW/wEAZf8BAP//////////Q/8AAP//A/8BAAv/AQAO/wEAIf8BANz/BQAR/wEAJf8BAG7/AQAJ/wEAIf8BACD/AQAV/wEAZf8BAP//////////Q/8AAP//BP8BAAv/AQAN/wEAIf8BAN3/AQAU/wEAJv8BAG3/AQAK/wEAIf8BAB//AQAW/wEAZP8BAP//////////Q/8AAP//BP8BAAv/AQAM/wEAIv8BAN3/AQAU/wEAJv8BAG3/AQAK/wEAIf8BAB//AQAW/wEAZP8BAP//////////Q/8AAP//BP8BAAz/AQAK/wEAI/8BAN3/AQAV/wEAJf8BAG7/AQAJ/wEAIf8BACD/AQAW/wEAZP8BAP//////////Qv8AAP//BP8BAAz/AQAK/wEAI/8BAN3/AQAV/wEAJv8BAG3/AQAJ/wEAIv8BAB//AQAW/wEAZP8BAP//////////Qv8AAP//BP8BAA3/AQAH/wIAJP8BAN3/AQAV/wEAJv8BAG3/AQAK/wEAIf8BAB//AQAX/wEAY/8BAP//////////Qv8AAP//BP8BAA7/AgAD/wIAJv8BAN3/AQAV/wEAJv8BAG7/AQAJ/wEAIf8BACD/AQAW/wEAZP8BAP//////////Qf8AAP//BP8BABD/AwAo/wEA3v8BABT/AQAn/wEAbf8BAAn/AQAh/wEAIP8BABf/AQBj/wEA//////////9B/wAA//8E/wEAO/8BAN7/AQAV/wEAJv8BAG7/AQAI/wEAIv8BAB//AQAX/wEAY/8BAP//////////Qf8AAP//BP8BADr/AQDf/wEAFf8BACb/AQBu/wEACP8BACL/AQAg/wEAFv8BAGP/AQD//////////0H/AAD//wT/AQA6/wEA3/8BABX/AQAn/wEAbf8BAAn/AQAh/wEAIP8BABf/AQBj/wEA//////////9A/wAA//8E/wEAOv8BAN//AQAV/wEAJ/8BAG7/AQAI/wEAIv8BAB//AQAX/wEAY/8BAP//////////QP8AAP//BP8BADr/AQDf/wEAFf8BACf/AQBu/wEACP8BACL/AQAg/wEAF/8BAGL/AQD//////////0D/AAD//wT/AQA6/wEA4P8BABT/AQAo/wEAbf8BAAj/AQAi/wEAIP8BABf/AQBj/wEA//////////8//wAA//8E/wEAOv8BAOD/AQAV/wEAJ/8BAG7/AQAI/wEAIv8BAB//AQAY/wEAYv8BAP//////////P/8AAP//BP8BADr/AQDg/wEAFf8BACf/AQBu/wEACP8BACL/AQAg/wEAF/8BACP/BgA5/wEA//////////8//wAA//8E/wEAOv8BAOD/AQAV/wEAKP8BAG3/AQAI/wEAIv8BACD/AQAY/wEAIP8CAAb/AgA4/wEA//////////8+/wAA//8E/wEAOv8BAOD/AQAV/wEAKP8BAG7/AQAH/wEAI/8BAB//AQAY/wEAHv8CAAr/AgA2/wEA//////////8+/wAA//8E/wEAOv8BAOD/AQAV/wEAKP8BAG7/AQAI/wEAIv8BACD/AQAX/wEAHf8BAA7/AgA0/wEA//////////8+/wAA//8E/wEAOv8BAOH/AQAU/wEAKP8BAG7/AQAI/wEAIv8BACD/AQAY/wEAG/8BABH/AQA0/wEA//////////89/wAA//8E/wEAOv8BAOH/AQAV/wEAKP8BAG7/AQAH/wEAI/8BAB//AQAY/wEAG/8BABL/AQAz/wEA//////////89/wAA//8E/wEAOf8BAOL/AQAV/wEAKP8BAG7/AQAH/wEAI/8BACD/AQAY/wEAGf8BABT/AgAy/wEA//////////88/wAA//8E/wEAOf8BAOL/AQAV/wEAKP8BAG7/AQAI/wEAIv8BACD/AQAY/wEAGf8BABb/AQAx/wEA//////////88/wAA//8E/wEAOf8BAOL/AQAV/wEAKf8BAG7/AQAH/wEAIv8BACD/AQAZ/wEAGP8BABf/AQAw/wEA//////////88/wAA//8E/wEAOf8BAOP/AQAU/wEAKf8BAG7/AQAH/wEAI/8BACD/AQAY/wEAF/8BABn/AQAw/wEA//////////87/wAA//8E/wEAOf8BAOP/AQAV/wEAKP8BAG7/AQAH/wEAI/8BACD/AQAY/wEAF/8BABr/AQAv/wEA//////////87/wAA//8E/wEAOf8BAOP/AQAV/wEAKf8BAG7/AQAH/wEAIv8BACD/AQAZ/wEAFv8BABv/AQAu/wEA//////////87/wAA//8E/wEAOf8BAOP/AQAV/wEAKf8BAG7/AQAH/wEAI/8BACD/AQAY/wEAFv8BABz/AQAt/wEA//////////87/wAA//8E/wEAOf8BAOP/AQAV/wEAKf8BAG7/AQAH/wEAI/8BACD/AQAZ/wEAFf8BABz/AQAu/wEA//////////86/wAA//8F/wEAOP8BAOP/AQAV/wEAKf8BAG//AQAG/wEAI/8BACD/AQAZ/wEAFf8BAB3/AQAt/wEA//////////86/wAA//8F/wEAOP8BAOT/AQAU/wEAKv8BAG7/AQAH/wEAI/8BACD/AQAY/wEAFf8BAB7/AQAs/wEA//////////86/wAA//8F/wEAOP8BAOT/AQAV/wEAKf8BAG7/AQAH/wEAI/8BACD/AQAZ/wEAFP8BAB//AQAr/wEA//////////86/wAA//8F/wEAOP8BAOT/AQAV/wEAKf8BAG//AQAG/wEAI/8BACD/AQAZ/wEAFP8BACD/AQAr/wEA//////////85/wAA//8F/wEAN/8BAOX/AQAV/wEAKv8BAG7/AQAG/wEAJP8BAB//AQAa/wEAE/8BACH/AQAq/wEA//////////85/wAA//8F/wEAN/8BAOX/AQAV/wEAKv8BAG7/AQAH/wEAI/8BACD/AQAZ/wEAE/8BACL/AQAp/wEA//////////85/wAA//8F/wEAN/8BAOX/AQAV/wEAKv8BAG//AQAG/wEAI/8BACD/AQAa/wEAEv8BACL/AQAp/wEA//////////85/wAA//8F/wEAN/8BAOb/AQAV/wEAKv8BAG7/AQAG/wEAI/8BACD/AQAa/wEAEv8BACP/AQAp/wEA//////////84/wAA//8F/wEAN/8BAOb/AQAV/wEAKv8BAG//AQAG/wEAI/8BACD/AQAZ/wEAEv8BACT/AQAo/wEA//////////84/wAA//8F/wEAN/8BAOb/AQAV/wEAKv8BAG//AQAG/wEAI/8BACD/AQAa/wEAEv8BACT/AQAn/wEA//////////84/wAA//8F/wEAN/8BAOb/AQAV/wEAKv8BAG//AQAG/wEAI/8BACD/AQAa/wEAEv8BACX/AQAm/wEA//////////84/wAA//8F/wEAN/8BAOb/AQAW/wEAKv8BAG//AQAF/wEAI/8BACH/AQAa/wEAEf8BACX/AQAn/wEA//////////83/wAA//8F/wEAN/8BAOb/AQAW/wEAKv8BAG//AQAG/wEAI/8BACD/AQAa/wEAEf8BACb/AQAm/wEA//////////83/wAA//8F/wEAN/8BAOf/AQAV/wEAKv8BAG//AQAG/wEAI/8BACD/AQAb/wEAEP8BACf/AQAl/wEA//////////83/wAA//8F/wEAN/8BAOf/AQAV/wEAK/8BAG//AQAF/wEAI/8BACH/AQAa/wEAEP8BACj/AQAk/wEA//////////83/wAA//8F/wEANv8BAOj/AQAV/wEAK/8BAG//AQAG/wEAI/8BACD/AQAb/wEAEP8BACf/AQAl/wEA//////////82/wAA//8F/wEANv8BAOj/AQAW/wEAKv8BAG//AQAG/wEAI/8BACD/AQAb/wEAEP8BACj/AQAk/wEA//////////82/wAA//8F/wEANv8BAOj/AQAW/wEAK/8BAG//AQAF/wEAI/8BACH/AQAb/wEAD/8BACj/AQAk/wEA//////////82/wAA//8F/wEANv8BAOj/AQAW/wEAK/8BAG//AQAF/wEAI/8BACH/AQAb/wEAD/8BACn/AQAk/wEA//////////81/wAA//8F/wEANv8BAOn/AQAV/wEAK/8BAG//AQAG/wEAI/8BACD/AQAc/wEAD/8BACn/AQAj/wEA//////////81/wAA//8G/wEANf8BAOn/AQAW/wEAK/8BAG7/AQAG/wEAI/8BACD/AQAc/wEAD/8BACn/AQAj/wEA//////////81/wAA//8G/wEANf8BAOn/AQAW/wEAK/8BAG//AQAF/wEAI/8BACH/AQAc/wEADv8BACr/AQAi/wEA//////////81/wAA//8G/wEANf8BAOn/AQAW/wEAK/8BAG//AQAF/wEAI/8BACH/AQAc/wEAD/8BACr/AQAi/wEA//////////80/wAA//8G/wEANf8BAOn/AQAW/wEALP8BAG7/AQAG/wEAI/8BACD/AQAd/wEADv8BACr/AQAi/wEA//////////80/wAA//8G/wEANf8BAOn/AQAW/wEALP8BAG//AQAF/wEAI/8BACH/AQAc/wEADv8BACv/AQAh/wEA//////////80/wAA//8G/wEANf8BAOr/AQAW/wEAK/8BAG//AQAF/wEAI/8BACH/AQAd/wEADv8BACr/AQAh/wEA//////////80/wAA//8G/wEANP8BAOv/AQAW/wEALP8BAG7/AQAG/wEAI/8BACD/AQAd/wEADv8BACv/AQAg/wEA//////////80/wAA//8G/wEANP8BAOv/AQAW/wEALP8BAG//AQAF/wEAI/8BACH/AQAd/wEADf8BACz/AQAg/wEA//////////8z/wAA//8G/wEANP8BAOv/AQAW/wEALP8BAG//AQAF/wEAI/8BACH/AQAd/wEADv8BACv/AQAg/wEA//////////8z/wAA//8G/wEANP8BAOv/AQAX/wEALP8BAG7/AQAF/wEAI/8BACH/AQAe/wEADf8BACz/AQAf/wEA//////////8z/wAA//8G/wEANP8BAOv/AQAX/wEALP8BAG7/AQAG/wEAI/8BACH/AQAd/wEADf8BACz/AQAf/wEA//////////8z/wAA//8G/wEANP8BAOz/AQAW/wEALP8BAG//AQAF/wEAI/8BACH/AQAe/wEADf8BACz/AQAe/wEA//////////8z/wAA//8G/wEANP8BAOz/AQAW/wEALP8BAG//AQAF/wEAI/8BACH/AQAf/wEADP8BACz/AQAe/wEA//////////8z/wAA//8H/wEAM/8BAOz/AQAW/wEALf8BAG7/AQAG/wEAIv8BACH/AQAf/wEADP8BAC3/AQAd/wEA//////////8z/wAA//8H/wEAM/8BAOz/AQAX/wEALP8BAG//AQAF/wEAI/8BACH/AQAf/wEADP8BACz/AQAe/wEA//////////8y/wAA//8H/wEAM/8BAOz/AQAX/wEALP8BAG//AQAF/wEAI/8BACH/AQAf/wEADP8BAC3/AQAd/wEA//////////8y/wAA//8H/wEAMv8BAO3/AQAX/wEALf8BAG7/AQAG/wEAIv8BACH/AQAg/wEAC/8BAC3/AQAd/wEA//////////8y/wAA//8H/wEAMv8BAO7/AQAW/wEALf8BAG//AQAF/wEAI/8BACH/AQAf/wEADP8BAC3/AQAc/wEA//////////8y/wAA//8H/wEAMv8BAO7/AQAX/wEALP8BAG//AQAF/wEAI/8BACH/AQAg/wEAC/8BAC3/AQAc/wEA//////////8y/wAA//8H/wEAMv8BAO7/AQAX/wEALf8BAG7/AQAF/wEAI/8BACH/AQAh/wEACv8BAC7/AQAb/wEA//////////8y/wAA//8H/wEAMv8BAO7/AQAX/wEALf8BAG//AQAF/wEAI/8BACH/AQAg/wEAC/8BAC7/AQAa/wEA//////////8y/wAA//8H/wEAMv8BAO7/AQAX/wEALf8BAG//AQAF/wEAI/8BACH/AQAh/wEACv8BAC7/AQAb/wEA//////////8x/wAA//8H/wEAMv8BAO7/AQAY/wEALf8BAG//AQAE/wEAI/8BACH/AQAh/wEACv8BAC//AQAa/wEA//////////8x/wAA//8I/wEAMf8BAO//AQAX/wEALf8BAG//AQAF/wEAIv8BACL/AQAh/wEACv8BAC7/AQAa/wEA//////////8x/wAA//8I/wEAMf8BAO//AQAX/wEALf8BAG//AQAF/wEAI/8BACH/AQAh/wEACv8BAC//AQAZ/wEA//////////8x/wAA//8I/wEAMf8BAO//AQAY/wEALf8BAG//AQAE/wEAI/8BACH/AQAi/wEACf8BADD/AQAY/wEA//////////8x/wAA//8I/wEAMP8BAPD/AQAY/wEALf8BAG//AQAF/wEAIv8BACL/AQAh/wEACv8BAC//AQAY/wEA//////////8x/wAA//8I/wEAMP8BAPD/AQAY/wEALf8BAG//AQAF/wEAI/8BACH/AQAi/wEACf8BADD/AQAY/wEA//////////8w/wAA//8I/wEAMP8BAPD/AQAY/wEALv8BAG//AQAE/wEAI/8BACH/AQAi/wEACf8BADH/AQAX/wEA//////////8w/wAA//8J/wEAL/8BAPH/AQAY/wEALf8BAG//AQAE/wEAI/8BACH/AQAj/wEACf8BADD/AQAX/wEA//////////8w/wAA//8J/wEAL/8BAPH/AQAY/wEALf8BAG//AQAF/wEAI/8BACH/AQAj/wEACP8BADH/AQAW/wEA//////////8w/wAA//8J/wEAL/8BAPH/AQAY/wEALf8BAHD/AQAE/wEAI/8BACH/AQAj/wEACP8BADH/AQAW/wEA//////////8w/wAA//8J/wEAL/8BAPH/AQAZ/wEALf8BAG//AQAE/wEAI/8BACH/AQAk/wEAB/8BADL/AQAW/wEA//////////8v/wAA//8J/wEAL/8BAPH/AQAZ/wEALf8BAG//AQAF/wEAI/8BACH/AQAk/wEAB/8BADL/AQAV/wEA//////////8v/wAA//8J/wEAL/8BAPH/AQAZ/wEALf8BAHD/AQAE/wEAI/8BACH/AQAl/wEABv8BADL/AQAV/wEA//////////8v/wAA//8K/wEALv8BAPL/AQAZ/wEALP8BAHD/AQAE/wEAI/8BACH/AQAl/wEABv8BADP/AQAU/wEA//////////8v/wAA//8K/wEALf8BAPP/AQAZ/wEALf8BAHD/AQAD/wEAJP8BACH/AQAl/wEABf8BADP/AQAU/wEA//////////8v/wAA//8K/wEALf8BAPP/AQAZ/wEALf8BAHD/AQAE/wEAI/8BACH/AQAm/wEABf8BADP/AQAT/wEA//////////8v/wAA//8K/wEALf8BAPP/AQAa/wEALP8BAHD/AQAE/wEAI/8BACH/AQAn/wEABP8BADP/AQAU/wEA//////////8u/wAA//8K/wEALf8BAPP/AQAa/wEALP8BAHH/AQAD/wEAI/8BACH/AQAn/wEABP8BADT/AQAT/wEA//////////8u/wAA//8K/wEALf8BAPT/AQAZ/wEALf8BAHD/AQAE/wEAI/8BACH/AQAn/wEAAv8BADX/AQAT/wEA//////////8u/wAA//8L/wEALP8BAPT/AQAa/wEALP8BAHD/AQAE/wEAI/8BACH/AQAo/wMANv8BABL/AQD//////////y7/AAD//wv/AQAs/wEA9P8BABr/AQAs/wEAcf8BAAP/AQAj/wEAIf8BAGH/AQAS/wEA//////////8u/wAA//8L/wEALP8BAPT/AQAa/wEALP8BAHH/AQAD/wEAJP8BACH/AQBh/wEAEf8BAP//////////Lv8AAP//C/8BACz/AQD0/wEAGv8BAC3/AQBx/wEAA/8BACP/AQAh/wEAYv8BABD/AQD//////////y7/AAD//wv/AQAr/wEA9f8BABv/AQAs/wEAcf8BAAP/AQAj/wEAIf8BAGL/AQAQ/wEA//////////8u/wAA//8M/wEAKv8BAPb/AQAa/wEALP8BAHH/AQAD/wEAJP8BACD/AQBj/wEAD/8BAP//////////Lv8AAP//DP8BACr/AQD2/wEAGv8BACz/AQBy/wEAA/8BACP/AQAh/wEAYv8BAA//AQD//////////y7/AAD//wz/AQAq/wEA9v8BABv/AQAs/wEAcf8BAAP/AQAj/wEAIf8BAGP/AQAO/wEA//////////8u/wAA//8M/wEAKv8BAPb/AQAb/wEALP8BAHH/AQAD/wEAJP8BACD/AQBj/wEADv8BAP//////////Lv8AAP//DP8BACr/AQD2/wEAG/8BACz/AQBy/wEAA/8BACP/AQAh/wEAY/8BAA3/AQD//////////y7/AAD//w3/AQAp/wEA9/8BABv/AQAr/wEAcv8BAAP/AQAj/wEAIf8BAGP/AQAN/wEA//////////8u/wAA//8N/wEAKf8BAPf/AQAb/wEALP8BAHH/AQAD/wEAJP8BACD/AQBk/wEADP8BAP//////////Lv8AAP//Df8BACj/AQD4/wEAG/8BACz/AQBy/wEAAv8BACT/AQAg/wEAZf8BAAv/AQD//////////y7/AAD//w3/AQAo/wEA+P8BABz/AQAr/wEAcv8BAAP/AQAj/wEAIf8BAGT/AQAL/wEA//////////8u/wAA//8N/wEAKP8BAPj/AQAc/wEAK/8BAHL/AQAD/wEAJP8BACD/AQBl/wEACv8BAP//////////Lv8AAP//Df8BACj/AQD4/wEAHf8BACv/AQBy/wEAAv8BACT/AQAg/wEAZv8BAAn/AQD//////////y7/AAD//w7/AQAn/wEA+f8BABz/AQAr/wEAcv8BAAP/AQAj/wEAIf8BAGX/AQAJ/wEA//////////8u/wAA//8O/wEAJ/8BAPn/AQAc/wEAK/8BAHL/AQAD/wEAJP8BACD/AQBm/wEAB/8BAP//////////L/8AAP//Dv8BACf/AQD5/wEAHf8BACv/AQBy/wEAAv8BACT/AQAg/wEAZ/8BAAb/AQD//////////y//AAD//w7/AQAn/wEA+f8BAB3/AQAr/wEAcv8BAAP/AQAj/wEAIP8BAGj/AQAF/wEA//////////8v/wAA//8O/wEAJv8BAPr/AQAe/wEAKv8BAHL/AQAD/wEAJP8BACD/AQBo/wEAAv8CAP//////////MP8AAP//D/8BACX/AQD6/wEAHv8BACr/AQBz/wEAAv8BACT/AQAg/wEAaf8CAP//////////Mv8AAP//D/8BACX/AQD7/wEAHf8BACv/AQBy/wEAA/8BACT/AQAf/wEA//////////+d/wAA//8P/wEAJf8BAPv/AQAe/wEAKv8BAHP/AQAC/wEAJP8BAB//AQD//////////53/AAD//w//AQAl/wEA+/8BAB7/AQAq/wEAc/8BAAL/AQAk/wEAIP8BAP//////////nP8AAP//EP8BACT/AQD7/wEAHv8BACr/AQBz/wEAAv8BACX/AQAf/wEA//////////+c/wAA//8Q/wEAJP8BAPv/AQAf/wEAKv8BAHP/AQAC/wEAJP8BAB//AQD//////////5z/AAD//xD/AQAj/wEA/P8BAB//AQAq/wEAc/8BAAL/AQAk/wEAH/8BAP//////////nP8AAP//EP8BACP/AQD8/wEAIP8BACn/AQB0/wEAAf8BACX/AQAf/wEA//////////+b/wAA//8R/wEAIv8BAP3/AQAf/wEAKf8BAHT/AQAC/wEAJP8BAB//AQD//////////5v/AAD//xH/AQAi/wEA/f8BAB//AQAq/wEAdP8BAAH/AQAk/wEAH/8BAP//////////m/8AAP//Ef8BACL/AQD9/wEAIP8BACn/AQB0/wEAAf8BACX/AQAe/wEA//////////+b/wAA//8S/wEAIP8BAP7/AQAg/wEAKf8BAHT/AQAC/wEAJP8BAB//AQD//////////5r/AAD//xL/AQAg/wEA/v8BACH/AQAo/wEAdf8BAAH/AQAk/wEAH/8BAP//////////mv8AAP//Ev8BACD/AQD+/wEAIf8BACn/AQB0/wEAAf8BACX/AQAe/wEA//////////+a/wAA//8T/wEAH/8BAP//AQAg/wEAKf8BAHT/AQAC/wEAJP8BAB7/AQD//////////5r/AAD//xP/AQAe/wEA//8B/wEAIf8BACj/AQB1/wEAAf8BACT/AQAf/wEA//////////+Z/wAA//8T/wEAHv8BAP//Af8BACH/AQAo/wEAdf8BAAL/AQAk/wEAHv8BAP//////////mf8AAP//E/8BAB7/AQD//wH/AQAi/wEAKP8BAHX/AQAB/wEAJP8BAB7/AQD//////////5n/AAD//xT/AQAd/wEA//8B/wEAIv8BACj/AQB1/wEAAf8BACX/AQAd/wEA//////////+Z/wAA//8U/wEAHf8BAP//Af8BACP/AQAn/wEAdf8BAAL/AQAk/wEAHv8BAP//////////mP8AAP//FP8BABz/AQD//wL/AQAj/wEAJ/8BAHb/AQAB/wEAJP8BAB7/AQD//////////5j/AAD//xX/AQAb/wEA//8D/wEAI/8BACf/AQB1/wEAAf8BACX/AQAd/wEA//////////+Y/wAA//8V/wEAG/8BAP//A/8BACP/AQAn/wEAdf8BAAL/AQAk/wEAHf8BAP//////////mP8AAP//Ff8BABv/AQD//wP/AQAk/wEAJv8BAHb/AQAB/wEAJf8BAB3/AQD//////////5f/AAD//xX/AQAb/wEA//8D/wEAJP8BACb/AQB2/wEAAv8BACT/AQAd/wEA//////////+X/wAA//8W/wEAGf8BAP//BP8BACX/AQAm/wEAdv8BAAH/AQAl/wEAHP8BAP//////////l/8AAP//Fv8BABn/AQD//wT/AQAl/wEAJv8BAHb/AQAB/wEAJf8BABz/AQD//////////5f/AAD//xb/AQAZ/wEA//8E/wEAJv8BACX/AQB3/wEAAf8BACT/AQAc/wEA//////////+X/wAA//8X/wEAGP8BAP//Bf8BACX/AQAl/wEAd/8BAAH/AQAl/wEAG/8BAP//////////l/8AAP//F/8BABj/AQD//wX/AQAm/wEAJP8BAHj/AgAl/wEAHP8BAP//////////lv8AAP//F/8BABf/AQD//wb/AQAm/wEAJf8BAHj/AgAl/wEAG/8BAP//////////lv8AAP//F/8BABf/AQD//wb/AQAn/wEAJP8BAHj/AgAl/wEAG/8BAP//////////lv8AAP//GP8BABb/AQD//wb/AQAn/wEAJP8BAHn/AQAl/wEAG/8BAP//////////lv8AAP//GP8BABb/AQD//wb/AQAo/wEAI/8BAHn/AgAl/wEAGv8BAP//////////lv8AAP//GP8BABb/AQD//wb/AQAo/wEAI/8BAHr/AQAl/wEAGv8BAP//////////lv8AAP//Gf8BABT/AQD//wf/AQAp/wEAI/8BAHn/AQAm/wEAGf8BAP//////////lv8AAP//Gf8BABT/AQD//wj/AQAo/wEAI/8BAHr/AQAl/wEAGv8BAP//////////lf8AAP//Gv8BABP/AQD//wj/AQAp/wEAIv8BAHr/AQAm/wEAGf8BAP//////////lf8AAP//Gv8BABP/AQD//wj/AQAp/wEAIv8BAKH/AQAZ/wEA//////////+V/wAA//8a/wEAEv8BAP//Cf8BACr/AQAi/wEAoP8BABn/AQD//////////5X/AAD//xv/AQAR/wEA//8J/wEAKv8BACL/AQCh/wEAGP8BAP//////////lf8AAP//G/8BABH/AQD//wn/AQAr/wEAIf8BAKH/AQAY/wEA//////////+V/wAA//8c/wEAD/8BAP//Cv8BACz/AQAg/wEAov8BABj/AQD//////////5T/AAD//xz/AQAP/wEA//8L/wEAK/8BACD/AQCi/wEAGP8BAP//////////lP8AAP//Hf8BAA7/AQD//wv/AQAs/wEAIP8BAKL/AQAX/wEA//////////+U/wAA//8d/wEADv8BAP//C/8BACz/AQAg/wEAov8BABf/AQD//////////5T/AAD//x7/AQAM/wEA//8M/wEALf8BAB//AQCj/wEAFv8BAP//////////lP8AAP//Hv8BAAz/AQD//wz/AQAt/wEAH/8BAKP/AQAW/wEA//////////+U/wAA//8f/wEACv8BAP//Df8BAC7/AQAe/wEApP8BABX/AQD//////////5T/AAD//x//AQAK/wEA//8N/wEAL/8BAB3/AQCk/wEAFf8BAP//////////lP8AAP//IP8BAAj/AQD//w7/AQAv/wEAHv8BAKT/AQAU/wEA//////////+U/wAA//8h/wEAB/8BAP//Dv8BADD/AQAd/wEApP8BABT/AQD//////////5T/AAD//yH/AQAG/wEA//8P/wEAMP8BAB3/AQCl/wEAE/8BAP//////////lP8AAP//Iv8BAAP/AgD//xD/AQAx/wEAHP8BAKX/AQAU/wEA//////////+T/wAA//8j/wMA//8S/wEAMv8BABv/AQCm/wEAE/8BAP//////////k/8AAP////84/wEAMv8BABv/AQCm/wEAE/8BAP//////////k/8AAP////84/wEAM/8BABv/AQCm/wEAEv8BAP//////////k/8AAP////84/wEAM/8BABv/AQCn/wEAEf8BAP//////////k/8AAP////84/wEANP8BABr/AQCo/wEAEP8BAP//////////k/8AAP////84/wEANP8BABr/AQCo/wEAEP8BAP//////////k/8AAP////84/wEANf8BABn/AQCp/wEAD/8BAP//////////k/8AAP////84/wEANv8BABj/AQCq/wEADv8BAP//////////k/8AAP////84/wEANv8BABn/AQCq/wEADf8BAP//////////k/8AAP////84/wEAN/8BABj/AQCr/wEADP8BAP//////////k/8AAP////84/wEAOP8BABf/AQCr/wEAC/8BAP//////////lP8AAP////83/wEAOf8BABf/AQCs/wEACv8BAP//////////lP8AAP////83/wEAOv8BABb/AQCt/wEACf8BAP//////////lP8AAP////83/wEAO/8BABX/AQCu/wIAB/8BAP//////////lP8AAP////83/wEAPP8BABT/AQCw/wEABv8BAP//////////lP8AAP////83/wEAPP8BABT/AQCx/wIAAv8CAP//////////lf8AAP////82/wEAPv8BABP/AQCz/wIA//////////+X/wAA/////zb/AQA//wEAE/8BAP////////////9M/wAA/////zb/AQA//wEAE/8BAP////////////9M/wAA/////zb/AQBA/wEAEv8BAP////////////9M/wAA/////zX/AQBC/wEAEf8BAP////////////9M/wAA/////zX/AQBD/wEAEP8BAP////////////9M/wAA/////zX/AQBE/wEAD/8BAP////////////9M/wAA/////zT/AQBG/wEADv8BAP////////////9M/wAA/////zT/AQBH/wEADf8BAP////////////9M/wAA/////zP/AQBJ/wEADP8BAP////////////9M/wAA/////zL/AQBL/wEACv8BAP////////////9N/wAA/////zH/AQBN/wEACf8BAP////////////9N/wAA/////4D/AQAI/wEA/////////////03/AAD/////gf8CAAb/AQD/////////////Tf8AAP////+D/wEABP8BAP////////////9O/wAA/////4T/AgAC/wEA/////////////07/AAD/////hv8CAP////////////9P/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAA///////////////////X/wAB";
                if (receta != null)
                {
                    using (repReceta objReport = new repReceta())
                    {
                        PersonalizarCamposReceta(objReport, receta);
                        objReport.FirmaMedico.Image = ImageFromBase64(firma);
                        if (objReport.FirmaMedico.Image == null)
                            objReport.FirmaMedico.Image = ImageFromBytes(firma);

                        vRet = new MemoryStream();
                        objReport.ExportToPdf(vRet);
                    }

                    return vRet == null ? Encoding.Unicode.GetBytes("No pudo armarse el PDF") : vRet.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Encoding.Unicode.GetBytes("No pudo armarse el PDF");
        }

        private Image ImageFromBase64(string firma)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(firma);

                Image image;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = Image.FromStream(ms);
                }

                return image;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
        private Image ImageFromBytes(string firma)
        {
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(firma);

                Image image;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = Image.FromStream(ms);
                }

                return image;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private DataTable CreateDummyDataTable()
        {
            DataSet ds = new DataSet();

            DataTable dt = new DataTable();
            dt.Columns.Add("colFecha");
            dt.Columns.Add("colTipoComprobante");
            dt.Columns.Add("colNroFactura");
            dt.Columns.Add("colImporte");

            dt.Rows.Add(new object[] { DateTime.Now, "FAC", "A002100000086", 270824.50 });

            dt.Rows.Add(new object[] { DateTime.Now, "FAC", "A002100000087", 100.50 });
            //ds.Tables.Add(dt);
            return dt;
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

        private List<string> PersonalizarCamposRecibo(repRecibo objReport, ReciboModel rm, DataTable dtImportes, DataTable dtFormas, string importeDescripcion, string concepto)
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

        private void PersonalizarCamposReceta(repReceta objReport, RecetaModel rm)
        {
            objReport.nroReceta.Text = rm.NroReceta.ToString("0000000000");
            //ReciboModel rm = new ReciboModel(dt, dtImp);
            objReport.Paciente.Text = rm.Nombre;
            objReport.Entidad.Text = rm.Cliente.ClienteId;
            objReport.plan.Text = rm.Plan.Id;
            objReport.NroAfiliado.Text = rm.NroAfiliado;
            objReport.Diagnostico.Text = rm.Diagnostico;
            objReport.TratamientoProlongado.Text = rm.flgTratamientoProlongado == 1? "SI" : "NO";
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


        private DataTable CreateDummyDataTable2()
        {
            DataSet ds = new DataSet();

            DataTable dt = new DataTable();
            dt.Columns.Add("colFormaDePago");
            dt.Columns.Add("colBanco");
            dt.Columns.Add("colNroCheque");
            dt.Columns.Add("colFecCheque");
            dt.Columns.Add("colImporte");

            dt.Rows.Add(new object[] { "EF", "", "", DateTime.Now, 270824.50 });
            dt.Rows.Add(new object[] { "CH", "SANTANDER RIO S.A.", 83981410, DateTime.Now, 270824.50 });

            //ds.Tables.Add(dt);
            return dt;
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