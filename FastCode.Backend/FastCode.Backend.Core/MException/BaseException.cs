using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FastCode.Backend.Core.MException
{
    /// <summary>
    /// Lớp BaseException trả về
    /// Created By: nguyenvanlongBG (3/7/2023)
    /// </summary>
    public class BaseException
    {
        #region Properties
        // Mã lỗi
        public int ErrorCode
        {
            get; set;
        }
        // Danh sách lỗi trả cho người dùng
        public List<string> UserMessage
        {
            get; set;
        }
        // Danh sách lỗi trả cho Dev
        public List<string> DevMessage
        {
            get; set;

        }
        // Mã truy xuất
        public string TraceId
        {
            get; set;
        }
        public string MoreInfo
        {
            get; set;
        }
        #endregion
        #region Methods
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
        #endregion
    }
}
