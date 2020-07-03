namespace WSShamanFECAE_CSharp.report
{
    partial class subRepMedicamentos
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
            this.colNombre = new DevExpress.XtraReports.UI.XRTableCell();
            this.colDroga = new DevExpress.XtraReports.UI.XRTableCell();
            this.colPresentacion = new DevExpress.XtraReports.UI.XRTableCell();
            this.colObservacion = new DevExpress.XtraReports.UI.XRTableCell();
            this.colCantidad = new DevExpress.XtraReports.UI.XRTableCell();
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
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(5F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(710F, 20.53572F);
            this.xrTable1.StylePriority.UseBorderColor = false;
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseBorderWidth = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.colNombre,
            this.colDroga,
            this.colPresentacion,
            this.colObservacion,
            this.colCantidad});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // colNombre
            // 
            this.colNombre.BorderWidth = 1F;
            this.colNombre.Font = new System.Drawing.Font("Arial", 9.75F);
            this.colNombre.Name = "colNombre";
            this.colNombre.StylePriority.UseBorderWidth = false;
            this.colNombre.StylePriority.UseFont = false;
            this.colNombre.StylePriority.UseTextAlignment = false;
            this.colNombre.Text = "NOMBRE";
            this.colNombre.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.colNombre.Weight = 0.54830549549419416D;
            // 
            // colDroga
            // 
            this.colDroga.BorderWidth = 1F;
            this.colDroga.Font = new System.Drawing.Font("Arial", 9.75F);
            this.colDroga.Name = "colDroga";
            this.colDroga.StylePriority.UseBorderWidth = false;
            this.colDroga.StylePriority.UseFont = false;
            this.colDroga.StylePriority.UseTextAlignment = false;
            this.colDroga.Text = "DROGA";
            this.colDroga.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.colDroga.Weight = 0.51403641541203626D;
            // 
            // colPresentacion
            // 
            this.colPresentacion.BorderWidth = 1F;
            this.colPresentacion.Font = new System.Drawing.Font("Arial", 9.75F);
            this.colPresentacion.Name = "colPresentacion";
            this.colPresentacion.StylePriority.UseBorderWidth = false;
            this.colPresentacion.StylePriority.UseFont = false;
            this.colPresentacion.StylePriority.UseTextAlignment = false;
            this.colPresentacion.Text = "PRESENTACIÓN";
            this.colPresentacion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.colPresentacion.Weight = 0.61684369371034886D;
            // 
            // colObservacion
            // 
            this.colObservacion.BorderWidth = 1F;
            this.colObservacion.Font = new System.Drawing.Font("Arial", 9.75F);
            this.colObservacion.Name = "colObservacion";
            this.colObservacion.StylePriority.UseBorderWidth = false;
            this.colObservacion.StylePriority.UseFont = false;
            this.colObservacion.StylePriority.UseTextAlignment = false;
            this.colObservacion.Text = "OBSERVACIÓN";
            this.colObservacion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.colObservacion.Weight = 0.61684370559828772D;
            // 
            // colCantidad
            // 
            this.colCantidad.BorderWidth = 1F;
            this.colCantidad.Font = new System.Drawing.Font("Arial", 9.75F);
            this.colCantidad.Name = "colCantidad";
            this.colCantidad.StylePriority.UseBorderWidth = false;
            this.colCantidad.StylePriority.UseFont = false;
            this.colCantidad.StylePriority.UseTextAlignment = false;
            this.colCantidad.Text = "CANT";
            this.colCantidad.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.colCantidad.Weight = 0.13707644055034293D;
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
            // subRepMedicamentos
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.BorderColor = System.Drawing.Color.DarkRed;
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.Margins = new System.Drawing.Printing.Margins(0, 131, 0, 5);
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
        private DevExpress.XtraReports.UI.XRTableCell colNombre;
        private DevExpress.XtraReports.UI.XRTableCell colPresentacion;
        private DevExpress.XtraReports.UI.XRTableCell colObservacion;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraReports.UI.XRTableCell colDroga;
        private DevExpress.XtraReports.UI.XRTableCell colCantidad;
    }
}
