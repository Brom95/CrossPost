using Xunit;
using CrossPost;
using CrossPost.MessageReceiver;

using System.Threading.Tasks;
using System.Threading;

namespace CrossPostTests;
public class MessageReceiversTest
{
    [Fact]
    public void Test1()
    {
        var mr = new MessageReceiverMock();
        var ctc = new CancellationToken();
        mr.StartReceiving(ctc);
    }
}

public class MessageReceiverMock : MessageReceiverBase
{
    public override Task<Message> ReceiveMessage(CancellationToken cancelToken)
    {
        throw new System.NotImplementedException();
    }
}