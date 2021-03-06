﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DbDelivery.Core.Config {

    /// <summary>
    /// Model of command in process flow
    /// </summary>
    public class CommandModel {

        /// <summary>
        /// Plugin name
        /// </summary>
        [XmlAttribute("pluginType")]
        public string PluginType { get; set; }

        /// <summary>
        /// List of settings for command
        /// </summary>
        public List<CommandSettingModel> Settings { get; set; }
    }
}
