using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace BilibiliVideoFetcher.Views
{
    public abstract class TaskDialog : Window
    {
        protected static Regex s_aivPicker = new Regex(@"(\d+)\b");

        /// <summary>
        /// 指示是否直接使用av号
        /// </summary>
        public bool UseAid {
            get { return (bool)GetValue(UseAidProperty); }
            set { SetValue(UseAidProperty, value); }
        }

        public static readonly DependencyProperty UseAidProperty;

        public event EventHandler<string> ErrorCaptured;

        protected void OnErrorCaptured(Exception ex)
        {
            this.ErrorCaptured?.Invoke(this, ex.Message);
        }

        /// <summary>
        /// 从知道的问题中提取av号
        /// 若提取失败，则返回string.Empty
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected static string GetAid(string text)
        {
            var m = s_aivPicker.Match(text);
            if (m.Success)
            {
                return m.Groups[1].Value;
            }
            return string.Empty;
        }

        static TaskDialog()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TaskDialog), new FrameworkPropertyMetadata(typeof(TaskDialog)));

            UseAidProperty = DependencyProperty.Register("UseAid", typeof(bool), typeof(TaskDialog), new PropertyMetadata(false));

        }
    }
}
