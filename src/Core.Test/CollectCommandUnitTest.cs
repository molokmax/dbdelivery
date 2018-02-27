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

namespace Core.Test {
    /// <summary>
    /// Summary description for CollectCommandUnitTest
    /// </summary>
    [TestClass]
    public class CollectCommandUnitTest {
        public CollectCommandUnitTest() {
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
        public void PathCombineTest() {
            string result = Path.Combine(@"C:\Windows\Test", "");
            Assert.AreEqual(@"C:\Windows\Test", result);
        }

        [TestMethod]
        public void CollectScripts() {
            CommandModel config = new CommandModel();
            config.PluginType = "CollectScripts";
            config.Settings = new List<CommandSettingModel>() {
                new CommandSettingModel() {
                    Name = "RootDirectory",
                    Value = "./src"
                },
                new CommandSettingModel() {
                    Name = "LeafDirectory",
                    Value = "init"
                },
                new CommandSettingModel() {
                    Name = "TempScriptDirectory",
                    Value = "ScriptCollect"
                },
                new CommandSettingModel() {
                    Name = "SourceDirectories",
                    Value = "module.*;module.*"
                }
            };
            ISettingStore settings = new SettingStore(config, null);
            IPluginCommand cmd = new CollectScriptsCommand(settings, null);
            bool res = cmd.Execute();
            Assert.IsTrue(res);

            DirectoryInfo dir = new DirectoryInfo("ScriptCollect");
            Assert.IsTrue(dir.Exists);

            IEnumerable<FileInfo> files = dir.GetFiles();
            Assert.AreEqual(4, files.Count());

            IEnumerable<DirectoryInfo> dirs = dir.GetDirectories();
            Assert.AreEqual(0, dirs.Count());

            Assert.IsTrue(files.Any(f => f.Name == "3_test.sql"));
            Assert.IsTrue(files.Any(f => f.Name == "4_test.sql"));
            //Assert.IsTrue(files.Any(f => f.Name == "5_test.sql"));
            //Assert.IsTrue(files.Any(f => f.Name == "6_test.sql"));
            Assert.IsTrue(files.Any(f => f.Name == "10_test.sql"));
            Assert.IsTrue(files.Any(f => f.Name == "11_test.sql"));
            //Assert.IsTrue(files.Any(f => f.Name == "12_test.sql"));
            //Assert.IsTrue(files.Any(f => f.Name == "13_test.sql"));
        }

        [TestMethod]
        public void CollectScriptsEmptyLeaf() {
            CommandModel config = new CommandModel();
            config.PluginType = "CollectScripts";
            config.Settings = new List<CommandSettingModel>() {
                new CommandSettingModel() {
                    Name = "RootDirectory",
                    Value = "./src"
                },
                new CommandSettingModel() {
                    Name = "TempScriptDirectory",
                    Value = "ScriptCollect"
                },
                new CommandSettingModel() {
                    Name = "SourceDirectories",
                    Value = "module.*;module.*"
                }
            };
            ISettingStore settings = new SettingStore(config, null);
            IPluginCommand cmd = new CollectScriptsCommand(settings, null);
            bool res = cmd.Execute();
            Assert.IsTrue(res);

            DirectoryInfo dir = new DirectoryInfo("ScriptCollect");
            Assert.IsTrue(dir.Exists);

            IEnumerable<FileInfo> files = dir.GetFiles();
            Assert.AreEqual(2, files.Count());

            IEnumerable<DirectoryInfo> dirs = dir.GetDirectories();
            Assert.AreEqual(0, dirs.Count());

            Assert.IsTrue(files.Any(f => f.Name == "7_test.sql"));
            Assert.IsTrue(files.Any(f => f.Name == "8_test.sql"));
        }
    }
}
