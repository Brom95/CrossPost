using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sender;
using System.Text;
using System.Threading.Tasks;

namespace CrossPost.MessageSender;
static class SenderFactory
{
    static public MessageSender GetMessageSender(IConfigurationSection configurationSection)
    {
        var pluginName = configurationSection.GetValue<string>("Type");
        var asm = Assembly.LoadFrom($"Plugins/Senders/{pluginName}.dll");
        foreach (Type type in asm.GetTypes())
        {
            if (typeof(ISenderFactory).IsAssignableFrom(type))
            {
                ISenderFactory? factory = Activator.CreateInstance(type) as ISenderFactory;
                return new MessageSender( factory.CreateSender(configurationSection));
            }
        }
        return null;
    }
}

