using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DomainClasses;
using DAL.Hubs;

namespace DAL
{
    public class GlobalPriceRepository
    {
        public IEnumerable<GlobalPrice> GetData(decimal dollarPrice)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GoldStreamerContext"].ConnectionString))
            {
                string todaysDate = DateTime.Now.ToString("yyyy/MM/dd");
                connection.Open();
                using (SqlCommand command = new SqlCommand(@"SELECT [Id], [Ask], [Bid], [Open], [High], [Low], [Close], [CaptureTime] FROM [dbo].[GlobalPrice] Where [CaptureTime]  >= '" + todaysDate + "'", connection))
                
                {
                    command.Notification = null;

                    SqlDependency dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    using (var reader = command.ExecuteReader())
                        return reader.Cast<IDataRecord>().Select(record => new GlobalPrice()
                            {
                                Id = record.GetInt32(0),
                                Ask = Math.Round(record.GetDecimal(1) * dollarPrice, 2),
                                Bid = record.GetDecimal(2),
                                Open = Math.Round(record.GetDecimal(3) * dollarPrice, 2),
                                High = Math.Round(record.GetDecimal(4) * dollarPrice, 2),
                                Low = Math.Round(record.GetDecimal(5) * dollarPrice, 2),
                                Close = Math.Round(record.GetDecimal(6) * dollarPrice, 2),
                                CaptureTime = record.GetDateTime(7),
                            }).ToList();
                }
            }
        }
        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            GlobalPriceHub.ShowGlobalPrices();
        }
    }

}

