using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.Backend.Core.Entity
{
    /// <summary>
    /// Lớp BaseEntity
    /// Created By: nguyenvanlongBG (24/6/2023
    /// </summary>
    public class BaseEntity
    {
        // Ngày tạo
        public DateTime CreatedDate
        {
            get; set;
        }
        // Người tạo
        public string CreatedBy
        {
            get; set;
        }
        // Ngày cập nhật
        public DateTime? ModifiedDate
        {
            get;
            set;
        }
        // Người cập nhật
        public string? ModifiedBy
        {
            get; set;

        }
    }
}
