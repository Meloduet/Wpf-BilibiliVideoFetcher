using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BilibiliVideoFetcher.Classes.XmlModel
{

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [XmlRoot(Namespace = "", ElementName = "i", DataType = "string", IsNullable = true)]
    public class XmlDanmu
    {

        private string chatserverField;

        private int chatidField;

        private int missionField;

        private int maxlimitField;

        private string sourceField;

        [XmlElement("chatserver")]
        public string ChatServer {
            get {
                return this.chatserverField;
            }
            set {
                this.chatserverField = value;
            }
        }

        [XmlElement("chatid")]
        public int ChatId {
            get {
                return this.chatidField;
            }
            set {
                this.chatidField = value;
            }
        }

        [XmlElement("mission")]
        public int Mission {
            get {
                return this.missionField;
            }
            set {
                this.missionField = value;
            }
        }

        [XmlElement("maxlimit")]
        public int MaxLimit {
            get {
                return this.maxlimitField;
            }
            set {
                this.maxlimitField = value;
            }
        }

        [XmlElement("source")]
        public string Source {
            get {
                return this.sourceField;
            }
            set {
                this.sourceField = value;
            }
        }

        /// <remarks/>
        [XmlElement("d")]
        public List<Danmaku> Items { get; set; }
        
    }
}
