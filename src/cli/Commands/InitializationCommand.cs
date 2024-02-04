using CommandLine;
using Handlers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Commands
{

    public class InitializationCommand
    {
        [Verb("init", HelpText = "Initialize the ADR functionality")]
        public class InitOptions : BaseConsoleCommand
        {
            [Option('n', "name", Default = "ADR", HelpText = "ADR project name")]
            public string ProjectName { get; set; }

            [Option("availableStatus", Default = new string[] { "draft", "proposed", "open", "accepted", "rejected", "deprecated", "superseded" }, HelpText = "List of statuses to be considered.")]
            public IEnumerable<string> AvailableStatuses { get; set; }

            [Option('t', "template", Default = "basic", HelpText = "ADR Template")]
            public string AdrTemplate { get; set; }

        }

        public class InitializationCommandHandler : ICommandHandler<InitOptions>
        {
            private readonly ISender _sender;
            private readonly ILogger<InitializationCommandHandler> _logger;

            public InitializationCommandHandler(ISender sender, ILoggerFactory loggerFactory)
            {
                _sender = sender;
                _logger = loggerFactory.CreateLogger<InitializationCommandHandler>();
            }

            public async Task<int> Execute(InitOptions initOptions)
            {
                _logger.LogWarning("Un bel warning");
                _logger.LogDebug("Un bel debug");
                if (!IsValid(initOptions))
                    return 0;
                Initialization.InitRequest settings = new Initialization.InitRequest();
                settings.Name = initOptions.ProjectName;
                settings.Template = initOptions.AdrTemplate;
                settings.AvailableStatuses = initOptions.AvailableStatuses.ToArray();

                try
                {

                    await _sender.Send(settings, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                    return 0;
                }

                return 1;
            }

            private bool IsValid(InitOptions initOptions)
            {
                return !string.IsNullOrWhiteSpace(initOptions.ProjectName)
                && !string.IsNullOrWhiteSpace(initOptions.AdrTemplate);
            }
        }

    }
}
