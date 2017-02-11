using BilibiliVideoFetcher.Classes;
using BilibiliVideoFetcher.Classes.JsonModel;
using BilibiliVideoFetcher.Data;
using BilibiliVideoFetcher.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilibiliVideoFetcher.Process
{
    public class TaskBuilder
    {
        private const string DOWNLOAD_API = "http://bilibili-service.daoapp.io/video/";
        private const string VIDEO_INFO_API = "http://api.bilibili.com/view?type=json&appkey=8e9fc618fbd41e28&id=";

        // http://interface.bilibili.com/playurl?cid=11292577&player=1&quality=3&sign=be5fc46637620e44a983813b7a40cedf
        private const string NATIVE_API = "http://interface.bilibili.com/playurl?";

        /// <summary>
        /// 由av号和起始，结束分段号建立批量任务
        /// </summary>
        /// <param name="aid">av号</param>
        /// <param name="start">开始分段，从1开始，小于1时，将会从第一个分段开始</param>
        /// <param name="end">结束分段，大于总分段数时，将从start一直下载到最后一个分段</param>
        /// <exception cref="ArgumentOutOfRangeException">分段数不合理时，将引发ArgumentOutOfRangeException</exception>
        public static void Build(string aid, int start = 1, int end = int.MaxValue)
        {
            string json = NetworkHelper.GetTextFromUri(VIDEO_INFO_API + aid + "&page=" + 1);
            if (json.Length > 100)
            {
                var videoInfo = JsonConvert.DeserializeObject<VideoInfo>(json);

                if (start > videoInfo.pages)
                {
                    throw new ArgumentOutOfRangeException("页面范围无效");
                    //Data.NotificationData.AddErrorNotifiction("页面范围无效!");
                    //return;
                }
                // 当start小于1时，取1作为起始分段
                start = Math.Max(start, 1);
                // 当end大于总分段数时，取总分段数作为结束分段
                end = Math.Min(end, videoInfo.pages);
                var aids = new FetcherTaskToken[end - start + 1];
                int j = 0;
                for (int i = start; i <= end; i++)
                {
                    aids[j++] = new FetcherTaskToken(aid, i);
                }
                Parallel.ForEach(aids, (i) => { Build(i); });
            }
            else
            {
                var errorMsg = JsonConvert.DeserializeObject<ServerSideException>(json);
                throw errorMsg;
                //Data.NotificationData.AddErrorNotifiction(errorMsg);
            }
        }

        public static void Build(string aid, int page)
        {
            Build(new FetcherTaskToken(aid, page));
        }


        public static void Build(FetcherTaskToken token)
        {
            var jsonVideoInfo = NetworkHelper.GetTextFromUri(token.ToUri());
            if (jsonVideoInfo.Length > 100)
            {
                var videoInfo = JsonConvert.DeserializeObject<VideoInfo>(jsonVideoInfo);
                VideoTask newTask = GetVideoTask(token, videoInfo);

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
                var errorMsg = JsonConvert.DeserializeObject<ServerSideException>(jsonVideoInfo);
                Data.NotificationData.AddErrorNotifiction(errorMsg);
            }
        }

        /// <summary>
        /// 获取任务的大小和下载地址
        /// </summary>
        /// <param name="newTask"></param>
        private static void GetDownloadUrl(VideoTask newTask)
        {
            var cid = newTask.VideoInfo.cid;

            if (AdvanceSettings.UseNativeApi)
            {
                BilibiliVideoInfo info = GetVideoInfoFromNativeApi(cid);
                if (info.Code < 0)
                {
                    throw new ServerSideException(info.Code, info.Result);
                }
                AppendFileSystemInfo(newTask, info);
                return;
            }

            var settings = ApplicationSettings.GetInstance();
            var downJson = NetworkHelper.GetTextFromUri(
                DOWNLOAD_API + cid + "?quality=1&type=" + settings.FetchingOption.Format);
            if (downJson.Length < 100)
            {
                NotificationData.AddErrorNotifiction(
                    "无法获取cid:" + cid + "的下载地址, 可能是非bilibili源的缘故");
                return;
            }
            var jvd = JsonConvert.DeserializeObject<jsonVideoDownload>(downJson);

            if (settings.FetchingOption.Quality == "high")
            {
                var quality = jvd.accept_quality.OrderByDescending(t => t).First();
                downJson = NetworkHelper.GetTextFromUri(
                    DOWNLOAD_API + newTask.VideoInfo.cid + "?quality=" + quality);
                jvd = JsonConvert.DeserializeObject<jsonVideoDownload>(downJson);
            }

            if (jvd.durl.Count == 0)
            {
                NotificationData.AddErrorNotifiction(
                    "无法获取cid:" + cid + "的下载地址, durl.count = 0");
                return;
            }

            AppendFileSystemInfo(newTask, jvd);

            NotificationData.AddNotifiction(
                   NotificationLevel.Info,
                   "成功获取cid:" + cid + "的下载地址, 请复制下载地址到其它软件下载");
        }

        private static void AppendFileSystemInfo(VideoTask newTask, jsonVideoDownload jvd)
        {
            var durl = jvd.durl[0];
            newTask.RawSize = durl.size;
            newTask.DownloadUrl.Add(durl.url);
            newTask.DownloadUrl.AddRange(durl.backup_url);
            newTask.State = FetchState.Done;
            //newTask.Name = newTask.Name.Substring(9);
        }

        private static void AppendFileSystemInfo(VideoTask newTask, BilibiliVideoInfo info)
        {
            int rawSize = 0;
            StringBuilder builder = new StringBuilder();
            foreach (var item in info.Durls)
            {
                rawSize += item.Size;
                builder.AppendLine(item.Url);
            }
            var durl = info.Durls[0];
            newTask.RawSize = rawSize;
            newTask.DownloadUrl.Add(builder.ToString().TrimEnd('\r', '\n'));
            //newTask.DownloadUrl.AddRange(durl.BackupUrl);
            newTask.State = FetchState.Done;
            //newTask.Name = newTask.Name.Substring(9);
        }

        private static VideoTask GetVideoTask(string aid, int page, VideoInfo videoInfo)
        {
            return GetVideoTask(new FetcherTaskToken(aid, page), videoInfo);
        }

        private static VideoTask GetVideoTask(FetcherTaskToken token, VideoInfo videoInfo)
        {
            var newTask = new VideoTask(token)
            {
                VideoInfo = videoInfo,
                State = FetchState.Waiting,

                Name = videoInfo.title,
                Partname = videoInfo.partname,

                DownloadUrl = new List<string>(),
                Danmu = "http://comment.bilibili.com/" + videoInfo.cid + ".xml"
            };
            return newTask;
        }

        public static BilibiliVideoInfo GetVideoInfoFromNativeApi(int cid)
        {
            VideoParams vp = new VideoParams(cid);

            var xml = NetworkHelper.GetTextFromUri(NATIVE_API + vp.ToQueryString());
            Console.WriteLine(xml);
            return BilibiliVideoInfo.ParseXml(xml);
        }
    }

}
