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
        public static void Build(int aid)
        {
            string json = Helper.NetworkHelper.GetTextFromUri("http://api.bilibili.com/view?type=json&appkey=8e9fc618fbd41e28&id=" + aid + "&page=" + page);
            if (json.Length > 100)
            {
                var videoInfo = JsonConvert.DeserializeObject<jsonVideoInfo>(json);                
                for (int i = 1; i <= videoInfo.pages; i++)
                {
                    Build(aid, i);
                }
            }
            else {
                var errorMsg = JsonConvert.DeserializeObject<jsonVideoInfoFailedMessage>(json);
                Data.NotificationData.GetInstance().Add(new Classes.NotifictionMessage(Classes.NotificationLevel.Error, "获取视频信息失败, 错误代号: " + errorMsg.code));
                return;
            }
        }
        public static void Build(int aid, int start, int end)
        {
            string json = Helper.NetworkHelper.GetTextFromUri("http://api.bilibili.com/view?type=json&appkey=8e9fc618fbd41e28&id=" + aid + "&page=" + page);
            if (json.Length > 100)
            {
                var videoInfo = JsonConvert.DeserializeObject<jsonVideoInfo>(json);
                if(start>videoInfo.pages||end>videoInfo.pages)
                {
                    Data.NotificationData.GetInstance().Add(
                        new Classes.NotifictionMessage(Classes.NotificationLevel.Error, "页面范围无效!"));
                    return;
                }
                for (int i = start; i <= end; i++)
                {
                    Build(aid, i);
                }
            }
            else
            {
                var errorMsg = JsonConvert.DeserializeObject<jsonVideoInfoFailedMessage>(json);
                Data.NotificationData.GetInstance().Add(new Classes.NotifictionMessage(Classes.NotificationLevel.Error, "获取视频信息失败, 错误代号: " + errorMsg.code));
                return;
            }
        }
        public static void Build(int aid, int page)
        {
            string json = Helper.NetworkHelper.GetTextFromUri("http://api.bilibili.com/view?type=json&appkey=8e9fc618fbd41e28&id=" + aid + "&page=" + page);
            if (json.Length > 100)
            {
                var videoInfo = JsonConvert.DeserializeObject<jsonVideoInfo>(json);
                var newTask = new Classes.VideoTask();
                newTask.VideoInfo = videoInfo;
                newTask.Name = "(获取下载地址中)" + videoInfo.title + (videoInfo.partname == null || videoInfo.partname == string.Empty ? string.Empty : " " + videoInfo.partname);
                newTask.Aid = aid.ToString();
                newTask.Partname = videoInfo.partname==string.Empty||videoInfo.partname==null? "无":videoInfo.partname;
                newTask.Size = "获取中";
                newTask.CreateTime = DateTime.Now.ToString();
                newTask.DownloadUrl = new List<string>();
                newTask.Page = page;
                newTask.Danmu = "http://comment.bilibili.com/" + videoInfo.cid + ".xml";
                Data.ApplicationSettings.GetInstance().Dispatcher.Invoke(delegate
                {
                    Data.FetchingTasks.GetInstance().Tasks.Add(newTask);
                });
                string downJson = Helper.NetworkHelper.GetTextFromUri("http://bilibili-service.daoapp.io/video/" + newTask.VideoInfo.cid + "?quality=1&type=" +
                    Data.ApplicationSettings.GetInstance().FetchingOption.Format);
                if (downJson.Length < 100)
                {
                    Data.NotificationData.GetInstance().Add(new Classes.NotifictionMessage(
                        NotificationLevel.Error, "无法获取cid:" + videoInfo.cid + "的下载地址, 可能是非bilibili源的缘故"));
                    return;
                }
                var jvd = JsonConvert.DeserializeObject<jsonVideoDownload>(downJson);
                if (Data.ApplicationSettings.GetInstance().FetchingOption.Quality == "high")
                {
                    var quality = jvd.accept_quality.OrderByDescending(t => t).First();
                    downJson = Helper.NetworkHelper.GetTextFromUri("http://bilibili-service.daoapp.io/video/" + newTask.VideoInfo.cid + "?quality=" + quality);
                    jvd = JsonConvert.DeserializeObject<jsonVideoDownload>(downJson);
                }
                if (jvd.durl.Count == 0)
                {

                    Data.NotificationData.GetInstance().Add(new Classes.NotifictionMessage(
                        NotificationLevel.Error, "无法获取cid:" + videoInfo.cid + "的下载地址, durl.count = 0"));
                    return;
                }
                
                newTask.Size = Helper.FileHelper.GetFileSizeString(jvd.durl[0].size);
                newTask.DownloadUrl.Add(jvd.durl[0].url);
                newTask.DownloadUrl.AddRange(jvd.durl[0].backup_url);
                newTask.Name = newTask.Name.Substring(9);
                return;
                    }
            else
            {
                var errorMsg = JsonConvert.DeserializeObject<jsonVideoInfoFailedMessage>(json);
                Data.NotificationData.GetInstance().Add(new Classes.NotifictionMessage(Classes.NotificationLevel.Error, "获取视频信息失败, 错误代号: " + errorMsg.code));
                return;
            }
        }
    }

}
