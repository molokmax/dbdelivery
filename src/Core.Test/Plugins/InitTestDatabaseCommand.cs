using DbDelivery.Plugin;
using System;
using System.Data.Common;
using System.IO;
using System.Text;

namespace Core.Test {

    /// <summary>
    /// Initializing database for db deliveries
    /// </summary>
    public class InitTestDatabaseCommand : AbstractDatabasePluginCommand {

        public InitTestDatabaseCommand(ISettingStore settings, IDataStore data) : base(settings, data) {
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
                using (DbCommand cmd = connection.CreateCommand()) {
                    cmd.CommandText = String.Format("delete from {0}", GetMigrationTableName());
                    cmd.ExecuteNonQuery();
                }
                using (DbCommand cmd = connection.CreateCommand()) {
                    StringBuilder commandText = new StringBuilder();
                    commandText.AppendFormat("insert into {0} (SCRIPT_NAME, APPLY_DATE) VALUES ('script1.sql', '2017-12-30 11:35:12')" + Environment.NewLine, GetMigrationTableName());
                    commandText.AppendFormat("insert into {0} (SCRIPT_NAME, APPLY_DATE) VALUES ('script0.sql', '2018-01-01 18:24')" + Environment.NewLine, GetMigrationTableName());
                    cmd.CommandText = commandText.ToString();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }


    }
}
