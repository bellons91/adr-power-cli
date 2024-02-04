using Core.Models;

namespace Interfaces;

public interface IConfigurationService
{
    Task<bool> ConfigExists(CancellationToken cancellationToken);
    Task InitializeAsync(AdrSettings settings, CancellationToken cancellationToken);
}
