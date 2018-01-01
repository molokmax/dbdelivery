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

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext) {
            CommandModel config = new CommandModel();
            config.PluginType = "InitTestDatabase";
            config.Settings = new List<CommandSettingModel>() {
                new CommandSettingModel() {
                    Name = "ProviderName",
                    Value = "System.Data.SqlClient"
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
            ISettingStore settings = new SettingStore(config);
            IDataStore data = new DataStore();
            IPluginCommand cmd = new InitTestDatabaseCommand(settings, data);
            bool res = cmd.Execute();



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
                    Value = "System.Data.SqlClient"
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
            ISettingStore settings = new SettingStore(config);
            IDataStore data = new DataStore();
            IPluginCommand cmd = new BuildScriptCommand(settings, data);
            bool res = cmd.Execute();
            Assert.IsTrue(res);

            List<string> scripts = data.GetValue<List<string>>("ScriptsToApply");
            Assert.AreEqual(2, scripts.Count);
            
            Assert.IsTrue(scripts.Contains("script2.sql"));
            Assert.IsTrue(scripts.Contains("script3.sql"));

        }
    }
}
