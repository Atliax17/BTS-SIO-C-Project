using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionnaireStock.ReportForm
{
    public partial class StockReport : Form
    {
        ReportDocument crystal = new ReportDocument();
        public StockReport()
        {
            InitializeComponent();
        }
        DataSet dst = new DataSet();
        private void button1_Click(object sender, EventArgs e)
        {
            crystal.Load(@"C:\Users\Jean-baptiste\source\repos\GestionnaireStock\GestionnaireStock\Reports\Stock.rpt");
            SqlConnection conn = new SqlConnection("Data Source=192.168.18.144;Initial Catalog=Stock;User ID=sa;Password=abcd4ABCD");
            conn.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select * From [Stock] where Cast( TransDate as Date) between '" + dateTimePicker1.Value.ToString("dd/MM/yyyy") + "' and '" + dateTimePicker2.Value.ToString("dd/MM/yyyy") + "'", conn);
            sda.Fill(dst, "Stock");
            crystal.SetDataSource(dst);
            crystal.SetParameterValue("@FromDate", dateTimePicker1.Value.ToString("dd/MM/yyyy"));
            crystal.SetParameterValue("@ToDate", dateTimePicker2.Value.ToString("dd/MM/yyyy"));
            crystalReportViewer1.ReportSource = crystal;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ExportOptions exportOption;
            DiskFileDestinationOptions diskFileDestinationOptions = new DiskFileDestinationOptions();
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Pdf Files|*.pdf";
            //sfd.Filter = "Excel|*.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                diskFileDestinationOptions.DiskFileName = sfd.FileName;
            }
            exportOption = crystal.ExportOptions;
            {
                exportOption.ExportDestinationType = ExportDestinationType.DiskFile;
                exportOption.ExportFormatType = ExportFormatType.PortableDocFormat;
                //exportOption.ExportFormatType = ExportFormatType.Excel;
                exportOption.ExportDestinationOptions = diskFileDestinationOptions;
                exportOption.ExportFormatOptions = new PdfRtfWordFormatOptions();
                //exportOption.ExportFormatOptions = new ExcelFormatOptions();
            }
            crystal.Export();
        }
    }
}
