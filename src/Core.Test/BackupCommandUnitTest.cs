using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DbDelivery.Plugin;
using DbDelivery.Core.Plugin;
using DbDelivery.Core.Config;
using DbDelivery.Core;
using System.IO;
using System.Linq;
using BackupPlugin;

namespace Core.Test {
    /// <summary>
    /// Summary description for BackupCommandUnitTest
    /// </summary>
    [TestClass]
    public class BackupCommandUnitTest {

        private const string ConnectionString = "Data Source=192.168.10.33;Initial Catalog=DbDelivery_Test;Persist Security Info=True;User ID=userdb;Password=qwerty1";
        //private const string ConnectionString = "Data Source=127.0.0.1,1401;Initial Catalog=DbDelivery_Test;Persist Security Info=True;User ID=sa;Password=Password123!#*";

        private const string ProviderName = "System.Data.SqlClient";


        public BackupCommandUnitTest() {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CollectScripts() {

            //if (Directory.Exists("./backup")) {
            //    IEnumerable<string> oldFiles = Directory.GetFiles("./backup", "backup*.bak");
            //    foreach (string item in oldFiles) {
            //        File.Delete(item);
            //    }
            //}

            CommandModel config = new CommandModel();
            config.PluginType = "MakeBackup";
            config.Settings = new List<CommandSettingModel>() {
                new CommandSettingModel() {
                    Name = "BackupDirectory",
                    Value = @"D:\MSSQL\BACKUP\"
                },
                new CommandSettingModel() {
                    Name = "BackupFileName",
                    Value = "backup_#TIMESTAMP#.bak"
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
            IPluginCommand cmd = new MakeBackupCommand(settings, null);
            bool res = cmd.Execute();
            Assert.IsTrue(res);

            //IEnumerable<string> files = Directory.GetFiles("./backup", "backup*.bak");
            //Assert.IsTrue(files.Any());
            
        }
    }
}
