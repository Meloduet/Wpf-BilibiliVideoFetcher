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
    /// SettingsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (checkBoxNoNoticeWhenDownload.IsChecked == true)
                Data.ApplicationSettings.GetInstance().FetchingOption.NoNoticeWhenDownload = true;
            else
                Data.ApplicationSettings.GetInstance().FetchingOption.NoNoticeWhenDownload = false;

            if (rbHigh.IsChecked == true)
                Data.ApplicationSettings.GetInstance().FetchingOption.Quality = "high";
            else
                Data.ApplicationSettings.GetInstance().FetchingOption.Quality = "low";

            if (rbFlv.IsChecked == true)
                Data.ApplicationSettings.GetInstance().FetchingOption.Format = "flv";
            else
                Data.ApplicationSettings.GetInstance().FetchingOption.Format = "mp4";

            this.Close();
        }

        private void WindowSettings_Loaded(object sender, RoutedEventArgs e)
        {
            checkBoxNoNoticeWhenDownload.IsChecked = Data.ApplicationSettings.GetInstance().FetchingOption.NoNoticeWhenDownload;

            if(Data.ApplicationSettings.GetInstance().FetchingOption.Quality=="high")
                rbHigh.IsChecked = true;
            else
                rbLow.IsChecked = true;

            if(Data.ApplicationSettings.GetInstance().FetchingOption.Format=="flv")
                rbFlv.IsChecked = true;
            else
                rbMp4.IsChecked = true;
        }
    }
}
