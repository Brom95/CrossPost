/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CrossPost.MessageSender
{
    internal class VKGroupOrUserWallMessageSender : MessageSender
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

        public override async Task Send(Message.Message message)
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
}
*/