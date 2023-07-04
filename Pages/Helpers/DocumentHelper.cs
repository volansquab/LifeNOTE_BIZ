
using Microsoft.WindowsAzure.Storage.Blob;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LifeNOTE_BIZ.Pages.Helpers
{
    public class DocumentHelper
    {
        public static async Task<string> GetXMLFromBlob(string accountName, string accessKey, string containerName_setting, string filename)
        {

            var credential = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
            var storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential, true);
            //blob
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container_setting = blobClient.GetContainerReference(containerName_setting);
            //DB本体のテキストファイル読み込み：検索後
            // Retrieve reference to a blob named "filename"
            CloudBlockBlob blockBlob = container_setting.GetBlockBlobReference(filename);
            string xmldoc;

            using (var memoryStream = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(memoryStream);
                xmldoc = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            return xmldoc;
        }
        public static Documentdata ReadDocXml(string xml)
        {
            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(xml);
            var stream = new MemoryStream(byteArray);

            // convert stream to string
            using (StreamReader sr = new StreamReader(stream))
            {
                var xmlSerializer = new XmlSerializer(typeof(Documentdata));
                var outdocdata = xmlSerializer.Deserialize(sr) as Documentdata;

                if (outdocdata == null)
                    throw new Exception("Couldn't get document data");

                return outdocdata;
            }
        }
        public static srcDocumentdata ReadResultsXml(string xml)
        {
            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(xml);
            var stream = new MemoryStream(byteArray);

            // convert stream to string
            using (StreamReader sr = new StreamReader(stream))
            {
                var xmlSerializer = new XmlSerializer(typeof(srcDocumentdata));
                var outsrcdocdata = xmlSerializer.Deserialize(sr) as srcDocumentdata;

                if (outsrcdocdata == null)
                    throw new Exception("Couldn't get result data");

                return outsrcdocdata;
            }
        }
        public static LocItem ReadLocXml(string xml)
        {
            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(xml);
            var stream = new MemoryStream(byteArray);

            // convert stream to string
            using (StreamReader sr = new StreamReader(stream))
            {
                var xmlSerializer = new XmlSerializer(typeof(LocItem));
                var locdata = xmlSerializer.Deserialize(sr) as LocItem;

                if (locdata == null)
                    throw new Exception("Couldn't get result data");

                return locdata;
            }
        }
        public static TitleItem ReadTitleXml(string xml)
        {
            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(xml);
            var stream = new MemoryStream(byteArray);

            // convert stream to string
            using (StreamReader sr = new StreamReader(stream))
            {
                var xmlSerializer = new XmlSerializer(typeof(TitleItem));
                var titledata = xmlSerializer.Deserialize(sr) as TitleItem;

                if (titledata == null)
                    throw new Exception("Couldn't get title data");

                return titledata;
            }
        }
        public static UserItem ReadUserXml(string xml)
        {
            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(xml);
            var stream = new MemoryStream(byteArray);

            // convert stream to string
            using (StreamReader sr = new StreamReader(stream))
            {
                var xmlSerializer = new XmlSerializer(typeof(UserItem));
                var userdata = xmlSerializer.Deserialize(sr) as UserItem;

                if (userdata == null)
                    throw new Exception("Couldn't get user data");

                return userdata;
            }
        }
        public static ConvLocItem ReadConvLocXml(string xml)
        {
            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(xml);
            var stream = new MemoryStream(byteArray);

            // convert stream to string
            using (StreamReader sr = new StreamReader(stream))
            {
                var xmlSerializer = new XmlSerializer(typeof(ConvLocItem));
                var cvdata = xmlSerializer.Deserialize(sr) as ConvLocItem;

                if (cvdata == null)
                    throw new Exception("Couldn't get user data");

                return cvdata;
            }
        }
    }
}
