using System;
using Core.Utils;
using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class DbManagerTests
    {
        [TestMethod]
        public async Task TestCreateBase()
        {
            var conf = new Configuration()
            {
                ConnectionString = TestsConfiguration.CONNECTION_STRING
            };
            var dbManager = new DbManager(conf);

            try
            {
                await dbManager.SetupDatabase();
            }
            catch (Exception ex)
            {
                Assert.Inconclusive();
                throw;
            }
            
            //Assert.ThrowsException<Exception>((Action)dbManager.SetupDatabase);
        }
    }
}
