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
                var sender = new MessageSender.ConsoleMessageSender();
                receiver.Subscribe(sender);
                foreach (IConfigurationSection senderSection in _config.GetSection("Senders").GetChildren())
                {
                    if (senderSection.GetValue<string>("Type") == "VkGroupOrPage")
                    {
                        MessageSender.VKGroupOrUserWallMessageSender realSender = new(
                            senderSection.GetValue<string>("access_token"), 
                            senderSection.GetValue<string>("groupOrUserID"), 
                            stoppingToken);
                        receiver.Subscribe(realSender);
                    }

                }
                tasks.Add(receiver.StartReceiving(stoppingToken));
            }

            await Task.WhenAll(tasks.ToArray());

        }
    }
}