using DbDelivery.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDelivery.Core {

    /// <summary>
    /// Interface of Factory for creating plugin command objects
    /// </summary>
    public interface IPluginFactory {

        /// <summary>
        /// Create plugin-command instance for given type
        /// </summary>
        /// <param name="pluginName"></param>
        /// <returns></returns>
        IPluginCommand CreateCommand(string pluginName, ISettingStore settings, IDataStore data);

        /// <summary>
        /// Register plugin-command type
        /// </summary>
        /// <param name="commandType"></param>
        void RegisterCommand(Type commandType);

        /// <summary>
        /// List of registered plugin-commands
        /// </summary>
        IEnumerable<string> AvailablePlugins { get; }
    }
}
