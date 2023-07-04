using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.Backend.Core.Resource
{
    public static class EntityResource
    {
        public const string notFound = "Không tìm thấy bản ghi.";
        public static string notFoundID(string entityName, string id)
        {
            return $"Không tìm thấy {entityName} có Id {id}.";
        }
    }
}
