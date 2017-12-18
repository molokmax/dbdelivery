using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDelivery.Plugin {

    /// <summary>
    /// Interface of data storage that transfer through whole execution cycle
    /// </summary>
    public interface IDataStore {

        /// <summary>
        /// List of data keys
        /// </summary>
        IEnumerable<string> Names { get; }

        /// <summary>
        /// Get value of given key from the storage
        /// If key does not exist - throw exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        T GetValue<T>(string name);

        /// <summary>
        /// Get value of given key from the storage
        /// If key does not exist - return default value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        T GetValue<T>(string name, T defaultValue);

        /// <summary>
        /// Set value for given key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void SetValue<T>(string name, T value);

        /// <summary>
        /// Check key for existing in the storage
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool Contains(string name);
    }
}
