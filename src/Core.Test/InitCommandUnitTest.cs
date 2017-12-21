using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DbDelivery.Plugin;
using DbDelivery.Core.Plugin;
using DbDelivery.Core.Config;
using DbDelivery.Core;

namespace Core.Test {
    /// <summary>
    /// Summary description for InitCommandUnitTest
    /// </summary>
    [TestClass]
    public class InitCommandUnitTest {
        public InitCommandUnitTest() {
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
        public void InitDatabase() {
            CommandModel config = new CommandModel();
            config.PluginType = "InitDatabase";
            config.Settings = new List<CommandSettingModel>() {
                new CommandSettingModel() {
                    Name = "ProviderName",
                    Value = "System.Data.SqlClient"
                },
                new CommandSettingModel() {
                    Name = "ConnectionString",
                    Value = "Data Source=192.168.10.33;Initial Catalog=DbDelivery_Test;Persist Security Info=True;User ID=userdb;Password=qwerty1"
                },
                new CommandSettingModel() {
                    Name = "TablePrefix",
                    Value = "TEST_" + DateTime.Now.ToString("yyyyMMddHHmmss")
                }
            };
            ISettingStore settings = new SettingStore(config);
            IPluginCommand cmd = new InitDatabaseCommand(settings, null);
            bool res = cmd.Execute();
            Assert.IsTrue(res);

        }
    }
}
