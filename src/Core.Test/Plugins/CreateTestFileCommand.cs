using DbDelivery.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Test {
    public class CreateTestFileCommand : AbstractPluginCommand {

        public CreateTestFileCommand(ISettingStore settings, IDataStore data) : base(settings, data) {
        }

        public override bool Execute() {
            string filename = Settings.Get("FileName");
            string msg = Settings.Get("TestData");
            File.WriteAllText(filename, msg);
            return true;
        }
    }
}
