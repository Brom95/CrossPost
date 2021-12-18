using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossPost.MessageSender
{
    public class ConsoleMessageSender : MessageSenderBase
    {
        public override async Task Send(Message message)
        {
            Console.WriteLine(message.Text);
        }
    }
}
