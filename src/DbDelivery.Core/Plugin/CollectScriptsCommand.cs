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
    /// collect all scripts from a project
    /// </summary>
    public class CollectScriptsCommand : AbstractPluginCommand {

        public CollectScriptsCommand(ISettingStore settings, IDataStore data) : base(settings, data) {
            //PluginType = "CollectScripts";
        }

        public override bool Execute() {
            // get directories with sql scripts
            IEnumerable<string> sourceFolders = GetSourceDirectoryList();
            // copy all scripts to temp directory
            string tempDir = GetTempScriptDirectory();
            foreach (string sourcePath in sourceFolders) {
                IEnumerable<string> scripts = Directory.EnumerateFiles(sourcePath, "*.sql", SearchOption.TopDirectoryOnly);
                foreach (string file in scripts) {
                    string fileName = Path.GetFileName(file);
                    File.Move(file, Path.Combine(tempDir, fileName));
                }
            }
            return true;
        }

        private string GetTempScriptDirectory() {
            string path = this.Settings.Get("TempScriptDirectory");
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.Exists) {
                foreach (FileInfo file in dir.GetFiles()) {
                    file.Delete();
                }
                foreach (DirectoryInfo item in dir.GetDirectories()) {
                    dir.Delete(true);
                }
            } else {
                dir.Create();
            }
            return path;
        }

        private IEnumerable<string> GetSourceDirectoryList() {
            List<string> result = new List<string>();
            string rootDir = this.Settings.Get("RootDirectory");
            string leafDir = this.Settings.Get("LeafDirectory", "");
            string sourceSettring = this.Settings.Get("SourceDirectories");
            if (String.IsNullOrEmpty(sourceSettring)) {
                throw new ApplicationException("Command setting SourceDirectories is empty");
            }
            IList<string> sourceSettingArray = sourceSettring
                .Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Distinct()
                .ToList();
            foreach (string sourcePath in sourceSettingArray) {
                IEnumerable<string> dirs = Directory.EnumerateDirectories(rootDir, sourcePath, SearchOption.AllDirectories);
                result.AddRange(dirs.Select(d => Path.Combine(d, leafDir)));
            }
            return result.Distinct();
        }
    }
}
