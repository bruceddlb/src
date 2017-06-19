using QSDMS.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            AccountBLL.Instance.aa();
            Console.ReadKey();
        }
    }
}
