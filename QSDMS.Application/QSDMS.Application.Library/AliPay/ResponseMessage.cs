using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSDMS.Application.Library.AliPay
{
    public class ResponseMessage
    {
        public ResponseMessage()
            : this(false)
        {
        }

        public ResponseMessage(bool result)
        {
            this.result = result;
            this.resultCode = 250;
            this.reason = "失败";
        }

        public string reason { get; set; }

        public bool result { get; set; }

        public int resultCode { get; set; }

        public object resultInfo { get; set; }
    }
}
