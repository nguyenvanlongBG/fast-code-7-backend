using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.Backend.Core.Interface.Service
{
    public interface IWriteService<TEntity, TEntityDto, TEntityCreateDto, TEntityUpdateDto> : IReadService<TEntity, TEntityDto>
    {
        /// <summary>
        /// Tạo bản ghi
        /// Author: nguyenvanlongBG(29/5/2023)
        /// </summary>
        /// <param name="entityCreateDto"></param> Thông tin bản ghi thêm mới
        /// <returns></returns> TEntityDto
        Task<int> PostAsync(TEntityCreateDto entityCreateDto);
        /// <summary>
        /// Cập nhật 1 bản ghi
        /// Author: nguyenvanlongBG (29/5/2023)
        /// </summary>
        /// <param name="id"></param> ID nhân viên
        /// <param name="entityUpdateDto"></param>Thông tin nhân viên cập nhật
        /// <returns></returns> TEntityDto
        Task<int> UpdateAsync(Guid id, TEntityUpdateDto entityUpdateDto);
        Task<bool> DeleteAsync(Guid id);
        /// <summary>
        /// Xóa nhiều bản ghi
        /// Author: nguyenvanlongBG (20/5/2023)
        /// </summary>
        /// <param name="listID"></param> Mảng chứa danh sách ID nhân viên cần xóa
        /// <returns></returns>
        Task<int> DeleteMultiAsync(List<Guid> listID);
        /// <summary>
        /// Validate trước khi tạo mới bản ghi
        /// Author: nguyevanlongBG (28/5/2023)
        /// </summary>
        /// <param name="entityCreateDto"></param> dữ liệu bản ghi mới
        /// <returns></returns> pass: true, dont pass: false
        Task<bool> ValidateBeforeCreate(TEntityCreateDto entityCreateDto);
        /// <summary>
        /// Validate trước khi cập nhật bản ghi
        /// Author: nguyenvanlongBG (28/5/2023)
        /// </summary>
        /// <param name="entityUpdateDto"></param> dữ liệu bản ghi cập nhật
        /// <returns></returns> Pass: true, Dont pass: false

    }
}
