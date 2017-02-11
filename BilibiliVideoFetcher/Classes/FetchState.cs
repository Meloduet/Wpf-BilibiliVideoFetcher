namespace BilibiliVideoFetcher.Classes
{
    public enum FetchState
    {
        /// <summary>
        /// 等待获取下载地址
        /// </summary>
        Waiting,
        /// <summary>
        /// 正在获取下载地址
        /// </summary>
        Active,
        /// <summary>
        /// 获取成功
        /// </summary>
        Done,
        /// <summary>
        /// 已暂停
        /// </summary>
        Pause,
        /// <summary>
        /// 获取出错
        /// </summary>
        Error
    }
}
