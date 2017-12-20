using DbDelivery.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDelivery.Core.Plugin {

    public class BuildScriptCommand : AbstructPluginCommand {

        public BuildScriptCommand(ISettingStore settings, IDataStore data) : base(settings, data) {
            //PluginType = "BuildScript";
        }

        public override bool Execute() {
            throw new NotImplementedException();
        }
    }
}
