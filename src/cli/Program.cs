using CommandLine;
using Commands;
using DependenciesCenter;
using Handlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static Commands.InitializationCommand;

var operationResult = Parser.Default.ParseArguments<InitOptions, object>(args)
.MapResult(
(InitOptions co) =>
{
    if (args.Length == 0)
    {
        Console.WriteLine("No command was specified.");
    }

    using var host = BuildCurrentHost(args, co);
    var sender = host.Services.GetRequiredService<ISender>();
    var loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();

    return new InitializationCommandHandler(sender, loggerFactory).Execute(co).GetAwaiter().GetResult();
},
err =>
{
    Console.WriteLine("Not recognized!");
    return 0;
}
);

Console.WriteLine(operationResult);

IHost BuildCurrentHost<T>(string[] args, T currentCommand) where T : BaseConsoleCommand
{
    var builder = Host.CreateApplicationBuilder(args);

    if (currentCommand.Verbose)
    {
        builder.Logging.SetMinimumLevel(LogLevel.Debug);
    }

    builder.Services.AddCliDependencies();
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Test).Assembly));

    return builder.Build();
}
