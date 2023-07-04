using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.Backend.Core.Dto
{
    /// <summary>
    /// Đối tượng dữ liệu để lọc, sắp xếp, phân trang
    /// Created By: nguyenvanlongBG (3/7/2023)
    /// </summary>
    public class FilterSortPagingDto
    {
        public IEnumerable<FilterDto> Filter { get; set; }
        public IEnumerable<SortDto> Sort { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
