using BilibiliVideoFetcher.Classes;
using BilibiliVideoFetcher.Classes.XmlModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BilibiliVideoFetcher.Process
{
    public class DanmuOpt
    {
        public static async void SaveToFile(VideoTask task)
        {
            if (task.Danmu == string.Empty)
            {
                Data.NotificationData.GetInstance().Add(
                    new NotifictionMessage(NotificationLevel.Warning, "下载地址尚未获取到, 请等待或删除本任务."));
                return;
            }
            Data.NotificationData.GetInstance().Add(new NotifictionMessage(NotificationLevel.Info,
                "正在下载弹幕...请稍等"));
            var thrTask = new Task<string>(delegate
            {
                var damnu =  Helper.NetworkHelper.GetTextFromUri(task.Danmu);
                return damnu;
            });
            thrTask.Start();
            var danmuXml = await thrTask;
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "[弹幕]" + Helper.FileHelper.CleanInvalidFileName(task.Name); // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "Xml 格式弹幕 (.xml)|*.xml"; // Filter files by extension

            // Show save file dialog box
            bool? result = dlg.ShowDialog();
            string filename = string.Empty;

            // Process save file dialog box results
            if (result == true)
            {
                File.WriteAllText(dlg.FileName, danmuXml);
                Data.NotificationData.GetInstance().Add(
                    new NotifictionMessage( NotificationLevel.Info, "弹幕已保存到" + dlg.FileName));
            }
            else
            {
                return;
            }
        }
        public static async void DownAndRegxFilt(VideoTask task,Regex regex)
        {
           
            if (task.Danmu == string.Empty)
            {
                Data.NotificationData.GetInstance().Add(
                    new NotifictionMessage(NotificationLevel.Warning, "下载地址尚未获取到, 请等待或删除本任务."));
                return;
            }
            
            Data.NotificationData.GetInstance().Add(new NotifictionMessage(NotificationLevel.Info,
                "正在下载并过滤弹幕...请稍等"));
            var thrTask = new Task<string>(delegate
            {
                var danmu = Helper.NetworkHelper.GetTextFromUri(task.Danmu);
                return danmu;
            });
            thrTask.Start();
            var danmuXmlBytes = await thrTask;
            var xmlDanmu =  Helper.XmlSerializerHelper.XmlDeserialize<xmlDanmu>(danmuXmlBytes);
            RegxFilt(xmlDanmu, regex);

            var danmuXml = Helper.XmlSerializerHelper.XmlSerialize<xmlDanmu>(xmlDanmu);
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "[弹幕]" + Helper.FileHelper.CleanInvalidFileName(task.Name); // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "Xml 格式弹幕 (.xml)|*.xml"; // Filter files by extension

            // Show save file dialog box
            bool? result = dlg.ShowDialog();
            string filename = string.Empty;

            // Process save file dialog box results
            if (result == true)
            {
                File.WriteAllText(dlg.FileName, danmuXml);
                Data.NotificationData.GetInstance().Add(
                    new NotifictionMessage(NotificationLevel.Info, "弹幕已保存到" + dlg.FileName));
            }
            else
            {
                return;
            }
        }
        public static void RegxFilt(xmlDanmu danmu,Regex regex)
        {
            danmu.d.RemoveAll(item => (item.Value==null||regex.IsMatch(item.Value)));
        }
    }
}
