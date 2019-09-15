using System;
using topical;
using Xunit;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var s = new Startup();
            Assert.Equal(2, s.Test());
        }
    }
}