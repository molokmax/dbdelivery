using DbDelivery.Core.Config;
using DbDelivery.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DbDelivery.Core {

    /// <summary>
    /// Implementation of startup point for database delivery
    /// </summary>
    public class DbDeliveryEngine : IDbDeliveryEngine {

        private readonly ConfigManager ConfigManager;

        private readonly IPluginFactory PluginFactory;
        private readonly ICommandInvoker CommandInvoker;

        public DbDeliveryEngine(IPluginFactory factory, ICommandInvoker invoker) {
            this.ConfigManager = new ConfigManager();
            this.PluginFactory = factory;
            this.CommandInvoker = invoker;
        }

        /// <summary>
        /// Initialize db delivery engine
        /// Register plugins
        /// </summary>
        public void Init() {
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // looking for plugin directory and loading plugin command classes
            string pluginFolder = this.ConfigManager.GetSettingByName("PluginFolder", "Plugins");
            if (Directory.Exists(pluginFolder)) {
                IEnumerable<string> pluginFiles = Directory.EnumerateFiles(pluginFolder, "*.dll", SearchOption.TopDirectoryOnly);
                foreach (string item in pluginFiles) {
                    FileInfo file = new FileInfo(item);
                    Assembly.LoadFile(file.FullName);
                }
            }
            // get all IPluginCommand implementations and register them
            Type pluginCommandInterfaceType = typeof(IPluginCommand);
            IEnumerable<Type> plugins = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(asm => asm
                    .GetTypes()
                    .Where(t => !t.IsInterface && !t.IsAbstract && pluginCommandInterfaceType.IsAssignableFrom(t)));
            foreach (Type pluginType in plugins) {
                this.PluginFactory.RegisterCommand(pluginType);
            }
        }

        /// <summary>
        /// Make migration for the application in the environment
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="environmentName"></param>
        public void Migrate(string applicationName, string environmentName) {
            ConfigModel config = this.ConfigManager.Load();
            EnvironmentModel envModel = this.ConfigManager.GetEnvironmentConfig(config, applicationName, environmentName);
            this.CommandInvoker.Invoke(envModel);
        }
    }
}
