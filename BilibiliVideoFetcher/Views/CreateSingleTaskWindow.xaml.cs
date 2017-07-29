using BilibiliVideoFetcher.Process;
using System;
using System.Windows;
using System.Windows.Input;

namespace BilibiliVideoFetcher.Views
{
    /// <summary>
    /// CreateSingleTaskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CreateSingleTaskWindow : TaskDialog
    {

        public CreateSingleTaskWindow()
        {
            InitializeComponent();
        }

        private void ButtonFetch_Click(object sender, RoutedEventArgs e)
        {
            if (base.UseAid)
            {
                var aid = TaskDialog.GetAid(textBoxAid.Text);
                if (aid != string.Empty)
                {
                    if (!int.TryParse(textBoxPart.Text.Trim(), out int page))
                    {
                        page = 1;
                    }
                    this.OnNewTaskRequested(aid, page);
                }
            }
            else
            {
                var url = textBoxUrl.Text.Trim();
                try
                {
                    this.OnNewTaskRequested(FetchingCore.GetTaskTokenFromUrl(url));
                }
                catch (Exception ex)
                {
                    base.OnErrorCaptured(ex);
                    return;
                }
            }

            this.Close();
        }

        private void TextBoxUrl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textBoxUrl.SelectAll();
        }

        public event NewTaskRequestEventHandler NewTaskRequested;

        public void OnNewTaskRequested(string aid, int partIndex)
        {
            this.OnNewTaskRequested(new FetcherTaskToken(aid, partIndex));
        }

        public void OnNewTaskRequested(FetcherTaskToken token)
        {
            this.NewTaskRequested?.Invoke(this, new NewTaskRequestEventArgs(token));
        }

    }
}
