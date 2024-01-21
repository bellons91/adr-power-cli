using Core.Models;
using Handlers;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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


        public async Task InitializeAsync(AdrSettings settings, CancellationToken cancellationToken)
        {
            string serializedSettings = JsonSerializer.Serialize(settings, options: jsonOptions);

            if (!Directory.Exists(CommonValues.RootFolder))
            {
                Directory.CreateDirectory(CommonValues.RootFolder);
            }

            await File.WriteAllTextAsync(SETTINGS_FILE_FULL_PATH, serializedSettings, cancellationToken);
        }
    }
}
