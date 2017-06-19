using QSDMS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QSDMS.Client.Api
{
    public class FileClient:BaseClient
    {
        /// <summary>
        /// 上传用户汇款截图
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ReturnMessage MemberRemit(HttpPostedFileBase file) 
        {
            var url = string.Format("{0}/Uplolad/MemberRemit", fileHost);
            return Post(url, string.Empty, file);
        }

        /// <summary>
        /// 获取用户二维码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ReturnMessage AccountCode(string url) 
        {
            return Get(string.Format("{0}/Upload/AccountCode?url={1}", fileHost, url));
        }
    }
}
