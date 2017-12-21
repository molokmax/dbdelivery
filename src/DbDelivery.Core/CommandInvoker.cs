using DbDelivery.Core.Config;
using DbDelivery.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDelivery.Core {
    /// <summary>
    /// Build from config and execute commands
    /// </summary>
    public class CommandInvoker : ICommandInvoker {

        protected IPluginFactory PluginFactory;
        
        public CommandInvoker(IPluginFactory factory) {
            if (factory == null) {
                throw new ArgumentNullException("factory");
            }
            this.PluginFactory = factory;
        }

        /// <summary>
        /// Build commands and execute it
        /// </summary>
        /// <param name="config"></param>
        public void Invoke(EnvironmentModel config) {
            if (config == null) {
                throw new ArgumentNullException("config");
            }
            IEnumerable<IPluginCommand> commandChain = BuildChain(config);
            ExecuteChain(commandChain);
        }

        /// <summary>
        /// Build list of commands for executing
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        protected virtual IEnumerable<IPluginCommand> BuildChain(EnvironmentModel config) {
            IList<IPluginCommand> result = new List<IPluginCommand>();
            IDataStore data = new DataStore();
            foreach (CommandModel cmd in config.Commands) {
                ISettingStore settings = new SettingStore(cmd);
                IPluginCommand plugin = PluginFactory.CreateCommand(cmd.PluginType, settings, data);
                result.Add(plugin);
            }
            return result;
        }

        /// <summary>
        /// Execute commands
        /// </summary>
        /// <param name="chain"></param>
        protected virtual void ExecuteChain(IEnumerable<IPluginCommand> chain) {
            // TODO
            foreach (IPluginCommand cmd in chain) {
                bool result = cmd.Execute();
                if (!result) {
                    throw new ApplicationException(String.Format("Error has occurred in '{0}' command. {1}", cmd.PluginType, cmd.Message));
                    //break;
                }
            }
        }
    }
}
