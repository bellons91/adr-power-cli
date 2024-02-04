using Core.Models;
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
                var settings = CreateFromRequest(request);

                if (!settings.IsValid())
                {
                    throw new InvalidInitializationException("Adr settings are not valid");
                }

                var alreadyExists = await _configService.ConfigExists(cancellationToken);

                if (alreadyExists)
                    throw new InvalidInitializationException("Adr settings already exists.");

                await _configService.InitializeAsync(settings, cancellationToken);
            }

            private static AdrSettings CreateFromRequest(InitRequest request)
            {
                return new AdrSettings
                {
                    AvailableStatus = (request.AvailableStatuses ?? new string[] { }).ToArray(),
                    Name = request.Name,
                    Template = request.Template,
                };
            }
        }
    }


    [Serializable]
    public class InvalidInitializationException : Exception
    {
        public InvalidInitializationException(string message) : base(message) { }
        protected InvalidInitializationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
