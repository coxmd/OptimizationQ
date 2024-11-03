using FastReport;
using FastReport.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using QueueManagementSystem.MVC.Data;
using QueueManagementSystem.MVC.Services;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QueueManagementSystem.MVC.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IDbContextFactory<QueueManagementSystemContext> _dbFactory;

        public ReportsController(IReportService reportService, IWebHostEnvironment hostingEnvironment, IConfiguration configuration, IDbContextFactory<QueueManagementSystemContext> dbFactory)
        {
            _reportService = reportService;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _dbFactory = dbFactory;
        }

        public async Task<IActionResult> Index(string reportType, DateTime? startDate, DateTime? endDate)
        {
            var report = new WebReport();

            // Choose report file based on reportType
            string reportFileName = reportType switch
            {
                "AverageWaitingTimePerService" => "AverageWaitingTimePerService.frx",
                "AverageWaitingTimePerServicePoint" => "AverageWaitingTimePerServicePoint.frx",
                "AverageServiceTimePerService" => "AverageServiceTimePerService.frx",
                "AverageServiceTimePerServicePoint" => "AverageServiceTimePerServicePoint.frx",
                "AverageServiceTimePerServiceProvider" => "AverageServiceTimePerServiceProvider.frx",
                "CustomersServedPerService" => "CustomersServedPerService.frx",
                "CustomersServedPerServicePoint" => "CustomersServedPerServicePoint.frx",
                "QueueDetailedReport" => "QueueDetailedReport.frx",
                "ServiceProviderEfficiencyReport" => "ServiceProviderEfficiencyReport.frx",
                "CustomerReturnRateAnalysis" => "CustomerReturnRateAnalysis.frx",
                "ServiceTimeTrendAnalysis" => "ServiceTimeTrendAnalysis.frx",
                "ServicePointLoadDistribution" => "ServicePointLoadDistribution.frx",
                "DailyPerformanceMetrics" => "DailyPerformanceMetrics.frx",
                "ServicePointUtilization" => "ServicePointUtilization.frx",
                _ => "AverageWaitingTimePerService.frx" // Default report
            };

            if (!string.IsNullOrEmpty(reportType) && startDate.HasValue && endDate.HasValue)
            {

                string reportPath = Path.Combine(_hostingEnvironment.WebRootPath, "reports", reportFileName);
                report.Report.Load(reportPath);

                // Get the connection string from configuration
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                // Set up the connection for the report
                report.Report.Dictionary.Connections[0].ConnectionString = connectionString;

                // If date range is provided, pass it as parameters to the report
                if (startDate.HasValue && endDate.HasValue)
                {
                    report.Report.SetParameterValue("DateFrom", startDate.Value.ToString("yyyy-MM-dd"));
                    report.Report.SetParameterValue("DateTo", endDate.Value.ToString("yyyy-MM-dd"));
                    report.Report.SetParameterValue("Tz", "EAT");
                }

                // Fetch company information
                await using var context = await _dbFactory.CreateDbContextAsync();
                var companyInfo = await context.CompanyInformation.FirstOrDefaultAsync();
                if (companyInfo != null)
                {
                    // Set company information as report parameters
                    report.Report.SetParameterValue("CompanyName", companyInfo.CompanyName);
                    report.Report.SetParameterValue("PhysicalAddress", companyInfo.PhysicalAddress);
                    report.Report.SetParameterValue("ReportFooterMessage", companyInfo.ReportFooterMessage);
                    report.Report.SetParameterValue("Website", companyInfo.Website);

                    //// Remove base64 prefix if exists
                    //var logoBase64 = Regex.Replace(companyInfo.Logo ?? string.Empty, @"^data:image\/[a-zA-Z]+;base64,", "");
                    //if (!string.IsNullOrEmpty(logoBase64))
                    //{
                    //    byte[] logoBytes = Convert.FromBase64String(logoBase64);
                    //    using var ms = new MemoryStream(logoBytes);
                    //    var pic = report.Report.FindObject("Picture1") as PictureObject;
                    //    if (pic != null)
                    //    {
                    //        pic.Image = System.Drawing.Image.FromStream(ms);
                    //        pic.ShouldDisposeImage = true;
                    //    }
                    //}
                }

                ViewBag.WebReport = report;
                ViewBag.ShowReport = true;
            }
            else
            {
                ViewBag.ShowReport = false;
            }

            return View();
        }

    }
}
