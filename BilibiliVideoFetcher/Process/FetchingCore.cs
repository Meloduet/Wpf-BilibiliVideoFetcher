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
        
        public static void NewTask(string url)
        {
            url = Helper.UrlHelper.FixUrl(url);
            //形如: http://www.bilibili.com/video/av1965474/index_2.html
            var regexPagePattern = new Regex(@"http:\/\/www\.bilibili\.com\/video\/av\d+\/index_\d+\.html");
            //形如: http://www.bilibili.com/video/av1965474
            var regexNormalPattern = new Regex(@"http:\/\/www\.bilibili\.com\/video\/av\d+");

            var pageMatch = regexPagePattern.Match(url);
            if (pageMatch.Success)
            {
                var fragments = pageMatch.Value.Substring(32).Split('/');
                var aid = -1;
                if (!int.TryParse(fragments[0], out aid))
                {
                    Data.NotificationData.GetInstance().Add
                        (new Classes.NotifictionMessage(Classes.NotificationLevel.Error, "错误的aid!"));
                    return;
                }
                var page =-1;
                if(!int.TryParse(fragments[1].Substring(6, fragments[1].Length - 11),out page))
                {
                    Data.NotificationData.GetInstance().Add(new Classes.NotifictionMessage(Classes.NotificationLevel.Error, "错误的page!"));
                    return;
                }
                Data.NotificationData.GetInstance().Add(new Classes.NotifictionMessage(Classes.NotificationLevel.Info, "任务aid" + aid + "已开始解析! 请稍等."));
                CreateTaskWithAid(aid, page);
                return;
            }
            var normalMatch = regexNormalPattern.Match(url);
            if (normalMatch.Success)
            {
                int aid = -1;
                if(!int.TryParse(normalMatch.Value.Substring(32), out aid))
                {
                    Data.NotificationData.GetInstance().Add
                        (new Classes.NotifictionMessage(Classes.NotificationLevel.Error, "错误的aid!"));
                    return;
                }
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
                int aid = -1;
                var fragments = normalMatch.Value.Substring(32).Split('/');
                if (!int.TryParse(fragments[0], out aid))
                {
                    Data.NotificationData.GetInstance().Add
                        (new Classes.NotifictionMessage(Classes.NotificationLevel.Error, "错误的aid!"));
                    return;
                }
                Data.NotificationData.GetInstance().Add(new Classes.NotifictionMessage(Classes.NotificationLevel.Info, "多集任务aid" + aid + "已开始解析! 请稍等."));

                Process.TaskBuilder.Build(aid, partStart, partEnd);
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

        private static void CreateTaskWithAid(int aid, int page = 1)
        {
            new Thread(delegate () {
                TaskBuilder.Build(aid, page);
            }).Start();
        }
    }
}
