using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Core.Models;
using Core.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class GoodsRepositoryTests
    {
        RepositoryManager _repManager;

        BaseRepository<Goods> _rep;

        public GoodsRepositoryTests()
        {
            //_repManager = new RepositoryManager(Configuration.CONNECTION_STRING);
            _rep = _repManager.GetRepository<Goods>();
        }

        [TestMethod]
        public async Task GetByIdTestMethod()
        {
            var res = await _rep.GetById(1);
            Assert.IsNotNull(res);
            Assert.AreEqual(res.Id, 1);
        }

        [TestMethod]
        public async Task GetByIdsTestMethod()
        {
            var res = await _rep.GetByIds(new int[] { 1,2 });
            Assert.IsNotNull(res);
            Assert.AreEqual(res.Count, 1);
        }

        [TestMethod]
        public async Task GetAllTestMethod()
        {
            var res = await _rep.GetAll();
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public async Task AddTestMethod()
        {
            var g = new Goods();
            g.Name = "iPhone 5s";
            g.Markup = 0.3;
            
            var res = await _rep.Add(g);
            Assert.IsNotNull(res);
            Assert.AreEqual(res.Id, 2);
        }

        [TestMethod]
        public async Task UpdateTestMethod()
        {
            var newName = "iPhone 4";
            var g = await _rep.GetById(1);
            g.Name = newName;
            g.Markup = 0.3;
            Assert.IsTrue(await _rep.Update(g));
            var newG = await _rep.GetById(1);
            Assert.AreEqual(g.Name, newG.Name);
        }

        [TestMethod]
        public async Task GetDetailsTestMethod()
        {
            var g = await _rep.GetById(1);
            var r = (GoodsRepository)_rep;
            var det = await r.GetDetails(g.Id);
            Assert.IsNotNull(det);
            Assert.IsNull(det.Item1);
            Assert.AreEqual(det.Item2, 0);
        }

        //[TestMethod]
        //public void ListTest()
        //{
        //    var l = new List<Goods>();
        //    var t = typeof(IEnumerable<>);
        //    Assert.IsTrue(l.GetType().IsAssignableFrom(t));
        //}
    }
}
