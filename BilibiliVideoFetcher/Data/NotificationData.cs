using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Threading;
using BilibiliVideoFetcher.Classes;
using BilibiliVideoFetcher.Classes.JsonModel;

namespace BilibiliVideoFetcher.Data
{
    //消息数据类
    class NotificationData
    {
        private static NotificationData _notifications = null;
        private Border _containerControl;
        private Label _titleLabel;
        private Label _messageLabel;

        /// <summary>
        /// 后进先出的消息栈
        /// </summary>
        private NotifictionMessageStack _messages;

        public static void Initialize(Border containerControl, Label titleLabel, Label messageLabel)
        {
            if (_notifications == null)
            {
                _notifications = new NotificationData();
                _notifications._containerControl = containerControl;
                _notifications._titleLabel = titleLabel;
                _notifications._messageLabel = messageLabel;
                _notifications._messages = new NotifictionMessageStack();
                //_notifications._lastMsg = null;
            }
        }
        private NotificationData() { }

        public static NotificationData GetInstance()
        {
            return _notifications;
        }

        public void Add(NotifictionMessage message)
        {
            _messages.Push(message);
            //if (this._lastMsg != null)
            //{
            //}
            //this._lastMsg = message;
            ShowLast();
        }

        private NotifictionMessage pre;

        public void ShowLast()
        {
            int msgCount = _messages.RawCount;
            if (msgCount > 0)
            {
                Data.ApplicationSettings.GetInstance().Dispatcher.Invoke(delegate ()
             {
                 _containerControl.Visibility = System.Windows.Visibility.Visible;
                 _titleLabel.Content = _messages.LatestMessage.LevelToString() + (_messages.Count > 0 ? $"(总共:{msgCount})" : string.Empty);
                 _messageLabel.Content = _messages.LatestMessage.Message;
             });

            }
            else
            {
                Data.ApplicationSettings.GetInstance().Dispatcher.Invoke(delegate ()
               {
                   _containerControl.Visibility = System.Windows.Visibility.Collapsed;
               });

            }


        }

        internal void RemoveLast()
        {
            _messages.Pop();

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

        /// <summary>
        /// 添加一个NotificationLevel为Error的通知
        /// </summary>
        /// <param name="message"></param>
        public static void AddErrorNotifiction(string message)
        {
            AddNotifiction(NotificationLevel.Error, message);
        }

        /// <summary>
        /// 添加一个NotificationLevel为Error的通知
        /// </summary>
        /// <param name="ex"></param>
        public static void AddErrorNotifiction(ServerSideException ex)
        {
            AddErrorNotifiction(ex.ToString());
        }
    }
}