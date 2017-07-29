using BilibiliVideoFetcher.Classes.JsonModel;
using BilibiliVideoFetcher.Process;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BilibiliVideoFetcher.Classes
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class VideoTask : INotifyPropertyChanged, IComparable<VideoTask>
    {
        [JsonProperty("Name")]
        private string _name;

        [JsonProperty("Aid")]
        private string _aid;

        [JsonProperty("CreateTime")]
        private DateTime _createTime;

        [JsonProperty("Partname")]
        private string _partname;

        [JsonProperty("Size")]
        private int _rawSize;

        [JsonProperty("DownloadUrl")]
        private List<string> _downloadUrl;

        [JsonProperty("Danmu")]
        private string _danmu;

        [JsonProperty("VideoInfo")]
        private VideoInfo _videoInfo;

        [JsonProperty("Page")]
        public int Page { get; set; }

        [JsonProperty("State")]
        private FetchState _state;

        public FetchState State {
            get { return _state; }
            set {
                if (_state != value)
                {
                    this._state = value;
                    this.OnPropertyChanged("State");
                    this.OnPropertyChanged("Name");
                }
            }
        }

        DateTime _updateTime;
        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonProperty("UpdateTime")]
        public DateTime UpdateTime {
            get { return _updateTime; }
            set {
                if (_updateTime != value)
                {
                    this._updateTime = value;
                    this.OnPropertyChanged("UpdateTime");
                }
                ;
            }
        }

        public string Id {
            get {
                return $"{this.Aid}-{this.Page}";
            }
        }

        /// <summary>
        /// 判断两个任务，是否实际为同一个任务
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsSameTask(VideoTask other)
        {
            return this.Id == other.Id;
        }

        public string Name {
            get {
                string prefix = "";
                switch (this.State)
                {
                    case FetchState.Waiting:
                        prefix = "(等待获取下载地址)";
                        break;
                    case FetchState.Active:
                        prefix = "(获取下载地址中)";
                        break;
                    case FetchState.Done:
                        break;
                    case FetchState.Pause:
                        prefix = "(暂停中)";
                        break;
                    case FetchState.Error:
                        prefix = "(任务出错)";
                        break;
                    default:
                        break;
                }
                if (string.IsNullOrWhiteSpace(this._partname))
                {
                    return prefix + _name;
                }
                return prefix + _name + " " + this._partname;
            }
            set {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Aid {
            get { return _aid; }
            set {
                if (_aid != value)
                {
                    _aid = value;
                    OnPropertyChanged("Aid");
                }
            }
        }

        /// <summary>
        /// 任务创建时间
        /// </summary>
        public DateTime CreateTime { get { return _createTime; } }

        public string Danmu {
            get { return _danmu; }
            set {
                if (_danmu != value)
                {
                    _danmu = value;
                    OnPropertyChanged("Danmu");
                }
            }
        }

        public string Partname {
            get {
                if (string.IsNullOrWhiteSpace(this._partname))
                {
                    return "无";
                }
                return _partname;
            }
            set {
                if (_partname != value)
                {
                    _partname = value;
                    OnPropertyChanged("Partname");
                }
            }
        }

        public string Size {
            get {
                if (this._rawSize < 0)
                {
                    return "未获取";
                }
                return Helper.FileHelper.GetFileSizeString(this._rawSize);
            }
        }

        public int RawSize {
            get { return this._rawSize; }
            set {
                if (_rawSize != value)
                {
                    this._rawSize = value;
                    OnPropertyChanged("Size");
                }
            }
        }

        public List<string> DownloadUrl {
            get {
                return _downloadUrl;
            }
            set {
                if (_downloadUrl != value)
                {
                    _downloadUrl = value;
                    OnPropertyChanged("DownloadUrl");
                }
            }
        }

        public VideoInfo VideoInfo {
            get { return _videoInfo; }
            set {
                if (_videoInfo != value)
                {
                    _videoInfo = value;
                    OnPropertyChanged("VideoInfo");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public int CompareTo(VideoTask other)
        {
            if (this._aid != other.Aid)
            {
                return int.Parse(this._aid).CompareTo(int.Parse(other._aid));
            }
            return this.Page.CompareTo(other.Page);
        }

        public VideoTask() : this(string.Empty, 1)
        {
        }

        public VideoTask(FetcherTaskToken token) : this(token.Aid, token.PartIndex)
        {
        }

        public VideoTask(string aid, int page)
        {
            this.Aid = aid;
            this.Page = page;
            this._createTime = DateTime.Now;
            this._rawSize = -1;
        }

        public static implicit operator FetcherTaskToken(VideoTask vk)
        {
            return new FetcherTaskToken(vk) { };
        }
    }
}
