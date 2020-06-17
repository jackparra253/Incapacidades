using Microsoft.VisualStudio.TestTools.UnitTesting;
using Datos;
using System.Linq;

namespace Test
{
    [TestClass]
    public class UnitTest1 : TestBase
    {
        public UnitTest1()
        {
            UseSqlite();
        }

        [TestMethod]
        public void TestMethod1()
        {
            using var context =  GetDbContext();

            var test = new Datos.Test
            {
                Id = 1,
                Name = "Jack"
            };

            context.Tests.Add(test);

            context.SaveChanges();

            Datos.Test actual = context.Tests.Where(x => x.Name == "Jack").FirstOrDefault();
            
            Assert.AreEqual("Jack", actual.Name);
        }

    }

}
