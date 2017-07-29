using BilibiliVideoFetcher.Classes;
using System;
using System.Windows;
using System.Windows.Controls;

namespace BilibiliVideoFetcher.Views
{
    [TemplatePart(Name = "PART_Image", Type = typeof(Image))]
    [TemplatePart(Name = "PART_msgLevel", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_msgContent", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_Button", Type = typeof(Button))]
    public class NotificationBar : Control
    {
        Image m_image;
        TextBlock m_msgLevel;
        TextBlock m_msgContent;
        Button m_button;

        public NotifictionMessage CurrentMessage {
            get { return (NotifictionMessage)GetValue(CurrentMessageProperty); }
            set { SetValue(CurrentMessageProperty, value); }
        }

        public static readonly DependencyProperty CurrentMessageProperty;

        public bool ShowIcon {
            get { return (bool)GetValue(ShowIconProperty); }
            set { SetValue(ShowIconProperty, value); }
        }

        public static readonly DependencyProperty ShowIconProperty;

        public NotificationLevel Level {
            get { return (NotificationLevel)GetValue(LevelProperty); }
            set { SetValue(LevelProperty, value); }
        }

        public static readonly DependencyProperty LevelProperty;

        public string MessageBody {
            get { return (string)GetValue(MessageBodyProperty); }
            set { SetValue(MessageBodyProperty, value); }
        }

        public static readonly DependencyProperty MessageBodyProperty;

        public string MessageType {
            get { return (string)GetValue(MessageTypeProperty); }
            set { SetValue(MessageTypeProperty, value); }
        }

        public static readonly DependencyProperty MessageTypeProperty;

        private NotifictionMessageStack _messageStack;

        public NotifictionMessageStack MessageStack {
            get {
                return this._messageStack;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.m_image = (Image)base.GetTemplateChild("PART_Image");
            this.m_msgLevel = (TextBlock)base.GetTemplateChild("PART_msgLevel");
            this.m_msgContent = (TextBlock)base.GetTemplateChild("PART_msgContent");
            this.m_button = (Button)base.GetTemplateChild("PART_Button");

            if (this.m_button != null)
            {
                this.m_button.Click += CloseButton_Click;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this._messageStack.Pop();
        }

        public NotificationBar()
        {
            this._messageStack = new NotifictionMessageStack();

            this._messageStack.PropertyChanged += MessageStack_PropertyChanged;
        }

        private void MessageStack_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LatestMessage")
            {
                Dispatcher.BeginInvoke(new Action(UpdateMessageDisplay));
            }
        }

        private void UpdateMessageDisplay()
        {

            var msg = this._messageStack.LatestMessage;
            if (msg == null)
            {
                this.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.MessageBody = msg.Message;
                this.MessageType = msg.LevelToString() + (this._messageStack.Count > 0 ? $"(总共:{this._messageStack.RawCount})" : string.Empty);
                this.Level = msg.Level;
                this.Visibility = Visibility.Visible;
            }

        }

        static NotificationBar()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationBar), new FrameworkPropertyMetadata(typeof(NotificationBar)));
            FrameworkElement.VisibilityProperty.OverrideMetadata(typeof(NotificationBar), new FrameworkPropertyMetadata(Visibility.Collapsed));

            CurrentMessageProperty = DependencyProperty.Register("CurrentMessage", typeof(NotifictionMessage), typeof(NotificationBar), new PropertyMetadata(null));
            ShowIconProperty = DependencyProperty.Register("ShowIcon", typeof(bool), typeof(NotificationBar), new PropertyMetadata(false));
            LevelProperty = DependencyProperty.Register("Level", typeof(NotificationLevel), typeof(NotificationBar), new PropertyMetadata(NotificationLevel.None));
            MessageBodyProperty = DependencyProperty.Register("MessageBody", typeof(string), typeof(NotificationBar), new PropertyMetadata(string.Empty));
            MessageTypeProperty = DependencyProperty.Register("MessageType", typeof(string), typeof(NotificationBar), new PropertyMetadata(string.Empty));
        }


        public static NotifictionMessageStack GlobalMessageStack { get; private set; }
    }
}
