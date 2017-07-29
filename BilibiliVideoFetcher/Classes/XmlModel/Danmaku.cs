using System.Xml.Serialization;

namespace BilibiliVideoFetcher.Classes.XmlModel
{
    /// <summary>
    /// 一条弹幕
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Text={Text}")]
    public class Danmaku
    {

        /// <remarks/>
        [XmlAttribute("p")]
        public string Property { get; set; }

        [XmlText()]
        public string Text { get; set; }
    }
}
