using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDelivery.Core {

    /// <summary>
    /// Interface of startup point for database delivery
    /// </summary>
    public interface IDbDeliveryEngine {

        /// <summary>
        /// Initialize db delivery engine
        /// Register plugins
        /// </summary>
        void Init();

        /// <summary>
        /// Make migration for the application in the environment
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="environmentName"></param>
        void Migrate(string applicationName, string environmentName);
    }
}
