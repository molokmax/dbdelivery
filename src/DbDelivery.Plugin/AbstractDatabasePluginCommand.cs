using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDelivery.Plugin {

    public abstract class AbstractDatabasePluginCommand : AbstractPluginCommand {

        public AbstractDatabasePluginCommand(ISettingStore settings, IDataStore data) : base(settings, data) {
        }

        protected const string MIGRATION_TABLE_NAME = "DB_DELIVERY_HISTORY";

        protected virtual DbConnection GetConnection() {
            string providerName = GetProviderName();
            string connectionString = GetConnectionString();
            DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);
            DbConnection connection = factory.CreateConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            return connection;
        }

        protected virtual string GetMigrationTableName() {
            string prefix = GetTablePrefix();
            return String.Format("{0}{1}", prefix, MIGRATION_TABLE_NAME);
        }

        protected string GetProviderName() {
            string result = this.Settings.Get("ProviderName", null);
            if (String.IsNullOrEmpty(result)) {
                result = this.Data.GetValue<string>("ProviderName");
            }
            return result;
        }
        protected string GetConnectionString() {
            string result = this.Settings.Get("ConnectionString", null);
            if (String.IsNullOrEmpty(result)) {
                result = this.Data.GetValue<string>("ConnectionString");
            }
            return result;
        }
        protected string GetTablePrefix() {
            string tablePrefix = this.Settings.Get("TablePrefix", null);
            return String.IsNullOrEmpty(tablePrefix) ? null : tablePrefix + "_";
        }
    }
}
