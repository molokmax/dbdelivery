using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDelivery.Plugin {
    /// <summary>
    /// Interface of settings storage for command
    /// </summary>
    public interface ISettingStore {

        /// <summary>
        /// Get setting value by name
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Get(string key);
    }
}
