using EMS.Data.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Business
{
    public class TestBLL : ITestService
    {
        /// <summary>
        /// 访问实例
        /// </summary>
        private static TestBLL m_Instance = new TestBLL();

        /// <summary>
        /// 访问实例
        /// </summary>
        public static TestBLL Instance
        {
            get { return m_Instance; }
        }

        public override int Insert()
        {
          return  InstanceDAL.Insert();
        }

        public override int Update()
        {
           return InstanceDAL.Update();
        }

        public override void Detele()
        {
           InstanceDAL.Detele();
        }
    }
}
