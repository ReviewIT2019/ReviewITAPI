using System;
using Xunit;

namespace ReviewIT.Backend.Web.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Assert.True(true);
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal("Hello TestWorld", "Hello TestWorld");
        }
    }
}
