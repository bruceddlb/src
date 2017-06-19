using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSDMS.Business;
using QSDMS.Model;
using System.Collections.Generic;
using QSDMS.Util.WebControl;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethodQuery()
        {
            List<TestEntity> list = TestBLL.Instance.Query();
            Assert.IsTrue(list.Count > 0);
        }
        [TestMethod]
        public void TestMethodInsert()
        {
            int count = TestBLL.Instance.Insert();
            Assert.IsTrue(count > 0);
        }
        [TestMethod]
        public void TestMethodUpdate()
        {
            int count = TestBLL.Instance.Update();
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void TestMethodDelete()
        {
            TestBLL.Instance.Delete();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestMethodPage()
        {
            Pagination pager = new Pagination();
            pager.page = 1;
            pager.rows = 15;
            var list = JobBLL.Instance.GetPageList(pager, "{organizeId:\"\",condition:\"\",keyword:\"\"}") as List<RoleEntity>;
            Assert.IsTrue(list.Count > 0);
        }
         [TestMethod]
        public void TestMethodGetEntity()
        {
            RoleEntity model = JobBLL.Instance.GetEntity("0439c72b-cb52-4a06-8e28-2b3431b472df");
            Assert.IsTrue(model!=null);
        }
    }
}
