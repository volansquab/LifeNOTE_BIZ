using LifeNOTE_BIZ.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Diagnostics.Metrics;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Xml.Serialization;

namespace LifeNOTE_BIZ.Pages.Services
{


    public interface IUserinitServices
    {
        public void InitNewUser(bool isNewUser,String userOId,String userName,String accountName,String accessKey);
        public Task<string[]> GetInitialNumbersAsync(String userOId, String accountName, String accessKey);
        public Task<int> GetInitialSelectAsync(String userOId, String accountName, String accessKey);
        public Task<List<string>> GetInitialTabNamesAsync(String userOId, String accountName, String accessKey);
        public Task<List<string>> GetInitialColoursAsync(String userOId, String accountName, String accessKey);
        public Task<List<string>> GetInitialTitlesAsync(String userOId, String accountName, String accessKey,int selecttab);
    }

    public class UserinitServices : IUserinitServices
    {
        public static string? containerName_file;
        public static string? containerName_folder;
        public static string? containerName_setting;
        public static Task? tss1;
        public static Task? tss2;
        public static string[]? Numbers;
        public static string[]? Hitcolour;
        public static DisplayClass? display;
        public static int initTab;
        public static TitleClass? title;
        public static TitleLists? titlelists;
        public static String? titlename;
        public static String? colour;
        public static List<string> tabnamelist = new List<string>();
        public static List<string> titlenamelist = new List<string>();
        public static List<string> inacttitlenamelist = new List<string>();
        public static List<string> TabColours = new List<string>();
        public static List<string> inactTabColours = new List<string>();



        //public void InitNewUser()
        public void InitNewUser(bool isNewUser, String userOId, String userName, String accountName, String accessKey)
        {
            DateTime timeUtc = DateTime.UtcNow;
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);

            string enrolleddate = cstTime.ToString("yyyy/MM/dd HH:mm:ss");

            containerName_file = userOId + "-filedata";
            containerName_folder = userOId + "-folder";
            containerName_setting = userOId + "-setting";
            //フォルダ作成
            //最初の設定ファイル作成
            var diststr_files = "FILES.xml";
            var diststr_files_s = "S_FILES.xml";
            var diststr_loclist = "Loclist.xml";
            var diststr_new = "新規.csv";
            var diststr_new_s = "新規-S.csv";
            var diststr_new_m = "マニュアル.csv";
            var diststr_new_m_s = "マニュアル-S.csv";
            ////    //If so, do what needs to be done
            //Initialize Service

            Init.InitService(isNewUser, accountName, accessKey, containerName_setting, diststr_loclist, containerName_file, containerName_folder, diststr_new, diststr_new_s, diststr_new_m, diststr_new_m_s, diststr_files, diststr_files_s);
            saveAllUser(userOId, userName, enrolleddate);

            //await Task.Delay(500);
        }

        public async Task<string[]> GetInitialNumbersAsync(String userOId, String accountName, String accessKey)
        {
            
            var credential = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
            var storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential, true);
            containerName_setting = userOId + "-setting";
            //blob
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("settingdata");

            // Retrieve reference to a previously created container.
            CloudBlobContainer container2 = blobClient.GetContainerReference(containerName_setting);

            //HTML生成用序数詞リスト読み込み
            // Retrieve reference to a blob named "filename"
            CloudBlockBlob blockBlob1 = container.GetBlockBlobReference("Numbers.csv");

            using (var memoryStream1 = new MemoryStream())
            {
                string Numbersgen = "";
                await blockBlob1.DownloadToStreamAsync(memoryStream1);
                Numbersgen = System.Text.Encoding.UTF8.GetString(memoryStream1.ToArray());
                Numbers = Numbersgen.Split(",");

            }
            
            return Numbers;

        }

        public async Task<int> GetInitialSelectAsync(String userOId, String accountName, String accessKey)
        {

            var credential = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
            var storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential, true);
            //blob
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("settingdata");

            //HTML生成用序数詞リスト読み込み
            CloudBlockBlob blockBlob3 = container.GetBlockBlobReference("Status.xml");

            string files3;
            using (var memoryStream3 = new MemoryStream())
            {
                await blockBlob3.DownloadToStreamAsync(memoryStream3);
                files3 = System.Text.Encoding.UTF8.GetString(memoryStream3.ToArray());
            }

            // convert string to stream
            byte[] byteArray3 = Encoding.UTF8.GetBytes(files3);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream3 = new MemoryStream(byteArray3);
            // convert stream to string
            StreamReader reader3 = new StreamReader(stream3);
            XmlSerializer serializer = new XmlSerializer(typeof(DisplayClass));

            display = null;
            display = (DisplayClass)serializer.Deserialize(reader3);
            reader3.Close();
            //await Task.Delay(500);
            initTab = display.TabLoc;
            return initTab;

        }

        public async Task<List<string>> GetInitialTabNamesAsync(String userOId, String accountName, String accessKey)
        {

            var credential = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
            var storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential, true);
            //blob
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            containerName_setting = userOId + "-setting";
            // Retrieve reference to a previously created container.
            CloudBlobContainer container2 = blobClient.GetContainerReference(containerName_setting);

            //HTML生成用序数詞リスト読み込み
            CloudBlockBlob blockBlob4 = container2.GetBlockBlobReference("FILES.xml");

            string files4;
            using (var memoryStream4 = new MemoryStream())
            {
                await blockBlob4.DownloadToStreamAsync(memoryStream4);
                files4 = System.Text.Encoding.UTF8.GetString(memoryStream4.ToArray());
            }

            // convert string to stream
            byte[] byteArray4 = Encoding.UTF8.GetBytes(files4);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream4 = new MemoryStream(byteArray4);
            // convert stream to string
            StreamReader reader4 = new StreamReader(stream4);
            XmlSerializer serializer2 = new XmlSerializer(typeof(TitleClass));
            TitleClass title = new TitleClass();
            title = (TitleClass)serializer2.Deserialize(reader4);
            for (int i = 0; i <= title.Items.Count - 1; i++)
            {
                var titlestr1 = (TitleItem)title.Items[i];
                var titlestr2 = titlestr1.Title;
                var titlestr3 = titlestr1.Status;
                

                titlename = titlestr2.Replace(".csv", "");

                if (titlestr3 == "ACTIVE")
                {
                    tabnamelist.Add(titlename);
                    

                    //count3 += 1;
                }
                else
                {
                    inacttitlenamelist.Add(titlename);
                   
                }
            }
            return tabnamelist;
        }

        public async Task<List<string>> GetInitialColoursAsync(String userOId, String accountName, String accessKey)
        {

            var credential = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
            var storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential, true);
            //blob
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            containerName_setting = userOId + "-setting";
            // Retrieve reference to a previously created container.
            CloudBlobContainer container2 = blobClient.GetContainerReference(containerName_setting);

            //HTML生成用序数詞リスト読み込み
            CloudBlockBlob blockBlob4 = container2.GetBlockBlobReference("FILES.xml");

            string files4;
            using (var memoryStream4 = new MemoryStream())
            {
                await blockBlob4.DownloadToStreamAsync(memoryStream4);
                files4 = System.Text.Encoding.UTF8.GetString(memoryStream4.ToArray());
            }

            // convert string to stream
            byte[] byteArray4 = Encoding.UTF8.GetBytes(files4);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream4 = new MemoryStream(byteArray4);
            // convert stream to string
            StreamReader reader4 = new StreamReader(stream4);
            XmlSerializer serializer2 = new XmlSerializer(typeof(TitleClass));
            TitleClass title = new TitleClass();
            title = (TitleClass)serializer2.Deserialize(reader4);
            for (int i = 0; i <= title.Items.Count - 1; i++)
            {
                var titlestr1 = (TitleItem)title.Items[i];
                
                var titlestr3 = titlestr1.Status;
                var titlestr4 = titlestr1.Colour;

               

                if (titlestr3 == "ACTIVE")
                {
                    TabColours.Add(titlestr4);
                }
                else
                {
                    
                    inactTabColours.Add(titlestr4);
                }
            }
            return TabColours;
        }

        public async Task<List<string>> GetInitialTitlesAsync(String userOId, String accountName, String accessKey,int selecttab)
        {

            var credential = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
            var storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential, true);
            //blob
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            containerName_setting = userOId + "-filemanage";
            // Retrieve reference to a previously created container.
            CloudBlobContainer container2 = blobClient.GetContainerReference(containerName_setting);
            var tabNo = selecttab.ToString() + ".xml";
            //HTML生成用序数詞リスト読み込み
            CloudBlockBlob blockBlob4 = container2.GetBlockBlobReference(tabNo);

            string files4;
            using (var memoryStream4 = new MemoryStream())
            {
                await blockBlob4.DownloadToStreamAsync(memoryStream4);
                files4 = System.Text.Encoding.UTF8.GetString(memoryStream4.ToArray());
            }

            // convert string to stream
            byte[] byteArray4 = Encoding.UTF8.GetBytes(files4);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream4 = new MemoryStream(byteArray4);
            // convert stream to string
            StreamReader reader4 = new StreamReader(stream4); 
            XmlSerializer serializer2 = new XmlSerializer(typeof(TitleLists));
            TitleLists titlelists = new TitleLists();
            titlelists = (TitleLists)serializer2.Deserialize(reader4);


            //foreach (var doc in titlelists.Items)
            //{
            //    titlenamelist.Add(doc.Title);
            //}

            for (int i = 0; i <= titlelists.Items.Count - 1; i++)
            {
                var titlestr1 = (DocItem)titlelists.Items[i];
                var titlestr3 = titlestr1.Title;
                titlenamelist.Add(titlestr3);
            }



            return titlenamelist;
        }


        public static async void saveAllUser(string userOId, string username, string enrolleddate)
        {

            var customerdata1 = "";
            //blobから設定ファイルをダウンロードする。
            //storageAccountの作成（接続情報の定義）
            //アカウントネームやキー情報はAzureポータルから確認できる。
            var credential1 = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(IndexModel.accountName, IndexModel.accessKey);
            var storageAccount1 = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential1, true);
            //blob
            CloudBlobClient blobClient1 = storageAccount1.CreateCloudBlobClient();
            // Retrieve reference to a previously created container.
            CloudBlobContainer container1 = blobClient1.GetContainerReference("settingdata");

            CloudBlockBlob blockBlob1 = container1.GetBlockBlobReference("Userlist.csv");

            using (var memoryStream1 = new MemoryStream())
            {
                await blockBlob1.DownloadToStreamAsync(memoryStream1);
                customerdata1 = System.Text.Encoding.UTF8.GetString(memoryStream1.ToArray());
            }

            var customerdata2 = customerdata1 + userOId + "," + username + "," + enrolleddate + ",";

            CloudBlockBlob blockBlob2 = container1.GetBlockBlobReference("Userlist.csv");
            var options1 = new BlobRequestOptions()
            {
                ServerTimeout = TimeSpan.FromMinutes(10)
            };

            using (var stream = new MemoryStream(Encoding.Default.GetBytes(customerdata2), true))
            {
                await blockBlob1.UploadFromStreamAsync(stream);
            }
            await Task.Delay(500);
        }
    }

    }
