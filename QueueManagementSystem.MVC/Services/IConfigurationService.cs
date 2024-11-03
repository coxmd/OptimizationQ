using QueueManagementSystem.MVC.Models.Configurations;

namespace QueueManagementSystem.MVC.Services
{
    public interface IConfigurationService
    {
        Task<string> GetStringConfigurationAsync(string configName, string defaultValue = "");
        Task<int> GetIntConfigurationAsync(string configName, int defaultValue = 0);
        Task<bool> GetBoolConfigurationAsync(string configName, bool defaultValue = false);
        Task SaveConfigurationAsync(string configName, string value);
        Task SaveConfigurationAsync(string configName, int value);
        Task SaveConfigurationAsync(string configName, bool value);
        Task<FormFieldConfiguration> GetFormFieldConfigurationAsync();
        Task SaveFormFieldConfigurationAsync(FormFieldConfiguration config);
        Task<bool> GetIndividualFormFieldSettingAsync(string fieldName, bool defaultValue = false);
    }
}
