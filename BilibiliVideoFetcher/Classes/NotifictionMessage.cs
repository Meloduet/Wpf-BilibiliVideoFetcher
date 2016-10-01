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
    public struct NotifictionMessage
    {
        public NotificationLevel Level { get;}
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
        public string Message { get; }
        public NotifictionMessage(NotificationLevel level, string message)
        {
            Level = level;
            Message = message;
        }
    }
}
