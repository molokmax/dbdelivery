using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDelivery.Plugin {

    /// <summary>
    /// Interface of plugin command
    /// </summary>
    public interface IPluginCommand {

        /// <summary>
        /// Type of plugin
        /// </summary>
        string PluginType { get; }

        /// <summary>
        /// Error message
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Execute the command
        /// </summary>
        /// <returns></returns>
        bool Execute();
    }
}
