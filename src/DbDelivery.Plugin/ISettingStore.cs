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
        /// throw exception if key nod found
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Get(string key);

        /// <summary>
        /// Get setting value by name
        /// return default value if key nod found
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Get(string key, string defaulValue);
    }
}
