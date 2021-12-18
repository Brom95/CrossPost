
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types.Enums;
using System.Runtime.CompilerServices;

namespace CrossPost.MessageReceiver;
public class TgChannelMessageReceiver : MessageReceiverBase
{
    private QueuedUpdateReceiver _updateReceiver;
    private ConfiguredCancelableAsyncEnumerable<Update>.Enumerator enumerator;
    public TgChannelMessageReceiver(string botToken, CancellationToken cancelToken)
    {
        var _bot = new TelegramBotClient(botToken);
        var receiverOptions = new ReceiverOptions
        {
            ThrowPendingUpdates = true,
            AllowedUpdates = new UpdateType[] { UpdateType.ChannelPost },
        };
        _updateReceiver = new QueuedUpdateReceiver(_bot, receiverOptions);
        enumerator = _updateReceiver.WithCancellation(cancelToken).GetAsyncEnumerator();
    }

    public override async Task<Message> ReceiveMessage()
    {
       
        await enumerator.MoveNextAsync();

        return new Message
        {
            Text = enumerator.Current.ChannelPost?.Text ?? ""
        };
    }
}

