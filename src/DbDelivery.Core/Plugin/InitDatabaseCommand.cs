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
    public class InitDatabaseCommand : AbstractDatabasePluginCommand {

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


    }
}
