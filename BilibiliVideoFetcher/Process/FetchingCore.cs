using BilibiliVideoFetcher.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BilibiliVideoFetcher.Process
{
    public static class FetchingCore
    {
        #region "Regexes"
        // 对于固定模式的正则表达式，直接作为静态成员，没必要每次调用的时候都去实例化

        static Regex aidPattern = new Regex(@"show\((\d+), \d+\);");

        //形如: http://www.bilibili.com/video/av1965474/index_2.html
        static Regex regexPagePattern = new Regex(@"https?:\/\/www\.bilibili\.com\/video\/av(\d+)\/index_(\d+)\.html");
        //形如: http://www.bilibili.com/video/av1965474
        static Regex regexNormalPattern = new Regex(@"https?:\/\/www\.bilibili\.com\/video\/av(\d+)");

        static Regex regexBangumiPattern = new Regex(@"https?:\/\/\w+\.bilibili\.com\/\w+\/v\/(\d+)");

        #endregion

        private static string GetAidFromSourceHtml(string html)
        {
            var match = aidPattern.Match(html);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return string.Empty;
            }
        }

        public static void ReFetchTask(VideoTask task)
        {
            CreateTaskWithAid(task.Aid, task.Page);
            Data.ApplicationSettings.GetInstance().Dispatcher.Invoke(() =>
            {
                Data.FetchingTasks.GetInstance().Tasks.Remove(task);
            });

        }

        /// <summary>
        /// 从URL中获取aid以及partIndex
        /// </summary>
        /// <param name="url"></param>
        /// <exception cref="ArgumentException">尝试解析URL失败</exception>
        /// <exception cref="NotSupportedException">暂不支持解析所提供的URL</exception>
        /// <returns></returns>
        public static FetcherTaskToken GetTaskTokenFromUrl(string url)
        {
            url = Helper.UrlHelper.FixUrl(url);

            var bangumiMatch = regexBangumiPattern.Match(url);
            if (bangumiMatch.Success)
            {
                string aid = GetAidFromSourceHtml(Helper.NetworkHelper.GetTextFromUri(url));
                if (aid == string.Empty)
                {
                    throw new ArgumentException("错误的地址格式! 无法获取到Aid.");
                }
                return new FetcherTaskToken(aid);
            }

            var pageMatch = regexPagePattern.Match(url);
            if (pageMatch.Success)
            {
                var aid = pageMatch.Groups[1].Value;
                var page = int.Parse(pageMatch.Groups[2].Value);
                return new FetcherTaskToken(aid, page);
            }

            var normalMatch = regexNormalPattern.Match(url);
            if (normalMatch.Success)
            {
                return new FetcherTaskToken(normalMatch.Groups[1].Value);
            }

            throw new NotSupportedException("错误的地址格式!");
        }

        public static void NewTask(string url)
        {
            try
            {

                CreateTask(GetTaskTokenFromUrl(url));
            }
            catch (ArgumentException ex)
            {
                Data.NotificationData.AddErrorNotifiction(ex.Message);
            }
            catch (NotSupportedException ex)
            {
                Data.NotificationData.AddErrorNotifiction(ex.Message);
            }
        }

        public static void NewMultiTask(string text, string tbPartStart, string tbPartEnd)
        {
            int partStart, partEnd;
            if (string.IsNullOrWhiteSpace(tbPartStart))
            {
                partStart = 1; // 不输入起始分集位置时，默认从1开始
            }
            else if (!int.TryParse(tbPartStart.Trim(), out partStart))
            {
                Data.NotificationData.AddErrorNotifiction("错误的起始分集位置!");
                return;
            }

            if (string.IsNullOrWhiteSpace(tbPartEnd))
            {
                partEnd = int.MaxValue; // 不输入结束分集位置时，默认一直下载到最后一集
            }
            else if (!int.TryParse(tbPartEnd.Trim(), out partEnd))
            {
                Data.NotificationData.AddErrorNotifiction("错误的结束分集位置!");
                return;
            }

            NewMultiTask(text, partStart, partEnd);
        }

        public static void NewMultiTask(string text, int partStart, int partEnd)
        {
            // 发现分集结束位置小于开始位置时，交换partStart和partEnd的值
            if (partEnd < partStart)
            {
                int temp = partStart;
                partStart = partEnd;
                partEnd = temp;
            }

            var normalMatch = regexNormalPattern.Match(text);
            if (normalMatch.Success)
            {
                var aid = normalMatch.Groups[1].Value;
                Data.NotificationData.GetInstance().Add(new Classes.NotifictionMessage(Classes.NotificationLevel.Info, "多集任务aid" + aid + "已开始解析! 请稍等."));
                TaskBuilder.Build(aid, partStart, partEnd);
            }
            else
            {
                Data.NotificationData.AddErrorNotifiction("无效的地址, 请输入带有av(aid)号的地址!");
                return;
            }

        }

        public static void NewMultiTask(string text)
        {
            var normalMatch = regexNormalPattern.Match(text);
            if (normalMatch.Success)
            {
                var aid = normalMatch.Groups[1].Value;
                Data.NotificationData.AddNotifiction(NotificationLevel.Info, "多集任务aid" + aid + "已开始解析! 请稍等.");
                TaskBuilder.Build(aid);
            }
            else
            {
                Data.NotificationData.AddErrorNotifiction("无效的地址, 请输入带有av(aid)号的地址!");
                return;
            }
        }

        public static void CreateTaskWithAid(string aid, int page = 1)
        {
            CreateTask(new FetcherTaskToken(aid, page));
        }

        public static void CreateTask(FetcherTaskToken token)
        {

            Task.Run(() => TaskBuilder.Build(token));
            Data.NotificationData.AddNotifiction(NotificationLevel.Info, $"任务aid{token.Aid}已开始解析! 请稍等.");
            //new Thread(delegate ()
            //{
            //    TaskBuilder.Build(token);
            //}).Start();
        }
    }
}
