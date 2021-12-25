namespace Sender;
public interface ISender
{
    public Task Send(Message.Message message);
}
