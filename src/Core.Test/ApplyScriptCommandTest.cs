using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DbDelivery.Core.Config;
using System.Collections.Generic;
using DbDelivery.Core;
using DbDelivery.Plugin;
using DbDelivery.Core.Plugin;
using System.IO;
using System.Data.Common;

namespace Core.Test {
    [TestClass]
    public class ApplyScriptCommandTest {

        private const string ConnectionString = "Data Source=192.168.10.33;Initial Catalog=DbDelivery_Test;Persist Security Info=True;User ID=userdb;Password=qwerty1";
        //private const string ConnectionString = "Data Source=127.0.0.1,1401;Initial Catalog=DbDelivery_Test;Persist Security Info=True;User ID=sa;Password=Password123!#*";

        private const string ProviderName = "System.Data.SqlClient";

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext) {
            TestDbHelper.Clean(ProviderName, ConnectionString);
            TestDbHelper.Init(ProviderName, ConnectionString, "NO");
        }

        //[ClassCleanup()]
        //public static void MyClassCleanup() {
        //    TestDbHelper.Clean(ProviderName, ConnectionString);
        //}

        [TestMethod]
        public void ApplyScripts() {
            CommandModel config = new CommandModel();
            config.PluginType = "ApplyScript";
            config.Settings = new List<CommandSettingModel>() {
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
            List<string> scripts = new List<string>() {
                "src1/sql/script2.sql",
                "src1/sql/script3.sql"
            };
            data.SetValue<List<string>>("ScriptsToApply", scripts);
            IPluginCommand cmd = new ApplyScriptCommand(settings, data);
            bool res = cmd.Execute();



            Assert.IsTrue(res);

            using (DbConnection connection = TestDbHelper.GetConnection(ProviderName, ConnectionString)) {
                using (DbCommand sqlCmd = connection.CreateCommand()) {
                    sqlCmd.CommandText = "select count(*) from TEST_BUILD_DB_DELIVERY_HISTORY";
                    int count = (int) sqlCmd.ExecuteScalar();
                    Assert.AreEqual(2, count);
                }
                using (DbCommand sqlCmd = connection.CreateCommand()) {
                    sqlCmd.CommandText = "select count(*) from TEST_TABLE";
                    int count = (int) sqlCmd.ExecuteScalar();
                    Assert.AreEqual(2, count);
                }
            }

        }


    }
}
