using System.Xml.Serialization;

namespace LifeNOTE_BIZ.Pages
{
    [XmlRoot(ElementName = "TitleClass")]
    public class TitleLists
    {
        [XmlArray("Items")]
        [XmlArrayItem("DocItem")]
        public List<DocItem> Items { get; set; } = new List<DocItem>();
    }

    public class DocItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
    }
}





