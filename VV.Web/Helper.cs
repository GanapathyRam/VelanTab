using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VV.Web
{
    public class Helper
    {
        public static string CurrentFiniancialYear()
        {
            int month = System.DateTime.Now.Month;
            var currentYear = Convert.ToString(DateTime.UtcNow.Year.ToString().Substring(2, 2));
            if (month > 3)
            {
                currentYear = Convert.ToString(Convert.ToInt16(currentYear));

            }
            return currentYear;
        }
    }
}