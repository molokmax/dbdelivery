using DbDelivery.Plugin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseConfigPlugin
{
    public class ParseConfigCommand : AbstractPluginCommand {
        public ParseConfigCommand(ISettingStore settings, IDataStore data) : base(settings, data) {
        }

        public override bool Execute() {
            // Получить из настроек путь к файлу конфига для разбора
            string configPath = this.Settings.Get("ConfigPath");
            if (!File.Exists(configPath)) {
                throw new ApplicationException(String.Format("Config file '{0}' not found", configPath));
            }
            // Получить значение строки подключения к БД и ProviderName из конфига
            string connectionStringName = this.Settings.Get("ConnectionStringName", "DatabaseConnection");
            ConnectionStringSettings conn = GetConnectionString(configPath, connectionStringName);
            // Сохранить строку подключения в настройки
            Data.SetValue("ConnectionString", conn.ConnectionString);
            Data.SetValue("ProviderName", conn.ProviderName);
            return true;
        }

        private ConnectionStringSettings GetConnectionString(string configPath, string name) {
            if (String.IsNullOrEmpty(name)) {
                throw new ApplicationException("Setting name is empty");
            }
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
            configMap.ExeConfigFilename = configPath;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            ConnectionStringSettings setting = config.ConnectionStrings.ConnectionStrings[name];
            if (setting == null) {
                throw new ApplicationException(String.Format("Connection string '{0}' in config '{1}' not found", name, configPath));
            } else {
                return setting;
            }
        }
    }
}
