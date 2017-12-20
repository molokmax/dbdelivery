using DbDelivery.Core.Config;
using DbDelivery.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDelivery.Core {

    /// <summary>
    /// Implementation of settings storage
    /// </summary>
    public class SettingStore : ISettingStore {

        private IDictionary<string, string> store = new Dictionary<string, string>();

        /// <summary>
        /// Create storage from config
        /// </summary>
        public SettingStore(CommandModel config) {
            foreach (CommandSettingModel cfg in config.Settings) {
                store[cfg.Name] = cfg.Value;
            }
        }

        /// <summary>
        /// Get setting value by setting name
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key) {
            if (String.IsNullOrEmpty(key)) {
                throw new ArgumentNullException("key");
            }
            string result;
            if (store.TryGetValue(key, out result)) {
                return result;
            } else {
                throw new ApplicationException(String.Format("Setting '{0}' not found", key));
            }
        }
    }
}
