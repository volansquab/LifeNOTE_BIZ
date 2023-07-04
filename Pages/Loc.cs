namespace LifeNOTE_BIZ.Pages
{
    public class LocItem
    {
        public int ID;
        public int Loc1;
        public int Loc2;
        public string Status;

        public LocItem()
        {
            ID = 0;
            Loc1 = 0;
            Loc2 = 0;
            Status = "";
        }
        public LocItem(int id, int loc1, int loc2, string status)
        {
            ID = id;
            Loc1 = loc1;
            Loc2 = loc2;
            Status = status;
        }
    }

    //シリアル化するクラス
    public class LocClass
    {
        //ArrayListに追加される型を指定する
        [System.Xml.Serialization.XmlArrayItem(typeof(LocItem)),
        System.Xml.Serialization.XmlArrayItem(typeof(string))]
        public System.Collections.ArrayList Items;
    }
}
