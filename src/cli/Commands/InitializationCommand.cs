using CommandLine;
using Handlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class InitializationCommand
    {
        [Verb("init", HelpText = "Initialize the ADR functionality")]
        public class InitOptions : IConsoleCommand
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

            public InitializationCommandHandler(ISender sender)
            {
                _sender = sender;
            }

            public int Execute(InitOptions initOptions)
            {

                if (!IsValid(initOptions))
                    return 0;
                Initialization.InitRequest settings = new Initialization.InitRequest();
                settings.Name = initOptions.ProjectName;
                settings.Template = initOptions.AdrTemplate;
                settings.AvailableStatuses = initOptions.AvailableStatuses.ToArray();


                _sender.Send(settings, CancellationToken.None).GetAwaiter().GetResult();


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
