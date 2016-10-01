using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace BilibiliVideoFetcher.Data
{
    public class ApplicationSettings
    {
        private static ApplicationSettings _settings = null;
        private const string CONFIG_FILE = "settings.json";
        public Dispatcher Dispatcher { get; set; }
        public FetchingOption FetchingOption { get; set; } =  new FetchingOption();
        private ApplicationSettings() {
            if (!File.Exists(CONFIG_FILE))
            {
                SaveToFile();
            }
            string jsonInput = File.ReadAllText(CONFIG_FILE, Encoding.UTF8);
            FetchingOption = JsonConvert.DeserializeObject<FetchingOption> (jsonInput);
        }
        public static ApplicationSettings GetInstance()
        {
            if (_settings == null)
            { _settings = new ApplicationSettings(); }
            return _settings;
        }
        
        public void SaveToFile()
        {
            File.WriteAllText(CONFIG_FILE, JsonConvert.SerializeObject(FetchingOption));
        }

    }
    public class FetchingOption
    {
        public bool NoNoticeWhenDownload = true;
        public string Quality = "high";
        public string Format = "mp4";
    }
}
