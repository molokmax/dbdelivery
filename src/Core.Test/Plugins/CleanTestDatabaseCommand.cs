using DbDelivery.Plugin;
using System;
using System.Data.Common;
using System.IO;
using System.Text;

namespace Core.Test {

    /// <summary>
    /// clean database for db deliveries
    /// </summary>
    public class CleanTestDatabaseCommand : AbstractDatabasePluginCommand {

        public CleanTestDatabaseCommand(ISettingStore settings, IDataStore data) : base(settings, data) {
        }
        
        public override bool Execute() {
            using (DbConnection connection = GetConnection()) {
                using (DbCommand cmd = connection.CreateCommand()) {
                    string commandText = "BEGIN " + Environment.NewLine +
                        "DECLARE @sql NVARCHAR(max) = '' " + Environment.NewLine +
                        "SELECT @sql += ' Drop table ' + QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME) + '; ' " + Environment.NewLine +
                        "FROM INFORMATION_SCHEMA.TABLES " + Environment.NewLine +
                        "WHERE  TABLE_TYPE = 'BASE TABLE' " + Environment.NewLine +
                        "Exec Sp_executesql @sql " + Environment.NewLine +
                        "END ";
                    cmd.CommandText = commandText;
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }


    }
}
