using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QSDMS.Services.Api.Controllers
{
    public class TestController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ///       
        [HttpGet]
        //[HttpPost]
        public JsonResult GetList()
        {
            List<UserInfo> list = new List<UserInfo>();
            list.Add(new UserInfo() { Name = "张三", Pwd = "123456" });
            list.Add(new UserInfo() { Name = "李四", Pwd = "123456" });
            list.Add(new UserInfo() { Name = "王五", Pwd = "123456" });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

    }

    public class UserInfo {

        public string Name { get; set; }

        public string Pwd { get; set; }
    }
}
