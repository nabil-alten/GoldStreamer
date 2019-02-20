using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BLL.DomainClasses
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
    }
}