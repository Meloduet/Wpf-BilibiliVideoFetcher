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
using System.Windows.Shapes;

namespace BilibiliVideoFetcher.Views
{
    /// <summary>
    /// CreateSingleTaskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CreateSingleTaskWindow : Window
    {
        
        public CreateSingleTaskWindow()
        {
            InitializeComponent();
        }
        public string Status
        {
            set
            {                
                textBoxStatus.Text = value;
                
            }
        }
        private void buttonFetch_Click(object sender, RoutedEventArgs e)
        {
            new Action(delegate {
                Process.FetchingCore.NewTask(textBoxUrl.Text);                
            })();
            this.Close();

        }
    }
}
