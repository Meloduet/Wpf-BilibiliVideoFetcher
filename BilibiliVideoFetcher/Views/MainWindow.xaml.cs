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
            Data.Log.GetLogger().Info("MainWindow->menuItemCreateSingleTask_Click", "Clicked menuItemCreateSingleTask.");
            new Views.CreateSingleTaskWindow().Show();

        }

        private void WindowMain_Loaded(object sender, RoutedEventArgs e)
        {
            Data.Log.GetLogger().Info("MainWindow->WindowMain_Loaded", "Main Window has been loaded.");
            Data.ApplicationSettings.GetInstance().Dispatcher = this.Dispatcher;
            Data.NotificationData.Initialize(this.borderMessage, this.labelMsgTitle, this.labelMsgContent);
            dataGrid.ItemsSource = Data.FetchingTasks.GetInstance().Tasks;

        }

        private void buttonCloseMessage_Click(object sender, RoutedEventArgs e)
        {
            Data.Log.GetLogger().Info("MainWindow->buttonCloseMessage_Click", "Clicked buttonCloseMessage.");
            Data.NotificationData.GetInstance().RemoveLast();

        }

        private void menuItemSettings_Click(object sender, RoutedEventArgs e)
        {
            Data.Log.GetLogger().Info("MainWindow->menuItemSettings_Click", "Clicked menuItemSettings.");
            new Views.SettingsWindow().ShowDialog();

        }

        private void menuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            Data.Log.GetLogger().Info("MainWindow->menuItemDelete_Click", "Clicked menuItemDelete.");
            while (dataGrid.SelectedItems.Count > 0)
            {
                Data.FetchingTasks.
                    GetInstance().Tasks.Remove(GetSelectedTask(dataGrid));
            }

        }

        private void menuItemViewDetail_Click(object sender, RoutedEventArgs e)
        {
            Data.Log.GetLogger().Info("MainWindow->menuItemViewDetail_Click", "Clicked menuItemViewDetail.");
            if (dataGrid.SelectedItems.Count == 0) return;
            Views.VideoInfoWindow.Start(GetSelectedTask(dataGrid));
        }

        private void menuItemCopyDownloadUrl_Click(object sender, RoutedEventArgs e)
        {
            Data.Log.GetLogger().Info("MainWindow->menuItemCopyDownloadUrl_Click", "Clicked menuItemCopyDownloadUrl");
            if (dataGrid.SelectedItems.Count == 0) return;
            var task = GetSelectedTask(dataGrid);
            if (task.DownloadUrl == null || task.DownloadUrl.Count == 0)
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
            Data.Log.GetLogger().Info("MainWindow->GetSelectedTask", "Called GetSelectedTask.");
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
            Data.Log.GetLogger().Info("MainWindow->menuItemAbout_Click", "Clicked menuItemAbout");
            new Views.AboutWindow().ShowDialog();
        }

        private void menuItemCreateMultiTask_Click(object sender, RoutedEventArgs e)
        {
            Data.Log.GetLogger().Info("MainWindow->menuItemCreateMultiTask_Click", "Clicked menuItemCreateMultiTask");
            new Views.CreatMultiTaskWindow().Show();
        }

        private void cmd_Exit(object sender, ExecutedRoutedEventArgs e)
        {
            Data.Log.GetLogger().Info("MainWindow->cmd_Exit", "Executed Application.Current.Shutdown()");
            Application.Current.Shutdown();

        }
        private void cmd_CopyDownloadUrl(object sender, ExecutedRoutedEventArgs e)
        {
            Data.Log.GetLogger().Info("MainWindow->cmd_CopyDownloadUrl", "Executed CopyDownloadUrl");
            if (dataGrid.SelectedItems.Count == 0)
            {
                Data.NotificationData.GetInstance().Add(
                    new Classes.NotifictionMessage(NotificationLevel.Warning, "尚未选择所要获取下载地址的项"));
                return;
            }
            var task = GetSelectedTask(dataGrid);
            if (task.DownloadUrl == null || task.DownloadUrl[0] == string.Empty)
            {
                Data.NotificationData.GetInstance().Add(
                    new Classes.NotifictionMessage(NotificationLevel.Warning, "尚未获取到所要复制的下载地址"));
            }
            else
            {
                Clipboard.SetText(task.DownloadUrl[0]);
                Data.NotificationData.GetInstance().Add(
                    new Classes.NotifictionMessage(NotificationLevel.Info, "已复制下载地址到剪切板"));
            }


        }

        private void WindowMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Data.Log.GetLogger().Info("MainWindow->WindowMain_Closing", "Closing MainWindow.");
        }

        private void menuItemDownloadDamnu_Click(object sender, RoutedEventArgs e)
        {
            Data.Log.GetLogger().Info("MainWindow->menuItemDownloadDamnu_Click", "Clicked menuItemDownloadDamnu");
            if (dataGrid.SelectedItems.Count == 0) return;
            var task = GetSelectedTask(dataGrid);
            Process.DanmuOpt.SaveToFile(task);
        }

        private void menuItemFiltAndDownload_Click(object sender, RoutedEventArgs e)
        {
            Data.Log.GetLogger().Info("MainWindow->menuItemFiltAndDownload_Checked", "Clicked menuItemFiltAndDownload");
            if (dataGrid.SelectedItems.Count == 0) return;
            var task = GetSelectedTask(dataGrid);
            var act = new Action<int, string>(delegate (int i, string s)
            {
                if (i == 0) return;
                Dispatcher.Invoke(() =>
                {
                    Process.DanmuOpt.DownAndRegxFilt(task, new System.Text.RegularExpressions.Regex(s));
                });
            });
            Views.InputWindow.Start("提示", "请输入正则表达式", act);
        }

        private void menuItemCopyDanmuDownloadUrl_Click(object sender, RoutedEventArgs e)
        {
            Data.Log.GetLogger().Info("MainWindow->menuItemCopyDanmuDownloadUrl_Click", "Executed menuItemCopyDanmuDownloadUrl");
            if (dataGrid.SelectedItems.Count == 0)
            {
                Data.NotificationData.GetInstance().Add(
                    new Classes.NotifictionMessage(NotificationLevel.Warning, "尚未选择所要获取下载地址的项"));
                return;
            }
            var task = GetSelectedTask(dataGrid);
            if (task.Danmu == string.Empty)
            {
                Data.NotificationData.GetInstance().Add(
                    new Classes.NotifictionMessage(NotificationLevel.Warning, "尚未获取到所要复制的下载地址"));
            }
            else
            {
                Clipboard.SetText(task.Danmu);
                Data.NotificationData.GetInstance().Add(
                    new Classes.NotifictionMessage(NotificationLevel.Info, "已复制下载地址到剪切板"));
            }

        }

        private void menuItemRefetchDownloadUrl_Click(object sender, RoutedEventArgs e)
        {
            Data.Log.GetLogger().Info("MainWindow->menuItemRefetchDownloadUrl_Click", "Executed menuItemRefetchDownloadUrl");
            if (dataGrid.SelectedItems.Count == 0)
            {
                Data.NotificationData.GetInstance().Add(
                    new Classes.NotifictionMessage(NotificationLevel.Warning, "尚未选择所要获取下载地址的项"));
                return;
            }
            var task = GetSelectedTask(dataGrid);            
            Data.NotificationData.GetInstance().Add(
                   new Classes.NotifictionMessage(NotificationLevel.Warning, "已开始刷新下载地址."));
            new Task(()=>
            {
                Process.FetchingCore.ReFetchTask(task);
            }).Start();
        }
    }
}
