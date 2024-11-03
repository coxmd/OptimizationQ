using Microsoft.EntityFrameworkCore;
using QueueManagementSystem.MVC.Data;
using QueueManagementSystem.MVC.Models.Configurations;
using System.Text.Json;

namespace QueueManagementSystem.MVC.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IDbContextFactory<QueueManagementSystemContext> _dbFactory;
        public const string QueueToRoomWithAvailableUsersOnly = "QueueToRoomWithAvailableUsersOnly";
        public const string IsPoolingEnabled = "IsPoolingEnabled";
        public const string FingerprintEnabled = "FingerprintEnabled";
        public const string FormEnabled = "FormEnabled";
        public const string RemovePatientsDuration = "RemovePatientsFromQueueAfterHrs";
        public const string RemovePatientsTime = "RemovePatientsFromQueueAtSpecificTime";
        public const string TicketCallRepetitions = "TicketCallRepetitions";
        public const string FormFieldConfigurationKey = "FormFieldConfiguration";

        public ConfigurationService(IDbContextFactory<QueueManagementSystemContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        private async Task<Configuration?> GetConfigurationAsync(string configName)
        {
            using var context = _dbFactory.CreateDbContext();
            return await context.Configurations
                .FirstOrDefaultAsync(c => c.ConfigurationName == configName);
        }

        public async Task<string> GetStringConfigurationAsync(string configName, string defaultValue = "")
        {
            var config = await GetConfigurationAsync(configName);
            return config?.StringValue ?? defaultValue;
        }

        public async Task<int> GetIntConfigurationAsync(string configName, int defaultValue = 0)
        {
            var config = await GetConfigurationAsync(configName);
            return config?.IntValue ?? defaultValue;
        }

        public async Task<bool> GetBoolConfigurationAsync(string configName, bool defaultValue = false)
        {
            var config = await GetConfigurationAsync(configName);
            return config?.BoolValue ?? defaultValue;
        }

        public async Task SaveConfigurationAsync(string configName, string value)
        {
            using var context = _dbFactory.CreateDbContext();
            var config = await context.Configurations
                .FirstOrDefaultAsync(c => c.ConfigurationName == configName);

            if (config == null)
            {
                config = new Configuration
                {
                    ConfigurationName = configName,
                    StringValue = value,
                    ValueType = Configuration.ConfigurationType.String
                };
                context.Configurations.Add(config);
            }
            else
            {
                config.StringValue = value;
                config.ValueType = Configuration.ConfigurationType.String;
            }

            await context.SaveChangesAsync();
        }

        public async Task SaveConfigurationAsync(string configName, int value)
        {
            using var context = _dbFactory.CreateDbContext();
            var config = await context.Configurations
                .FirstOrDefaultAsync(c => c.ConfigurationName == configName);

            if (config == null)
            {
                config = new Configuration
                {
                    ConfigurationName = configName,
                    IntValue = value,
                    ValueType = Configuration.ConfigurationType.Integer
                };
                context.Configurations.Add(config);
            }
            else
            {
                config.IntValue = value;
                config.ValueType = Configuration.ConfigurationType.Integer;
            }

            await context.SaveChangesAsync();
        }

        public async Task SaveConfigurationAsync(string configName, bool value)
        {
            using var context = _dbFactory.CreateDbContext();
            var config = await context.Configurations
                .FirstOrDefaultAsync(c => c.ConfigurationName == configName);

            if (config == null)
            {
                config = new Configuration
                {
                    ConfigurationName = configName,
                    BoolValue = value,
                    ValueType = Configuration.ConfigurationType.Boolean
                };
                context.Configurations.Add(config);
            }
            else
            {
                config.BoolValue = value;
                config.ValueType = Configuration.ConfigurationType.Boolean;
            }

            await context.SaveChangesAsync();
        }

        public async Task<FormFieldConfiguration> GetFormFieldConfigurationAsync()
        {
            using var context = _dbFactory.CreateDbContext();
            var config = await context.Configurations
                .FirstOrDefaultAsync(c => c.ConfigurationName == FormFieldConfigurationKey);

            if (config?.StringValue == null)
            {
                return new FormFieldConfiguration();
            }

            try
            {
                return JsonSerializer.Deserialize<FormFieldConfiguration>(config.StringValue)
                    ?? new FormFieldConfiguration();
            }
            catch
            {
                return new FormFieldConfiguration();
            }
        }

        public async Task SaveFormFieldConfigurationAsync(FormFieldConfiguration formConfig)
        {
            using var context = _dbFactory.CreateDbContext();
            var config = await context.Configurations
                .FirstOrDefaultAsync(c => c.ConfigurationName == FormFieldConfigurationKey);

            var jsonValue = JsonSerializer.Serialize(formConfig);

            if (config == null)
            {
                config = new Configuration
                {
                    ConfigurationName = FormFieldConfigurationKey,
                    StringValue = jsonValue,
                    ValueType = Configuration.ConfigurationType.String,
                    IntValue = null,
                    BoolValue = null
                };
                context.Configurations.Add(config);
            }
            else
            {
                config.StringValue = jsonValue;
                config.ValueType = Configuration.ConfigurationType.String;
                config.IntValue = null;
                config.BoolValue = null;
            }

            await context.SaveChangesAsync();
        }

        public async Task<bool> GetIndividualFormFieldSettingAsync(string fieldName, bool defaultValue = false)
        {
            var formConfig = await GetFormFieldConfigurationAsync();
            return fieldName switch
            {
                "ShowIdNumber" => formConfig.ShowIdNumber,
                "RequireIdNumber" => formConfig.RequireIdNumber,
                "ShowName" => formConfig.ShowName,
                "RequireName" => formConfig.RequireName,
                "ShowPhoneNumber" => formConfig.ShowPhoneNumber,
                "RequirePhoneNumber" => formConfig.RequirePhoneNumber,
                "ShowEmail" => formConfig.ShowEmail,
                "RequireEmail" => formConfig.RequireEmail,
                _ => defaultValue
            };
        }
    }
}
