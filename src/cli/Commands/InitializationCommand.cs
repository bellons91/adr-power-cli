using CommandLine;
using Handlers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Commands;


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

    public class InitializationCommandHandler(ISender sender, ILoggerFactory loggerFactory) : ICommandHandler<InitOptions>
    {
        private readonly ISender _sender = sender;
        private readonly ILogger<InitializationCommandHandler> _logger = loggerFactory.CreateLogger<InitializationCommandHandler>();

        public async Task<int> Execute(InitOptions command)
        {
            _logger.LogWarning("Un bel warning");
            _logger.LogDebug("Un bel debug");
            if (!IsValid(command))
            {
                return 0;
            }

            var settings = new Initialization.InitRequest
            {
                Name = command.ProjectName,
                Template = command.AdrTemplate,
                AvailableStatuses = command.AvailableStatuses.ToArray()
            };

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

        private static bool IsValid(InitOptions initOptions) => !string.IsNullOrWhiteSpace(initOptions.ProjectName)
            && !string.IsNullOrWhiteSpace(initOptions.AdrTemplate);
    }

}
