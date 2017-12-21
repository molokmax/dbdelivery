using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DbDelivery.Core.Config;
using System.IO;
using System.Linq;

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

            try {
                ConfigModel config = GetTestConfigModel();

                ConfigManager configManager = new ConfigManager();
                configManager.Save(config);

                Assert.IsTrue(File.Exists("config.xml"));
            } finally {
                if (File.Exists("config.xml")) {
                    File.Delete("config.xml");
                }
            }
        }


        [TestMethod]
        public void LoadTestConfigFile() {
            try {
                MakeTestConfigFile();

                ConfigManager configManager = new ConfigManager();
                ConfigModel config = configManager.Load();

                Assert.AreEqual(1, config.Applications.Count);
                ApplicationModel app = config.Applications.First();
                Assert.AreEqual("DbDelivery", app.Name);
                Assert.AreEqual(1, app.Environments.Count);
                EnvironmentModel env = app.Environments.First();
                Assert.AreEqual("test", env.Name);
                Assert.AreEqual(1, env.Commands.Count);
                CommandModel cmd = env.Commands.First();
                Assert.AreEqual("InitDatabase", cmd.PluginType);
                Assert.AreEqual(2, cmd.Settings.Count);
                CommandSettingModel providerName = cmd.Settings.FirstOrDefault(c => c.Name == "ProviderName");
                Assert.IsNotNull(providerName);
                Assert.IsFalse(String.IsNullOrEmpty(providerName.Value));
                CommandSettingModel connString = cmd.Settings.FirstOrDefault(c => c.Name == "ConnectionString");
                Assert.IsNotNull(connString);
                Assert.IsFalse(String.IsNullOrEmpty(connString.Value));
            } finally {
                if (File.Exists("config.xml")) {
                    File.Delete("config.xml");
                }
            }
        }

        [TestMethod]
        public void GetEnvironmentConfigFile() {
            ConfigModel config = GetTestConfigModel();
            ConfigManager configManager = new ConfigManager();
            EnvironmentModel env = configManager.GetEnvironmentConfig(config, "DbDelivery", "test");
                
            Assert.AreEqual("test", env.Name);
            Assert.AreEqual(1, env.Commands.Count);
            CommandModel cmd = env.Commands.First();
            Assert.AreEqual("InitDatabase", cmd.PluginType);
            Assert.AreEqual(2, cmd.Settings.Count);
            CommandSettingModel providerName = cmd.Settings.FirstOrDefault(c => c.Name == "ProviderName");
            Assert.IsNotNull(providerName);
            Assert.IsFalse(String.IsNullOrEmpty(providerName.Value));
            CommandSettingModel connString = cmd.Settings.FirstOrDefault(c => c.Name == "ConnectionString");
            Assert.IsNotNull(connString);
            Assert.IsFalse(String.IsNullOrEmpty(connString.Value));
        }

        private ConfigModel GetTestConfigModel() {
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

            return config;
        }

        private void MakeTestConfigFile() {
            File.Copy("test_config.xml", "config.xml");
        }

    }
}
