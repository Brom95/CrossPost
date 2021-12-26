using Microsoft.Extensions.Configuration;

namespace Receiver;

public interface IReceiver
{
    public Task<Message.Message> ReceiveMessage();
}

public interface IReceiverFactory
{
    public IReceiver CreateReceiver(IConfigurationSection receiverSection, CancellationToken cancelToken);
}