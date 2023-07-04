namespace LifeNOTE_BIZ.Pages
{
    public class TitleItem
    {
        public int ID;
        public string Title;
        public string Type;
        public string Status;
        public string Colour;

        public TitleItem()
        {
            ID = 0;
            Title = "";
            Type = "";
            Status = "";
            Colour = "";
        }
        public TitleItem(int id, string title, string type, string status, string colour)
        {
            ID = id;
            Title = title;
            Type = type;
            Status = status;
            Colour = colour;
        }
    }

    //シリアル化するクラス
    public class TitleClass
    {
        //ArrayListに追加される型を指定する
        [System.Xml.Serialization.XmlArrayItem(typeof(TitleItem)),
        System.Xml.Serialization.XmlArrayItem(typeof(string))]
        public System.Collections.ArrayList? Items;
    }
}
