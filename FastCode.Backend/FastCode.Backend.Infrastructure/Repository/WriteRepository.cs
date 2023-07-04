using Dapper;
using FastCode.Backend.Core.Interface.Repository;
using FastCode.Backend.Core.MException;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.Backend.Infrastructure.Repository
{
    public abstract class WriteRepository<TEntity> : ReadRepository<TEntity>, IWriteRepository<TEntity>
    {
        protected WriteRepository(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Tầng giao tiếp csdl Hàm xóa bản ghi trong csdl
        /// Author: nguyenvanlongBG (21/5/2023)
        /// ModifiedDate: 23/5/2023
        /// </summary>
        /// <param name="id"></param> id bản ghi
        /// <returns>Trạng thái xóa thành công hay thất bại 1: Thành công, 0: Thất bại</returns>
        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            Type entityType = typeof(TEntity);
            string entityName = entityType.Name;
            var connection = await GetOpenConnectionAsync();
            string procDeleteName = "Proc_" + entityName + "_Delete";
            DynamicParameters parameters = new DynamicParameters();
            string entityIDName = "@" + entityName + "ID";
            parameters.Add(entityIDName, id);
            var transaction = connection.BeginTransaction();
            var check = 0;
            try
            {
                check = await connection.QueryFirstOrDefaultAsync<int>(procDeleteName, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                List<string> errors = new List<string>();
                errors.Add(ex.Message);
                throw new InternalException(errors, errors);
            }
            finally
            {
                transaction.Dispose();
                connection.Dispose();
                connection.Close();
            }
            if (check > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Xóa nhiều bản ghi 
        /// Author: nguyenvanlongBG (25/5/2023)
        /// ModifiedDate: 25/5/2023
        /// </summary>
        /// <param name="listID"></param>Danh sách id bản ghi cần xóa
        /// <returns></returns> Trạng thái xóa 1: Thành công 0: Thất bại
        public async Task<int> DeleteMultiAsync(List<Guid> listID)
        {
            Type entityType = typeof(TEntity);
            string entityName = entityType.Name;
            var connection = await GetOpenConnectionAsync();
            var procDeleteMultiName = "Proc_" + entityName + "_DeleteMulti";
            DynamicParameters parameters = new DynamicParameters();
            string ids = "";
            foreach (var id in listID)
            {
                ids += "," + id.ToString();
            }
            parameters.Add("listID", ids);
            var transaction = connection.BeginTransaction();
            int countSuccessRecord = 0;
            try
            {
                countSuccessRecord = await connection.ExecuteAsync(procDeleteMultiName, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                transaction.Commit();
                connection.Close();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                List<string> errors = new List<string>();
                errors.Add(ex.Message);
                throw new InternalException(errors, errors);
            }
            finally
            {
                transaction?.Dispose();
                connection?.Dispose();
                connection.Close();
            }
            return countSuccessRecord;
        }

        /// <summary>
        /// Tàng giao tiếp với csdl thêm 1 bản ghi
        /// Author: nguyenvanlongBG (21/5/2023)
        /// ModifiedDate: 23/5/2023
        /// </summary>
        /// <param name="entity"></param>Thông tin đối tượng cần thêm
        /// <returns></returns>Thông tin đối tượng sau khi thêm
        public async Task<TEntity?> PostAsync(TEntity entity)
        {
            Type entityType = typeof(TEntity);
            string entityName = entityType.Name;
            PropertyInfo createdDateProperty = entityType.GetProperty("CreatedDate");
            PropertyInfo createdByProperty = entityType.GetProperty("CreatedBy");
            PropertyInfo entityIDProperty = entityType.GetProperty(entityName + "ID");
            Guid guid = Guid.NewGuid();
            createdDateProperty.SetValue(entity, DateTime.Now);
            entityIDProperty.SetValue(entity, guid);
            createdByProperty.SetValue(entity, "Admin");
            // In ra tên của các thuộc tính
            var connection = await GetOpenConnectionAsync();
            var procCreateName = "Proc_" + entityName + "_Insert";
            var transaction = connection.BeginTransaction();
            int check = 0;
            try
            {
                check = await connection.ExecuteAsync(procCreateName, entity, commandType: CommandType.StoredProcedure, transaction: transaction);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                List<string> errors = new List<string>();
                errors.Add(ex.Message);
                throw new InternalException(errors, errors);
            }
            finally
            {
                transaction.Dispose();
                connection.Dispose();
                connection.Close();
            }
            if (check == 1)
            {
                var entityCreated = await GetAsync(guid);
                return entityCreated;
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Tàng giao tiếp với csdl thêm nhiều bản ghi
        /// Author: nguyenvanlongBG (1/6/2023)
        /// ModifiedDate: 
        /// </summary>
        /// <param name="entity"></param>Thông tin đối tượng cần thêm
        /// <returns></returns>Thông tin đối tượng sau khi thêm
        public async Task<int> PostManyAsync(IEnumerable<TEntity> listEntity)
        {
            Type entityType = typeof(TEntity);
            string entityName = entityType.Name;
            PropertyInfo createdDateProperty = entityType.GetProperty("CreatedDate");
            PropertyInfo createdByProperty = entityType.GetProperty("CreatedBy");
            PropertyInfo entityIDProperty = entityType.GetProperty(entityName + "ID");
            foreach (var entity in listEntity)
            {
                Guid guid = Guid.NewGuid();
                createdDateProperty.SetValue(entity, DateTime.Now);
                entityIDProperty.SetValue(entity, guid);
                createdByProperty.SetValue(entity, "Admin");
            }
            var connection = await GetOpenConnectionAsync();
            var procCreateManyName = "Proc_" + entityName + "_InsertMany";
            var number = 0;
            var transaction = connection.BeginTransaction();
            try
            {
                number = await connection.ExecuteAsync(procCreateManyName, listEntity, commandType: CommandType.StoredProcedure, transaction: transaction);
                transaction.Commit();
                connection.Close();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                List<string> errors = new List<string>();
                errors.Add(ex.Message);
                throw new InternalException(errors, errors);
            }
            finally
            {
                transaction.Dispose();
                connection.Dispose();
                connection.Close();
            }
            return number;
        }

        /// <summary>
        /// Tầng giao tiếp với csdl ập nhật thông tin 1 bản ghi
        /// Author: nguyenvanlongBG (21/5/2023)
        /// ModifiedDate: 23/5/2023
        /// </summary>
        /// <param name="id"></param>id bản ghi. 
        /// <param name="entity"></param>Thông tin đối tượng cần cập nhật
        /// <returns></returns>Thông tin bản ghi sau khi cập nhật
        public virtual async Task<int> UpdateAsync(Guid id, TEntity entity)
        {
            Type entityType = typeof(TEntity);
            string entityName = entityType.Name;
            PropertyInfo[] properties = typeof(TEntity).GetProperties();
            DynamicParameters parameters = new DynamicParameters();
            // Lấy ra tên của các thuộc tính
            foreach (var property in properties)
            {
                string paramName = "@" + property.Name;
                if (paramName == "@ModifiedDate")
                {
                    parameters.Add(paramName, DateTime.Now);
                }
                else
                {
                    if (paramName == "@ModifiedBy")
                    {
                        parameters.Add(paramName, "Admin");
                    }
                    else
                    {
                        parameters.Add(paramName, property.GetValue(entity));
                        var value = property.GetValue(entity);
                    }
                }
            }
            var connection = await GetOpenConnectionAsync();
            var procUpdateName = "Proc_" + entityName + "_Update";
            var transaction = connection.BeginTransaction();
            TEntity entityUpdated;
            try
            {
                await connection.QueryFirstOrDefaultAsync<TEntity>(procUpdateName, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                transaction.Commit();
                entityUpdated = await GetAsync(id);
                connection.Close();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                List<string> errors = new List<string>();
                errors.Add(ex.Message);
                throw new InternalException(errors, errors);
            }
            finally
            {
                transaction.Dispose();
                connection.Dispose();
                connection.Close();
            }
            if (entityUpdated != null)
            {
                return 1;
            }
            return 0;
        }
    }
}
