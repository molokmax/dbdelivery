using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DbDelivery.Core.Config {

    /// <summary>
    /// Model of application
    /// </summary>
    public class ApplicationModel {

        /// <summary>
        /// Name of system/application
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// List of environments
        /// </summary>
        public List<EnvironmentModel> Environments { get; set; }
    }
}
