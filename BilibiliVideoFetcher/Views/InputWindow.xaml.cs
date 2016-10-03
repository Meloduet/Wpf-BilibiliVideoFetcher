using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BilibiliVideoFetcher.Views
{
    /// <summary>
    /// InputWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InputWindow : Window
    {
        public InputWindow()
        {
            InitializeComponent();
        }
        Action<int, string> _callBack = null;
        public static void Start(string title,string content,Action<int,string> callBack, string defaultText = "")
        {
            var iw = new InputWindow();
            iw._callBack = callBack;
            iw.Title = title;
            iw.textBlockTitle.Text = content;
            iw.textBoxContent.Text = defaultText;
            iw.ShowDialog();
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            _callBack.Invoke(1, textBoxContent.Text);
            this.Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            _callBack.Invoke(0, textBoxContent.Text);
            this.Close();
        }
    }
}
