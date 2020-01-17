using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Text2Sql {
    class Program {
        static void Main(string[] args) {
            var connStr = @"server=localhost\sqlexpress;database=Text2Sql;trusted_connection=true;";
            using var conn = new SqlConnection(connStr);
            conn.Open();
            if(conn.State != System.Data.ConnectionState.Open) {
                throw new Exception("Connection did not open");
            }
            var sql = "insert into datatable values (@c0, @c1, @c2, @c3, @c4, @c5, @c6, @c7)";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@c0", null));
            cmd.Parameters.Add(new SqlParameter("@c1", null));
            cmd.Parameters.Add(new SqlParameter("@c2", null));
            cmd.Parameters.Add(new SqlParameter("@c3", null));
            cmd.Parameters.Add(new SqlParameter("@c4", null));
            cmd.Parameters.Add(new SqlParameter("@c5", null));
            cmd.Parameters.Add(new SqlParameter("@c6", null));
            cmd.Parameters.Add(new SqlParameter("@c7", null));

            var data = LoadFile();
            LoadDb(data, cmd);
        }
        static void LoadDb(List<List<string>> data, SqlCommand cmd) {
            for(var idx = 1; idx < data.Count; idx++) {
                var row = data[idx];
                cmd.Parameters["@c0"].Value = row[0];
                cmd.Parameters["@c1"].Value = row[1];
                cmd.Parameters["@c2"].Value = row[2];
                cmd.Parameters["@c3"].Value = row[3];
                cmd.Parameters["@c4"].Value = row[4];
                cmd.Parameters["@c5"].Value = row[5];
                cmd.Parameters["@c6"].Value = row[6];
                cmd.Parameters["@c7"].Value = row[7];
                var rowsAffected = cmd.ExecuteNonQuery();
                if(rowsAffected != 1) {
                    throw new Exception("Update failed!");
                }
            }
        }
        static List<List<string>> LoadFile() { 
            var infileLines = System.IO.File.ReadAllLines(@"c:\Users\gpdou\Downloads\User.prn");
            var rows = new List<List<string>>();
            foreach(var line in infileLines) {
                var flds = line.Split('|');
                for(var idx = 0; idx < flds.Length; idx++) {
                    flds[idx] = flds[idx].Trim();
                }
                var fields = new List<string>(flds);
                rows.Add(fields);
            }
            return rows;
        }
    }
}
