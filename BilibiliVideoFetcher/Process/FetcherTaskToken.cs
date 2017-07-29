using BilibiliVideoFetcher.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilibiliVideoFetcher.Process
{
    public class FetcherTaskToken
    {
        public string Aid { get; set; }

        /// <summary>
        /// 从一开始的分集数
        /// </summary>
        public int PartIndex { get; set; }

        public VideoTask Task { get; set; }

        public string Id {
            get {
                return $"{this.Aid}-{this.PartIndex}";
            }
        }

        public bool Equals(FetcherTaskToken other)
        {
            return this.Aid.Equals(other.Aid) && this.PartIndex.Equals(other.PartIndex);
        }

        public override bool Equals(object obj)
        {
            if (obj is FetcherTaskToken)
            {
                return this.Equals((FetcherTaskToken)obj);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Aid.GetHashCode() ^ this.PartIndex.GetHashCode();
        }

        public string ToUri()
        {
            return "http://api.bilibili.com/view?type=json&appkey=8e9fc618fbd41e28&id=" + $"{Aid}&page={PartIndex}";
        }

        public FetcherTaskToken(string aid, int partIndex, VideoTask vk)
        {
            this.Aid = aid;
            this.PartIndex = partIndex;
            this.Task = vk;
        }

        public FetcherTaskToken(VideoTask vk) : this(vk.Aid, vk.Page, vk)
        {
        }

        public FetcherTaskToken(string aid, int partIndex) : this(aid, partIndex, null)
        {
        }

        public FetcherTaskToken(string aid) : this(aid, 1)
        {
        }

        public FetcherTaskToken() : this(string.Empty)
        {

        }
    }

    public class NewTaskRequestEventArgs : EventArgs
    {
        public FetcherTaskToken Token { get; set; }

        public NewTaskRequestEventArgs(FetcherTaskToken token)
        {
            this.Token = token;
        }
    }

    public class NewMultiTaskRequestEventArgs : EventArgs
    {
        /// <summary>
        /// 视频的av号
        /// </summary>
        public string Aid { get; set; }

        /// <summary>
        /// 起始分段，从1开始
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// 结束分段，若大于最大分段，代表要解析至最大分段
        /// </summary>
        public int End { get; set; }

        public NewMultiTaskRequestEventArgs(string aid) : this(aid, 1)
        {
        }

        public NewMultiTaskRequestEventArgs(string aid, int start) : this(aid, start, int.MaxValue)
        {
        }

        public NewMultiTaskRequestEventArgs(string aid, int start, int end)
        {
            this.Aid = aid;
            this.Start = start;
            this.End = end;
        }
    }

    /// <summary>
    /// 添加新的解析任务事件处理委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void NewTaskRequestEventHandler(object sender, NewTaskRequestEventArgs e);

    /// <summary>
    /// 添加新的批量解析任务事件处理委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void NewMultiTaskRequestEventHandler(object sender, NewMultiTaskRequestEventArgs e);
}
