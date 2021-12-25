

using Receiver;

namespace CrossPost.MessageReceiver;
public class MessageReceiver : IObservable<Message.Message>
{
    private List<IObserver<Message.Message>> _subscribers = new();
    public IReceiver Receiver { get; set; }
    public async Task StartReceiving(CancellationToken cancelToken)
    {
        while (!cancelToken.IsCancellationRequested)
        {
            var message = await Receiver.ReceiveMessage();
            foreach (var subscriber in _subscribers)
            {
                subscriber.OnNext(message);
            }

        }
    }
    public IDisposable Subscribe(IObserver<Message.Message> observer)
    {
        _subscribers.Add(observer);

        return new Unsubscriber(_subscribers, observer);
    }

    private class Unsubscriber : IDisposable
    {
        private List<IObserver<Message.Message>> _observers;
        private IObserver<Message.Message> _observer;

        public Unsubscriber(List<IObserver<Message.Message>> observers, IObserver<Message.Message> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observer != null && _observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}

