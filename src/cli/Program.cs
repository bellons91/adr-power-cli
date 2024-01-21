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



internal class Program
{
    private static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        if (args.Length == 0)
        {
            Console.WriteLine("No command was specified.");
            return;
        }

        builder.Services.AddCliDependencies();
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Test).Assembly);
        });


        using IHost host = builder.Build();
        ISender sender = host.Services.GetRequiredService<ISender>();

        var ins = Parser.Default.ParseArguments<InitOptions, object>(args)
            .MapResult(
            (InitOptions co) =>
            {
                return new InitializationCommandHandler(sender).Execute(co);
            },
            err => { Console.WriteLine("Not recognized!"); return 0; }
            );
        Console.WriteLine(ins);

    }

}