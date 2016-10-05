using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aria2Controller.Models
{
    public class Aria2TaskManager : ObservableCollection<Task>
    {
        /// <summary>
        /// 获取或设置刷新频率
        /// </summary>
        public int Interval { get; set; }


        public bool IsEnabled { get; set; }
    }
}
