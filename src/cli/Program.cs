
using Core;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(TestOperation).Assembly);
});

using IHost host = builder.Build();
//IHost host = CreateHost();
ISender sender = host.Services.GetRequiredService<ISender>();

var result = await sender.Send(new TestOperation.TestRequest { Name = "Davide" });
Console.WriteLine(result);

static IHost CreateHost() =>
Host
.CreateDefaultBuilder()
.ConfigureServices((context, services) =>
{
    services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(typeof(TestOperation).Assembly);
    });
})

.Build();