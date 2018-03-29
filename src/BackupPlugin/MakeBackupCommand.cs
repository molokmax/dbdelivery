using DbDelivery.Plugin;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupPlugin {

    /// <summary>
    /// Command make backup of the database
    /// </summary>
    public class MakeBackupCommand : AbstractDatabasePluginCommand {

        public MakeBackupCommand(ISettingStore settings, IDataStore data) : base(settings, data) {
        }

        public override bool Execute() {

            // Get path and filename for backup
            using (DbConnection connection = GetConnection()) {
                using (DbCommand cmd = connection.CreateCommand()) {
                    cmd.CommandTimeout = GetCommandTimeout();
                    string dir = this.Settings.Get("BackupDirectory");
                    string databaseName = connection.Database;
                    string filename = this.Settings.Get("BackupFileName", databaseName + "_#TIMESTAMP#.bak");
                    filename = filename.Replace("#TIMESTAMP#", DateTime.Now.ToString("yyyyMMddHHmm"));
                    string path = Path.Combine(dir, filename);

                    string commandText = String.Format("BACKUP DATABASE {0} TO Disk = '{1}' WITH COMPRESSION", databaseName, path);
                    cmd.CommandText = commandText;
                    cmd.ExecuteNonQuery();
                }
            }

            return true;
        }
        
    }
}
