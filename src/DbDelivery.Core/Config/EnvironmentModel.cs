using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DbDelivery.Core.Config {

    /// <summary>
    /// Model of environment
    /// </summary>
    public class EnvironmentModel {

        /// <summary>
        /// Name of environment (test, prod)
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        public List<CommandSettingModel> BaseSettings { get; set; }

        /// <summary>
        /// List of commands
        /// </summary>
        public List<CommandModel> Commands { get; set; }
    }
}
