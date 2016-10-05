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
        public static void Build(string aid)
        {
            var json = Helper.NetworkHelper.GetTextFromUri(VIDEO_INFO_API + aid + "&page=" + 1);
            if (json.Length > 100)
            {
                var videoInfo = JsonConvert.DeserializeObject<jsonVideoInfo>(json);
                for (int i = 1; i <= videoInfo.pages; i++)
                {
                    Build(aid, i.ToString());
                }
            }
            else
            {
                var errorMsg = JsonConvert.DeserializeObject<jsonVideoInfoFailedMessage>(json);
                Data.NotificationData.GetInstance().Add(
                    new NotifictionMessage(NotificationLevel.Error, "获取视频信息失败, 错误代号: " + errorMsg.code));
                return;
            }
        }

        /// <summary>
        /// 由av号和起始，结束分段号建立批量任务
        /// </summary>
        /// <param name="aid">av号</param>
        /// <param name="start">开始分段，从1开始，小于1时，将会从第一个分段开始</param>
        /// <param name="end">结束分段，大于总分段数时，将从start一直下载到最后一个分段</param>
        public static void Build(string aid, int start, int end = int.MaxValue)
        {
            string json = Helper.NetworkHelper.GetTextFromUri(VIDEO_INFO_API + aid + "&page=" + 1);
            if (json.Length > 100)
            {
                var videoInfo = JsonConvert.DeserializeObject<jsonVideoInfo>(json);

                if (start > videoInfo.pages)
                {
                    Data.NotificationData.GetInstance().Add(
                        new Classes.NotifictionMessage(Classes.NotificationLevel.Error, "页面范围无效!"));
                    return;
                }
                // 当start小于1时，取1作为起始分段
                start = Math.Max(start, 1);
                // 当end大于总分段数时，取总分段数作为结束分段
                end = Math.Min(end, videoInfo.pages);
                for (int i = start; i <= end; i++)
                {
                    Build(aid, i.ToString());
                }
            }
            else
            {
                var errorMsg = JsonConvert.DeserializeObject<jsonVideoInfoFailedMessage>(json);
                Data.NotificationData.GetInstance().Add(new NotifictionMessage(NotificationLevel.Error, "获取视频信息失败, 错误代号: " + errorMsg.code));
                return;
            }
        }

        public static void Build(string aid, string page)
        {
            var jsonVideoInfo = Helper.NetworkHelper.GetTextFromUri(VIDEO_INFO_API + aid + "&page=" + page);
            if (jsonVideoInfo.Length > 100)
            {
                var videoInfo = JsonConvert.DeserializeObject<jsonVideoInfo>(jsonVideoInfo);
                var newTask = new VideoTask();
                newTask.VideoInfo = videoInfo;
                newTask.Name = "(获取下载地址中)" + videoInfo.title + (videoInfo.partname == null || videoInfo.partname == string.Empty ? string.Empty : " " + videoInfo.partname);
                newTask.Aid = aid.ToString();
                newTask.Partname = videoInfo.partname == string.Empty || videoInfo.partname == null ? "无" : videoInfo.partname;
                newTask.Size = "获取中";
                newTask.CreateTime = DateTime.Now.ToString();
                newTask.DownloadUrl = new List<string>();
                newTask.Page = page;
                newTask.Danmu = "http://comment.bilibili.com/" + videoInfo.cid + ".xml";
                Data.ApplicationSettings.GetInstance().Dispatcher.Invoke(delegate
                {
                    Data.FetchingTasks.GetInstance().Tasks.Add(newTask);
                });
                var downJson = Helper.NetworkHelper.GetTextFromUri(
                    DOWNLOAD_API + newTask.VideoInfo.cid + "?quality=1&type=" +
                    Data.ApplicationSettings.GetInstance().FetchingOption.Format);
                if (downJson.Length < 100)
                {
                    Data.NotificationData.GetInstance().Add(new NotifictionMessage(
                        NotificationLevel.Error, "无法获取cid:" + videoInfo.cid + "的下载地址, 可能是非bilibili源的缘故"));
                    return;
                }
                var jvd = JsonConvert.DeserializeObject<jsonVideoDownload>(downJson);
                if (Data.ApplicationSettings.GetInstance().FetchingOption.Quality == "high")
                {
                    var quality = jvd.accept_quality.OrderByDescending(t => t).First();
                    downJson = Helper.NetworkHelper.GetTextFromUri(
                        DOWNLOAD_API + newTask.VideoInfo.cid + "?quality=" + quality);
                    jvd = JsonConvert.DeserializeObject<jsonVideoDownload>(downJson);
                }
                if (jvd.durl.Count == 0)
                {

                    Data.NotificationData.GetInstance().Add(new NotifictionMessage(
                        NotificationLevel.Error, "无法获取cid:" + videoInfo.cid + "的下载地址, durl.count = 0"));
                    return;
                }

                newTask.Size = Helper.FileHelper.GetFileSizeString(jvd.durl[0].size);
                newTask.DownloadUrl.Add(jvd.durl[0].url);
                newTask.DownloadUrl.AddRange(jvd.durl[0].backup_url);
                newTask.Name = newTask.Name.Substring(9);
                Data.NotificationData.GetInstance().Add(new NotifictionMessage(
                       NotificationLevel.Info, "成功获取cid:" + videoInfo.cid + "的下载地址, 请复制下载地址到其它软件下载"));
                return;
            }
            else
            {
                var errorMsg = JsonConvert.DeserializeObject<jsonVideoInfoFailedMessage>(jsonVideoInfo);
                Data.NotificationData.GetInstance().Add(new NotifictionMessage(NotificationLevel.Error, "获取视频信息失败, 错误代号: " + errorMsg.code));
                return;
            }
        }
    }

}
