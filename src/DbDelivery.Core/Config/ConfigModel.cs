using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDelivery.Core.Config {

    /// <summary>
    /// Model of configuration file
    /// </summary>
    public class ConfigModel {

        /// <summary>
        /// List of applications
        /// </summary>
        public List<ApplicationModel> Applications { get; set; }
    }
}
