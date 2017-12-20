using DbDelivery.Plugin;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDelivery.Core.Plugin {

    public class CollectScriptsCommand : AbstructPluginCommand {

        public CollectScriptsCommand(ISettingStore settings, IDataStore data) : base(settings, data) {
            //PluginType = "CollectScripts";
        }

        public override bool Execute() {
            throw new NotImplementedException();
        }
    }
}
