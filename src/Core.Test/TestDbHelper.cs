using DbDelivery.Core;
using DbDelivery.Core.Config;
using DbDelivery.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Test {
    public static class TestDbHelper {

        public static void Clean(string providerName, string connectionString) {
            CommandModel config = new CommandModel();
            config.PluginType = "CleanTestDatabase";
            config.Settings = new List<CommandSettingModel>() {
                new CommandSettingModel() {
                    Name = "ProviderName",
                    Value = providerName
                },
                new CommandSettingModel() {
                    Name = "ConnectionString",
                    Value = connectionString
                }
            };
            ISettingStore settings = new SettingStore(config);
            IDataStore data = new DataStore();
            IPluginCommand cmd = new CleanTestDatabaseCommand(settings, data);
            cmd.Execute();
        }

        public static void Init(string providerName, string connectionString, string fillHistory = "YES") {
            CommandModel config = new CommandModel();
            config.PluginType = "InitTestDatabase";
            config.Settings = new List<CommandSettingModel>() {
                new CommandSettingModel() {
                    Name = "ProviderName",
                    Value = providerName
                },
                new CommandSettingModel() {
                    Name = "ConnectionString",
                    Value = connectionString
                },
                new CommandSettingModel() {
                    Name = "TablePrefix",
                    Value = "TEST_BUILD"
                },
                new CommandSettingModel() {
                    Name = "FillHistory",
                    Value = fillHistory
                }
            };
            ISettingStore settings = new SettingStore(config);
            IDataStore data = new DataStore();
            IPluginCommand cmd = new InitTestDatabaseCommand(settings, data);
            bool res = cmd.Execute();
        }
    }
}
