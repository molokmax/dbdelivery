using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDelivery.Plugin {
    /// <summary>
    /// Builder of plugin names
    /// </summary>
    public class PluginNameFormatter {
        /// <summary>
        /// Get plugin name for the command
        /// </summary>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public string GetPluginName(Type commandType) {
            int commandSuffixIndex = commandType.Name.LastIndexOf("Command");
            int pluginNameLength = commandSuffixIndex < 0 ? commandType.Name.Length : commandSuffixIndex;
            return commandType.Name.Substring(0, pluginNameLength);
        }
    }
}
