using BilibiliVideoFetcher.Classes.XmlModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml.Serialization;


namespace UnitTestFetcher
{
    [TestClass]
    public class TestXmlDanmu
    {
        /// <summary>
        /// 测试能否正常解析XML弹幕文件
        /// </summary>
        [TestMethod]
        public void TestDeserializeXmlDanmu()
        {
            Stream stream = File.Open("av6642908.xml", FileMode.Open);
            XmlSerializer ser = new XmlSerializer(typeof(XmlDanmu));
            var danmu = (XmlDanmu)ser.Deserialize(stream);
            Assert.AreEqual(danmu.ChatServer, "chat.bilibili.com");
            Assert.AreEqual(danmu.Mission, 0);
            Assert.AreEqual(danmu.MaxLimit, 1000);
            Assert.AreEqual(danmu.Source, "k-v");
            Assert.AreEqual(danmu.Items.Count, 646);
        }
    }
}
