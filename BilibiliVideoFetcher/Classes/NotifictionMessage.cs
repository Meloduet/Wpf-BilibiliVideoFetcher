using BilibiliVideoFetcher.Classes.JsonModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace BilibiliVideoFetcher.Classes
{
    public enum NotificationLevel : int
    {
        None = -1,
        Debug = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }

    public class NotifictionMessage
    {
        public NotificationLevel Level { get; private set; }

        public string LevelToString()
        {
            switch (Level)
            {
                case NotificationLevel.Debug:
                    return "调试消息";
                case NotificationLevel.Info:
                    return "提示";
                case NotificationLevel.Warning:
                    return "警告";
                case NotificationLevel.Error:
                    return "错误";
            }
            return string.Empty;
        }

        public string Message { get; private set; }

        public TimeSpan TimeRemain { get; set; }

        public DateTime CreateTime { get; private set; }


        public NotifictionMessage(NotificationLevel level, string message, TimeSpan span)
        {
            Level = level;
            Message = message;
            TimeRemain = span;
            CreateTime = DateTime.Now;
            Id = null;
        }

        public NotifictionMessage(NotificationLevel level, string message) : this(level, message, new TimeSpan(0, 0, 0, 0, 3000))
        {
        }

        private NotifictionMessage() : this(NotificationLevel.Debug, string.Empty)
        {
        }

        public NotifictionMessage Clone()
        {
            return new NotifictionMessage(this.Level, this.Message, this.TimeRemain)
            {
                CreateTime = this.CreateTime,
                Id = this.Id
            };
        }

        public string Id { get; set; }
    }

    /// <summary>
    /// 使用List模拟出的Stack，能做到先进后出
    /// </summary>
    public class NotifictionMessageStack : List<NotifictionMessage>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        private NotifictionMessage _latestMessage;

        public NotifictionMessage LatestMessage {
            get { return _latestMessage; }
            private set {
                if (_latestMessage != value)
                {
                    this._latestMessage = value;
                    this.OnPropertyChanged("LatestMessage");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected void OnCollectionChanged(NotifyCollectionChangedAction action, NotifictionMessage element)
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, element));
        }

        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            this.CollectionChanged?.Invoke(this, e);
        }

        public void Push(NotificationLevel level, string message)
        {
            this.Push(new NotifictionMessage(level, message));
        }

        public void Push(NotifictionMessage item)
        {
            if (item.Id != null)
            {
                var id = item.Id;
                base.RemoveAll((i) => { return i.Id == id; });
            }

            if (this.LatestMessage == null)
            {
                this.LatestMessage = item;
            }
            else
            {
                base.Add(item);
            }
            this.OnCollectionChanged(NotifyCollectionChangedAction.Add, item);
        }

        /// <summary>
        /// 获取最后添加的元素，若Count为0，则返回null
        /// </summary>
        /// <returns></returns>
        public NotifictionMessage Pop()
        {
            NotifictionMessage msg = this.LatestMessage;
            if (msg == null)
            {
                return null;
            }

            if (this.Count > 0)
            {
                int lastIndex = this.Count - 1;
                var last = this[lastIndex];
                base.RemoveAt(lastIndex);
                this.LatestMessage = last;
            }
            else
            {
                this.LatestMessage = null;
            }
            this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, msg);
            return msg;
        }

        /// <summary>
        /// 实际消息数
        /// </summary>
        public int RawCount {
            get {
                return this.Count + ((this.LatestMessage == null) ? 0 : 1);
            }
        }

# region "语法糖"
        /// <summary>
        /// 添加通知
        /// </summary>
        /// <param name="message"></param>
        public void AddNotifiction(NotifictionMessage message)
        {
            this.Push(message);
        }

        /// <summary>
        /// 添加通知
        /// </summary>
        public void AddNotifiction(NotificationLevel level, string message)
        {
            this.Push(new NotifictionMessage(level, message));
        }

        /// <summary>
        /// 添加一个NotificationLevel为Error的通知
        /// </summary>
        /// <param name="message"></param>
        public void AddErrorNotifiction(string message)
        {
            this.Push(NotificationLevel.Error, message);
        }

        /// <summary>
        /// 添加一个NotificationLevel为Error的通知
        /// </summary>
        /// <param name="ex"></param>
        public void AddErrorNotifiction(ServerSideException ex)
        {
            this.AddErrorNotifiction(ex.ToString());
        }
        #endregion

    }
}
