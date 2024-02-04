using Core.Models;
using Interfaces;
using System.Text.Json;

namespace FileSystemAccess
{
    public class ConfigurationService : IConfigurationService
    {
        private const string SETTINGS_FILE_NAME = "settings.json";
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
                Directory.CreateDirectory(CommonValues.RootFolder);
            }

            return File.WriteAllTextAsync(SETTINGS_FILE_FULL_PATH, serializedSettings, cancellationToken);
        }
    }
}
