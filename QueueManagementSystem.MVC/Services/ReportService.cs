using QueueManagementSystem.MVC.Models;
using FastReport;
using FastReport.Web;
using FastReport.Utils;
using FastReport.Data;
using System.Drawing;

namespace QueueManagementSystem.MVC.Services
{
    public class ReportService : IReportService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportService(IWebHostEnvironment hostingEnvironment, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            RegisteredObjects.AddConnection(typeof(PostgresDataConnection));
            _httpContextAccessor = httpContextAccessor;
        }
        public Report GenerateTicketReport(Ticket ticket)
        {   
            //TODO: catch exceptions for null or invalid tickets
            Report report = new Report();
            string reportPath = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "Ticket.frx");
            report.Load(reportPath);

            // Get the current request
            var request = _httpContextAccessor.HttpContext.Request;

            // Build the complete URL
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var fullUrl = $"{baseUrl}/Queue/TicketTrackingPage/{ticket.TicketNumber}";

            report.SetParameterValue("TicketNo", ticket.TicketNumber);
            report.SetParameterValue("serviceID", ticket.ServiceName);
            report.SetParameterValue("printTime", ticket.PrintTime);
            report.SetParameterValue("URL", fullUrl);

            report.Prepare();

            return report;
        }

        public WebReport GenerateWebReport(string reportType, string startDate, string endDate, byte[] qrCodeImage)
        {
            var report = new WebReport();

            // Load the appropriate report template based on report type
            var reportPath = reportType switch
            {
                "AverageWaitingTimePerService" => Path.Combine(_hostingEnvironment.WebRootPath, "reports", "AverageWaitingTimePerService.frx"),
                "AverageWaitingTimePerServicePoint" => Path.Combine(_hostingEnvironment.WebRootPath, "reports", "AverageWaitingTimePerServicePoint.frx"),
                "AverageServiceTimePerService" => Path.Combine(_hostingEnvironment.WebRootPath, "reports", "AverageServiceTimePerService.frx"),
                "AverageServiceTimePerServicePoint" => Path.Combine(_hostingEnvironment.WebRootPath, "reports", "AverageServiceTimePerServicePoint.frx"),
                "CustomersServed" => Path.Combine(_hostingEnvironment.WebRootPath, "reports", "CustomersServed.frx"),
                _ => throw new Exception("Invalid report type")
            };

            report.Report.Load(reportPath);

            // Set up the PostgreSQL connection
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Create a new PostgresDataConnection for FastReport
            var pgConnection = new PostgresDataConnection();
            //pgConnection.ConnectionString = connectionString;

            //// Register the connection in the report
            //report.Report.Dictionary.Connections.Add(pgConnection);


            // Ensure the PostgreSQL connection is registered and available
            report.Report.Dictionary.Connections.Clear();
            //report.Report.Dictionary.RegisterData(serviceStats, "serviceStats", true);

            DataBand db1 = (DataBand)report.Report.FindObject("Data1");

            report.Report.SetParameterValue("URL", "Queue/TicketTrackingPage");
            report.Report.SetParameterValue("StartDate", startDate);
            report.Report.SetParameterValue("EndDate", endDate);

            report.Report.PrepareAsync();

            return report;
        }
    }
}
