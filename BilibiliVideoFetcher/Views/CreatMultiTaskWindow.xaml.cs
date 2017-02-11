using BilibiliVideoFetcher.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// CreatMultiTaskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CreatMultiTaskWindow : TaskDialog
    {
        public CreatMultiTaskWindow()
        {
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            int partStart, partEnd;
            if (!int.TryParse(tbPartStart.Text.Trim(), out partStart))
            {
                partStart = 1;
            }
            if (!int.TryParse(tbPartEnd.Text.Trim(), out partEnd))
            {
                partEnd = int.MaxValue;
            }

            string aid = null;

            if ((bool)cbUseAid.IsChecked)
            {
                aid = TaskDialog.GetAid(textBoxAid.Text);
                if (aid != string.Empty)
                {
                    this.OnNewMultiTaskRequested(aid, partStart, partEnd);
                }
            }
            else
            {
                var url = tbUrl.Text.Trim();
                try
                {
                    var token = FetchingCore.GetTaskTokenFromUrl(url);
                    this.OnNewMultiTaskRequested(token.Aid, partStart, partEnd);
                }
                catch (Exception ex)
                {
                    base.OnErrorCaptured(ex);
                    return;
                }
            }

            Close();
        }

        private void tbUrl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            tbUrl.SelectAll();
        }

        public event NewMultiTaskRequestEventHandler NewMultiTaskRequested;

        public void OnNewMultiTaskRequested(string aid, int partStart, int partEnd)
        {
            this.NewMultiTaskRequested?.Invoke(this, new NewMultiTaskRequestEventArgs(aid, partStart, partEnd));
        }
    }
}
