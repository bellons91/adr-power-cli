using System.Text.Json;
using Core.Models;
using Interfaces;
using Microsoft.Extensions.Logging;

namespace FileSystemAccess;

public class ConfigurationService(ILogger<ConfigurationService> logger) : IConfigurationService
{
    private const string SETTINGS_FILE_NAME = "settings.json";
    private readonly ILogger<ConfigurationService> _logger = logger;
    private readonly string SETTINGS_FILE_FULL_PATH = Path.Combine(CommonValues.RootFolder, SETTINGS_FILE_NAME);

    private readonly JsonSerializerOptions? jsonOptions = new()
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
        var serializedSettings = JsonSerializer.Serialize(settings, options: jsonOptions);

        if (!Directory.Exists(CommonValues.RootFolder))
        {
            _logger.LogInformation($"The root folder {CommonValues.RootFolder} does not exist in the current location. I'm creating it.");
            Directory.CreateDirectory(CommonValues.RootFolder);
        }

        _logger.LogInformation($"I'm creating the settings file under {SETTINGS_FILE_FULL_PATH}.");
        return File.WriteAllTextAsync(SETTINGS_FILE_FULL_PATH, serializedSettings, cancellationToken);
    }
}
