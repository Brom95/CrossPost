using Receiver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CrossPost.MessageReceiver;
static class ReceiverFactory
{
    public static MessageReceiver? GetMessageReceiver(IConfigurationSection configurationSection, CancellationToken cancellationToken)
    {
        var pluginName = configurationSection.GetValue<string>("Type");
        var asm = Assembly.LoadFrom($"Plugins/Receivers/{pluginName}.dll");
        foreach (Type type in asm.GetTypes())
        {
            if (typeof(IReceiverFactory).IsAssignableFrom(type))
            {
                IReceiverFactory? factory = Activator.CreateInstance(type) as IReceiverFactory;
                return new MessageReceiver(receiver: factory.CreateReceiver(configurationSection, cancellationToken));
            }
        }
        return null;

    }
}
