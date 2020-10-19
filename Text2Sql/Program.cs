using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Text2Sql {
    class Program {
        static void Main(string[] args) {
            string[] colNames = {
                "PAS_ID", "SRC_CD", "ACCTG_BASIS_CD", "REF_ID", "CMPNY_CD", "EAS_ACCT_NO",
                "CLNDR_ACCTG_MNTH_NO", "CLNDR_ACCTG_YR_NO", "LOB_CD", "DBT_CR_IND",
                "CONVERTED_AMT", "BCTR_CD", "REINS_CMPNY_CD", "STATE_CD", "TAX_STAT_TXT",
                "HDR_DESC_TXT", "PLAN_CD", "SAP_ACCT_NO", "PRFT_CTR_TXT", "PLCY_NO",
                "TRANS_DT", "CLNDR_DT", "MEMO_CD"
            };
            var slib = new Dsi.SqlLib();
            var conn = slib.CreateSqlConnection("localhost\\sqlexpress", "Text2Sql");
            slib.OpenConnection();
            slib.ConstructSqlCommand("GERBER_PAS_DAILY", colNames);

            var data = LoadFile();

            for(var r = 1; r < data.Count; r++) {
                var row = data[r];
                for(var c = 0; c < row.Count; c++) {
                    slib.SetSqlParameterValue($"@{colNames[c]}", row[c]);
                }
                slib.ExecuteNonQuery(); // exception on error
            }
            //LoadDb(data, cmd);
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
            var infileLines = System.IO.File.ReadAllLines(@"c:\Users\gpdou\Downloads\gerber-pas-daily.pipedelim");
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
