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
            string configFile = GetConfigFileName();
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
            string configFile = GetConfigFileName();
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
        /// Get file name of configuration
        /// </summary>
        /// <returns></returns>
        private string GetConfigFileName() {
            string configName = ConfigurationManager.AppSettings["ConfigName"];
            if (String.IsNullOrEmpty(configName)) {
                throw new ApplicationException(String.Format("Application setting '{0}' is empty", configName));
            } else {
                return configName;
            }
        }
    }
}
