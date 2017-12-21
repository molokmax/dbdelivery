using DbDelivery.Core;
using DbDelivery.Core.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Test {
    [TestClass]
    public class CommandInvokeUnitTest {

        [ClassCleanup()]
        public static void CommandInvokeUnitTestCleanup() {
            if (File.Exists("test_file.txt")) {
                File.Delete("test_file.txt");
            }
        }

        [TestMethod]
        public void SingleCommandInvoke() {
            EnvironmentModel env = GetTestEnvironmentConfig();

            IPluginFactory factory = new PluginFactory();
            factory.RegisterCommand(typeof(CreateTestFileCommand));
            ICommandInvoker invoker = new CommandInvoker(factory);
            invoker.Invoke(env);

            Assert.IsTrue(File.Exists("test_file.txt"));
            string content = File.ReadAllText("test_file.txt");
            Assert.AreEqual("TEST_MESSAGE", content);
        }

        private EnvironmentModel GetTestEnvironmentConfig() {
            EnvironmentModel env = new EnvironmentModel();
            env.Name = "test";
            env.Commands = new List<CommandModel>();

            CommandModel cmd = new CommandModel();
            cmd.PluginType = "CreateTestFile";
            cmd.Settings = new List<CommandSettingModel>();

            CommandSettingModel filename = new CommandSettingModel();
            filename.Name = "FileName";
            filename.Value = "test_file.txt";
            cmd.Settings.Add(filename);

            CommandSettingModel message = new CommandSettingModel();
            message.Name = "TestData";
            message.Value = "TEST_MESSAGE";
            cmd.Settings.Add(message);

            env.Commands.Add(cmd);

            return env;
        }
    }
}
