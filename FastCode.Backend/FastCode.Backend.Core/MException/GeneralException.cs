using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.Backend.Core.MException
{
    /// <summary>
    /// Lớp lỗi chung kế thừa Exception
    /// Created By: nguyenvanlongBG (3/7/2023)
    /// </summary>
    public class GeneralException : Exception
    {
        public int ErrorCode
        {
            get; set;
        }
        public List<string> UserMessage
        {
            get; set;
        }
        public List<string> DevMessage
        {
            get; set;
        }
        public GeneralException(int errorCode, List<string> userMessage, List<string> devMessage)
        {
            ErrorCode = errorCode;
            UserMessage = userMessage;
            DevMessage = devMessage;
        }
    }
}
