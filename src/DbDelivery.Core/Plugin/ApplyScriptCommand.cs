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
                            string encName = this.Settings.Get("Encoding", "utf-8");
                            Encoding encoding = Encoding.GetEncoding(encName);
                            cmd.Transaction = transaction;
                            List<string> historyScript = new List<string>();
                            foreach (string scriptPath in scripts) {
                                int scriptPartIndex = 0;
                                int scriptPartCount = 0;
                                try {
                                    string commandText = File.ReadAllText(scriptPath, encoding);
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
            IList<string> separators = new List<string>();

            separators.Add("\r\nGO\r\n");
            separators.Add("\r\nGO;\r\n");
            separators.Add("\r\ngo\r\n");
            separators.Add("\r\ngo;\r\n");
            separators.Add("\r\nGo\r\n");
            separators.Add("\r\nGo;\r\n");
            separators.Add("\r\ngO\r\n");
            separators.Add("\r\ngO;\r\n");

            separators.Add("\rGO\r");
            separators.Add("\rGO;\r");
            separators.Add("\rgo\r");
            separators.Add("\rgo;\r");
            separators.Add("\rGo\r");
            separators.Add("\rGo;\r");
            separators.Add("\rgO\r");
            separators.Add("\rgO;\r");

            separators.Add("\nGO\n");
            separators.Add("\nGO;\n");
            separators.Add("\ngo\n");
            separators.Add("\ngo;\n");
            separators.Add("\nGo\n");
            separators.Add("\nGo;\n");
            separators.Add("\ngO\n");
            separators.Add("\ngO;\n");

            separators.Add("\rGO\n");
            separators.Add("\rGO;\n");
            separators.Add("\rgo\n");
            separators.Add("\rgo;\n");
            separators.Add("\rGo\n");
            separators.Add("\rGo;\n");
            separators.Add("\rgO\n");
            separators.Add("\rgO;\n");

            separators.Add("\nGO\r");
            separators.Add("\nGO;\r");
            separators.Add("\ngo\r");
            separators.Add("\ngo;\r");
            separators.Add("\nGo\r");
            separators.Add("\nGo;\r");
            separators.Add("\ngO\r");
            separators.Add("\ngO;\r");

            return source.Split(separators.ToArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        private string GetScriptName(string filePath) {
            return Path.GetFileName(filePath)
                .Replace("\'", "\'\'");
        }

        
    }
}
