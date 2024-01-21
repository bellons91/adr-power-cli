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

        private JsonSerializerOptions? jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
        };

        public async Task InitializeAsync(AdrSettings settings, CancellationToken cancellationToken)
        {
            string serializedSettings = JsonSerializer.Serialize(settings, options: jsonOptions);
            await File.WriteAllTextAsync(SETTINGS_FILE_NAME, serializedSettings, cancellationToken);
        }
    }
}
