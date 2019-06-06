using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace WSShamanFECAE_CSharp.report
{
    public partial class repCertificadoCaja : DevExpress.XtraReports.UI.XtraReport
    {
        public repCertificadoCaja()
        {
            InitializeComponent();
        }

        private void XrLabel1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
    }
}
