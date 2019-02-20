using System;
using FeederCapturer.DbHelper;

namespace FeederCapturer.Models
{
    public class TraderPricesChart
    {
        public int GlobalPriceId { get; set; }
        public SqlServerSavableEntity Entity { get; set; }
        public void SaveToEntity()
        {
            Entity = new SqlServerSavableEntity(GetType().Name.ToLower());

            Entity.Attributes.Add("@global_price_id", GlobalPriceId);
        }
    }
}
