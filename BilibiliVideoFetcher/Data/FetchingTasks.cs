using BilibiliVideoFetcher.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilibiliVideoFetcher.Data
{
    public class FetchingTasks
    {
        private static FetchingTasks _instance;

        public ObservableCollection<VideoTask> Tasks { get; private set; }

        public static volatile Queue<VideoTask> TaskQueue;

        private const string CONFIG_FILE = "tasks.json";

        private FetchingTasks()
        {
            //if (!File.Exists(CONFIG_FILE))
            //{
            //    Tasks = new ObservableCollection<VideoTask>();
            //    SaveToFile();
            //}
            //string jsonInput = File.ReadAllText(CONFIG_FILE, Encoding.UTF8);
            //Tasks = JsonConvert.DeserializeObject<ObservableCollection<VideoTask>>(jsonInput);
        }

        static FetchingTasks()
        {
            _instance = new FetchingTasks();

            if (File.Exists(CONFIG_FILE))
            {
                string jsonInput = File.ReadAllText(CONFIG_FILE, Encoding.UTF8);
                _instance.Tasks = JsonConvert.DeserializeObject<ObservableCollection<VideoTask>>(jsonInput);
            }
            else
            {
                _instance.Tasks = new ObservableCollection<VideoTask>();
            }

            TaskQueue = new Queue<VideoTask>();
        }

        public static void AddTask(VideoTask vt)
        {
            _instance.Tasks.Add(vt);

            // TODO: 将VideoTask添加到任务队列中，交由后台进程获取实际下载地址
            //TaskQueue.Enqueue(vt);
        }

        public static FetchingTasks GetInstance()
        {
            //test : _instance.Tasks.Add(new Model.VideoTask() { Name = "a", CreateTime = "b", DownloadUrl = "c" });
            return _instance;
        }

        ~FetchingTasks()
        {
            SaveToFile();
        }


        public void SaveToFile()
        {
            File.WriteAllText(CONFIG_FILE, JsonConvert.SerializeObject(Tasks));
        }
    }
}
