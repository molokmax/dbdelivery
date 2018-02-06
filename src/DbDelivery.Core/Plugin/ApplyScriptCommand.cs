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
                                int scriptPartIndex = 0;
                                int scriptPartCount = 0;
                                try {
                                    string commandText = File.ReadAllText(scriptPath);
                                    string[] scriptParts = SplitScript(commandText);
                                    scriptPartCount = scriptParts.Length;
                                    for (scriptPartIndex = 0; scriptPartIndex < scriptParts.Length; scriptPartIndex++) {
                                        cmd.CommandText = scriptParts[scriptPartIndex];
                                        cmd.ExecuteNonQuery();
                                    }
                                } catch (Exception e) {
                                    throw new ApplicationException(String.Format("Error has occurred in '{0}' (part {2}/{3}) script. {1}", Path.GetFileName(scriptPath), e.Message, scriptPartIndex, scriptPartCount), e);
                                }
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

        private string[] SplitScript(string source) {
            string sep1 = Environment.NewLine + "GO" + Environment.NewLine;
            string sep2 = Environment.NewLine + "GO;" + Environment.NewLine;
            string sep3 = Environment.NewLine + "go" + Environment.NewLine;
            string sep4 = Environment.NewLine + "go;" + Environment.NewLine;
            string sep5 = Environment.NewLine + "Go" + Environment.NewLine;
            string sep6 = Environment.NewLine + "Go;" + Environment.NewLine;
            string sep7 = Environment.NewLine + "gO" + Environment.NewLine;
            string sep8 = Environment.NewLine + "gO;" + Environment.NewLine;
            string[] separators = new string[] { sep1, sep2, sep3, sep4, sep5, sep6, sep7, sep8 };
            return source.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        private string GetScriptName(string filePath) {
            return Path.GetFileName(filePath)
                .Replace("\'", "\'\'");
        }

        
    }
}
