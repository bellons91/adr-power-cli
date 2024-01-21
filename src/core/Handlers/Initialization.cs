using Interfaces;
using MediatR;

namespace Handlers
{
    public class Initialization
    {

        public class InitRequest : IRequest
        {
            public string Name { get; set; }
            public string Template { get; set; }
            public string[] AvailableStatuses { get; set; }


        }

        public class InitializationHandler : IRequestHandler<InitRequest>
        {
            private readonly IConfigurationService _configService;

            public InitializationHandler(IConfigurationService configService)
            {
                _configService = configService;
            }

            public async Task Handle(InitRequest request, CancellationToken cancellationToken)
            {
                var settings = new AdrSettings();

                if (!settings.IsValid())
                {
                    throw new Exception("Adr settings are not valid");
                }

                await _configService.InitializeAsync(settings, cancellationToken);
            }
        }

        public class AdrSettings
        {
            public string Name { get; set; }
            public string Template { get; set; }
            public string[] AvailableStatus { get; set; }
            public bool IsValid()
            {
                return !string.IsNullOrEmpty(Name);
            }
        }
    }
}
