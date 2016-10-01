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
    /// AboutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }
        private void StartPage(string url)
        {
            System.Diagnostics.Process.Start(url);
        }
        private void buttonBlog_Click(object sender, RoutedEventArgs e)
        {
            StartPage("https://v.meloduet.com/");
        }

        private void buttonZhihu_Click(object sender, RoutedEventArgs e)
        {
            StartPage("https://www.zhihu.com/people/spotband");
        }

        private void buttonPublishPage_Click(object sender, RoutedEventArgs e)
        {
            StartPage("https://v.meloduet.com/programming/bilibili-video-fetcher.html");
        }
    }
}
