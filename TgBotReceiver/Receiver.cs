using Microsoft.Extensions.Configuration;
using Receiver;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types.Enums;
using System.Runtime.CompilerServices;

namespace TgBotReceiver;


public class TgChannelMessageReceiver : Receiver.IReceiver
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

    public async Task<global::Message.Message> ReceiveMessage()
    {
        await enumerator.MoveNextAsync();

        return new global::Message.Message
        {
            Text = enumerator.Current.ChannelPost?.Text ?? ""
        };
    }
}



public class TgBotReceiverFactory : Receiver.IReceiverFactory
{
    public IReceiver CreateReceiver(IConfigurationSection receiverSection, CancellationToken cancelToken)
    {
        var token = receiverSection.GetValue<string>("TgBotToken");
        return new TgChannelMessageReceiver(token, cancelToken);
    }
}
