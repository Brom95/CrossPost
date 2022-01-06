using Microsoft.Extensions.Configuration;
using Sender;
using System.Net;
using System.Text;
namespace VkGroupOrPage;

public class VKGroupOrUserWallMessageSender : ISender
{
    private readonly string _accessToken;
    private readonly string _userId;
    private readonly string _url = "https://api.vk.com/method/wall.post?v=5.131&from_group=1";
    private readonly HttpClient client = new();
    private readonly CancellationToken _ctc;
    public VKGroupOrUserWallMessageSender(string accessToken, string userId, CancellationToken ctc)
    {
        (_accessToken, _userId, _ctc) = (accessToken, userId, ctc);

    }

    public async Task Send(Message.Message message)
    {
        var requestBuilder = new StringBuilder(_url);
        requestBuilder.Append($"&message={message.Text}");
        requestBuilder.Append($"&access_token={_accessToken}");
        requestBuilder.Append($"&owner_id={_userId}");
        var response = await client.GetAsync(requestBuilder.ToString(), _ctc);
        if (response.StatusCode != HttpStatusCode.OK)
            Console.WriteLine(await response.Content.ReadAsStringAsync());
    }
}

public class VKSenderFactory : ISenderFactory
{
    public ISender CreateSender(IConfigurationSection receiverSection, CancellationToken cancelToken)
    {
        var accessToken = receiverSection.GetValue<string>("access_token");
        var userId = receiverSection.GetValue<string>("groupOrUserID");
        return new VKGroupOrUserWallMessageSender(accessToken, userId, cancelToken);
    }
}
