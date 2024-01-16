using CommandLine;
using Handlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Channels;
using static Commands.InitializationCommand;
using static Handlers.Initialization;



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
//IHost host = CreateHost();
ISender sender = host.Services.GetRequiredService<ISender>();

Console.WriteLine(string.Concat(args.Select(s => s + ',')));

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
//var result = await sender.Send(new Test.TestRequest { Name = "Davide" });
//Console.WriteLine(result);

//await sender.Send(new Initialization.InitRequest());
//Console.ReadLine();


static IHost CreateHost() =>
Host
.CreateDefaultBuilder()
.ConfigureServices((context, services) =>
{
    services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(typeof(Test).Assembly);
    });
})

.Build();


