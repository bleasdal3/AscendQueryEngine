using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendQueryEngine
{
    public static class DateCriteriaData
    {
        /* A word about this class - 
         * I need a way to pass information between the two windows.
         * I could mess about creating my own event handler 
         * and subscribe the button to the other windows event...
         * or... I could just use this one small class as a repository
         * and wipe it after use. \_Ò.Ó_/
         */
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
