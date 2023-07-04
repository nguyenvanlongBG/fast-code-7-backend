using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.Backend.Core.MException
{
    public class ValidateException : GeneralException
    {
        public ValidateException(List<string> userMessage, List<string> devMessage) : base(400, userMessage, devMessage)
        {

        }
    }
}
