using QueueManagementSystem.MVC.Models;
using FastReport;
using FastReport.Web;

namespace QueueManagementSystem.MVC.Services
{
    public interface IReportService
    {
        Report GenerateTicketReport(Ticket ticket);
        WebReport GenerateWebReport(string reportType, string startDate, string endDate, byte[] qrCodeImage);

    }
}