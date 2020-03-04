namespace WSShamanFECAE_CSharp.report
{
    partial class subRepReciboFormasPago
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
            this.colFormaDePago = new DevExpress.XtraReports.UI.XRTableCell();
            this.colBanco = new DevExpress.XtraReports.UI.XRTableCell();
            this.colNroCheque = new DevExpress.XtraReports.UI.XRTableCell();
            this.colFecCheque = new DevExpress.XtraReports.UI.XRTableCell();
            this.colImporte = new DevExpress.XtraReports.UI.XRTableCell();
            this.ImporteDescripcionPart1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel59 = new DevExpress.XtraReports.UI.XRLabel();
            this.ImporteDescripcionPart2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel64 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel40 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.formattingRule1 = new DevExpress.XtraReports.UI.FormattingRule();
            this.ConceptoPart1 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.ConceptoPart1,
            this.xrTable1,
            this.ImporteDescripcionPart1,
            this.xrLabel59,
            this.ImporteDescripcionPart2,
            this.xrLabel64,
            this.xrLabel40});
            this.Detail.HeightF = 360F;
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
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(7F, 75F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(391.5F, 25F);
            this.xrTable1.StylePriority.UseBorderColor = false;
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseBorderWidth = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.colFormaDePago,
            this.colBanco,
            this.colNroCheque,
            this.colFecCheque,
            this.colImporte});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // colFormaDePago
            // 
            this.colFormaDePago.BorderWidth = 1F;
            this.colFormaDePago.Name = "colFormaDePago";
            this.colFormaDePago.StylePriority.UseBorderWidth = false;
            this.colFormaDePago.StylePriority.UseTextAlignment = false;
            this.colFormaDePago.Text = "FORMA";
            this.colFormaDePago.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.colFormaDePago.Weight = 0.50958965687575075D;
            // 
            // colBanco
            // 
            this.colBanco.BorderWidth = 1F;
            this.colBanco.Name = "colBanco";
            this.colBanco.StylePriority.UseBorderWidth = false;
            this.colBanco.StylePriority.UseTextAlignment = false;
            this.colBanco.Text = "BANCO";
            this.colBanco.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.colBanco.Weight = 0.87358228872487054D;
            // 
            // colNroCheque
            // 
            this.colNroCheque.BorderWidth = 1F;
            this.colNroCheque.Name = "colNroCheque";
            this.colNroCheque.StylePriority.UseBorderWidth = false;
            this.colNroCheque.StylePriority.UseTextAlignment = false;
            this.colNroCheque.Text = "N° CHEQUE";
            this.colNroCheque.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.colNroCheque.Weight = 0.77651755988062976D;
            // 
            // colFecCheque
            // 
            this.colFecCheque.BorderWidth = 1F;
            this.colFecCheque.Name = "colFecCheque";
            this.colFecCheque.StylePriority.UseBorderWidth = false;
            this.colFecCheque.StylePriority.UseTextAlignment = false;
            this.colFecCheque.Text = "FEC. CHEQ.";
            this.colFecCheque.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.colFecCheque.Weight = 0.72798523253139324D;
            // 
            // colImporte
            // 
            this.colImporte.BorderWidth = 1F;
            this.colImporte.Name = "colImporte";
            this.colImporte.StylePriority.UseBorderWidth = false;
            this.colImporte.StylePriority.UseTextAlignment = false;
            this.colImporte.Text = "IMPORTE";
            this.colImporte.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.colImporte.Weight = 0.91240811160920421D;
            // 
            // ImporteDescripcionPart1
            // 
            this.ImporteDescripcionPart1.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ImporteDescripcionPart1.LocationFloat = new DevExpress.Utils.PointFloat(185F, 2.5F);
            this.ImporteDescripcionPart1.Multiline = true;
            this.ImporteDescripcionPart1.Name = "ImporteDescripcionPart1";
            this.ImporteDescripcionPart1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.ImporteDescripcionPart1.SizeF = new System.Drawing.SizeF(217.0001F, 20.00003F);
            this.ImporteDescripcionPart1.StylePriority.UseBorders = false;
            this.ImporteDescripcionPart1.StylePriority.UseFont = false;
            this.ImporteDescripcionPart1.Text = "ImporteDescripcionPart1";
            // 
            // xrLabel59
            // 
            this.xrLabel59.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel59.LocationFloat = new DevExpress.Utils.PointFloat(6.999715F, 0F);
            this.xrLabel59.Name = "xrLabel59";
            this.xrLabel59.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel59.SizeF = new System.Drawing.SizeF(178F, 23.95834F);
            this.xrLabel59.StylePriority.UseBorders = false;
            this.xrLabel59.StylePriority.UseFont = false;
            this.xrLabel59.Text = "Recibimos la suma de pesos";
            // 
            // ImporteDescripcionPart2
            // 
            this.ImporteDescripcionPart2.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ImporteDescripcionPart2.LocationFloat = new DevExpress.Utils.PointFloat(6.999918F, 23.95833F);
            this.ImporteDescripcionPart2.Multiline = true;
            this.ImporteDescripcionPart2.Name = "ImporteDescripcionPart2";
            this.ImporteDescripcionPart2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.ImporteDescripcionPart2.SizeF = new System.Drawing.SizeF(395F, 20F);
            this.ImporteDescripcionPart2.StylePriority.UseBorders = false;
            this.ImporteDescripcionPart2.StylePriority.UseFont = false;
            this.ImporteDescripcionPart2.Text = "ImporteDescripcionPart2";
            // 
            // xrLabel64
            // 
            this.xrLabel64.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel64.LocationFloat = new DevExpress.Utils.PointFloat(6.999918F, 43.95833F);
            this.xrLabel64.Name = "xrLabel64";
            this.xrLabel64.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel64.SizeF = new System.Drawing.SizeF(151F, 23.95832F);
            this.xrLabel64.StylePriority.UseBorders = false;
            this.xrLabel64.StylePriority.UseFont = false;
            this.xrLabel64.Text = "en concepto del periodo";
            // 
            // xrLabel40
            // 
            this.xrLabel40.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel40.LocationFloat = new DevExpress.Utils.PointFloat(7F, 105F);
            this.xrLabel40.Name = "xrLabel40";
            this.xrLabel40.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel40.SizeF = new System.Drawing.SizeF(152.9841F, 23.95828F);
            this.xrLabel40.StylePriority.UseFont = false;
            this.xrLabel40.Text = "s/liquidación al margen.";
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 23F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 12F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // formattingRule1
            // 
            this.formattingRule1.Condition = "[ap_Piso] == 0";
            this.formattingRule1.Name = "formattingRule1";
            // 
            // ConceptoPart1
            // 
            this.ConceptoPart1.Font = new System.Drawing.Font("Arial", 9.5F);
            this.ConceptoPart1.LocationFloat = new DevExpress.Utils.PointFloat(157.9999F, 43.95833F);
            this.ConceptoPart1.Name = "ConceptoPart1";
            this.ConceptoPart1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.ConceptoPart1.SizeF = new System.Drawing.SizeF(243.9999F, 23.95832F);
            this.ConceptoPart1.StylePriority.UseBorders = false;
            this.ConceptoPart1.StylePriority.UseFont = false;
            this.ConceptoPart1.Text = "ConceptoPart1";
            // 
            // subRepReciboFormasPago
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.FormattingRuleSheet.AddRange(new DevExpress.XtraReports.UI.FormattingRule[] {
            this.formattingRule1});
            this.Margins = new System.Drawing.Printing.Margins(50, 397, 23, 12);
            this.Version = "14.2";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.FormattingRule formattingRule1;
        internal DevExpress.XtraReports.UI.XRLabel ImporteDescripcionPart1;
        internal DevExpress.XtraReports.UI.XRLabel xrLabel59;
        internal DevExpress.XtraReports.UI.XRLabel ImporteDescripcionPart2;
        internal DevExpress.XtraReports.UI.XRLabel xrLabel64;
        internal DevExpress.XtraReports.UI.XRLabel xrLabel40;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell colFormaDePago;
        private DevExpress.XtraReports.UI.XRTableCell colBanco;
        private DevExpress.XtraReports.UI.XRTableCell colNroCheque;
        private DevExpress.XtraReports.UI.XRTableCell colImporte;
        private DevExpress.XtraReports.UI.XRTableCell colFecCheque;
        internal DevExpress.XtraReports.UI.XRLabel ConceptoPart1;
    }
}
