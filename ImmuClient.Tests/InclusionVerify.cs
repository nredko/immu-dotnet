using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ImmuClient.Tests
{
    [TestClass]
    public class InclusionVerify
    {
        [TestMethod]
        public void Verify_DefaultValues()
        {
            Assert.IsTrue(Utils.Inclusion.VerifyPath(new List<byte[]>(), 0, 0, new byte[] { }, new byte[] { }));
        }

        [TestMethod]
        public void Verify_AtMoreThanI()
        {
            Assert.IsFalse(Utils.Inclusion.VerifyPath(new List<byte[]>(), 0, 1, new byte[] { }, new byte[] { }));
        }

        [TestMethod]
        public void Verify_AtLessThanI()
        {
            Assert.IsFalse(Utils.Inclusion.VerifyPath(new List<byte[]>(), 1, 0, new byte[] { }, new byte[] { }));
        }

        [TestMethod]
        public void Verify_AtEqualsI()
        {
            Assert.IsFalse(Utils.Inclusion.VerifyPath(new List<byte[]>(), 1, 1, new byte[] { }, new byte[] { }));
        }
    }
}
