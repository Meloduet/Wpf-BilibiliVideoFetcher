using BilibiliVideoFetcher.Classes;
using BilibiliVideoFetcher.Data;
using BilibiliVideoFetcher.Process;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void WindowMain_Loaded(object sender, RoutedEventArgs e)
        {
            Log.GetLogger().Info("MainWindow->WindowMain_Loaded", "Main Window has been loaded.");
            ApplicationSettings.GetInstance().Dispatcher = this.Dispatcher;
            NotificationData.Initialize(this.borderMessage, this.labelMsgTitle, this.labelMsgContent);
            dataGrid.ItemsSource = Data.FetchingTasks.GetInstance().Tasks;

        }

        private void ButtonCloseMessage_Click(object sender, RoutedEventArgs e)
        {
            Log.GetLogger().Info("MainWindow->buttonCloseMessage_Click", "Clicked buttonCloseMessage.");
            NotificationData.GetInstance().RemoveLast();

        }

        private void MenuItemSettings_Click(object sender, RoutedEventArgs e)
        {
            Log.GetLogger().Info("MainWindow->menuItemSettings_Click", "Clicked menuItemSettings.");
            new Views.SettingsWindow().ShowDialog();

        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            Log.GetLogger().Info("MainWindow->menuItemDelete_Click", "Clicked menuItemDelete.");
            while (dataGrid.SelectedItems.Count > 0)
            {
                FetchingTasks.
                    GetInstance().Tasks.Remove(GetSelectedTask(dataGrid));
            }

        }

        private void MenuItemViewDetail_Click(object sender, RoutedEventArgs e)
        {
            Log.GetLogger().Info("MainWindow->menuItemViewDetail_Click", "Clicked menuItemViewDetail.");
            if (dataGrid.SelectedItems.Count == 0) return;
            Views.VideoInfoWindow.Start(GetSelectedTask(dataGrid));
        }

        private void MenuItemCopyDownloadUrl_Click(object sender, RoutedEventArgs e)
        {
            Log.GetLogger().Info("MainWindow->menuItemCopyDownloadUrl_Click", "Clicked menuItemCopyDownloadUrl");
            if (dataGrid.SelectedItems.Count == 0) return;
            var task = GetSelectedTask(dataGrid);
            if (task.DownloadUrl == null || task.DownloadUrl.Count == 0)
            {
                NotificationData.GetInstance().Add(
                    new NotifictionMessage(NotificationLevel.Warning, "下载地址尚未获取到, 请等待或删除本任务."));
            }
            else
            {
                Clipboard.SetText(GetSelectedTask(dataGrid).DownloadUrl[0]);
            }

        }
        private VideoTask GetSelectedTask(DataGrid dg)
        {
            Log.GetLogger().Info("MainWindow->GetSelectedTask", "Called GetSelectedTask.");
            return (VideoTask)dg.SelectedItem;
            //var index = dg.SelectedIndex;
            //DataGridRow row = dg.ItemContainerGenerator.ContainerFromIndex(index) as DataGridRow;
            //return dg.ItemContainerGenerator.ItemFromContainer(row) as VideoTask;
        }
        private void MenuItemViewInBilibili_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 0) return;
            var task = GetSelectedTask(dataGrid);
            System.Diagnostics.Process.Start("http://www.bilibili.com/video/av" + task.Aid + "/index_" + task.Page + ".html");
        }

        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            Log.GetLogger().Info("MainWindow->menuItemAbout_Click", "Clicked menuItemAbout");
            new Views.AboutWindow().ShowDialog();
        }

        private void Cmd_Exit(object sender, ExecutedRoutedEventArgs e)
        {
            Log.GetLogger().Info("MainWindow->cmd_Exit", "Executed Application.Current.Shutdown()");
            Application.Current.Shutdown();

        }
        private void Cmd_CopyDownloadUrl(object sender, ExecutedRoutedEventArgs e)
        {
            Log.GetLogger().Info("MainWindow->cmd_CopyDownloadUrl", "Executed CopyDownloadUrl");
            if (dataGrid.SelectedItems.Count == 0)
            {
                NotificationData.GetInstance().Add(
                    new NotifictionMessage(NotificationLevel.Warning, "尚未选择所要获取下载地址的项"));
                return;
            }
            var task = GetSelectedTask(dataGrid);
            if (task.DownloadUrl == null || task.DownloadUrl[0] == string.Empty)
            {
                NotificationData.GetInstance().Add(
                    new NotifictionMessage(NotificationLevel.Warning, "尚未获取到所要复制的下载地址"));
            }
            else
            {
                Clipboard.SetText(task.DownloadUrl[0]);
                NotificationData.GetInstance().Add(
                    new NotifictionMessage(NotificationLevel.Info, "已复制下载地址到剪切板"));
            }


        }

        private void WindowMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Log.GetLogger().Info("MainWindow->WindowMain_Closing", "Closing MainWindow.");
        }

        private void MenuItemDownloadDamnu_Click(object sender, RoutedEventArgs e)
        {
            Log.GetLogger().Info("MainWindow->menuItemDownloadDamnu_Click", "Clicked menuItemDownloadDamnu");
            if (dataGrid.SelectedItems.Count == 0) return;
            var task = GetSelectedTask(dataGrid);
            DanmuOpt.SaveToFile(task);
        }

        private void MenuItemFiltAndDownload_Click(object sender, RoutedEventArgs e)
        {
            Log.GetLogger().Info("MainWindow->menuItemFiltAndDownload_Checked", "Clicked menuItemFiltAndDownload");
            if (dataGrid.SelectedItems.Count == 0) return;
            var task = GetSelectedTask(dataGrid);
            var act = new Action<int, string>(delegate (int i, string s)
            {
                if (i == 0) return;
                Dispatcher.Invoke(() =>
                {
                    DanmuOpt.DownAndRegxFilt(task, new System.Text.RegularExpressions.Regex(s));
                });
            });
            Views.InputWindow.Start("提示", "请输入正则表达式", act);
        }

        private void MenuItemCopyDanmuDownloadUrl_Click(object sender, RoutedEventArgs e)
        {
            Log.GetLogger().Info("MainWindow->menuItemCopyDanmuDownloadUrl_Click", "Executed menuItemCopyDanmuDownloadUrl");
            if (dataGrid.SelectedItems.Count == 0)
            {
                NotificationData.GetInstance().Add(
                    new NotifictionMessage(NotificationLevel.Warning, "尚未选择所要获取下载地址的项"));
                return;
            }
            var task = GetSelectedTask(dataGrid);
            if (task.Danmu == string.Empty)
            {
                NotificationData.GetInstance().Add(
                    new NotifictionMessage(NotificationLevel.Warning, "尚未获取到所要复制的下载地址"));
            }
            else
            {
                Clipboard.SetText(task.Danmu);
                NotificationData.GetInstance().Add(
                    new NotifictionMessage(NotificationLevel.Info, "已复制下载地址到剪切板"));
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
            new Task(() =>
            {
                Process.FetchingCore.ReFetchTask(task);
            }).Start();
        }

        #region "添加解析任务"

        private void WaitForClose(Window dialog)
        {
            do
            {
                try
                {
                    dialog.ShowDialog();
                }
                catch (ArgumentException ex)
                {
                    NotificationData.AddErrorNotifiction(ex.Message);
                }
                catch (NotSupportedException ex)
                {
                    NotificationData.AddErrorNotifiction(ex.Message);
                }
            } while (dialog.Visibility == Visibility.Visible);

        }

        private void AddNewTaskCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Log.GetLogger().Info("MainWindow->AddNewTaskCommand_Executed", $"{sender}, AddNewTaskCommand_Executed.");
            var dialog = new Views.CreateSingleTaskWindow() { Owner = this };
            dialog.NewTaskRequested += Dialog_NewTaskRequested;
            dialog.ErrorCaptured += Dialog_ErrorCaptured;

            dialog.ShowDialog();
        }

        private void AddNewMultiTaskCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Log.GetLogger().Info("MainWindow->AddNewMultiTaskCommand_Executed", $"{sender}, AddNewMultiTaskCommand_Executed");
            var dialog = new Views.CreatMultiTaskWindow() { Owner = this };
            dialog.NewMultiTaskRequested += Dialog_NewMultiTaskRequested;
            dialog.ErrorCaptured += Dialog_ErrorCaptured;

            dialog.ShowDialog();
        }

        private void Dialog_ErrorCaptured(object sender, string e)
        {
            NotificationData.AddErrorNotifiction(e);
        }

        private void Dialog_NewTaskRequested(object sender, Process.NewTaskRequestEventArgs e)
        {
            FetchingCore.CreateTask(e.Token);
        }

        private void Dialog_NewMultiTaskRequested(object sender, NewMultiTaskRequestEventArgs e)
        {
            FetchingCore.NewMultiTask(e.Aid, e.Start, e.End);
        }
        #endregion
    }
}
