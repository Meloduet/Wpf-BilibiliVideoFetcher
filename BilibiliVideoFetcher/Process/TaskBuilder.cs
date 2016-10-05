using BilibiliVideoFetcher.Classes.JsonModel;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BilibiliVideoFetcher.Classes;

namespace BilibiliVideoFetcher.Process
{
    public class TaskBuilder
    {
        private const string DOWNLOAD_API = "http://bilibili-service.daoapp.io/video/";
        private const string VIDEO_INFO_API = "http://api.bilibili.com/view?type=json&appkey=8e9fc618fbd41e28&id=";

        /// <summary>
        /// 由av号和起始，结束分段号建立批量任务
        /// </summary>
        /// <param name="aid">av号</param>
        /// <param name="start">开始分段，从1开始，小于1时，将会从第一个分段开始</param>
        /// <param name="end">结束分段，大于总分段数时，将从start一直下载到最后一个分段</param>
        public static void Build(string aid, int start = 1, int end = int.MaxValue)
        {
            string json = Helper.NetworkHelper.GetTextFromUri(VIDEO_INFO_API + aid + "&page=" + 1);
            if (json.Length > 100)
            {
                var videoInfo = JsonConvert.DeserializeObject<jsonVideoInfo>(json);

                if (start > videoInfo.pages)
                {
                    Data.NotificationData.AddErrorNotifiction("页面范围无效!");
                    return;
                }
                // 当start小于1时，取1作为起始分段
                start = Math.Max(start, 1);
                // 当end大于总分段数时，取总分段数作为结束分段
                end = Math.Min(end, videoInfo.pages);
                var aids = new string[end - start];
                int j = 0;
                for (int i = start; i <= end; i++)
                {
                    aids[j++] = i.ToString();
                }
                Parallel.ForEach(aids, (i) => { Build(aid, i); });
            }
            else
            {
                var errorMsg = JsonConvert.DeserializeObject<jsonVideoInfoFailedMessage>(json);
                Data.NotificationData.AddErrorNotifiction("获取视频信息失败, 错误代号: " + errorMsg.code);
            }
        }

        public static void Build(string aid, string page)
        {
            var jsonVideoInfo = Helper.NetworkHelper.GetTextFromUri(VIDEO_INFO_API + aid + "&page=" + page);
            if (jsonVideoInfo.Length > 100)
            {
                var videoInfo = JsonConvert.DeserializeObject<jsonVideoInfo>(jsonVideoInfo);
                VideoTask newTask = GetVideoTask(aid, page, videoInfo);

                var settings = Data.ApplicationSettings.GetInstance();

                settings.Dispatcher.Invoke(delegate
                {
                    Data.FetchingTasks.AddTask(newTask);
                });

                var task = new Task(() => { GetDownloadUrl(newTask); });

                // TODO: 改用后台进程/进程池实现
                task.Start();

            }
            else
            {
                var errorMsg = JsonConvert.DeserializeObject<jsonVideoInfoFailedMessage>(jsonVideoInfo);
                Data.NotificationData.AddErrorNotifiction("获取视频信息失败, 错误代号: " + errorMsg.code);
            }
        }

        /// <summary>
        /// 获取任务的大小和下载地址
        /// </summary>
        /// <param name="newTask"></param>
        private static void GetDownloadUrl(VideoTask newTask)
        {
            var settings = Data.ApplicationSettings.GetInstance();
            var cid = newTask.VideoInfo.cid;
            var downJson = Helper.NetworkHelper.GetTextFromUri(
                DOWNLOAD_API + cid + "?quality=1&type=" + settings.FetchingOption.Format);
            if (downJson.Length < 100)
            {
                Data.NotificationData.AddErrorNotifiction(
                    "无法获取cid:" + cid + "的下载地址, 可能是非bilibili源的缘故");
                return;
            }
            var jvd = JsonConvert.DeserializeObject<jsonVideoDownload>(downJson);

            if (settings.FetchingOption.Quality == "high")
            {
                var quality = jvd.accept_quality.OrderByDescending(t => t).First();
                downJson = Helper.NetworkHelper.GetTextFromUri(
                    DOWNLOAD_API + newTask.VideoInfo.cid + "?quality=" + quality);
                jvd = JsonConvert.DeserializeObject<jsonVideoDownload>(downJson);
            }

            if (jvd.durl.Count == 0)
            {
                Data.NotificationData.AddErrorNotifiction(
                    "无法获取cid:" + cid + "的下载地址, durl.count = 0");
                return;
            }

            AppendFileSystemInfo(newTask, jvd);

            Data.NotificationData.AddNotifiction(
                   NotificationLevel.Info,
                   "成功获取cid:" + cid + "的下载地址, 请复制下载地址到其它软件下载");
        }

        private static void AppendFileSystemInfo(VideoTask newTask, jsonVideoDownload jvd)
        {
            var durl = jvd.durl[0];
            newTask.Size = Helper.FileHelper.GetFileSizeString(durl.size);
            newTask.DownloadUrl.Add(durl.url);
            newTask.DownloadUrl.AddRange(durl.backup_url);
            newTask.Name = newTask.Name.Substring(9);
        }

        private static VideoTask GetVideoTask(string aid, string page, jsonVideoInfo videoInfo)
        {
            var newTask = new VideoTask();
            newTask.VideoInfo = videoInfo;
            if (string.IsNullOrEmpty(videoInfo.partname))
            {
                newTask.Name = $"(获取下载地址中){videoInfo.title}";
                newTask.Partname = "无";
            }
            else
            {
                newTask.Name = $"(获取下载地址中){videoInfo.title} {videoInfo.partname}";
                newTask.Partname = videoInfo.partname;
            }

            newTask.Aid = aid;
            newTask.Size = "获取中";
            newTask.CreateTime = DateTime.Now.ToString();
            newTask.DownloadUrl = new List<string>();
            newTask.Page = page;
            newTask.Danmu = "http://comment.bilibili.com/" + videoInfo.cid + ".xml";
            return newTask;
        }
    }

}
