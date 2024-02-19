namespace LifeNOTE_BIZ.Pages
{
    // メモ: 生成されたコードは、少なくとも .NET Framework 4.5または .NET Core/Standard 2.0 が必要な可能性があります。
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class TitleLists
    {

        private ArrayOfDocListDocList[] docListField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DocList")]
        public ArrayOfDocListDocList[] DocList
        {
            get
            {
                return this.docListField;
            }
            set
            {
                this.docListField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ArrayOfDocListDocList
    {

        private string idField;

        private string titleField;

        /// <remarks/>
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }
    }


}
