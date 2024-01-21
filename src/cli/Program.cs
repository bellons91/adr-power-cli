using CommandLine;
using Handlers;
using Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Channels;
using static Commands.InitializationCommand;
using static Handlers.Initialization;
using DependenciesCenter;



HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

if (args.Length == 0)
{
    Console.WriteLine("No command was specified.");
    return;
}

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Test).Assembly);
});

using IHost host = builder.Build();
ISender sender = host.Services.GetRequiredService<ISender>();


var ins = Parser.Default.ParseArguments<Commands.InitializationCommand.InitOptions, object>(args)
    .MapResult(
    (Commands.InitializationCommand.InitOptions co) =>
    {
        return new InitializationCommandHandler().Execute(co);
    },
    err => { Console.WriteLine("Not recognized!"); return 0; }
    );
;
Console.WriteLine(ins);

static IHost CreateHost() =>
Host
.CreateDefaultBuilder()
.ConfigureServices((context, services) =>
{
    services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(typeof(Test).Assembly);
    });
    services.AddCliDependencies();
})

.Build();


