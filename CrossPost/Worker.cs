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

            foreach (IConfigurationSection receiverSection in _config.GetSection("Receivers").GetChildren())
            {
                _logger.LogInformation(receiverSection.GetValue<string>("Type"));
                var receiver = MessageReceiver.ReceiverFactory.GetMessageReceiver(receiverSection, stoppingToken);

                foreach (IConfigurationSection senderSection in _config.GetSection("Senders").GetChildren())
                {
                    var sender = MessageSender.SenderFactory.GetMessageSender(senderSection);
                    receiver?.Subscribe(sender);
                }
                tasks.Add(receiver?.StartReceiving(stoppingToken));
            }

            await Task.WhenAll(tasks.ToArray());

        }
    }
}