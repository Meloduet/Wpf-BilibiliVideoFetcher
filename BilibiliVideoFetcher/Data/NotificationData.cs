using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace BilibiliVideoFetcher.Data
{
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
                _notifications._messages = new List<Classes.NotifictionMessage>();
            }            
        }
        private NotificationData() { }
        public static NotificationData GetInstance()
        {            
            return _notifications;
        }
        
        private List<Classes.NotifictionMessage> _messages;

        public int Add(Classes.NotifictionMessage message)
        {
            _messages.Add(message);

            //current = -1, count = 1
            if (_containerControl.Visibility == System.Windows.Visibility.Collapsed)
            {
                ShowNext();
            } else
            {
                Data.ApplicationSettings.GetInstance().Dispatcher.Invoke(delegate () {
                    _titleLabel.Content = _messages[0].LevelToString() + "(总共:" + _messages.Count + ")";
                });
                   
            }
            return _messages.Count - 1;
        }

        public void ShowNext()
        {
            
            if (_messages.Count>1)
            {
                _messages.RemoveAt(0);
                Data.ApplicationSettings.GetInstance().Dispatcher.Invoke(delegate () {
                    _containerControl.Visibility = System.Windows.Visibility.Visible;
                    _titleLabel.Content = _messages[0].LevelToString() + "(总共:" + _messages.Count + ")";
                    _messageLabel.Content = _messages[0].Message;
                });
                
            }else if(_messages.Count==1)
            {
                Data.ApplicationSettings.GetInstance().Dispatcher.Invoke(delegate () {
                    _containerControl.Visibility = System.Windows.Visibility.Visible;
                    _titleLabel.Content = _messages[0].LevelToString();
                    _messageLabel.Content = _messages[0].Message;
                });
               
                _messages.RemoveAt(0);
            }
            else
            {
                Data.ApplicationSettings.GetInstance().Dispatcher.Invoke(delegate () {
                    _containerControl.Visibility = System.Windows.Visibility.Collapsed;
                });
                
                _messages.Clear();
            }
        }

    }
}
