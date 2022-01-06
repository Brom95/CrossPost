using Microsoft.Extensions.Configuration;

namespace Sender;
public interface ISender
{
    public Task Send(Message.Message message);
}
public interface ISenderFactory
{
    public ISender CreateSender(IConfigurationSection receiverSection, CancellationToken cancelToken);
}