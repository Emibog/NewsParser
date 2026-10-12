using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(service =>
    {
        service.AddHostedService<NewsWorker>();
    })
    .Build();

await host.RunAsync();