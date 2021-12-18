

namespace CrossPost.MessageReceiver;
public abstract class MessageReceiverBase : IObservable<Message>
{
    private List<IObserver<Message>> _subscribers = new();
    public abstract Task<Message> ReceiveMessage();
    public async Task StartReceiving(CancellationToken cancelToken)
    {
        while (!cancelToken.IsCancellationRequested)
        {
            var message = await ReceiveMessage();
            foreach (var subscriber in _subscribers)
            {
                subscriber.OnNext(message);
            }

        }
    }
    public IDisposable Subscribe(IObserver<Message> observer)
    {
        _subscribers.Add(observer);

        return new Unsubscriber(_subscribers, observer);
    }

    private class Unsubscriber : IDisposable
    {
        private List<IObserver<Message>> _observers;
        private IObserver<Message> _observer;

        public Unsubscriber(List<IObserver<Message>> observers, IObserver<Message> observer)
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

