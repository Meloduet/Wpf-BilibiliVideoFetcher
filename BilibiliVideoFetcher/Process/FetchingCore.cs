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
    public class FetchingCore
    {
        
        private static string GetAidFromSourceHtml(string html)
        {
            var aidPattern = new Regex(@"show\(\d+, \d+\);");
            var match = aidPattern.Match(html);
            if (match.Success)
            {
                var fragments = match.Value.Split(',');
                return fragments[0].Substring(5);
            }
            else
            {
                return string.Empty;
            }
        }

        public static void NewTask(string url)
        {
            url = Helper.UrlHelper.FixUrl(url);
            //形如: http://www.bilibili.com/video/av1965474/index_2.html
            var regexPagePattern = new Regex(@"http:\/\/www\.bilibili\.com\/video\/av\d+\/index_\d+\.html");
            //形如: http://www.bilibili.com/video/av1965474
            var regexNormalPattern = new Regex(@"http:\/\/www\.bilibili\.com\/video\/av\d+");

            var regexBangumiPattern = new Regex(@"http:\/\/\w+\.bilibili\.com\/\w+\/v\/\d+");

            var bangumiMatch = regexBangumiPattern.Match(url);
            if (bangumiMatch.Success)
            {
                string aid = GetAidFromSourceHtml(Helper.NetworkHelper.GetTextFromUri(url));
                if(aid==string.Empty)
                {
                    Data.NotificationData.GetInstance().Add
                        (new Classes.NotifictionMessage(Classes.NotificationLevel.Error, "错误的地址格式! 无法获取到Aid."));
                    return;
                }
                CreateTaskWithAid(aid);
                return;

            }


            var pageMatch = regexPagePattern.Match(url);
            if (pageMatch.Success)
            {
                var fragments = pageMatch.Value.Substring(32).Split('/');
                var aid = fragments[0];                
                var page = fragments[1].Substring(6, fragments[1].Length - 11);                
                Data.NotificationData.GetInstance().Add(new Classes.NotifictionMessage(Classes.NotificationLevel.Info, "任务aid" + aid + "已开始解析! 请稍等."));
                CreateTaskWithAid(aid, page);
                return;
            }
            var normalMatch = regexNormalPattern.Match(url);
            if (normalMatch.Success)
            {
                var aid = normalMatch.Value.Substring(32);
                Data.NotificationData.GetInstance().Add(new Classes.NotifictionMessage(Classes.NotificationLevel.Info, "任务aid" + aid + "已开始解析! 请稍等."));
                CreateTaskWithAid(aid);
                return;
            }
            Data.NotificationData.GetInstance().Add
                        (new Classes.NotifictionMessage(Classes.NotificationLevel.Error, "错误的地址格式!"));
        }

        public static void NewMultiTask(string text, string tbPartStart, string tbPartEnd)
        {
            int partStart;
            if(!int.TryParse(tbPartStart, out partStart))
            {
                Data.NotificationData.GetInstance().Add
                        (new Classes.NotifictionMessage(Classes.NotificationLevel.Error, "错误的起始分集位置!"));
                return;
            }
            int partEnd;
            if(!int.TryParse(tbPartEnd, out partEnd))
            {
                Data.NotificationData.GetInstance().Add
                       (new Classes.NotifictionMessage(Classes.NotificationLevel.Error, "错误的结束分集位置!"));
                return;
            }
            if (partEnd < partStart || partEnd<=0 || partStart<=0)
            {
                Data.NotificationData.GetInstance().Add
                       (new Classes.NotifictionMessage(Classes.NotificationLevel.Error, "下载范围矛盾!"));
                return;
            }

            var regexNormalPattern = new Regex(@"http:\/\/www\.bilibili\.com\/video\/av\d+");
            var normalMatch = regexNormalPattern.Match(text);
            if(normalMatch.Success)
            {
                var fragments = normalMatch.Value.Substring(32).Split('/');
                var aid = fragments[0];
                Data.NotificationData.GetInstance().Add(new Classes.NotifictionMessage(Classes.NotificationLevel.Info, "多集任务aid" + aid + "已开始解析! 请稍等."));
                TaskBuilder.Build(aid, partStart, partEnd);
            }else
            {
                Data.NotificationData.GetInstance().Add
                        (new Classes.NotifictionMessage(Classes.NotificationLevel.Error, "无效的地址, 请输入带有av(aid)号的地址!"));
                return;
            }
        }
        public static void NewMultiTask(string text)
        {
            

        }

        private static void CreateTaskWithAid(string aid, string page = "1")
        {
            new Thread(delegate () {
                TaskBuilder.Build(aid, page);
            }).Start();
        }
    }
}
