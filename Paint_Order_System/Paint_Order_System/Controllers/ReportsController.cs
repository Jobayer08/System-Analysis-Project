using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;

namespace Paint_Order_System.Controllers
{
    public class ReportsController : Controller
    {
        private string connectionString = "Data Source=DESKTOP-I7TV81F;Initial Catalog=PaintIndustryDB;Integrated Security=True";

        private ReportDocument LoadReport(string reportName, DataTable data, string tableName)
        {
            ReportDocument rd = new ReportDocument();
            string path = Server.MapPath($"~/Reports/{reportName}.rpt");
            rd.Load(path);
            rd.SetDataSource(data);
            return rd;
        }

        private ActionResult ExportToPdf(ReportDocument report, string fileName = "Report.pdf")
        {
            Stream stream = report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", fileName);
        }

        public ActionResult CustomerReport()
        {
            var data = GetData("SELECT * FROM Customers");
            var report = LoadReport("CustomerReport", data, "Customers");
            return ExportToPdf(report, "CustomerReport.pdf");
        }

        public ActionResult ProductReport()
        {
            var data = GetData("SELECT * FROM Products");
            var report = LoadReport("ProductReport", data, "Products");
            return ExportToPdf(report, "ProductReport.pdf");
        }

        public ActionResult OrderReport()
        {
            var data = GetData("SELECT * FROM Orders");
            var report = LoadReport("OrderReport", data, "Orders");
            return ExportToPdf(report, "OrderReport.pdf");
        }

        public ActionResult OrderDetailReport()
        {
            var data = GetData("SELECT * FROM OrderDetails");
            var report = LoadReport("OrderDetailReport", data, "OrderDetails");
            return ExportToPdf(report, "OrderDetailReport.pdf");
        }

        // 🔽 STEP: Filter Form (GET)
        public ActionResult FilteredOrderReport()
        {
            return View();
        }

        // 🔽 STEP: Filtered Report (POST)
        [HttpPost]
        public ActionResult FilteredOrderReport(DateTime fromDate, DateTime toDate)
        {
            string query = "SELECT * FROM Orders WHERE OrderDate BETWEEN @fromDate AND @toDate";

            DataTable data = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(data);
                    }
                }
            }

            var report = LoadReport("OrderReport", data, "Orders");
            return ExportToPdf(report, "FilteredOrderReport.pdf");
        }


        private DataTable GetData(string query)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }
    }
}
