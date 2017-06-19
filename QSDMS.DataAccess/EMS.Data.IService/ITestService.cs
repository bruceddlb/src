using QSDMS.Data.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Data.IService
{
    public abstract class ITestService : BaseDataAdapter<ITestService>
    {
        public abstract int Insert();
        public abstract int Update();
        public abstract void Detele();
    }
}
