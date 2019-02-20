using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{

    public class CalculateAverageBuySellService
    {
        public void GetWeekStartEnd(DateTime date, out DateTime weekStart, out DateTime weekEnd)
        {

            weekStart = date.AddDays(-6);
            weekEnd = date;
        }
        public void GetMonthStartEnd(DateTime date, out DateTime MonthStart, out DateTime MonthEnd)
        {
            MonthStart = date.AddDays(-29);
            MonthEnd = date;
        }
    }
}
 