using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmployesWork.Repository.Abstract;
using EmployesWork.Repository.Concrete;

namespace EmployesWork.UnitTests
{
    [TestClass]
    public class TestRepository
    {
        [TestMethod]
        public void TestCountEmployer()
        {
            IEmployeRepository rep = new EmployeRepository();
            Assert.IsTrue(rep.Employes.Count() > 0);

            
        }
    }
}
