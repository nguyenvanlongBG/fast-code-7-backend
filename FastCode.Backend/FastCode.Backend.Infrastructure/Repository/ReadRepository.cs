using Dapper;
using FastCode.Backend.Core.Constant;
using FastCode.Backend.Core.Dto;
using FastCode.Backend.Core.Interface.Repository;
using FastCode.Backend.Core.MException;
using FastCode.Backend.Core.Resource;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.Backend.Infrastructure.Repository
{
    public abstract class ReadRepository<TEntity> : IReadRepository<TEntity>
    {
        #region Property
        // Chuỗi cấu hình kết nối database
        protected readonly string _connectionString;
        #endregion
        #region Constructor
        /// <summary>
        /// Constructor BaseRepository để khởi tạo các lớp kế thừa của BaseRepository
        /// Author: nguyenvanlongBG (2/6/2023)
        /// </summary>
        /// <param name="configuration"></param> Thể hiện của IConfiguration
        public ReadRepository(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionString"] ?? "";
        }
        #endregion

        /// <summary>
        /// Hàm kế nối với csdl
        /// Author: nguyenvanlongBG (21/5/2023)
        /// ModifiedDate: 23/5/2023
        /// </summary>
        /// <returns></returns>Kết nối
        public async Task<DbConnection> GetOpenConnectionAsync()
        {
            var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
        /// <summary>
        /// Hàm lọc dữ liệu
        /// Created By: nguyenvanlongBG (5/7/2023
        /// </summary>
        /// <param name="filters"></param> Danh sách điều kiện lọc
        /// <param name="sorts"></param> Danh sách điều kiện sắp xêp
        /// <param name="pageSize"></param> Số bản ghi trên 1 trang
        /// <param name="offset"></param> Chỉ số bản ghi bắt đầu
        /// <returns></returns> object data gồm danh sách dữ liệu và tổng số bản ghi database
        public async Task<object> FilterAsync(List<FilterDto> filters, List<SortDto> sorts, int pageSize, int offset)
        {
            Type entityType = typeof(TEntity);
            string entityName = entityType.Name;
            var connection = await GetOpenConnectionAsync();
            var procGetListName = "Proc_" + entityName + "_GetList";
            string filterString = "";
            DynamicParameters parameters = new DynamicParameters();
            var listFilterName = FilterConstant.GetFilterColumnName(entityName);
            foreach (var filter in filters)
            {
                filterString += " AND " + "temp_table." + filter.Name + " " + FilterConstant.operatorValueFilter(filter.OperatorType, filter.Value);
                //parameters.Add(filter.Name, FilterConstant.paramValueFilter( filter.OperatorType,filter.Value));
            }
            if (sorts.Count > 0)
            {
                filterString += " ORDER BY";
            }
            var lengthSort = sorts.Count;
            for (int i = 0; i < lengthSort; i++)
            {
                filterString += SortConstant.GetQuerySort(sorts[i].Type, sorts[i].Name);
                if (i < lengthSort - 1)
                {
                    filterString += ",";
                }
            }
            parameters.Add("FilterString", filterString);
            parameters.Add("PageSize", pageSize);
            parameters.Add("Offset", offset);
            //var listMerchandise = await connection.QueryAsync<TEntity>(strSQL, parameters);
            //var listMerchandise = await connection.QueryAsync<TEntity>(procGetListName, parameters, commandType: CommandType.StoredProcedure);
            var multi = await connection.QueryMultipleAsync(procGetListName, parameters, commandType: CommandType.StoredProcedure);
            var listMerchandise = multi.Read<TEntity>().AsList();
            int totalCount = await multi.ReadSingleAsync<int>();
            var data = new
            {
                entities = listMerchandise,
                totalRecord = totalCount
            };
            return data;
        }


        /// <summary>
        /// Tầng giao tiếp với csdl hàm lấy danh sách bản ghi
        /// Author: nguyenvanlongBG (21/5/2023)
        /// ModifiedDAte: 23/5/2023
        /// </summary>
        /// <returns></returns>Danh sách bản ghi
        public virtual async Task<IEnumerable<TEntity>> GetListAsync()
        {
            Type entityType = typeof(TEntity);
            string entityName = entityType.Name;
            var connection = await GetOpenConnectionAsync();
            var procGetListName = "Proc_" + entityName + "_GetList";
            IEnumerable<TEntity> listEntity;
            try
            {
                listEntity = await connection.QueryAsync<TEntity>(procGetListName, commandType: CommandType.StoredProcedure);
                return listEntity;
            }
            catch (Exception ex)
            {
                List<string> errors = new List<string>();
                errors.Add(ex.Message);
                throw new InternalException(errors, errors);
            }
            finally
            {
                connection.Dispose();
                connection.Close();
            }
            if (listEntity != null)
            {
                return listEntity;
            }
            return default;
        }

        /// <summary>
        /// Tầng giao tiếp csdl Hàm lấy ra 1 bản ghi theo id
        /// Author: nguyenvanlongBG (21/5/2023)
        /// ModifiedDate: 23/5/2023
        /// </summary>
        /// <param name="id"></param> id bản ghi
        /// <returns></returns>Đối tượng chứa các trường thông tin trả vè người dùng

        public virtual async Task<TEntity?> GetAsync(Guid id)
        {
            Type entityType = typeof(TEntity);
            string entityName = entityType.Name;
            List<string> userMessages = new List<string>();
            List<string> devMessages = new List<string>();
            var connection = await GetOpenConnectionAsync();
            string procGetByIDName = "Proc_" + entityName + "_GetByID";
            DynamicParameters parameters = new DynamicParameters();
            string entityIDName = "@" + entityName + "ID";
            parameters.Add(entityIDName, id);
            var transaction = connection.BeginTransaction();
            try
            {
                var entity = await connection.QueryFirstOrDefaultAsync<TEntity>(procGetByIDName, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                transaction.Commit();
                if (entity != null)
                {
                    return entity;
                }
                else
                {
                    userMessages.Add(EntityResource.notFound);
                    devMessages.Add(EntityResource.notFoundID(entityName, id.ToString()));
                    throw new NotFoundException(userMessages, devMessages);
                }
            }
            catch (Exception e)
            {
                transaction.Rollback();
            }
            finally
            {
                transaction.Dispose();
                connection.Close();
                connection.Dispose();
            }
            return default;
        }
    }
}
