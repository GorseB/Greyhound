using Greyhound;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // create 3 punters, set the second punter to joe, and if his name is joe it all worked! YAY!
            Punter[] Punters = new Punter[3];
            Punters[1] = Punter_Factory.ReturnPunter(1);
            if (Punters[1].Name == "Joe")
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}