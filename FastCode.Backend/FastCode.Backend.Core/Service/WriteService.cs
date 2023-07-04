using AutoMapper;
using FastCode.Backend.Core.Interface.Repository;
using FastCode.Backend.Core.Interface.Service;
using FastCode.Backend.Core.MException;
using FastCode.Backend.Core.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.Backend.Core.Service
{
    public abstract class WriteService<TEntity, TEntityDto, TEntityCreateDto, TEntityUpdateDto> : ReadService<TEntity, TEntityDto>, IWriteService<TEntity, TEntityDto, TEntityCreateDto, TEntityUpdateDto>
    {
        #region Properties
        // BaseRepository
        protected readonly IWriteRepository<TEntity> _writeRepository;

        protected readonly IMapper _mapper;
        #endregion
        /// <summary>
        /// Constructor lớp BaseService
        /// Author: nguyenvanlongBG (2/6/2023)
        /// </summary>
        /// <param name="baseRepository"></param> BaseRepository
        /// <param name="mapper"></param> Mapper
        public WriteService(IWriteRepository<TEntity> writeRepository, IMapper mapper) : base((IReadRepository<TEntity>)writeRepository, mapper)
        {
            _writeRepository = writeRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Tầng Bussiness hàm xóa bản ghi dựa vào id
        /// Created by: nguyenvanlongBG (21/5/2023)
        /// Modified By: nguyenvanlongBG (23/5/2023)
        /// </summary>
        /// <param name="id"></param> id bản ghi
        /// <returns>Trạng thái xóa 1: Thành công, 0: Xóa không thành công</returns>
        /// <exception cref="NotFoundException"></exception> Lỗi không tìn thấy bản ghi
        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _readRepository.GetAsync(id);
            List<string> userMessages = new();
            var devMessages = new List<string>();
            if (entity == null)
            {
                Type entityType = typeof(TEntity);
                string entityName = entityType.Name;
                userMessages.Add(EntityResource.notFound);
                devMessages.Add(EntityResource.notFoundID(entityName, id.ToString()));
                throw new NotFoundException(userMessages, devMessages);
            }
            var check = await _writeRepository.DeleteAsync(id);
            return check;
        }
        /// <summary>
        /// Xóa nhiều bản ghi
        /// Author: nguyenvanlongBG (25/5/2023)
        /// ModifiedDate: 25/5/2023
        /// </summary>
        /// <param name="listID"></param> Danh sách id bản ghi cần xóa
        /// <returns></returns>
        public async Task<int> DeleteMultiAsync(List<Guid> listID)
        {
            var numberDeleted = await _writeRepository.DeleteMultiAsync(listID);
            return numberDeleted;
        }

        /// <summary>
        /// Tầng Bussiness thêm 1 bản ghi
        /// Author: nguyenvanlongBG (21/5/2023)
        /// ModifiedDate: 23/5/2023
        /// </summary>
        /// <param name="entityCreateDto"></param> Dữ liệu chứa các tường cần thiết do người dùng nhập
        /// <returns>Bản ghi sau khi đã thêm với các trường cần thiết trả về cho người dùng</returns>
        public virtual async Task<int> PostAsync(TEntityCreateDto entityCreateDto)
        {
            List<string> userMessages = new List<string>();
            List<string> devMessages = new List<string>();
            if (entityCreateDto != null)
            {
                var entity = _mapper.Map<TEntity>(entityCreateDto);
                await ValidateBeforeCreate(entityCreateDto);
                var entityCreated = await _writeRepository.PostAsync(entity);
                return 1;
            }
            else
            {
                userMessages.Add(ErrorResource.errorRequest);
                devMessages.Add(ErrorResource.emptyRequest);
                throw new ValidateException(userMessages, devMessages);
            }
            return 0;
        }
        /// <summary>
        /// Cập nhật thông tin bản ghi
        /// Author: nguyenvanlongBG (21/5/2023)
        /// ModifiedDate: 23/5/2023
        /// </summary>
        /// <param name="entityUpdateDto"></param>
        /// <returns>Bản ghi sau khi cập nhật với các tường cần thiết trong thông tin trả về người dùng</returns>
        public virtual async Task<int> UpdateAsync(Guid id, TEntityUpdateDto entityUpdateDto)
        {
            List<string> userMessages = new List<string>();
            List<string> devMessages = new List<string>();
            if (entityUpdateDto != null)
            {
                await ValidateBeforeUpdate(id, entityUpdateDto);
                var entity = _mapper.Map<TEntity>(entityUpdateDto);
                var status = await _baseRepository.UpdateAsync(id, entity);
                return status;
            }
            else
            {
                userMessages.Add(ErrorResource.errorRequest);
                devMessages.Add(ErrorResource.emptyRequest);
                throw new ValidateException(userMessages, devMessages);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityCreateDto"></param>
        /// <returns></returns>
        public virtual async Task<bool> ValidateBeforeCreate(TEntityCreateDto entityCreateDto)
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entityUpdateDto"></param>
        /// <returns></returns>
        public virtual async Task<bool> ValidateBeforeUpdate(Guid id, TEntityUpdateDto entityUpdateDto)
        {
            return true;

        }
    }
}
