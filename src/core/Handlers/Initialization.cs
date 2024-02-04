using Core.Models;
using Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Handlers;

public class Initialization
{
    public class InitRequest : IRequest
    {
        public string Name { get; set; }
        public string Template { get; set; }
        public string[] AvailableStatuses { get; set; }
    }

    public class InitializationHandler(IConfigurationService configService, ILogger<InitializationHandler> logger) : IRequestHandler<InitRequest>
    {
        private readonly IConfigurationService _configService = configService;
        private readonly ILogger<InitializationHandler> _logger = logger;

        public async Task Handle(InitRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var settings = CreateFromRequest(request);

                _logger.LogDebug("Trying to initialize the repo with the following setting: {InitializationHandler}");

                if (!settings.IsValid())
                {
                    _logger.LogWarning("The current settings are not valid.");
                    throw new InvalidInitializationException("Adr settings are not valid");
                }

                _logger.LogDebug("Trying to check if the config file already exists.");

                var alreadyExists = await _configService.ConfigExists(cancellationToken);

                if (alreadyExists)
                {
                    _logger.LogDebug("Adr settings already exists");
                    throw new InvalidInitializationException("Adr settings already exists.");
                }

                _logger.LogDebug("Initializing ADRs.");
                await _configService.InitializeAsync(settings, cancellationToken);
            }
            catch (InvalidInitializationException)
            { throw; }
            catch (Exception ex)
            {

                throw new InvalidInitializationException(ex.Message, ex);
            }
        }

        private static AdrSettings CreateFromRequest(InitRequest request) => new AdrSettings
        {
            AvailableStatus = [.. (request.AvailableStatuses ?? [])],
            Name = request.Name,
            Template = request.Template,
        };
    }
}


[Serializable]
public class InvalidInitializationException : Exception
{
    public InvalidInitializationException(string message) : base(message) { }
    public InvalidInitializationException(string message, Exception innerException) : base(message, innerException) { }
    protected InvalidInitializationException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
