using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ImmuClient.Tests
{
    [TestClass]
    public class ConsistencyVerify
    {
        [TestMethod]
        public void Verify_DefaultValues()
        {
            Assert.IsTrue(Utils.Consistency.VerifyPath(new List<byte[]>(), 0, 0, new byte[] { }, new byte[] { }));
        }

        [TestMethod]
        public void Verify_SinglePathSecondEqualsFirst()
        {
            Assert.IsTrue(Utils.Consistency.VerifyPath(new List<byte[]>(), 1, 1, new byte[] { }, new byte[] { }));
        }

        [TestMethod]
        public void Verify_SinglePathFirstMoreThanSecond()
        {
            Assert.IsFalse(Utils.Consistency.VerifyPath(new List<byte[]>(), 0, 1, new byte[] { }, new byte[] { }));
        }

        [TestMethod]
        public void Verify_SinglePathSecondMoreThanFirst()
        {
            Assert.IsFalse(Utils.Consistency.VerifyPath(new List<byte[]>(), 1, 0, new byte[] { }, new byte[] { }));
        }

        [TestMethod]
        public void Verify_SinglePathSecondHashNotEqualsFirstHash()
        {
            Assert.IsFalse(Utils.Consistency.VerifyPath(new List<byte[]>(), 0, 0, new byte[] { 1 }, new byte[] { 2 }));
        }

        [TestMethod]
        public void Verify_ManyPathSecondMoreThanFirst()
        {
            Assert.IsFalse(Utils.Consistency.VerifyPath(new List<byte[]>() { new byte[] { }, new byte[] { }, new byte[] { } }, 2, 1, new byte[] { }, new byte[] { }));
        }
    }
}
