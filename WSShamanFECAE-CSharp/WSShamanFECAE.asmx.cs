﻿using System.Web.Services;
using System.ComponentModel;
using System.Data;

using System;

using WSShamanFECAE_CSharp.Enums;
using WSShamanFECAE_CSharp.Models;
using NLog;

namespace WSShamanFECAE_CSharp
{
    /// <summary>
    /// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    /// System.Web.Script.Services.ScriptService()
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class WSShamanFECAE : WebService
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDocId"></param>
        /// <param name="pUsrId"></param>
        /// <returns></returns>
        [WebMethod()]
        public byte[] GetPDF_Cache(decimal pDocId, decimal pUsrId)
        {
            ShamanFECAE FECAE = new ShamanFECAE();
            return FECAE.GetPDF_Cache(pDocId, pUsrId);
        }

        /// <summary>
        /// GetRecetaPdf
        /// </summary>
        /// <param name="pRecetaId"></param>
        /// <param name="pUsrId"></param>
        /// <returns></returns>
        [WebMethod()]
        public byte[] GetRecetaPdf(decimal pRecetaId, decimal pUsrId)
        {
            ShamanFECAE FECAE = new ShamanFECAE();
            return FECAE.GetRecetaPdf(pRecetaId, pUsrId);
        }

        /// <summary>
        /// GetInformeCovidPdf
        /// </summary>
        /// <param name="pIncidenteId"></param>
        /// <returns></returns>
        [WebMethod()]
        public byte[] GetInformeCovidPdf(decimal pIncidenteId)
        {
            ShamanFECAE FECAE = new ShamanFECAE();
            // Se intentara 3 veces.
            try
            {
                logger.Info($"GetInformeCovidPdfCrystal pIncidenteId={pIncidenteId} (primer intento)");
                return FECAE.GetInformeCovidPdfCrystal(pIncidenteId);
            }
            catch (Exception)
            {
                try
                {
                    logger.Info($"GetInformeCovidPdfCrystal pIncidenteId={pIncidenteId} (segundo intento)");
                    return FECAE.GetInformeCovidPdfCrystal(pIncidenteId);
                }
                catch (Exception)
                {
                    try
                    {
                        logger.Info($"GetInformeCovidPdfCrystal pIncidenteId={pIncidenteId} (tercer intento)");
                        return FECAE.GetInformeCovidPdfCrystal(pIncidenteId);
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"GetInformeCovidPdfCrystal pIncidenteId={pIncidenteId} (fallaron los tres intentos) Exception = {ex}");
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// GetInformeCovidPdfV2
        /// </summary>
        /// <param name="pInformeEpidemiologia"></param>
        /// <returns></returns>
        [WebMethod()]
        public byte[] GetInformeCovidPdfV2(InformeEpidemiologia pInformeEpidemiologia)
        {
            ShamanFECAE FECAE = new ShamanFECAE();
            return FECAE.GetInformeCovidPdfV2(pInformeEpidemiologia);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pInc"></param>
        /// <param name="pUsrId"></param>
        /// <returns></returns>
        [WebMethod()]
        public byte[] GetReportsIncidente_Cache(decimal pInc, decimal pUsrId)
        {

            ShamanFECAE FECAE = new ShamanFECAE();
            return FECAE.GetReportsIncidente_Cache(pInc, pUsrId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pUsr"></param>
        /// <returns></returns>
        [WebMethod()]
        public DataTable GetCuentaCorriene(decimal pUsr)
        {
            ShamanFECAE FECAE = new ShamanFECAE();
            return FECAE.GetCuentaCorriene(pUsr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pNroOp"></param>
        /// <param name="pRetId"></param>
        /// <returns></returns>
        [WebMethod()]
        public byte[] GetCertificadoRetencion_Tango(string pNroOp, Certificado pRetId)
        {
            ShamanFECAE FECAE = new ShamanFECAE();
            return FECAE.GetCertificadoRetencion_Tango(pNroOp, pRetId);
        }

        /// <summary>
        /// Retorna el contrato en un Array de byte, correspondiente a un pdf.
        /// </summary>
        /// <param name="pClienteId"></param>
        /// <returns></returns>
        [WebMethod()]
        public byte[] GetContratoVenta(string pClienteId)
        {
            ShamanFECAE FECAE = new ShamanFECAE();
            return FECAE.GetContratoVenta(pClienteId);
        }

        /// <summary>
        /// Retorna el contrato en un Array de byte, correspondiente a un pdf.
        /// </summary>
        /// <param name="pNroComprobante"></param>
        /// <returns></returns>
        [WebMethod()]
        public byte[] GetComprobante(int pNroComprobante)
        {
            ShamanFECAE FECAE = new ShamanFECAE();
            var a = FECAE.GetComprobante(pNroComprobante);
            return a;
        }

        /// <summary>
        /// GetPuntosEnPoligono
        /// </summary>
        /// <param name="pLat"></param>
        /// <param name="pLon"></param>
        /// <param name="pTip"></param>
        /// <returns></returns>
        [WebMethod()]
        public string GetPuntosEnPoligono(float pLat, float pLon, string pTip)
        {
            ShamanFECAE FECAE = new ShamanFECAE();
            string r = FECAE.GetPuntosEnPoligono(pLat, pLon, pTip);
            return r;
        }
        
    }
}