using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.Backend.Core.Interface.Repository
{
    public interface IWriteRepository<TEntity>
    {
        Task<TEntity> PostAsync(TEntity entityCreateDto);
        Task<int> PostManyAsync(IEnumerable<TEntity> listEntityCreateDto);
        Task<int> UpdateAsync(Guid id, TEntity entityUpdateDto);
        Task<bool> DeleteAsync(Guid id);
        Task<int> DeleteMultiAsync(List<Guid> listID);
    }
}
