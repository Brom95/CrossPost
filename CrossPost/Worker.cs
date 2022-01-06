using CrossPost.PluginsController;

namespace CrossPost;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _config;
    private readonly PluginFactory _pluginFactory;
    public Worker(ILogger<Worker> logger, IConfiguration configuration, PluginFactory pluginFactory) => (_logger, _config, _pluginFactory) = (logger, configuration, pluginFactory);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        List<Task> tasks = new();

        foreach (IConfigurationSection receiverSection in _config.GetSection("Receivers").GetChildren())
        {
            _logger.LogInformation(receiverSection.GetValue<string>("Type"));
            var receiver = _pluginFactory.GetMessageReceiver(receiverSection, stoppingToken);

            foreach (IConfigurationSection senderSection in _config.GetSection("Senders").GetChildren())
            {
                _logger.LogInformation(senderSection.GetValue<string>("Type"));
                try
                {
                    var sender = _pluginFactory.GetMessageSender(senderSection, stoppingToken);
                    receiver?.Subscribe(sender);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"{ex}");
                }
            }
            tasks.Add(receiver?.StartReceiving(stoppingToken));
        }

        await Task.WhenAll(tasks.ToArray());

    }
}
