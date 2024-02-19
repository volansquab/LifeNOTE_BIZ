namespace LifeNOTE_BIZ.Pages.Shared
{
           // メモ: 生成されたコードは、少なくとも .NET Framework 4.5または .NET Core/Standard 2.0 が必要な可能性があります。
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class DisplayClass
        {

            private int tabLocField;

            private int docLocField;

            private string? statusField;

            /// <remarks/>
            public int TabLoc
            {
                get
                {
                    return this.tabLocField;
                }
                set
                {
                    this.tabLocField = value;
                }
            }

            /// <remarks/>
            public int DocLoc
            {
                get
                {
                    return this.docLocField;
                }
                set
                {
                    this.docLocField = value;
                }
            }

            /// <remarks/>
            public string Status
            {
                get
                {
                    return this.statusField;
                }
                set
                {
                    this.statusField = value;
                }
            }
        }
}
