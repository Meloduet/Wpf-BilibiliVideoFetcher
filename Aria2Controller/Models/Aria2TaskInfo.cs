using Newtonsoft.Json;

namespace Aria2Controller.Models
{

    public enum TaskStatus
    {
        /// <summary>
        /// 正在下载或做种中
        /// </summary>
        Active,
        /// <summary>
        /// 在下载队列中
        /// </summary>
        Waiting,
        /// <summary>
        /// 下载还未开始，通常是与初始化任务或服务器建立连接的过程中
        /// </summary>
        Download,
        /// <summary>
        ///  已暂停
        /// </summary>
        Paused,
        /// <summary>
        /// 因为错误而暂停
        /// </summary>
        Error,
        /// <summary>
        /// 下载完成
        /// </summary>
        Complete,
        /// <summary>
        /// 已被用户移除
        /// </summary>
        Removed
    }

    /// <summary>
    /// 提供一些常用的字符串数组，作为获取下载任务状态时的keys参数
    /// </summary>
    public static class StandardKeys
    {
        /// <summary>
        /// 仅获取保存文件夹，下载速度，总大小，已下载大小，任务从状态
        /// </summary>
        public static readonly string[] SimpleDownlad =
            new string[] {
            Aria2TaskInfo.KEY_DIR,
            Aria2TaskInfo.KEY_DOWNLOADSPEED,
            Aria2TaskInfo.KEY_COMPLETEDLENGTH,
            Aria2TaskInfo.KEY_TOTALLENGTH,
            Aria2TaskInfo.KEY_STATUS
        };
    }

    /// <summary>
    /// Aria2下载任务详情
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Aria2TaskInfo
    {
        public const string KEY_BITFIELD = "bitfield";
        public const string KEY_COMPLETEDLENGTH = "completedLength";
        public const string KEY_CONNECTIONS = "connections";
        public const string KEY_DIR = "dir";
        public const string KEY_DOWNLOADSPEED = "downloadSpeed";
        public const string KEY_FILES = "files";
        public const string KEY_GID = "gid";
        public const string KEY_STATUS = "status";
        public const string KEY_TOTALLENGTH = "totalLength";

        /// <summary>
        /// Hexadecimal representation of the download progress.
        /// The highest bit corresponds to the piece at index 0.
        /// Any set bits indicate loaded pieces, while unset bits indicate not yet loaded and/or missing pieces.
        /// Any overflow bits at the end are set to zero. When the download was not started yet, this key will not be included in the response.
        /// </summary>
        [JsonProperty(KEY_BITFIELD)]
        public string Bitfield { get; set; }

        [JsonProperty(KEY_COMPLETEDLENGTH)]
        private string m_completedLength = "0";

        /// <summary>
        /// 已下载大小，单位：Byte
        /// </summary>
        public int CompletedLength {
            get {
                return int.Parse(this.m_completedLength);
            }
        }

        [JsonProperty(KEY_CONNECTIONS)]
        private string m_connections = "0";

        /// <summary>
        /// 当前连接的 peers/servers数
        /// </summary>
        public int Connections {
            get {
                return int.Parse(this.m_connections);
            }
        }

        /// <summary>
        /// 保存的文件夹
        /// </summary>
        [JsonProperty(KEY_DIR)]
        public string Dir { get; set; }

        [JsonProperty(KEY_DOWNLOADSPEED)]
        private string m_downloadSpeed = "0";

        /// <summary>
        /// 当前下载速度，单位 Byte/s
        /// </summary>
        public int DownloadSpeed {
            get {
                return int.Parse(this.m_downloadSpeed);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(KEY_FILES)]
        public FileToken[] Files { get; set; }

        /// <summary>
        /// 下载任务的GID
        /// </summary>
        [JsonProperty(KEY_GID)]
        public string Gid { get; set; }

        [JsonProperty("numPieces")]
        private string m_numPieces = "0";

        /// <summary>
        /// 文件数据包数量
        /// </summary>
        public int NumPieces {
            get {
                return int.Parse(this.m_numPieces);
            }
        }

        [JsonProperty("pieceLength")]
        private string m_pieceLength = "0";

        /// <summary>
        /// 每个数据包的大小，单位 Byte
        /// </summary>
        public int PieceLength {
            get {
                return int.Parse(this.m_pieceLength);
            }
        }

        /// <summary>
        /// 下载状态
        /// </summary>
        [JsonProperty(KEY_STATUS)]
        public TaskStatus Status { get; set; }


        [JsonProperty(KEY_TOTALLENGTH)]
        private string m_totalLength = "0";

        /// <summary>
        /// 下载任务的总长度，单位：Byte
        /// </summary>
        public int TotalLength {
            get {
                return int.Parse(this.m_totalLength);
            }
        }

        [JsonProperty("uploadLength")]
        private string m_uploadLength = "0";

        /// <summary>
        /// 下载任务已上传的大小，单位：Byte
        /// </summary>
        public int UploadLength {
            get {
                return int.Parse(this.m_uploadLength);
            }
        }

        [JsonProperty("uploadSpeed")]
        private string m_uploadSpeed = "0";
        /// <summary>
        /// 当前上传速度，单位 Byte/s
        /// </summary>
        public int UploadSpeed {
            get {
                return int.Parse(this.m_uploadSpeed);
            }
        }

        // TODO: compelete
        /// <summary>
        /// The code of the last error for this item, if any.
        /// The value is a string. The error codes are defined in the EXIT STATUS section.
        /// This value is only available for stopped/completed downloads.
        /// </summary>
        [JsonProperty("errorCode", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorCode;

        /// <summary>
        /// The (hopefully) human readable error message associated to errorCode.
        /// </summary>
        [JsonProperty("errorMessage", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorMessage;

        /// <summary>
        /// List of GIDs which are generated as the result of this download.
        /// For example, when aria2 downloads a Metalink file, it generates downloads described in the Metalink (see the --follow-metalink option).
        /// This value is useful to track auto-generated downloads.
        /// If there are no such downloads, this key will not be included in the response.
        /// </summary>
        [JsonProperty("followedBy", NullValueHandling = NullValueHandling.Ignore)]
        public string FollowedBy;

        /// <summary>
        /// The number of verified number of bytes while the files are being hash checked.
        /// This key exists only when this download is being hash checked.
        /// </summary>
        [JsonProperty("verifiedLength, NullValueHandling = NullValueHandling.Ignore")]
        public string verifiedLength;

        /// <summary>
        /// true if this download is waiting for the hash check in a queue.
        /// This key exists only when this download is in the queue.
        /// </summary>
        [JsonProperty("verifyIntegrityPending", NullValueHandling = NullValueHandling.Ignore)]
        public bool verifyIntegrityPending;

        #region "BitTorrent only"
        /// <summary>
        /// InfoHash. BitTorrent only.
        /// </summary>
        [JsonProperty("infoHash", NullValueHandling = NullValueHandling.Ignore)]
        public string InfoHash { get; set; }

        /// <summary>
        /// The number of seeders aria2 has connected to. BitTorrent only.
        /// </summary>
        [JsonProperty("numSeeders", NullValueHandling = NullValueHandling.Ignore)]
        public string numSeeders = "0";
        public int NumSeeders {
            get {
                return int.Parse(this.numSeeders);
            }
        }
        /// <summary>
        /// true if the local endpoint is a seeder.
        /// Otherwise false. BitTorrent only.
        /// </summary>
        [JsonProperty("seeder", NullValueHandling = NullValueHandling.Ignore)]
        public bool Seeder { get; set; }

        /// <summary>
        /// Struct which contains information retrieved from the .torrent (file).
        /// BitTorrent only. It contains following keys.
        /// </summary>
        [JsonProperty("bittorrent", NullValueHandling = NullValueHandling.Ignore)]
        public object Bittorrent;
        #endregion


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}
