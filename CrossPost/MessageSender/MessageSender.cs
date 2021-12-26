using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossPost.MessageSender
{
    public class MessageSender : IObserver<Message.Message>
    {
        private Sender.ISender _sender;
        public MessageSender(Sender.ISender sender)
        {
            _sender = sender;
        }
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public async void OnNext(Message.Message value)
        {
            await _sender.Send(value);
        }
    }
}
