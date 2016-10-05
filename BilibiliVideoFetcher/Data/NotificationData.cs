using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Threading;
using BilibiliVideoFetcher.Classes;

namespace BilibiliVideoFetcher.Data
{
    //消息数据类
    class NotificationData
    {
        private static NotificationData _notifications = null;
        private Border _containerControl;
        private Label _titleLabel;
        private Label _messageLabel;
        public static void Initialize(Border containerControl, Label titleLabel, Label messageLabel)
        {
            if (_notifications == null)
            {
                _notifications = new NotificationData();
                _notifications._containerControl = containerControl;
                _notifications._titleLabel = titleLabel;
                _notifications._messageLabel = messageLabel;
                _notifications._messages = new List<NotifictionMessage>();
            }
        }
        private NotificationData() { }

        public static NotificationData GetInstance()
        {
            return _notifications;
        }

        private List<NotifictionMessage> _messages;

        public void Add(NotifictionMessage message)
        {
            _messages.Add(message);
            ShowLast();
        }

        private NotifictionMessage pre;

        public void ShowLast()
        {
            var index = _messages.Count - 1;
            if (index < 0)
                Data.ApplicationSettings.GetInstance().Dispatcher.Invoke(delegate ()
                {
                    _containerControl.Visibility = System.Windows.Visibility.Collapsed;
                });
            else
            {
                var lastMsg = _messages[_messages.Count - 1];
                Data.ApplicationSettings.GetInstance().Dispatcher.Invoke(delegate ()
                {
                    _containerControl.Visibility = System.Windows.Visibility.Visible;
                    _titleLabel.Content = lastMsg.LevelToString() + (_messages.Count > 1 ? "(总共:" + _messages.Count + ")" : string.Empty);
                    _messageLabel.Content = lastMsg.Message;
                });

            }
        }

        internal void RemoveLast()
        {
            var lastMsg = _messages[_messages.Count - 1];
            _messages.Remove(lastMsg);
            ShowLast();
        }

        /// <summary>
        /// 语法糖，相当于NotificationData.GetInstance().Add
        /// </summary>
        /// <param name="message"></param>
        public static void AddNotifiction(NotifictionMessage message)
        {
            _notifications.Add(message);
        }

        /// <summary>
        /// 语法糖，相当于NotificationData.GetInstance().Add
        /// </summary>
        public static void AddNotifiction(NotificationLevel level, string message)
        {
            AddNotifiction(new NotifictionMessage(level, message));
        }
    }
}