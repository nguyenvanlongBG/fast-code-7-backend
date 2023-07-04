namespace FastCode.Backend.Controllers
{
    [ApiController]

    public abstract class ReadController<TEntity, TEntityDto> : ControllerBase
    {
        protected readonly Core.Interface.Service.IReadService<TEntity, TEntityDto> _readService;
        /// <summary>
        /// Lớp BaseController
        /// Author: nguyenvanlongBG (2/6/2023)
        /// </summary>
        /// <param name="baseService"></param> Thể hiện của IBaseService để khởi tạo BaseController
        public ReadController(IReadService<TEntity, TEntityDto> readService)
        {
            _readService = readService;
        }
        /// <summary>
        /// Hàm lấy danh sách bản ghi
        /// Authod: nguyenvanlongBG (21/5/2023)
        /// ModifiedDate: 23/5/2023
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public virtual async Task<IActionResult?> GetListAsync()
        {
            var responseReturn = await _readService.GetListAsync();
            return Ok(responseReturn);
        }
        /// <summary>
        /// Hàm lấy ra 1 bản ghi dựa vào id
        /// Params: id (id đối tượng)
        /// Authod: nguyenvanlongBG (21/5/2023)
        /// ModifiedDate: 23/5/2023
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public virtual async Task<IActionResult?> GetAsync(Guid id)
        {
            var responseReturn = await _readService.GetAsync(id);
            return Ok(responseReturn);
        }
    }
}
