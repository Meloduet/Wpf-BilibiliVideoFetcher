using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BilibiliVideoFetcher.Process;
using BilibiliVideoFetcher.Helper;

namespace UnitTestFetcher
{
    [TestClass]
    public class TestTaskBuilder
    {
        private const string NATIVE_API = "http://interface.bilibili.com/playurl?";
        [TestMethod]
        public void TestMethod1()
        {
            BilibiliVideoFetcher.Classes.BilibiliVideoInfo info;
            Console.WriteLine(info = TaskBuilder.GetVideoInfoFromNativeApi(11292577));
            Assert.AreEqual(info.Code, 0);
            Console.WriteLine(info = TaskBuilder.GetVideoInfoFromNativeApi(11292578));
            Assert.IsTrue(info.Code < 0);
        }
    }
}
