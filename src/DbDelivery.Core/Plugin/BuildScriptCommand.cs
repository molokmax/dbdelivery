using DbDelivery.Core.Plugin.Model;
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
    /// get 
    /// </summary>
    public class BuildScriptCommand : AbstractDatabasePluginCommand {

        public BuildScriptCommand(ISettingStore settings, IDataStore data) : base(settings, data) {
            //PluginType = "BuildScript";
        }

        public override bool Execute() {
            // get list of applied scripts
            IList<MigrationScriptModel> appliedScripts = GetAppliedScripts();
            // get full list of scripts
            FileInfo[] files = GetScriptFiles();
            // get list of not applied scripts
            ISet<string> appliedScriptNames = new HashSet<string>(appliedScripts.Select(s => s.ScriptName));
            IEnumerable<string> notAppliedScripts = files
                .Where(f => !appliedScriptNames.Contains(f.Name))
                .Select(f => f.FullName)
                .OrderBy(f => f);
            // save list of not applied scripts to use it in other command
            Data.SetValue<List<string>>("ScriptsToApply", notAppliedScripts.ToList());
            return true;
        }

        private FileInfo[] GetScriptFiles() {
            string path = this.Settings.Get("TempScriptDirectory");
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.Exists) {
                FileInfo[] files = dir.GetFiles();
                return files;
            } else {
                throw new ApplicationException(String.Format("Temp script folder '{0}' is not found", path));
            }
        }

        private List<MigrationScriptModel> GetAppliedScripts() {
            using (DbConnection connection = GetConnection()) {
                using (DbCommand cmd = connection.CreateCommand()) {
                    const string scriptFileName = "getapplied.sql";
                    string scriptPath = Path.Combine("SqlCommandResources", scriptFileName);
                    if (!File.Exists(scriptPath)) {
                        throw new ApplicationException(String.Format("Get Applied script '{0}' is not found", scriptPath));
                    }
                    string script = File.ReadAllText(scriptPath, Encoding.UTF8);
                    cmd.CommandText = String.Format(script, GetMigrationTableName());
                    using (DbDataReader reader = cmd.ExecuteReader()) {
                        List<MigrationScriptModel> result = new List<MigrationScriptModel>();
                        while (reader.Read()) {
                            MigrationScriptModel record = new MigrationScriptModel();
                            if (!reader.IsDBNull(0)) {
                                record.ScriptName = reader.GetString(0);
                                if (reader.IsDBNull(1)) {
                                    record.ApplyDate = null;
                                } else {
                                    record.ApplyDate = reader.GetDateTime(1);
                                }
                                result.Add(record);
                            }
                        }
                        return result;
                    }
                }
            }
        }
    }
}
