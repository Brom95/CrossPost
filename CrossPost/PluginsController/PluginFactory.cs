using Receiver;
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
        private string GetSendersPath(string pluginName) => Path.Join(new string[] { _sendersBasePath, pluginName, $"{pluginName}.dll" });

        private Receiver.IReceiverFactory? GetReceiverPlugin(string pluginName)
        {
            var asm = PluginLoadContext.LoadPlugin(GetReceiverPath(pluginName));
            foreach (Type type in asm.GetTypes())
            {
                if (typeof(IReceiverFactory).IsAssignableFrom(type))
                {
                    return Activator.CreateInstance(type) as IReceiverFactory;
                }
            }
            throw new Exception("Plugin not found");
        }

        public MessageReceiver.Receiver? GetMessageReceiver(IConfigurationSection configurationSection, CancellationToken cancellationToken)
        {
            var pluginName = configurationSection.GetValue<string>("Type");
            var factory = GetReceiverPlugin(pluginName);
            return new MessageReceiver.Receiver(receiver: factory.CreateReceiver(configurationSection, cancellationToken));
        }
    }
}
