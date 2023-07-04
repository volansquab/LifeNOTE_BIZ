namespace LifeNOTE_BIZ.Pages
{
    public class ConvLocItem
    {
        public int mainid;
        public int filterdid;

        public ConvLocItem()
        {
            mainid = 0;
            filterdid = 0;
        }
        public ConvLocItem(int id1, int id2)
        {
            mainid = id1;
            filterdid = id2;
        }
    }

    //シリアル化するクラス
    public class ConvLoc
    {
        //ArrayListに追加される型を指定する
        [System.Xml.Serialization.XmlArrayItem(typeof(ConvLoc)),
        System.Xml.Serialization.XmlArrayItem(typeof(string))]
        public System.Collections.ArrayList Items;
    }
}
