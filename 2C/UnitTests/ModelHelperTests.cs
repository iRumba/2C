using System;
using Core.Models;
using Core.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class ModelHelperTests
    {
        [TestMethod]
        public void GetColumnNameTest()
        {
            var dir = ModelHelper.GetColumnName<Arrival>(nameof(Arrival.Date));
            var fk = ModelHelper.GetColumnName<Arrival>(nameof(Arrival.Purveyor));
            Assert.AreEqual(dir, "Date");
            Assert.AreEqual(fk, "PurveyorId");
        }
    }
}
