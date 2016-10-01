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
        private static FetchingTasks _instance = _instance = new FetchingTasks();
        public ObservableCollection<VideoTask> Tasks { get; private set; }
        private const string CONFIG_FILE = "tasks.json";
        private FetchingTasks()
        {
            if (!File.Exists(CONFIG_FILE))
            {
                Tasks = new ObservableCollection<VideoTask>();
                SaveToFile();
            }
            string jsonInput = File.ReadAllText(CONFIG_FILE, Encoding.UTF8);
            Tasks = JsonConvert.DeserializeObject<ObservableCollection<VideoTask>>(jsonInput);
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
