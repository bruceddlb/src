using EMS.Data.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Data.Service.SqlServer
{
    public class TestService : ITestService
    {
        public override int Insert()
        {
            throw new NotImplementedException();
        }

        public override int Update()
        {
            var test = T_Test.SingleOrDefault("where Name=@0", "aaaa");
            test.Remark = "备注-修改111";
            return test.Update();
        }

        public override void Detele()
        {
            throw new NotImplementedException();
        }
    }
}
