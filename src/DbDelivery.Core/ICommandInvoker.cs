using DbDelivery.Core.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDelivery.Core {
    /// <summary>
    /// Interface of plugin-command executor
    /// </summary>
    public interface ICommandInvoker {
        /// <summary>
        /// build commands and execute it
        /// </summary>
        /// <param name="config"></param>
        void Invoke(EnvironmentModel config);
    }
}
