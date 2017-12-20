using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DbDelivery.Core.Config;
using System.IO;

namespace Core.Test {
    /// <summary>
    /// Summary description for ConfigUnitTest
    /// </summary>
    [TestClass]
    public class ConfigUnitTest {
        public ConfigUnitTest() {
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
        public void CreateTestConfigFile() {

            ConfigModel config = new ConfigModel();
            config.Applications = new List<ApplicationModel>();

            ApplicationModel app = new ApplicationModel();
            app.Name = "DbDelivery";
            app.Environments = new List<EnvironmentModel>();

            EnvironmentModel env = new EnvironmentModel();
            env.Name = "test";
            env.Commands = new List<CommandModel>();

            CommandModel cmd = new CommandModel();
            cmd.PluginType = "InitDatabase";
            cmd.Settings = new List<CommandSettingModel>();

            CommandSettingModel connString = new CommandSettingModel();
            connString.Name = "ConnectionString";
            connString.Value = "Data Source=192.168.10.33;Initial Catalog=DbDelivery_Test;Persist Security Info=True;User ID=userdb;Password=qwerty1";
            cmd.Settings.Add(connString);

            CommandSettingModel providerName = new CommandSettingModel();
            providerName.Name = "ProviderName";
            providerName.Value = "System.Data.SqlClient";
            cmd.Settings.Add(providerName);

            env.Commands.Add(cmd);
            app.Environments.Add(env);
            config.Applications.Add(app);

            ConfigManager configManager = new ConfigManager();
            configManager.Save(config);

            Assert.IsTrue(File.Exists("config.xml"));

        }
    }
}
