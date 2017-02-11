using BilibiliVideoFetcher.Data;
using System.Configuration;
using System.Windows;

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

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            ApplicationSettings currentSettings = ApplicationSettings.GetInstance();
            var fetchingOption = currentSettings.FetchingOption;
            if (checkBoxNoNoticeWhenDownload.IsChecked == true)
                fetchingOption.NoNoticeWhenDownload = true;
            else
               fetchingOption.NoNoticeWhenDownload = false;

            if (rbHigh.IsChecked.Value)
                fetchingOption.Quality = "high";
            else
                fetchingOption.Quality = "low";

            if (rbFlv.IsChecked.Value)
                fetchingOption.Format = "flv";
            else
                fetchingOption.Format = "mp4";

            currentSettings.SaveToFile();

            AdvanceSettings.UseNativeApi = tgbUseNativeApi.IsChecked.Value;
            AdvanceSettings.AccessKey = txtAccessKey.Text.Trim();
            AdvanceSettings.Save();
            this.Close();
        }

        private void WindowSettings_Loaded(object sender, RoutedEventArgs e)
        {

            ApplicationSettings currentSettings = ApplicationSettings.GetInstance();
            var fetchingOption = currentSettings.FetchingOption;
            checkBoxNoNoticeWhenDownload.IsChecked = fetchingOption.NoNoticeWhenDownload;

            if (fetchingOption.Quality == "high")
                rbHigh.IsChecked = true;
            else
                rbLow.IsChecked = true;

            if (fetchingOption.Format == "flv")
                rbFlv.IsChecked = true;
            else
                rbMp4.IsChecked = true;

            tgbUseNativeApi.IsChecked = AdvanceSettings.UseNativeApi;
            txtAccessKey.Text = AdvanceSettings.AccessKey;
        }

        private void ButtonCanel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
