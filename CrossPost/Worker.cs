namespace CrossPost
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;
        public Worker(ILogger<Worker> logger, IConfiguration config) => (_logger, _config) = (logger, config);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            List<Task> tasks = new();

            foreach (IConfigurationSection section in _config.GetSection("Receivers").GetChildren())
            {
                _logger.LogInformation(section.GetValue<string>("Type"));
                var receiver = new MessageReceiver.TgChannelMessageReceiver(section.GetValue<string>("TgBotToken"), stoppingToken);
                var sender = new MessageSender.ConsoleMessageSender();
                receiver.Subscribe(sender);
                tasks.Add(receiver.StartReceiving(stoppingToken));
            }

            await Task.WhenAll(tasks.ToArray());

        }
    }
}