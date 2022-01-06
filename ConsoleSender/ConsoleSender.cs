using Microsoft.Extensions.Configuration;
using Sender;

namespace ConsoleSender;
public class ConsoleSender : ISender
{
    public async Task Send(Message.Message message)
    {
        Console.WriteLine(message.Text);
    }
}

public class ConsoleSenderFactory : ISenderFactory
{

    public ISender CreateSender(IConfigurationSection receiverSection, CancellationToken cancelToken)
    {
        return new ConsoleSender();
    }
}