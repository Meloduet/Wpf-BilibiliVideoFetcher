using Aria2Controler;
using Aria2Controler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;

namespace UnitTestFetcher
{
    /// <summary>
    /// 测试Aria2Helper是否能够正常工作，
    /// 请保证测试时能正常访问互联网，Aria2已运行并且enable-rpc=true
    /// </summary>
    [TestClass]
    public class TestAria2Helper
    {
        [TestMethod]
        public void TestTellDownlaods()
        {
            Console.WriteLine("TellActive");
            Console.WriteLine(string.Join(",", (object[])Aria2Helper.TellActive()));

            Console.WriteLine("TellStopped");
            Console.WriteLine(string.Join(",", (object[])Aria2Helper.TellStopped(0, 1000)));

            Console.WriteLine("TellWaiting");
            Console.WriteLine(string.Join(",", (object[])Aria2Helper.TellWaiting(0, 1000)));
        }

        [TestMethod]
        public void TestIsAria2Running()
        {
            Assert.IsTrue(Aria2Helper.IsAria2Running, "未启动Aria2");
        }

        [TestMethod]
        public void TestParseTaskToken()
        {
            string jsonString = System.IO.File.ReadAllText("dumy_taskInfo.json");

            Aria2TaskInfo info = JsonConvert.DeserializeObject<Aria2TaskInfo>(jsonString);

            Assert.AreEqual<TaskStatus>(info.Status, TaskStatus.Active, "解析出错");
            Console.WriteLine(info.Status);
            // Aria2Helper.AddUri("http://www.baidu.com");
        }

        [TestMethod]
        public void TestAddTask()
        {
            Aria2Helper.AddUri("http://www.baidu.com");
        }

        /// <summary>
        /// 保存百度主页，并
        /// </summary>
        [TestMethod]
        public void TestAddTaskPlus()
        {
            string gid = Aria2Helper.AddUri("http://www.baidu.com",
                 new Dictionary<string, string>()
                 {
                     ["dir"] = "D:\\",
                     ["out"] = "1.txt",
                     ["split"] = "3",
                     ["max-connection-per-server"] = "3"
                 }
             );
            var status = Aria2Helper.TellStatus(gid);
            Console.WriteLine(status);
            Console.WriteLine(gid);
        }

        [TestMethod]
        public void TestList()
        {
            Console.WriteLine(string.Join("\r\n", (object[])Aria2Helper.ListMethods()));
            Console.WriteLine(string.Join("\r\n", (object[])Aria2Helper.ListNotifications()));
        }

        /// <summary>
        /// 模拟测试一个下载流程
        /// </summary>
        [TestMethod]
        public void TsetTaskControl()
        {
            // 随手找的手游安装包，体积够大，防止在测试过程中被下载好。
            string fileUri = @"https://dl.hdslb.com/biligame/yys/yys_v1.0.7/3284A388F7AC464D.apk";
            string dir = @"D:\";

            string gid = Aria2Helper.AddUri(
                fileUri,
                 new Dictionary<string, string>()
                 {
                     ["dir"] = dir,
                     ["out"] = "1.apk",
                     ["split"] = "3",
                     ["max-connection-per-server"] = "3"
                 }
             );
            var status = Aria2Helper.TellStatus(gid);

            Assert.IsTrue(status.Status == TaskStatus.Active || status.Status == TaskStatus.Waiting);

            // 暂停所有下载任务
            Aria2Helper.PauseAll();

            Thread.Sleep(100); // 等待任务被暂停

            // 仅获取任务的状态信息
            status = Aria2Helper.TellStatus(gid, gid, Aria2TaskInfo.KEY_STATUS);

            Assert.AreEqual<TaskStatus>(status.Status, TaskStatus.Paused, $"任务没有被暂停{status.Status}");

            // 移动为下载队列中的第一位
            int pos = Aria2Helper.ChangePosition(gid, 0, PositionOrigin.Begin);

            Assert.AreEqual<int>(pos, 0, $"调整任务顺序失败，{pos}");

            var r = Aria2Helper.Unpause(gid);

            Assert.IsTrue(r, "恢复下载失败");

            Thread.Sleep(100); // 等待任务恢复

            // 仅获取保存文件夹，下载速度，总大小，已下载大小，任务从状态
            status = Aria2Helper.TellStatus(gid, gid, StandardKeys.SimpleDownlad);

            Assert.AreEqual<string>(status.Dir, dir);

            Console.WriteLine($"当前下载速度： {status.DownloadSpeed}");
            Console.WriteLine($"下载进度： {status.CompletedLength} / {status.TotalLength}");

            // 暂停下载
            Aria2Helper.Pause(gid);

            // 移除下载
            Aria2Helper.Remove(gid);

            status = Aria2Helper.TellStatus(gid);
            Console.WriteLine(status);
        }

    }
}
