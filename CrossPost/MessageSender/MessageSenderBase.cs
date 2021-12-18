using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossPost.MessageSender
{
    public abstract class MessageSenderBase : IObserver<Message>
    {
        public abstract Task Send(Message message);
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public async void OnNext(Message value)
        {
            await Send(value);
        }
    }
}
