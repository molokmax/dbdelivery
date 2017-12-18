using DbDelivery.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Test {
    public class PluginBadName : AbstructPluginCommand {

        public PluginBadName(ISettingStore settings, IDataStore data) : base(settings, data) {
        }

        public override bool Execute() {
            throw new NotImplementedException();
        }
    }
}
