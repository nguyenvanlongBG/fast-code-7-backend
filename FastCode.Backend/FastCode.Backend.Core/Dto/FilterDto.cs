using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.Backend.Core.Dto
{
    /// <summary>
    /// Lớp lọc
    /// Created By: nguyenvanlongBG (3/7/2023)
    /// </summary>
    public class FilterDto
    {
        public string Name { get; set; }
        public int OperatorType { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
        public int RelationType { get; set; }
    }
}
