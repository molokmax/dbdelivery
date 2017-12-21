using DbDelivery.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDelivery.Core {

    /// <summary>
    /// Implementation of data storage
    /// </summary>
    public class DataStore : IDataStore {

        private IDictionary<string, object> store = new Dictionary<string, object>();

        /// <summary>
        /// List of existing keys in storage
        /// </summary>
        public IEnumerable<string> Names {
            get {
                return store.Keys;
            }
        }

        /// <summary>
        /// If exists the key
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name) {
            if (String.IsNullOrEmpty(name)) {
                throw new ArgumentNullException("name");
            }
            return store.ContainsKey(name);
        }

        /// <summary>
        /// Get value for the key from storage
        /// throw exception if key is not found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetValue<T>(string name) {
            if (String.IsNullOrEmpty(name)) {
                throw new ArgumentNullException("name");
            }
            object result;
            if (store.TryGetValue(name, out result)) {
                return (T) result;
            } else {
                throw new ApplicationException("Key '{0}' does not exist in storage");
            }
        }

        /// <summary>
        /// Get value for the key from storage
        /// return default value if key is not found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetValue<T>(string name, T defaultValue) {
            if (String.IsNullOrEmpty(name)) {
                throw new ArgumentNullException("name");
            }
            object result;
            if (store.TryGetValue(name, out result)) {
                return (T) result;
            } else {
                return defaultValue;
            }
        }

        /// <summary>
        /// Set value of key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetValue<T>(string name, T value) {
            if (String.IsNullOrEmpty(name)) {
                throw new ArgumentNullException("name");
            }
            store[name] = value;
        }
    }
}
