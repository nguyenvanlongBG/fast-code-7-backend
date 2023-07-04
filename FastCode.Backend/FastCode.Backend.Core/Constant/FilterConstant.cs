using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.Backend.Core.Constant
{
    public static class FilterConstant
    {
        public static Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>
        {
            { "Merchandise", new string[] { "SKUCode", "Name", "Status", "DisplaySellScreen", "CategoryName", "UnitName", "AverageSellprice" } },
            { "Unit", new string[] {  } }
        };
        public static string operatorValueFilter(int type, string name)
        {
            switch (type)
            {
                case 0: return " LIKE CONCAT('%', '" + name + "' ,'%')";
                case 1: return " = '" + name + "'";
                case 2: return " LIKE CONCAT('', '" + name + "', '%')";
                case 3: return " LIKE CONCAT('%', '" + name + "','')";
                case 4: return " NOT LIKE CONCAT('%', '" + name + "','%')";
                case 5: return " <" + name + "";
                case 6: return " <=" + name + "";
                case 7: return " >" + name + "";
                case 8: return " >=" + name + "";
                default: return null;
            }
        }
        public static string paramValueFilter(int type, string value)
        {
            switch (type)
            {
                case 0: return value;
                case 1: return value;
                case 2: return value;
                case 3: return value;
                case 4: return value;
                default: return null;
            }
        }
        public static string[] GetFilterColumnName(string tableName)
        {
            return dictionary[tableName];
        }
    }
}
