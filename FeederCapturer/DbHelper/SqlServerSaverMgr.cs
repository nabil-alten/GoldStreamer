using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FeederCapturer.DbHelper
{
    public class SqlServerSaverMgr
    {
        public SqlServerSaverMgr(string connectionString)
        {
            ConnectionString = connectionString;
            Outputs = new Dictionary<int, object>();
        }

        public SqlConnection SqlCon { get; private set; }
        public SqlCommand SqlCmd { get; private set; }
        public Dictionary<int, object> Outputs { get; private set; }
        public string ConnectionString { get; set; }

        public void SaveElement(SqlServerSavableEntity entity)
        {
            PrepareCon();
            SqlCmd.CommandType = CommandType.StoredProcedure;
            string cmdText = "dbo.sp_" + entity.ElementName + "_insert";
            SqlCmd.CommandText = cmdText;

            foreach (KeyValuePair<string, object> pair in entity.Attributes)
                SqlCmd.Parameters.AddWithValue(pair.Key, pair.Value);

            Outputs.Clear();
            foreach (int output in entity.OutputParameters)
            {
                SqlCmd.Parameters[output].Direction = ParameterDirection.Output;
                Outputs.Add(output, 0);
            }
            SqlCmd.ExecuteNonQuery();

            foreach (int output in entity.OutputParameters)
                Outputs[output] = SqlCmd.Parameters[output].Value;
            CloseCon();
        }

        public void UpdateTraderPricesChart(SqlServerSavableEntity entity)
        {
            PrepareCon();
            SqlCmd.CommandType = CommandType.StoredProcedure;
            string cmdText = "dbo.sp_" + entity.ElementName + "_update";
            SqlCmd.CommandText = cmdText;

            foreach (KeyValuePair<string, object> pair in entity.Attributes)
                SqlCmd.Parameters.AddWithValue(pair.Key, pair.Value);

            Outputs.Clear();
            foreach (int output in entity.OutputParameters)
            {
                SqlCmd.Parameters[output].Direction = ParameterDirection.Output;
                Outputs.Add(output, 0);
            }
            SqlCmd.ExecuteNonQuery();

            foreach (int output in entity.OutputParameters)
                Outputs[output] = SqlCmd.Parameters[output].Value;
            CloseCon();
        }
        public void PrepareCon()
        {
            SqlCon = new SqlConnection(ConnectionString);
            if (SqlCon.State != ConnectionState.Open)
                SqlCon.Open();
            SqlCmd = new SqlCommand { Connection = SqlCon };
        }
        public void CloseCon()
        {
            SqlCon.Close();
            SqlCon.Dispose();
            SqlCmd.Dispose();
        }
    }
}
