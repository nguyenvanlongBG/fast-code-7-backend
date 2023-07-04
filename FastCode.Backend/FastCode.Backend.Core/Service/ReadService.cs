using AutoMapper;
using FastCode.Backend.Core.Constant;
using FastCode.Backend.Core.Dto;
using FastCode.Backend.Core.Enum;
using FastCode.Backend.Core.Interface.Repository;
using FastCode.Backend.Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.Backend.Core.Service
{
    public class ReadService<TEntity, TEntityDto> : IReadService<TEntity, TEntityDto>
    {
        #region Properties
        // BaseRepository
        protected readonly IReadRepository<TEntity> _readRepository;
        protected readonly IMapper _mapper;
        #endregion
        #region Constructor
        /// <summary>
        /// Constructor lớp BaseService
        /// Author: nguyenvanlongBG (2/6/2023)
        /// </summary>
        /// <param name="baseRepository"></param> BaseRepository
        /// <param name="mapper"></param> Mapper
        public ReadService( IReadRepository<TEntity> readRepository, IMapper mapper)
        {
            _readRepository = readRepository;
            _mapper = mapper;
        }
        #endregion
        public Task<object> FilterAsync(List<FilterDto> filters, List<SortDto> sorts, int pageSize, int offset)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Lọc dữ liệu
        /// Author: nguyenvanlongBG ( 25/6/2023)
        /// </summary>
        /// <param name="filters"></param> Danh sách đối tượng lọc
        /// <param name="sorts"></param> Danh sách đối tượng sắp xếp
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual async Task<object> FilterAsync(FilterSortPagingDto filterSortPagingDto)
        {
            var filterListHasValue = new List<FilterDto>();
            var sortListHasValue = new List<SortDto>();
            Type entityType = typeof(TEntity);
            string entityName = entityType.Name;
            var listFilterName = FilterConstant.GetFilterColumnName(entityName);
            var listSortName = SortConstant.GetSortColumnName(entityName);
            var filters = filterSortPagingDto.Filter;
            var sorts = filterSortPagingDto.Sort;
            int pageSize = 10;
            int offset = 0;
            if (filterSortPagingDto.PageSize != null)
            {
                pageSize = filterSortPagingDto.PageSize;
            }
            if (filterSortPagingDto.PageIndex != null)
            {
                if (filterSortPagingDto.PageIndex == 0)
                {
                    filterSortPagingDto.PageIndex = 1;
                }
                offset = (filterSortPagingDto.PageIndex - 1) * pageSize;
            }
            else
            {
                offset = 0;
            }
            foreach (var filter in filters)
            {
                if (filter.Value != "" && listFilterName.Contains(filter.Name))
                {
                    filterListHasValue.Add(filter);
                }
            }
            foreach (var sort in sorts)
            {
                if (sort.Type >= SortType.MIN && sort.Type <= SortType.MAX && listSortName.Contains(sort.Name))
                {
                    sortListHasValue.Add(sort);
                }
            }
            var data = await _readRepository.FilterAsync(filterListHasValue, sortListHasValue, pageSize, offset);
            if (data != null)
            {
                var propertyEntity = data.GetType().GetProperty("entities");
                var listEntity = (IEnumerable<TEntity>)propertyEntity.GetValue(data);
                var listEntityDto = _mapper.Map<IEnumerable<TEntity>, IEnumerable<TEntityDto>>(listEntity);
                var propertyCount = data.GetType().GetProperty("totalRecord");
                int count = (int)propertyCount.GetValue(data);
                var listEmployeePaginated = PaginatedList<TEntityDto>.Create(listEntityDto.ToList(), count, filterSortPagingDto.PageIndex, pageSize);
                return listEmployeePaginated;
            }
            return null;
        }



        /// <summary>
        /// Tầng Bussiness hàm lấy ra 1 bản ghi dựa vào id
        /// Author: nguyenvanlongBG (21/5/2023)
        /// ModifiedDate: 23/5/2023
        /// </summary>
        /// <param name="id"></param> id bản ghi
        /// <returns> Bản ghi chứa các trường nhu trong Dto trả về</returns>
        public virtual async Task<TEntityDto> GetAsync(Guid id)
        {
            var entity = await _readRepository.GetAsync(id);
            if (entity != null)
            {
                var entityDto = _mapper.Map<TEntityDto>(entity);
                return entityDto;
            }
            return default;
        }
        /// <summary>
        /// Tầng Bussiness hàm lấy ra danh sách bản ghi
        /// Author: nguyenvanlongBG (21/5/2023)
        /// ModifiedDate: 23/5/2023
        /// </summary>
        /// <returns>Danh sách bản ghi với thông tin các trường cần thiết với ngườu dùng</returns>
        public virtual async Task<IEnumerable<TEntityDto>> GetListAsync()
        {
            var listEntity = await _readRepository.GetListAsync();
            var listEntityDto = _mapper.Map<IEnumerable<TEntity>, IEnumerable<TEntityDto>>(listEntity);
            return listEntityDto;
        }
    }
}
