using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace BilibiliVideoFetcher.Classes
{
    [XmlRoot("video")]
    public class BilibiliVideoInfo
    {
        [XmlElement("result")]
        public string Result { get; set; }

        [XmlElement("code")]
        public int Code { get; set; }

        [XmlElement("timelength")]
        public int TimeLength { get; set; }

        [XmlElement("format")]
        public string FormatString { get; set; }

        [XmlIgnore]
        public VideoFormat Format {
            get {
                switch (this.FormatString)
                {
                    case "flv":
                        return VideoFormat.Flv;
                    case "mp4":
                        return VideoFormat.Mp4;
                    case "hdmp4":
                        return VideoFormat.HDmp4;
                    default:
                        return VideoFormat.Unknown;
                }
            }
        }

        [XmlElement("accept_format")]
        public string AcceptFormatString { get; set; }

        [XmlElement("accept_quality")]
        public string AcceptQuality { get; set; }

        [XmlElement("durl")]
        public DownloadUrl[] Durls { get; set; }
        
        static XmlSerializer s_serializer;
        static BilibiliVideoInfo()
        {
            s_serializer = new XmlSerializer(typeof(BilibiliVideoInfo));
        }

        public static BilibiliVideoInfo ParseXml(string xml)
        {
            return (BilibiliVideoInfo)s_serializer.Deserialize(XmlReader.Create(new StringReader(xml)));
        }
    }

    [XmlRoot("durl")]
    public class DownloadUrl
    {

        [XmlElement("order")]
        public int Order { get; set; }
        [XmlElement("length")]
        public int Length { get; set; }
        [XmlElement("size")]
        public int Size { get; set; }
        [XmlElement("url")]
        public Url Url { get; set; }
        [XmlArray("backup_url"), XmlArrayItem("url")]
        public Url[] BackupUrl { get; set; }
    }

    [XmlRoot("url")]
    [DebuggerDisplay("{URL}")]
    public class Url
    {
        [XmlText]
        public string URL { get; set; }

        public static implicit operator string(Url url)
        {
            return url.URL;
        }
    }
}
