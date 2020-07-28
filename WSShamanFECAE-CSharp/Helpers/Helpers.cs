using System;
using System.IO;
using System.Text;
using System.Data;
using System.Drawing;
using System.Web.Configuration;
using System.Collections.Specialized;
using System.Collections.Generic;

using DevExpress.XtraReports.UI;

namespace WSShamanFECAE_CSharp.Helpers
{
    public static class Helpers
    {
        public static Image ImageFromBase64(string firma)
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
        public static Image ImageFromBytes(string firma)
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
        public static DataTable CreateDummyDataTable()
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
        public static DataTable CreateDummyDataTable2()
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
        public static void BindSection(Band pBand, object pSou, List<string> pCamposPersonalizados = null)
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
        public static ConnectionStringCache GetConnectionString()
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
    }
}