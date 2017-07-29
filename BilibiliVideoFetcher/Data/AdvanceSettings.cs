using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace BilibiliVideoFetcher.Data
{
    /// <summary>
    /// 高级设置
    /// </summary>
    public static class AdvanceSettings
    {
        const string EXE_PATH = "BilibiliVideoFetcher.exe";

        /// <summary>
        /// 用于对queryString 进行签名的 AccessKey
        /// </summary>
        public static string AccessKey { get; set; }

        /// <summary>
        /// 设置，是否使用Bilibili原生API获取视频下载地址
        /// </summary>
        public static bool UseNativeApi { get; set; }

        static AdvanceSettings()
        {

            try
            {
                var appSettings = ConfigurationManager.OpenExeConfiguration(EXE_PATH).AppSettings.Settings;
                UseNativeApi = bool.Parse(appSettings["UseNativeApi"].Value);
                AccessKey = appSettings["AccessKey"].Value;
            }
            catch (ConfigurationErrorsException)
            {
                CreateDefaultSettings();
            }
            catch (NullReferenceException)
            {
                CreateDefaultSettings();
            }

        }

        /// <summary>
        /// 创建默认配置文件
        /// </summary>
        private static void CreateDefaultSettings()
        {
            UseNativeApi = false;
            AccessKey = "1c15888dc316e05a15fdd0a02ed6584f";
            File.WriteAllText(
                "BilibiliVideoFetcher.exe.config",
                "<?xml version=\"1.0\" encoding=\"utf-8\" ?><configuration></configuration>",
                Encoding.UTF8);
            var config = ConfigurationManager.OpenExeConfiguration(EXE_PATH);
            var appSettings = config.AppSettings.Settings;
            appSettings.Add("UseNativeApi", bool.FalseString);
            appSettings.Add("AccessKey", AccessKey);
            config.Save();
        }

        /// <summary>
        /// 保存配置更改
        /// </summary>
        public static void Save()
        {
            var config = ConfigurationManager.OpenExeConfiguration(EXE_PATH);
            var appSettings = config.AppSettings.Settings;
            appSettings.Remove("UseNativeApi");
            appSettings.Remove("AccessKey");
            appSettings.Add("UseNativeApi", UseNativeApi.ToString());
            appSettings.Add("AccessKey", AccessKey);
            config.Save();
        }
    }
}
