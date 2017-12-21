using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDelivery.Plugin {

    public abstract class AbstructPluginCommand : IPluginCommand {

        protected readonly ISettingStore Settings;

        protected readonly IDataStore Data;

        public AbstructPluginCommand(ISettingStore settings, IDataStore data) {
            this.Settings = settings;
            this.Data = data;
            PluginNameFormatter pluginNameFormatter = new PluginNameFormatter();
            PluginType = pluginNameFormatter.GetPluginName(this.GetType());
        }

        public string PluginType { get; protected set; }

        public string Message { get; protected set; }

        public abstract bool Execute();
    }
}
