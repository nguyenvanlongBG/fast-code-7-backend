using Microsoft.AspNetCore.Mvc;

namespace FastCode.Backend.Controllers
{
    [ApiController]

    public abstract class WriteController<TEntity, TEntityDto, TEntityCreateDto, TEntityUpdateDto> : ReadController<TEntity, TEntityDto>
    {
        protected readonly IWriteService<TEntity, TEntityDto, TEntityCreateDto, TEntityUpdateDto> _writeService;
        protected WriteController(IWriteService<TEntity, TEntityDto, TEntityCreateDto, TEntityUpdateDto> writeService) : base((IReadService<TEntity, TEntityDto>)writeService)
        {
            _writeService = writeService;
        }
        /// <summary>
        /// Hàm xóa bản ghi dựa vào id
        /// Params: id( id đối tượng)
        /// Authod: nguyenvanlongBG (21/5/2023)
        /// ModifiedDate: 23/5/2023
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> DeleteAsync(Guid id)
        {
            var responseReturn = await _writeService.DeleteAsync(id);
            return Ok(responseReturn);
        }
        /// <summary>
        /// Hàm thêm 1 bản ghi
        /// Params: entityCreateDto(thông tin về bản ghi)
        /// Authod: nguyenvanlongBG (21/5/2023)
        /// ModifiedDate: 23/5/2023
        /// </summary>
        /// <returns></returns>
        [HttpPost()]

        public virtual async Task<IActionResult?> PostAsync([FromBody] TEntityCreateDto entityCreateDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                throw new ValidateException(errors, errors);

            }
            var responseReturn = await _writeService.PostAsync(entityCreateDto);
            return StatusCode(201, responseReturn);
        }
        /// <summary>
        /// Hàm cập nhật bản ghi
        /// Params: id( id đối tượng), entityUpdateDto (dữ liệu về bản ghi cập nhật)
        /// Authod: nguyenvanlongBG (21/5/2023)
        /// ModifiedDate: 23/5/2023
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPut("{id}")]
        public virtual async Task<IActionResult?> UpdateAsync(Guid id, [FromBody] TEntityUpdateDto entityUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                throw new ValidateException(errors, errors);

            }
            var responseReturn = await _writeService.UpdateAsync(id, entityUpdateDto);
            return Ok(responseReturn);
        }
        /// <summary>
        ///  Controller xóa nhiều bản ghi
        ///  Author: nguyenvanlongBG (29/5/2023)
        /// </summary>
        /// <param name="listID"></param> Mảng chứa danh sách Id bản ghi cần xóa
        /// <returns></returns>
        [HttpDelete()]
        public virtual async Task<IActionResult> DeleteMultiAsync(List<Guid> listID)
        {
            var response = await _writeService.DeleteMultiAsync(listID);
            return Ok(response);
        }
    }
}
