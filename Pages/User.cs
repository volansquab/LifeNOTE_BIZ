namespace LifeNOTE_BIZ.Pages
{
    public class UserItem
    {

        public string? username { get; set; }
        public string? userOId { get; set; }
        public string? mailaddress { get; set; }
        public string? enrolleddate { get; set; }
        public UserItem()
        {
            username = "";
            userOId = "";
            mailaddress = "";
            enrolleddate = "";
        }
        public UserItem(string username, string userOId, string mailaddress, string enrolleddate)
        {

            username = username;
            userOId = userOId;
            mailaddress = mailaddress;
            enrolleddate = enrolleddate;

        }
    }
    //シリアル化するクラス
    public class UserClass
    {
        //ArrayListに追加される型を指定する
        [System.Xml.Serialization.XmlArrayItem(typeof(UserItem)),
        System.Xml.Serialization.XmlArrayItem(typeof(string))]
        public System.Collections.ArrayList? Items;
    }
}
