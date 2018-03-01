using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DbDelivery.Core.Config {

    /// <summary>
    /// Model of command setting
    /// </summary>
    public class CommandSettingModel {

        /// <summary>
        /// Key of setting
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Value of setting
        /// </summary>
        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
