using BilibiliVideoFetcher.Classes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BilibiliVideoFetcher
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void menuItemCreateSingleTask_Click(object sender, RoutedEventArgs e)
        {
            new Views.CreateSingleTaskWindow().Show();
           
        }

        private void WindowMain_Loaded(object sender, RoutedEventArgs e)
        {
            Data.ApplicationSettings.GetInstance().Dispatcher = this.Dispatcher;
            Data.NotificationData.Initialize(this.borderMessage, this.labelMsgTitle, this.labelMsgContent);
            dataGrid.ItemsSource = Data.FetchingTasks.GetInstance().Tasks;
            //Data.FetchingTasks.GetInstance().Tasks.Add(new Classes.VideoTask() {Aid="666",VideoInfo=new Classes.JsonModel.jsonVideoInfo() { allow_bp=1} });
            //Data.NotificationData.GetInstance().Add(
              //  new NotifictionMessage(NotificationLevel.Debug,"Hello"));
        }

        private void buttonCloseMessage_Click(object sender, RoutedEventArgs e)
        {
            Data.NotificationData.GetInstance().RemoveLast();
        }

        private void menuItemSettings_Click(object sender, RoutedEventArgs e)
        {
            new Views.SettingsWindow().ShowDialog();
        }

        private void menuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            while (dataGrid.SelectedItems.Count > 0)
            {
                Data.FetchingTasks.
                    GetInstance().Tasks.Remove(GetSelectedTask(dataGrid));
            }

        }

        private void menuItemViewDetail_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 0) return;
            Views.VideoInfoWindow.Start(GetSelectedTask(dataGrid));
        }

        private void menuItemCopyDownloadUrl_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 0) return;
                var task = GetSelectedTask(dataGrid);
            if(task.DownloadUrl==null|| task.DownloadUrl.Count==0)
            {
                Data.NotificationData.GetInstance().Add(
                    new NotifictionMessage(NotificationLevel.Warning, "下载地址尚未获取到, 请等待或删除本任务."));
            }
            else
            {
                Clipboard.SetText(GetSelectedTask(dataGrid).DownloadUrl[0]);
            }
            
        }
        private VideoTask GetSelectedTask(DataGrid dg)
        {
            var index = dg.SelectedIndex;
            DataGridRow row = dg.ItemContainerGenerator.ContainerFromIndex(index) as DataGridRow;
            return dg.ItemContainerGenerator.ItemFromContainer(row) as VideoTask;
        }
        private void menuItemViewInBilibili_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 0) return;
            var task = GetSelectedTask(dataGrid);
            System.Diagnostics.Process.Start("http://www.bilibili.com/video/av" + task.Aid + "/index_" + task.Page + ".html");
        }

        private void menuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            new Views.AboutWindow().ShowDialog();
        }

        private void menuItemCreateMultiTask_Click(object sender, RoutedEventArgs e)
        {
            new Views.CreatMultiTaskWindow().Show();
        }

        private void cmd_Exit(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
            
        }
        private void cmd_CopyDownloadUrl(object sender, ExecutedRoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 0)
            {
                Data.NotificationData.GetInstance().Add(
                    new Classes.NotifictionMessage(NotificationLevel.Warning, "尚未选择所要获取下载地址的项"));
                return;
                    }
            var task = GetSelectedTask(dataGrid);
            if(task.DownloadUrl==null||task.DownloadUrl[0]==string.Empty)
            {
                Data.NotificationData.GetInstance().Add(
                    new Classes.NotifictionMessage(NotificationLevel.Warning, "尚未获取到所要复制的下载地址"));
            }
            else
            {
                Clipboard.SetText(task.DownloadUrl[0]);
                Data.NotificationData.GetInstance().Add(
                    new Classes.NotifictionMessage ( NotificationLevel.Info,"已复制下载地址到剪切板"));
            }
            

        }
    }
}
