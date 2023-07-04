using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.Backend.Core.MException
{
    public class NotFoundException : GeneralException
    {
        public NotFoundException(List<string> userMessage, List<string> devMessage) : base(404, userMessage, devMessage)
        {

        }
    }
}
