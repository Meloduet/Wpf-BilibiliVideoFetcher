using Newtonsoft.Json;

namespace Aria2Controller.Models
{
    public class FileToken
    {
        [JsonProperty("index")]
        private string m_index = "0";

        /// <summary>
        /// Index of the file, starting at 1, in the same order as files appear in the multi-file torrent.
        /// </summary>
        public int Index {
            get {
                return int.Parse(this.m_index);
            }
        }

        /// <summary>
        /// File path
        /// </summary>
        [JsonProperty("path")]
        public string Path;

        [JsonProperty("length")]
        private string m_length = "0";

        /// <summary>
        /// 文件大小，单位：Byte
        /// </summary>
        public int Length {
            get {
                return int.Parse(this.m_length);
            }
        }

        [JsonProperty("completedLength")]
        private string m_completedLength = "0";

        /// <summary>
        /// 已下载文件大小，单位：Byte
        /// Please note that it is possible that sum of completedLength is less than the completedLength returned by the aria2.tellStatus() method.
        /// This is because completedLength in aria2.getFiles() only includes completed pieces.
        /// On the other hand, completedLength in aria2.tellStatus() also includes partially completed pieces.
        /// </summary>
        public int CompletedLength {
            get {
                return int.Parse(this.m_completedLength);
            }
        }

        /// <summary>
        /// true if this file is selected by --select-file option.
        /// If --select-file is not specified or this is single-file torrent or not a torrent download at all,
        /// this value is always true. Otherwise false.
        /// </summary>
        [JsonProperty("selected")]
        public bool Selected;

        /// <summary>
        /// Returns a list of URIs for this file
        /// </summary>
        [JsonProperty("uris")]
        public UriToken[] URIs;
    }
}
