using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDelivery.Core.Plugin.Model {
    public class MigrationScriptModel {
        public string ScriptName { get; set; }
        public DateTime? ApplyDate { get; set; }

        //public override int GetHashCode() {
        //    return String.IsNullOrEmpty(ScriptName) ? 0 : ScriptName.GetHashCode();
        //}

        //public override bool Equals(object obj) {
        //    if (obj is string) {
        //        return false;
        //    } else if (obj is FileInfo) {

        //    } else if (obj is MigrationScriptModel) {

        //    }
        //    return base.Equals(obj);
        //}
    }
}
