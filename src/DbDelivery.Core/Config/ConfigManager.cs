using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DbDelivery.Core.Config {

    /// <summary>
    /// Manager for dbDelivery configuration
    /// </summary>
    public class ConfigManager {

        private XmlSerializer serializer = null;

        public ConfigManager() {
            serializer = new XmlSerializer(typeof(ConfigModel));
        }
        
        /// <summary>
        /// Save configuration
        /// </summary>
        /// <param name="config"></param>
        public void Save(ConfigModel config) { 
            string configFile = GetSettingByName("ConfigFileName");
            using (FileStream file = new FileStream(configFile, FileMode.Create, FileAccess.Write)) {
                using (XmlWriter writer = XmlWriter.Create(file)) {
                    serializer.Serialize(writer, config);
                }
            }
        }

        /// <summary>
        /// Load configuration
        /// </summary>
        /// <returns></returns>
        public ConfigModel Load() {
            string configFile = GetSettingByName("ConfigFileName");
            if (!File.Exists(configFile)) {
                throw new ApplicationException(String.Format("Config file '{0}' not found", configFile));
            }
            using (FileStream file = new FileStream(configFile, FileMode.Open, FileAccess.Read)) {
                using (XmlReader reader = XmlReader.Create(file)) {
                    ConfigModel config = (ConfigModel) serializer.Deserialize(reader);
                    return config;
                }
            }
        }

        /// <summary>
        /// Get environment config for the application
        /// </summary>
        /// <param name="config"></param>
        /// <param name="applicationName"></param>
        /// <param name="environmentName"></param>
        /// <returns></returns>
        public EnvironmentModel GetEnvironmentConfig(ConfigModel config, string applicationName, string environmentName) {
            if (config == null) {
                throw new ArgumentNullException("config");
            }
            if (String.IsNullOrEmpty(applicationName)) {
                throw new ArgumentNullException("applicationName");
            }
            if (String.IsNullOrEmpty(environmentName)) {
                throw new ArgumentNullException("environmentName");
            }
            ApplicationModel appConfig = config.Applications.FirstOrDefault(a => String.Equals(a.Name, applicationName, StringComparison.OrdinalIgnoreCase));
            if (appConfig == null) {
                throw new ApplicationException(String.Format("Application element '{0}' is not found in configuration", applicationName));
            }
            EnvironmentModel envConfig = appConfig.Environments.FirstOrDefault(e => String.Equals(e.Name, environmentName, StringComparison.OrdinalIgnoreCase));
            if (envConfig == null) {
                throw new ApplicationException(String.Format("Environment element '{0}' is not found in configuration", environmentName));
            }
            return envConfig;
        }

        /// <summary>
        /// Get app setting
        /// </summary>
        /// <returns></returns>
        public string GetSettingByName(string settingName) {
            string result = ConfigurationManager.AppSettings[settingName];
            if (String.IsNullOrEmpty(result)) {
                throw new ApplicationException(String.Format("Application setting '{0}' is empty", settingName));
            } else {
                return result;
            }
        }

        /// <summary>
        /// Get app setting
        /// </summary>
        /// <returns></returns>
        public string GetSettingByName(string settingName, string defaultValue) {
            string result = ConfigurationManager.AppSettings[settingName];
            if (result == null) {
                return defaultValue;
            } else {
                return result;
            }
        }
    }
}
