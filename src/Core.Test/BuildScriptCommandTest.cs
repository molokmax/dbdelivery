using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DbDelivery.Core.Config;
using System.Collections.Generic;
using DbDelivery.Core;
using DbDelivery.Plugin;
using DbDelivery.Core.Plugin;
using System.IO;

namespace Core.Test {
    [TestClass]
    public class BuildScriptCommandTest {

        private const string ConnectionString = "Data Source=127.0.0.1,1401;Initial Catalog=DbDelivery_Test;Persist Security Info=True;User ID=sa;Password=Password123!#*";

        private const string ProviderName = "System.Data.SqlClient";

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext) {
            TestDbHelper.Init(ProviderName, ConnectionString);

        }

        [TestMethod]
        public void BuildScripts() {
            CommandModel config = new CommandModel();
            config.PluginType = "BuildScript";
            config.Settings = new List<CommandSettingModel>() {
                new CommandSettingModel() {
                    Name = "TempScriptDirectory",
                    Value = "src1/sql"
                },
                new CommandSettingModel() {
                    Name = "ProviderName",
                    Value = ProviderName
                },
                new CommandSettingModel() {
                    Name = "ConnectionString",
                    Value = ConnectionString
                },
                new CommandSettingModel() {
                    Name = "TablePrefix",
                    Value = "TEST_BUILD"
                }
            };
            ISettingStore settings = new SettingStore(config, null);
            IDataStore data = new DataStore();
            IPluginCommand cmd = new BuildScriptCommand(settings, data);
            bool res = cmd.Execute();
            Assert.IsTrue(res);

            List<string> scripts = data.GetValue<List<string>>("ScriptsToApply");
            Assert.AreEqual(2, scripts.Count);
            
            Assert.IsTrue(scripts[0].EndsWith("script2.sql"));
            Assert.IsTrue(scripts[1].EndsWith("script3.sql"));

        }
    }
}
