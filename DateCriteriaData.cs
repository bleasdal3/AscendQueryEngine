using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendQueryEngine
{
    public static class DateCriteriaData
    {
        public static string ColumnName { get; set; }
        public static string StartDate {get; set;}

        public static string EndDate { get; set; }

        public static string Clause { get; set; }

        public static string ConstructClause()
        {
            Clause = ColumnName + " BETWEEN '" + StartDate + "' AND '" + EndDate + "'";          
            StartDate = string.Empty; //reset
            EndDate = string.Empty;
            return Clause;
        }

    }

}
