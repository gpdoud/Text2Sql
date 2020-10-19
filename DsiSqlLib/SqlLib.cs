using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace Dsi {

    public class SqlLib {

        public SqlConnection SqlConnection { get; set; } = null;
        public SqlCommand SqlCommand { get; set; } = null;

        public SqlLib() {}

        public void ExecuteNonQuery() {
            int rowsAffected = SqlCommand.ExecuteNonQuery();
            if(rowsAffected != 1) {
                throw new Exception("Command did not affect 1 row.");
            }
        }
        public void SetSqlParameterValue(string parameter, object value) {
            if(parameter.Equals("@PAS_ID")) { 
                value = Convert.ToInt32(value); 
            }
            SqlCommand.Parameters[parameter].Value = value;
        }

        // This method will create sql parameters that match the table column names
        // i.e. name : @name
        public void ConstructSqlCommand(string tablename, params string[] parms) {
            var sql = new StringBuilder($" INSERT into {tablename} ");
            // columns
            var cols = string.Join(", ", parms);
            sql.AppendFormat(" ({0}) ", cols);
            // values
            var vals = new List<string>();
            foreach(var parm in parms) {
                vals.Add($"'@{parm}'");
            }
            var vals2 = string.Join(", ", vals);
            sql.AppendFormat(" VALUES ({0}) ", vals2);
            SqlCommand = new SqlCommand(sql.ToString(), SqlConnection);
            // add the sql parameters
            foreach(var parm in parms) {
                SqlCommand.Parameters.AddWithValue($"@{parm}", null);
            }
        }
        
        public SqlConnection CreateSqlConnection(string server, string database, bool trustedConnection = true) {
            var connStr = $"server={server};database={database};trusted_connection={trustedConnection}";
            SqlConnection = new SqlConnection(connStr);
            return SqlConnection;
        }

        public void OpenConnection() {
            try {
                SqlConnection.Open();
            } catch(ConfigurationErrorsException) {
                throw;
            } catch(InvalidOperationException) {
                throw;
            } catch(SqlException) {
                throw;
            } catch (Exception) {
                throw;
            }
        }

        public void CloseConnection() {
            try {
                SqlCommand.Dispose();
                SqlCommand = null;
                SqlConnection.Close();
                SqlConnection = null;
            } catch(SqlException) {
                throw;
            }
        }
    }
}
