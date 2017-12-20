using DbDelivery.Plugin;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
            // TODO: move to resources
            InitDatabaseCommandText = "CREATE TABLE DB_DELIVERY_HISTORY ( " + Environment.NewLine +
                "[SCRIPT_NAME] [NVARCHAR](200) NOT NULL, " + Environment.NewLine +
                "[APPLY_DATE] [DATETIME] NOT NULL " + Environment.NewLine +
                ")";
        }

        private readonly string InitDatabaseCommandText;

        private string GetProviderName() {
            return this.Settings.Get("ProviderName");
        }
        private string GetConnectionString() {
            return this.Settings.Get("ConnectionString");
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

        public override bool Execute() {
            using (DbConnection connection = GetConnection()) {
                using (DbCommand cmd = connection.CreateCommand()) {
                    cmd.CommandText = InitDatabaseCommandText;
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
    }
}
