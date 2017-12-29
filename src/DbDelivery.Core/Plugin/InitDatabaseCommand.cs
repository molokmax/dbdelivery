using DbDelivery.Plugin;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDelivery.Core.Plugin {

    /// <summary>
    /// Initializing database for db deliveries
    /// </summary>
    public class InitDatabaseCommand : AbstructPluginCommand {

        public InitDatabaseCommand(ISettingStore settings, IDataStore data) : base(settings, data) {
            // sql command for initializing
        }
        
        public override bool Execute() {
            using (DbConnection connection = GetConnection()) {
                using (DbCommand cmd = connection.CreateCommand()) {
                    const string initScriptFileName = "initialize.sql";
                    string initScriptPath = Path.Combine("SqlCommandResources", initScriptFileName);
                    if (!File.Exists(initScriptPath)) {
                        throw new ApplicationException(String.Format("Init script '{0}' is not found", initScriptPath));
                    }
                    string initScript = File.ReadAllText(initScriptPath);
                    cmd.CommandText = String.Format(initScript, GetMigrationTableName());
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }


        private DbConnection GetConnection() {
            string providerName = GetProviderName();
            string connectionString = GetConnectionString();
            DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);
            DbConnection connection = factory.CreateConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            return connection;
        }

        private string GetMigrationTableName() {
            string prefix = GetTablePrefix();
            return String.Format("{0}DB_DELIVERY_HISTORY", prefix);
        }

        private string GetProviderName() {
            return this.Settings.Get("ProviderName");
        }
        private string GetConnectionString() {
            return this.Settings.Get("ConnectionString");
        }
        private string GetTablePrefix() {
            string tablePrefix = this.Settings.Get("TablePrefix", null);
            return String.IsNullOrEmpty(tablePrefix) ? null : tablePrefix + "_";
        }

    }
}
