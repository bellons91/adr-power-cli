using Core.Models;
using Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FileSystemAccess
{
    public class ConfigurationService : IConfigurationService
    {
        public ConfigurationService(ILogger<ConfigurationService> logger)
        {
            _logger = logger;
        }

        private const string SETTINGS_FILE_NAME = "settings.json";
        private readonly ILogger<ConfigurationService> _logger;
        private string SETTINGS_FILE_FULL_PATH = Path.Combine(CommonValues.RootFolder, SETTINGS_FILE_NAME);

        private JsonSerializerOptions? jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
        };

        public Task<bool> ConfigExists(CancellationToken cancellationToken)
        {
            var fileSettingsExists = File.Exists(SETTINGS_FILE_FULL_PATH);
            return Task.FromResult(fileSettingsExists);
        }

        public Task InitializeAsync(AdrSettings settings, CancellationToken cancellationToken)
        {
            string serializedSettings = JsonSerializer.Serialize(settings, options: jsonOptions);

            if (!Directory.Exists(CommonValues.RootFolder))
            {
                _logger.LogInformation($"The root folder {CommonValues.RootFolder} does not exist in the current location. I'm creating it.");
                Directory.CreateDirectory(CommonValues.RootFolder);
            }

            _logger.LogInformation($"I'm creating the settings file under {SETTINGS_FILE_FULL_PATH}.");
            return File.WriteAllTextAsync(SETTINGS_FILE_FULL_PATH, serializedSettings, cancellationToken);
        }
    }
}
