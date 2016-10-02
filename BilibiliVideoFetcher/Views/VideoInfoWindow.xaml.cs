using BilibiliVideoFetcher.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BilibiliVideoFetcher.Views
{
    /// <summary>
    /// VideoInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class VideoInfoWindow : Window
    {
        private VideoInfoWindow()
        {
            InitializeComponent();
        }
        public VideoTask Task { get; set; }
        public static VideoInfoWindow Start(VideoTask task)
        {
            var win = new VideoInfoWindow();
            win.Task = task;           
            win.Show();
            return win;
        }

        private void WindowVideoInfo_Loaded(object sender, RoutedEventArgs e)
        {
            var task = Task;
            this.Title = task.Name + " 视频信息";
            tbVideoName.Text = task.Name;
            tbPartname.Text = task.Partname;

            tbPublisher.Text = task.VideoInfo.author;
            tbDanmuUrl.Text = task.Danmu;
            tbCoverDownloadUrl.Text = task.VideoInfo.pic;
            if (task.DownloadUrl.Count > 0)
            {
                tbDownloadUrl.Text = task.DownloadUrl[0];
                if (task.DownloadUrl.Count == 3)
                {
                    tbDownloadUrlBak1.Text = task.DownloadUrl[1];
                    tbDownloadUrlBak2.Text = task.DownloadUrl[2];
                }
            }
            new Task(new Action(delegate
            {
                var bytes = Helper.NetworkHelper.GetBytesFromUri(task.VideoInfo.pic);
                Action updateImage = () => {                    
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        bi.StreamSource = new MemoryStream(bytes);
                        bi.EndInit();
                        picCover.Source = bi;                    

                };
                Dispatcher.BeginInvoke(updateImage);
            })).Start();
        }
    }
}
