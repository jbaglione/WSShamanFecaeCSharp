namespace WSShamanFECAE_CSharp.report
{
    partial class subRepImportes
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.colFecha = new DevExpress.XtraReports.UI.XRTableCell();
            this.colTipoComprobante = new DevExpress.XtraReports.UI.XRTableCell();
            this.colNroFactura = new DevExpress.XtraReports.UI.XRTableCell();
            this.colImporte = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.Detail.HeightF = 100F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable1
            // 
            this.xrTable1.BorderColor = System.Drawing.Color.Navy;
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.BorderWidth = 1F;
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(319F, 25F);
            this.xrTable1.StylePriority.UseBorderColor = false;
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseBorderWidth = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.colFecha,
            this.colTipoComprobante,
            this.colNroFactura,
            this.colImporte});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // colFecha
            // 
            this.colFecha.BorderWidth = 1F;
            this.colFecha.Name = "colFecha";
            this.colFecha.StylePriority.UseBorderWidth = false;
            this.colFecha.StylePriority.UseTextAlignment = false;
            this.colFecha.Text = "FECHA";
            this.colFecha.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.colFecha.Weight = 0.66873065052679437D;
            // 
            // colTipoComprobante
            // 
            this.colTipoComprobante.BorderWidth = 1F;
            this.colTipoComprobante.Name = "colTipoComprobante";
            this.colTipoComprobante.StylePriority.UseBorderWidth = false;
            this.colTipoComprobante.StylePriority.UseTextAlignment = false;
            this.colTipoComprobante.Text = "TIPO";
            this.colTipoComprobante.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.colTipoComprobante.Weight = 0.44582043924482995D;
            // 
            // colNroFactura
            // 
            this.colNroFactura.BorderWidth = 1F;
            this.colNroFactura.Name = "colNroFactura";
            this.colNroFactura.StylePriority.UseBorderWidth = false;
            this.colNroFactura.StylePriority.UseTextAlignment = false;
            this.colNroFactura.Text = "N° FACTURA";
            this.colNroFactura.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.colNroFactura.Weight = 0.89164086238216522D;
            // 
            // colImporte
            // 
            this.colImporte.BorderWidth = 1F;
            this.colImporte.Name = "colImporte";
            this.colImporte.StylePriority.UseBorderWidth = false;
            this.colImporte.StylePriority.UseTextAlignment = false;
            this.colImporte.Text = "IMPORTE";
            this.colImporte.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.colImporte.Weight = 0.83814234380407915D;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 5F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.Name = "sqlDataSource1";
            // 
            // subRepImportes
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.BorderColor = System.Drawing.Color.DarkRed;
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.Margins = new System.Drawing.Printing.Margins(0, 522, 0, 5);
            this.Version = "14.2";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell colFecha;
        private DevExpress.XtraReports.UI.XRTableCell colNroFactura;
        private DevExpress.XtraReports.UI.XRTableCell colImporte;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraReports.UI.XRTableCell colTipoComprobante;
    }
}
