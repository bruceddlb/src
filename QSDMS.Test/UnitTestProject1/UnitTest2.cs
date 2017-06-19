using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EMS.Business;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            int count = TestBLL.Instance.Update();
            Assert.IsTrue(count > 0);
        }
    }
}
