using System;
using FeederCapturer.DbHelper;

namespace FeederCapturer.Models
{
    public class GlobalPrice
    {
        public int Id { get; set; }
        public decimal Ask { get; set; }
        public decimal Bid { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public DateTime CaptureTime { get; set; }

        public SqlServerSavableEntity Entity { get; set; }
        public void SaveToEntity()
        {
            Entity = new SqlServerSavableEntity(GetType().Name.ToLower());
            Entity.Attributes.Add("@id", Id);
            Entity.Attributes.Add("@ask", Ask);
            Entity.Attributes.Add("@bid", Bid);
            Entity.Attributes.Add("@open", Open);
            Entity.Attributes.Add("@high", High);
            Entity.Attributes.Add("@low", Low);
            Entity.Attributes.Add("@close", Close);
            Entity.Attributes.Add("@capture_time", CaptureTime);
            Entity.OutputParameters.Add(0);
        }
    }
}
