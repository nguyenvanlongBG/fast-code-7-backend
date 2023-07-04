using FastCode.Backend.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.Backend.Core.Interface.Service
{
    public interface IReadService<TEntity, TEntityDto>
    {
        Task<TEntityDto> GetAsync(Guid id);
        /// <summary>
        /// Lấy danh sách tất cả bản ghi
        /// Author: nguyenvanlongBG (29/5/2023)
        /// </summary>
        /// <returns></returns> IEnumerable<TEntityDto>
        Task<IEnumerable<TEntityDto>> GetListAsync();
        /// Lọc dữ liệu
        /// Author: nguyenvanlongBG ( 25/6/2023)
        /// </summary>
        /// <param name="filters"></param> Danh sách đối tượng lọc
        /// <param name="sorts"></param> Danh sách đối tượng sắp xếp
        /// <returns></returns>
        Task<object> FilterAsync(FilterSortPagingDto filterSortPagingDto);
    }
}
