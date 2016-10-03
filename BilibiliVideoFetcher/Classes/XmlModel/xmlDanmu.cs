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
    public partial class xmlDanmu
        {

            private string chatserverField;

            private uint chatidField;

            private byte missionField;

            private ushort maxlimitField;

            private string sourceField;

            private List<iD> dField;

            /// <remarks/>
            public string chatserver
            {
                get
                {
                    return this.chatserverField;
                }
                set
                {
                    this.chatserverField = value;
                }
            }

            /// <remarks/>
            public uint chatid
            {
                get
                {
                    return this.chatidField;
                }
                set
                {
                    this.chatidField = value;
                }
            }

            /// <remarks/>
            public byte mission
            {
                get
                {
                    return this.missionField;
                }
                set
                {
                    this.missionField = value;
                }
            }

            /// <remarks/>
            public ushort maxlimit
            {
                get
                {
                    return this.maxlimitField;
                }
                set
                {
                    this.maxlimitField = value;
                }
            }

            /// <remarks/>
            public string source
            {
                get
                {
                    return this.sourceField;
                }
                set
                {
                    this.sourceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("d")]
            public List<iD> d
            {
                get
                {
                    return this.dField;
                }
                set
                {
                    this.dField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class iD
        {

            private string pField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string p
            {
                get
                {
                    return this.pField;
                }
                set
                {
                    this.pField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }


}
