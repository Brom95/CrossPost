using CrossPost;
using CrossPost.PluginsController;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<PluginFactory>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
