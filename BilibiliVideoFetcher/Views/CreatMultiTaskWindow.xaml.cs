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
    /// CreatMultiTaskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CreatMultiTaskWindow : Window
    {
        public CreatMultiTaskWindow()
        {
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            var aid = textBoxAid.Text.Trim();
            if (aid != string.Empty)
            {
                new Action(delegate {
                    Process.FetchingCore.NewMultiTask("http://www.bilibili.com/video/av" + aid);
                })();


            }else
            {
                new Action(delegate {
                    if (tbPartStart.Text.Trim() == string.Empty && tbPartEnd.Text.Trim() == string.Empty)
                    {
                        Process.FetchingCore.NewMultiTask(tbUrl.Text);
                        return;
                    }
                    Process.FetchingCore.NewMultiTask(tbUrl.Text, tbPartStart.Text, tbPartEnd.Text);
                })();
            }
            
            Close();
        }

        private void tbUrl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            tbUrl.SelectAll();
        }

        private void cbUseAid_Checked(object sender, RoutedEventArgs e)
        {
            gridCustomAid.Visibility = Visibility.Visible;
        }

        private void cbUseAid_Unchecked(object sender, RoutedEventArgs e)
        {
            gridCustomAid.Visibility = Visibility.Collapsed;
        }
    }
}
