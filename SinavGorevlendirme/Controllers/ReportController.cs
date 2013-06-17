using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SG_BLL;

namespace SinavGorevlendirme.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/
        [HttpPost]
        public ActionResult PrintReport(string reportType)
        {
            
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/rptOgretmenler.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource("DSOgretmenListe", TeacherManager.GetTeacherListForReport());
            
            localReport.DataSources.Add(reportDataSource);
            
            //string reportType = "Excel";
            string mimeType;
            string encoding;
            string fileNameExtension;
            //The DeviceInfo settings should be changed based on the reportType             
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx             
            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>PDF</OutputFormat>" +
                "  <PageWidth>8.5in</PageWidth>" +
                "  <PageHeight>11in</PageHeight>" +
                "  <MarginTop>0.5in</MarginTop>" +
                "  <MarginLeft>0.5in</MarginLeft>" +
                "  <MarginRight>0.5in</MarginRight>" +
                "  <MarginBottom>0.5in</MarginBottom>" +
                "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Render the report      
            
            renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            //Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension);             
            return File(renderedBytes, mimeType);
        }

        [HttpPost]
        public ActionResult PrintSinavGorevlendirme(string reportType, int SinavOturumId)
        {

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/rptSinavGorevlendirme.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource("DSSinavGorevlendirme", SinavManager.GetSinavGorevlendirmeForReport(SinavOturumId));

            localReport.DataSources.Add(reportDataSource);

            //string reportType = "Excel";
            string mimeType;
            string encoding;
            string fileNameExtension;
            //The DeviceInfo settings should be changed based on the reportType             
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx             
            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>PDF</OutputFormat>" +
                "  <PageWidth>8.5in</PageWidth>" +
                "  <PageHeight>11in</PageHeight>" +
                "  <MarginTop>0.5in</MarginTop>" +
                "  <MarginLeft>0.5in</MarginLeft>" +
                "  <MarginRight>0.5in</MarginRight>" +
                "  <MarginBottom>0.5in</MarginBottom>" +
                "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Render the report      

            renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            //Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension);             
            return File(renderedBytes, mimeType);
        }
    }
}
