
using Core;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host = CreateHost();
ISender sender = host.Services.GetRequiredService<ISender>();

var result = await sender.Send(new TestOperation.TestRequest { Name = "Davide" });
Console.WriteLine(result);

static IHost CreateHost() =>
Host
.CreateDefaultBuilder()
.ConfigureServices((context, services) =>
{
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<TestOperation>());
})

.Build();