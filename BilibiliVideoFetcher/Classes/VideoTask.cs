using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilibiliVideoFetcher.Classes
{
    public class VideoTask: INotifyPropertyChanged
    {
        private string _name;
        private string _aid;
        private string _createTime;
        private string _partname;
        private string _size;
        
        private List<string> _downloadUrl;
        private string _danmu;
        private JsonModel.jsonVideoInfo _videoInfo;
        public int Page { get; set; }
        public string Name { get { return _name; } set { _name = value; NotiFy("Name"); } }
        public string Aid { get { return _aid; } set { _aid = value; NotiFy("Aid"); } }
        public string CreateTime { get { return _createTime; } set { _createTime = value; NotiFy("CreateTime"); } }
        public string Danmu { get { return _danmu; } set { _danmu = value; NotiFy("Danmu"); } }
        public string Partname { get { return _partname; } set { _partname = value; NotiFy("Partname"); } }
        public string Size { get { return _size; } set { _size = value; NotiFy("Size"); } }

        public List<string> DownloadUrl { get { return _downloadUrl; } set { _downloadUrl = value; NotiFy("DownloadUrl"); } }

        public JsonModel.jsonVideoInfo VideoInfo { get { return _videoInfo; } set { _videoInfo = value; NotiFy("VideoInfo"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotiFy(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
