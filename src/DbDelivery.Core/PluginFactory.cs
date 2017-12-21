using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbDelivery.Plugin;

namespace DbDelivery.Core {

    /// <summary>
    /// Factory for creating plugin command objects
    /// </summary>
    public class PluginFactory : IPluginFactory {

        private IDictionary<string, Type> CommandTypes = new Dictionary<string, Type>();
        private readonly PluginNameFormatter PluginNameFormatter = new PluginNameFormatter();

        public PluginFactory() {
            //PluginNameFormatter = new PluginNameFormatter();
        }

        /// <summary>
        /// Create plugin-command instance for given type
        /// </summary>
        /// <param name="pluginName"></param>
        /// <returns></returns>
        public virtual IPluginCommand CreateCommand(string pluginName, ISettingStore settings, IDataStore data) {
            if (String.IsNullOrEmpty(pluginName)) {
                throw new ArgumentNullException("pluginName");
            }
            Type pluginType;
            if (CommandTypes.TryGetValue(pluginName, out pluginType)) {
                return (IPluginCommand) Activator.CreateInstance(pluginType, settings, data);
            } else {
                throw new KeyNotFoundException(String.Format("Plugin '{0}' is not registered", pluginName));
            }
        }

        

        /// <summary>
        /// Register plugin-command type
        /// </summary>
        /// <param name="commandType"></param>
        public virtual void RegisterCommand(Type commandType) {
            Type basePluginType = typeof(IPluginCommand);
            if (!basePluginType.IsAssignableFrom(commandType)) {
                throw new Exception(String.Format("{0} is not IPluginCommand", commandType.Name));
            } else {
                string pluginName = PluginNameFormatter.GetPluginName(commandType);
                CommandTypes[pluginName] = commandType;
            }
        }

        public IEnumerable<string> AvailablePlugins { get {
                return CommandTypes.Keys;
            }
        }
    }
}
