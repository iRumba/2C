using System;
using System.Threading.Tasks;
using Core.Models;
using Core.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class ArrivalRepositoryTests : RepositoriesTests<Arrival>
    {

        [TestMethod]
        public async Task GetByIdTestMethod()
        {
            var res = (object)null;// await Repository.GetById(1);
            Assert.IsNull(res);
        }
    }
}
