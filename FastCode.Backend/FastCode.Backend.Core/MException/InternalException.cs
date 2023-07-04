using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.Backend.Core.MException
{
    public class InternalException : GeneralException
    {
        public InternalException(List<string> userMessage, List<string> devMessage) : base(500, userMessage, devMessage)
        {

        }
    }
}
