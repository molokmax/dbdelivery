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

        //private const string ConnectionString = "Data Source=192.168.10.33;Initial Catalog=DbDelivery_Test;Persist Security Info=True;User ID=userdb;Password=qwerty1";
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

        [TestMethod]
        public void BuildActualScripts() {
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
                },
                new CommandSettingModel() {
                    Name = "ActualScriptFileName",
                    Value = "log/NotAppliedScripts.txt"
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

            string actScriptPath = "src1/sql/log/NotAppliedScripts.txt";
            Assert.IsTrue(File.Exists(actScriptPath));

            //string expectedScript = GetExpectedScript();
            //string actScript = File.ReadAllText(actScriptPath);
            //Assert.AreEqual(expectedScript, actScript);

        }

        private string GetExpectedScript() {
            string expectedScript = @"-- Not Applied Scripts:
-- D:\develop\Workspace\dbdelivery\src\Core.Test\bin\Debug\src1\sql\script2.sql
-- D:\develop\Workspace\dbdelivery\src\Core.Test\bin\Debug\src1\sql\script3.sql




-- D:\develop\Workspace\dbdelivery\src\Core.Test\bin\Debug\src1\sql\script2.sql"
+ Environment.NewLine + Environment.NewLine +
@"CREATE TABLE TEST_TABLE ( 
	[COLUMN1] [NVARCHAR](200) NOT NULL, 
	[COLUMN2] [DATETIME] NOT NULL
)


-- D:\develop\Workspace\dbdelivery\src\Core.Test\bin\Debug\src1\sql\script3.sql"
+ Environment.NewLine + Environment.NewLine +
@"INSERT INTO TEST_TABLE ([COLUMN1], [COLUMN2]) VALUES ('test1', '2017-01-01 20:55');
INSERT INTO TEST_TABLE ([COLUMN1], [COLUMN2]) VALUES ('test2', '2017-01-02 21:33');
";
            return expectedScript;
        }
    }
}
