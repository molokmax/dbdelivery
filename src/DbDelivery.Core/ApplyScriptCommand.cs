using DbDelivery.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDelivery.Core {

    /// <summary>
    /// Command executes script on the database
    /// </summary>
    public class ApplyScriptCommand : AbstructPluginCommand {

        public ApplyScriptCommand(ISettingStore settings, IDataStore data) : base(settings, data) {
            //PluginType = "ApplyScript";
        }

        public override bool Execute() {
            throw new NotImplementedException();
        }
    }
}
