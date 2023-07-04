using FastCode.Backend.Core.Dto;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.Backend.Core.Interface.Repository
{
    public interface IReadRepository<TEntity>
    {
        /// <summary>
        /// Kết nối database
        /// Created By: nguyenvanlongBG (5/7/2023
        /// </summary>
        /// <returns></returns> Đối tượng DBConnection
        Task<DbConnection> GetOpenConnectionAsync();
        /// <summary>
        /// Lấy ra 1 bản ghi
        /// Created By: nguyenvanlongBG (5/7/2023)
        /// </summary>
        /// <param name="id"></param> ID đối tượng
        /// <returns></returns>
        Task<TEntity?> GetAsync(Guid id);
        Task<IEnumerable<TEntity>> GetListAsync();
        /// <summary>
        /// Lọc dữ liệu
        /// Author: nguyenvanlongBG ( 25/6/2023)
        /// </summary>
        /// <param name="filters"></param> Danh sách đối tượng lọc
        /// <param name="sorts"></param> Danh sách đối tượng sắp xếp
        /// <returns></returns>
        Task<object> FilterAsync(List<FilterDto> filters, List<SortDto> sorts, int pageSize, int offset);
    }
}
