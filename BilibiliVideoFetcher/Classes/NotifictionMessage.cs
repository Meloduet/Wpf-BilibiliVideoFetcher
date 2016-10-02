using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilibiliVideoFetcher.Classes
{
    public enum NotificationLevel
    {
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
            switch(Level)
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
        public NotifictionMessage(NotificationLevel level, string message)
        {
            Level = level;
            Message = message;
            TimeRemain = new TimeSpan(0,0,0,0,3000);
            CreateTime = DateTime.Now;
        }
        public NotifictionMessage(NotificationLevel level, string message, TimeSpan span)
        {
            Level = level;
            Message = message;
            TimeRemain = span;
            CreateTime = DateTime.Now;
        }
        private NotifictionMessage() { }
        public NotifictionMessage Clone()
        {
            return new NotifictionMessage()
            {
                Level = this.Level,
                Message = this.Message,
                TimeRemain = this.TimeRemain,
                CreateTime = this.CreateTime,

            };
        }
    }
}
