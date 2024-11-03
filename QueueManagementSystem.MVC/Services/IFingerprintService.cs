using QueueManagementSystem.MVC.Models;

namespace QueueManagementSystem.MVC.Services
{
    public interface IFingerprintService
    {
        Task<bool> InitializeAsync();
        Task<bool> OpenDeviceAsync();
        Task<FingerprintResult> EnrollAsync();
        Task<FingerprintResult> SearchIdnoAsync(CustomerInfo customer);
        Task<FingerprintResult> ContinuousScanAsync(CustomerInfo customer);
        Task<FingerprintResult> MatchFingerPrintAsync(CustomerInfo customer);
        Task<FingerprintResult> AuthenticateAsync();
        Task CloseDeviceAsync();
        Task<int> GetDeviceCountAsync();
    }
}
