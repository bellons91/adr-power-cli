using CommandLine;
using Commands;
using DependenciesCenter;
using Handlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static Commands.InitializationCommand;

internal class Program
{
    private static void Main(string[] args)
    {
        var operationResult = Parser.Default.ParseArguments<InitOptions, object>(args)
            .MapResult(
            (InitOptions co) =>
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("No command was specified.");
                }

                using IHost host = BuildCurrentHost(args, co);
                ISender sender = host.Services.GetRequiredService<ISender>();
                ILoggerFactory loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();

                return new InitializationCommandHandler(sender, loggerFactory).Execute(co).GetAwaiter().GetResult();
            },
            err =>
            {
                Console.WriteLine("Not recognized!");
                return 0;
            }
            );

        Console.WriteLine(operationResult);
    }

    private static IHost BuildCurrentHost<T>(string[] args, T currentCommand) where T : BaseConsoleCommand
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        if (currentCommand.Verbose)
            builder.Logging.SetMinimumLevel(LogLevel.Debug);

        builder.Services.AddCliDependencies();
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Test).Assembly);
        });

        return builder.Build();
    }


}