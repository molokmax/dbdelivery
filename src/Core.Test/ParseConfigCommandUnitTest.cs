using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DbDelivery.Plugin;
using DbDelivery.Core.Config;
using DbDelivery.Core;
using ParseConfigPlugin;

namespace Core.Test {
    /// <summary>
    /// Summary description for ParseConfigCommandUnitTest
    /// </summary>
    [TestClass]
    public class ParseConfigCommandUnitTest {
        
        public ParseConfigCommandUnitTest() {
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
        public void ParseConfig() {

            CommandModel config = new CommandModel();
            config.PluginType = "ParseConfig";
            config.Settings = new List<CommandSettingModel>() {
                new CommandSettingModel() {
                    Name = "ConfigPath",
                    Value = @"./infrastructure.config"
                },
                new CommandSettingModel() {
                    Name = "ConnectionStringName",
                    Value = "DatabaseConnection"
                }
            };
            IDataStore data = new DataStore();
            ISettingStore settings = new SettingStore(config, null);
            IPluginCommand cmd = new ParseConfigCommand(settings, data);
            bool res = cmd.Execute();
            Assert.IsTrue(res);
            Assert.AreEqual("Data Source=127.0.0.1;Initial Catalog=TestDB;Persist Security Info=True;User ID=user;Password=pass", data.GetValue("ConnectionString", ""));
            Assert.AreEqual("System.Data.SqlClient", data.GetValue("ProviderName", ""));
            
        }
    }
}
