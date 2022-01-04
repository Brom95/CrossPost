using Receiver;
using Sender;
namespace CrossPost.PluginsController
{
    public class PluginFactory
    {
        
        private string _receiverBasePath;
        private string _sendersBasePath;
        public PluginFactory(IConfiguration config)
        {
            var _pluginsBasePath = config.GetValue<string>("PluginsPath");
            _receiverBasePath = Path.Join(_pluginsBasePath, "Receivers");
            _sendersBasePath = Path.Join(_pluginsBasePath, "Senders");
        }
        private string GetReceiverPath(string pluginName) => Path.Join(new string[] { _receiverBasePath, pluginName, $"{pluginName}.dll" });
        private string GetSenderPath(string pluginName) => Path.Join(new string[] { _sendersBasePath, pluginName, $"{pluginName}.dll" });
        private T GetPlugin<T>(string pluginPath) where T : class
        {
            var asm = PluginLoadContext.LoadPlugin(GetReceiverPath(pluginPath));
            foreach (Type type in asm.GetTypes())
            {
                if (typeof(IReceiverFactory).IsAssignableFrom(type))
                {
                    return Activator.CreateInstance(type) as T;
                }
            }
            throw new Exception("Plugin not found");
        }

        public MessageReceiver.Receiver? GetMessageReceiver(IConfigurationSection configurationSection, CancellationToken cancellationToken)
        {
            var pluginName = configurationSection.GetValue<string>("Type");
            var factory = GetPlugin<IReceiverFactory>(GetReceiverPath(pluginName));
            return new MessageReceiver.Receiver(receiver: factory.CreateReceiver(configurationSection, cancellationToken));
        }
        public MessageSender.MessageSender GetMessageSender(IConfigurationSection configurationSection)
        {
            var pluginName = configurationSection.GetValue<string>("Type");
            var factory = GetPlugin<ISenderFactory>(GetSenderPath(pluginName));
            return new MessageSender.MessageSender(factory.CreateSender(configurationSection));
        }
    }
}
