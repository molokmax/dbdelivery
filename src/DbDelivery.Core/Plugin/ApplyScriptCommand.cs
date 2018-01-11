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
    /// Command executes script on the database
    /// </summary>
    public class ApplyScriptCommand : AbstractDatabasePluginCommand {

        public ApplyScriptCommand(ISettingStore settings, IDataStore data) : base(settings, data) {
            //PluginType = "ApplyScript";
        }

        public override bool Execute() {

            List<string> scripts = this.Data.GetValue<List<string>>("ScriptsToApply");

            if (scripts != null && scripts.Any()) {
                using (DbConnection connection = GetConnection()) {
                    DbTransaction transaction = connection.BeginTransaction();
                    try {
                        using (DbCommand cmd = connection.CreateCommand()) {
                            cmd.Transaction = transaction;
                            List<string> historyScript = new List<string>();
                            foreach (string scriptPath in scripts) {
                                string commandText = File.ReadAllText(scriptPath);
                                cmd.CommandText = commandText;
                                cmd.ExecuteNonQuery();

                                string historyCmd = String.Format("(N'{0}', '{1:yyyy-MM-dd HH:mm:ss}')", GetScriptName(scriptPath), DateTime.Now);
                                historyScript.Add(historyCmd);
                            }

                            string tableName = GetMigrationTableName();
                            cmd.CommandText = String.Format("INSERT INTO {0} (SCRIPT_NAME, APPLY_DATE) VALUES {1}", tableName, String.Join(", ", historyScript));
                            cmd.ExecuteNonQuery();

                            transaction.Commit();
                        }
                    } catch (Exception) {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return true;
        }

        private string GetScriptName(string filePath) {
            return Path.GetFileName(filePath)
                .Replace("\'", "\'\'");
        }

        
    }
}
