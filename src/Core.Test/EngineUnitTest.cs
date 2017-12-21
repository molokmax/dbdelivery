using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DbDelivery.Core;
using System.Linq;
using System.IO;
using Ninject.Modules;
using Ninject;

namespace Core.Test {
    /// <summary>
    /// Summary description for EngineUnitTest
    /// </summary>
    [TestClass]
    public class EngineUnitTest {
        public EngineUnitTest() {
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

        private void MakeTestConfigFile() {
            File.Copy("test_config2.xml", "config.xml");
        }

        [TestMethod]
        public void InitEngine() {
            IPluginFactory factory = new PluginFactory();
            IDbDeliveryEngine engine = new DbDeliveryEngine(factory, null);
            engine.Init();
            Assert.AreEqual(6, factory.AvailablePlugins.Count());
        }

        [TestMethod]
        public void MigrateEngine() {
            try {
                MakeTestConfigFile();
                IPluginFactory factory = new PluginFactory();
                ICommandInvoker invoker = new CommandInvoker(factory);
                IDbDeliveryEngine engine = new DbDeliveryEngine(factory, invoker);
                engine.Init();
                engine.Migrate("DbDelivery", "test");

                Assert.IsTrue(File.Exists("test_file2.txt"));
                string content = File.ReadAllText("test_file2.txt");
                Assert.AreEqual("TEST_MESSAGE_2", content);

            } finally {
                if (File.Exists("config.xml")) {
                    File.Delete("config.xml");
                }
                if (File.Exists("test_file2.txt")) {
                    File.Delete("test_file2.txt");
                }
            }
        }

        [TestMethod]
        public void IoCEngine() {

            try {
                MakeTestConfigFile();

                IoC.Kernel.Load(new INinjectModule[] { new DbDeliveryModule() });
                IDbDeliveryEngine engine = IoC.Kernel.Get<IDbDeliveryEngine>();

                Assert.AreEqual(typeof(DbDeliveryEngine), engine.GetType());

                engine.Init();
                engine.Migrate("DbDelivery", "test");

                Assert.IsTrue(File.Exists("test_file2.txt"));
                string content = File.ReadAllText("test_file2.txt");
                Assert.AreEqual("TEST_MESSAGE_2", content);

            } finally {
                if (File.Exists("config.xml")) {
                    File.Delete("config.xml");
                }
                if (File.Exists("test_file2.txt")) {
                    File.Delete("test_file2.txt");
                }
            }
        }
    }
}
