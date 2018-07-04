using DocsScraper;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class TextFormatterTests
    {
        [Test]
        public void TestFormatting()
        {
            Assert.AreEqual("Testing formatting", TextFormatter.Format("Testing    formatting"));
            Assert.AreEqual("Testing formatting", TextFormatter.Format("Testing \n\r\r\n   formatting"));
        }
    }
}
