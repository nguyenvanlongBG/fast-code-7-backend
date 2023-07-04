using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.Backend.Infrastructure.Constant
{
    public static class SortConstant
    {
        public static Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>
        {
            { "Merchandise", new string[] { "SKUCode", "Name", "CategoryName", "UnitName", "AverageSellprice" } },
            { "Unit", new string[] {  } }
        };
        public static string GetQuerySort(int type, string name)
        {
            if (type == SortType.DESC)
            {
                return " " + name + " DESC";
            }
            else
            {
                return " " + name + " ASC";
            }
            return "";
        }
        public static string[] GetSortColumnName(string tableName)
        {
            return dictionary[tableName];
        }
    }
}
