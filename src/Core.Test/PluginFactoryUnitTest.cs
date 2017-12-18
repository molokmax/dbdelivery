using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DbDelivery.Core;
using DbDelivery.Plugin;
using System.Linq;

namespace Core.Test {
    [TestClass]
    public class PluginFactoryUnitTest {
        [TestMethod]
        public void RegisterPlugin() {
            IPluginFactory factory = new PluginFactory();
            Type applyScriptCommandType = typeof(ApplyScriptCommand);
            factory.RegisterCommand(applyScriptCommandType);
            Assert.AreEqual(1, factory.AvailablePlugins.Count());
            Assert.AreEqual("ApplyScript", factory.AvailablePlugins.First());
        }

        [TestMethod]
        public void RegisterPluginBadName() {
            IPluginFactory factory = new PluginFactory();
            Type commandType = typeof(PluginBadName);
            factory.RegisterCommand(commandType);
            Assert.AreEqual(1, factory.AvailablePlugins.Count());
            Assert.AreEqual("PluginBadName", factory.AvailablePlugins.First());
        }

        [TestMethod]
        public void RegisterWrongPlugin() {
            IPluginFactory factory = new PluginFactory();
            Type commandType = typeof(string);
            try {
                factory.RegisterCommand(commandType);
                Assert.Fail("string can not be plugin");
            } catch (Exception e) {
                Assert.AreEqual("String is not IPluginCommand", e.Message);
            }
        }

        [TestMethod]
        public void CreateCommand() {
            IPluginFactory factory = new PluginFactory();
            Type applyScriptCommandType = typeof(ApplyScriptCommand);
            factory.RegisterCommand(applyScriptCommandType);
            IPluginCommand plugin = factory.CreateCommand("ApplyScript", null, null);
            Assert.AreEqual(typeof(ApplyScriptCommand), plugin.GetType());
        }
    }
}
