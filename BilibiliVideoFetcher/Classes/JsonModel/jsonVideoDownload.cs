using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilibiliVideoFetcher.Classes.JsonModel
{
    public class Durl
    {
        public int length { get; set; }
        public int size { get; set; }
        public string url { get; set; }
        public List<string> backup_url { get; set; }
    }

    public class jsonVideoDownload
    {
        public string format { get; set; }
        public int timelength { get; set; }
        public string accept_format { get; set; }
        public List<int> accept_quality { get; set; }
        public List<Durl> durl { get; set; }
    }
}
