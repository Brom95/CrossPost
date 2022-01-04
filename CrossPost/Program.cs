using CrossPost;
using CrossPost.PluginsController;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<PluginFactory>();
    })
    .Build();

await host.RunAsync();
