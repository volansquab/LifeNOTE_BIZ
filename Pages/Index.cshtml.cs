using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


using System.Security.Claims;
using System.Text;


using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

using Microsoft.WindowsAzure.Storage.Blob;

using LifeNOTE_BIZ.Pages.Services;
using System.Xml.Serialization;
using LifeNOTE_BIZ.Pages.Shared;
using System.Threading.Tasks;
using System.Security.Policy;

namespace LifeNOTE_BIZ.Pages
{
    //[Authorize]
    [DisableRequestSizeLimit]
    [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
    public class IndexModel : PageModel
    {
        private IWebHostEnvironment _environment;
        private IUserinitServices _userinit;
        public IndexModel(IWebHostEnvironment environment, ILogger<IndexModel> logger, IConfiguration configuration, IUserinitServices userinitService)
        {
            _environment = environment;
            _logger = logger;
            _configuration = configuration;
            _userinit = userinitService;
        }

        [BindProperty]
        public List<IFormFile> Upload { get; set; }
        [BindProperty]
        public string Name { get; set; }
        private readonly ILogger<IndexModel> _logger;
        public static List<string> extensions = new List<string>()
            {"doc", "docx","docm","dotm","dotx",
            "xlsx", "xlsb", "xls", "xlsm",
            "pptx", "ppsx", "ppt", "pps", "pptm", "potm", "ppam", "potx", "ppsm"};
        public static List<string> extensions2 = new List<string>()
            {"pdf"};
        public static List<List<Documentdata>> Alldoc = new List<List<Documentdata>>();
        public static List<Documentdata> Documents = new List<Documentdata>();
        public static Documentdata Document = new Documentdata();
        public static List<List<srcDocumentdata>> srcAlldoc = new List<List<srcDocumentdata>>();
        public static List<srcDocumentdata> srcDocuments = new List<srcDocumentdata>();
        public static srcDocumentdata srcDocument = new srcDocumentdata();
        public static List<string> indx = new List<string>();
        public static List<string> scdindx = new List<string>();
        public static List<string> tablists = new List<string>();
        public static List<string> titlelists = new List<string>();
        public static Task<List<string>>? TabLists;
        public static Task<List<string>>? TitleNameLists;
        public static List<string> inacttitlenamelist = new List<string>();
        public static List<string> TabColoursGen = new List<string>();
        public static List<string> TabColours = new List<string>();
        public static Task<List<string>> TabColoursTaks;
        public static string screenmode = "normal";
        public static int datasize;
        public static int selecttab;
        public static Task<int>? SelectTabTask;
        public static string? bodyelement;
        public static string? bodyelement2;
        public static string? list;
        public static string[]? Numbers;
        public static Task<string[]>? NumbersTask;

        public static string[]? keywords;
        public static string[]? Hitcolour;
        public static string? activetabname;
        public static string? activetabname2;
        public static string[]? Attachfiles;

        public string titlename;
        public static string? evnt;
        public string htmldata;
        public string textdata;
        public static TitleClass? result;
        public static TitleClass? result2;
        public static TitleClass? result3;
        public static TitleClass? resultall;
        public static LocClass? LocGen;
        public static LocClass? Loc;
        public static UserClass? Users;
        public static string? accountName;
        public static string? accessKey;
        public static string? keyword;
        public static string? field;
        public static string? global;
        public static string? temp;
        public static string? author;
        public static string? updatedate;
        public static string? attachfile;
        public static string? folderstring;
        public static string? attachlink1;
        public static string? errormsg;
        public static string? file;
        public static string? errormessage;
        public static string? title;
        public static string? title2;
        public static string? indxid;
        public static string? tabindx;
        public static string? titlechangemode;
        public static string? uploadmode;
        public static string? addtab;
        public static string? inactivetab;
        public static string? deleteno;
        public static int locstr;
        public static string? showtab;
        public static string? deletetab;
        public static string? key2;
        public static string? SelectedTabName;
        public static string? initattachfile;
        public static string? initname;
        public static int activetab;
        public static string? scrollloc;
        public static string? textheight;

        public static string? deletemode;
        public static string? rerativeloc;
        private BlobContinuationToken ctoken;
        public readonly IConfiguration _configuration;
        public static bool getf;
        public static string? username;
        public static string? containerName_file;
        public static string? containerName_folder;
        public static string? containerName_setting;
        public static Task? tss1;
        public static Task? tss2;
        public static Task? tss3;
        public static Task? tss4;
        public static Task? tss5;
        public static Task? tss6;
        public static Task? tss7;
        public static ConvLoc? ConvLoc;
        public static LocItem? locstr1;
        public static int locstr2;
        public static LocItem? locstr_l;
        public static int normal_row;
        public static int searched_row;
        public static ConvLocItem? locstr_c;
        public static int all_loc_c;
        public static DisplayClass? display;
        public static TitleClass? titlec;


        public static List<string> inactTabColours = new List<string>();

        public static int count2;
        private readonly IUserinitServices _myService = null;
        public string ResultText;
        //private IUserinitServices _userinit;

        public async Task OnGetAsync()
        {
            //new user process
            bool isNewUser = User.Claims.FirstOrDefault(x => x.Type == "newUser") != null;
            string userOId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            username = User.Identity?.Name;
            accountName = _configuration.GetValue<string>("accountName");
            accessKey = _configuration.GetValue<string>("accessKey");
            //新規ユーザーの場合の処理
            _userinit.InitNewUser(isNewUser,userOId,username, accountName, accessKey);
            //await Task.Delay(500);

            ////タブ色のリスト読み込み
            //CloudBlockBlob blockBlob2 = container.GetBlockBlobReference("hitcolour.csv");

            //using (var memoryStream2 = new MemoryStream())
            //{
            //    string hitcolourgen = "";
            //    await blockBlob2.DownloadToStreamAsync(memoryStream2);
            //    hitcolourgen = System.Text.Encoding.UTF8.GetString(memoryStream2.ToArray());
            //    Hitcolour = hitcolourgen.Split(",");
            //}
            //await Task.Delay(500);

            NumbersTask = _userinit.GetInitialNumbersAsync(userOId, accountName, accessKey);
            Numbers = await NumbersTask;
            SelectTabTask = _userinit.GetInitialSelectAsync(userOId, accountName, accessKey);
            selecttab = await SelectTabTask;
            TabLists = _userinit.GetInitialTabNamesAsync(userOId, accountName, accessKey);
            tablists = await TabLists;
            TabColoursTaks = _userinit.GetInitialColoursAsync(userOId, accountName, accessKey);
            TabColours = await TabColoursTaks;
            TitleNameLists = _userinit.GetInitialTitlesAsync(userOId, accountName, accessKey, selecttab);
            titlelists = await TitleNameLists;
            //await Task.Delay(500);
        }

        public async Task<JsonResult> OnPostAsync()
        {

            
            //string jsonString = JsonSerializer.Serialize(v1);

            string jsonString = "";

            return new JsonResult(jsonString);
        }

        public static bool Chkdublication(String str)
        {
            var flag = true;

            for (int i = 0; i <= IndexModel.Alldoc[IndexModel.selecttab - 1].Count - 1; i++)
            {
                if (IndexModel.Alldoc[IndexModel.selecttab - 1][i].Title == str)
                {
                    flag = true;
                    break;
                }
                else
                {
                    flag = false;
                }
            }

            return flag;
        }

        public static bool Chkdublication2(String str)
        {

            bool flag = false;
            foreach (TitleItem item in result.Items)
            {
                var str1 = item.Title;
                if (str1 == str)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }
        public static async void saveDoc()
        {
            string csv_body1 = "";
            //Alldoc = new List<List<Documentdata>>();
            for (int i = 0; i <= IndexModel.Alldoc[IndexModel.selecttab - 1].Count - 1; i++)
            {
                csv_body1 = csv_body1
                    + IndexModel.Alldoc[IndexModel.selecttab - 1][i].Id.Replace(";", "<seprt1>") + "<spert2>"
                    + IndexModel.Alldoc[IndexModel.selecttab - 1][i].Title.Replace(";", "<seprt1>") + "<spert2>"
                    + IndexModel.Alldoc[IndexModel.selecttab - 1][i].Body.Replace(";", "<seprt1>") + "<spert2>"
                    + IndexModel.Alldoc[IndexModel.selecttab - 1][i].Htmlbody.Replace(";", "<seprt1>") + "<spert2>"
                    + IndexModel.Alldoc[IndexModel.selecttab - 1][i].Upddate.Replace(";", "<seprt1>") + "<spert2>"
                    + IndexModel.Alldoc[IndexModel.selecttab - 1][i].Updauther.Replace(";", "<seprt1>") + "<spert2>"
                    + IndexModel.Alldoc[IndexModel.selecttab - 1][i].Attchfiles.Replace(";", "<seprt1>") + "<spert2>"
                    + IndexModel.Alldoc[IndexModel.selecttab - 1][i].Scroolloc.Replace(";", "<seprt1>") + "<spert2>";
            }
            csv_body1 = csv_body1.Substring(0, csv_body1.Length - "<seprt2>".Length);
            //blobから設定ファイルをダウンロードする。
            //storageAccountの作成（接続情報の定義）
            //アカウントネームやキー情報はAzureポータルから確認できる。
            var credential1 = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(IndexModel.accountName, IndexModel.accessKey);
            var storageAccount1 = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential1, true);
            //blob
            CloudBlobClient blobClient1 = storageAccount1.CreateCloudBlobClient();
            // Retrieve reference to a previously created container.
            CloudBlobContainer container1 = blobClient1.GetContainerReference(containerName_file);
            ConvLoc ConvLoc = new ConvLoc();
            ConvLoc.Items = new System.Collections.ArrayList();
            var count = 0;
            for (int i = 0; i <= LocGen.Items.Count - 1; i++)
            {
                var locstr10 = (LocItem)LocGen.Items[i];
                var locstr40 = locstr10.Status;
                if (locstr40 == "ACTIVE")
                {
                    ConvLoc.Items.Add(new ConvLocItem(i, count));
                    count = count + 1;
                }
            }
            var locstr1 = (ConvLocItem)ConvLoc.Items[IndexModel.selecttab - 1];
            var loc1 = locstr1.mainid;
            var titlestr1 = (TitleItem)result.Items[loc1];
            var titlestr2 = titlestr1.Title;
            CloudBlockBlob blockBlob1 = container1.GetBlockBlobReference(titlestr2);
            var options1 = new BlobRequestOptions()
            {
                ServerTimeout = TimeSpan.FromMinutes(10)
            };

            using (var stream = new MemoryStream(Encoding.Default.GetBytes(csv_body1), false))
            {
                await blockBlob1.UploadFromStreamAsync(stream);
            }
            await Task.Delay(5000);

        }
        public static async void saveDoc2()
        {
            //srcAlldoc = new List<List<srcDocumentdata>>();
            for (int indx = 0; indx <= IndexModel.srcAlldoc.Count - 1; indx++)
            {
                string csv_body1 = "";
                for (int i = 0; i <= IndexModel.srcAlldoc[indx].Count - 1; i++)
                {
                    csv_body1 = csv_body1
                    + IndexModel.srcAlldoc[indx][i].Id.Replace(";", "<seprt1>") + "<spert2>"
                    + IndexModel.srcAlldoc[indx][i].Title.Replace(";", "<seprt1>") + "<spert2>"
                    + IndexModel.srcAlldoc[indx][i].Body.Replace(";", "<seprt1>") + "<spert2>"
                    + IndexModel.srcAlldoc[indx][i].Htmlbody.Replace(";", "<seprt1>") + "<spert2>"
                    + IndexModel.srcAlldoc[indx][i].Upddate.Replace(";", "<seprt1>") + "<spert2>"
                    + IndexModel.srcAlldoc[indx][i].Updauther.Replace(";", "<seprt1>") + "<spert2>"
                    + IndexModel.srcAlldoc[indx][i].Attchfiles.Replace(";", "<seprt1>") + "<spert2>"
                    + "0.0" + "<spert2>"
                    + IndexModel.srcAlldoc[indx][i].Loc.Replace(";", "<seprt1>") + "<spert2>";
                }
                if (IndexModel.srcAlldoc[indx].Count != 0)
                {
                    csv_body1 = csv_body1.Substring(0, csv_body1.Length - "<seprt2>".Length);
                }


                //blobから設定ファイルをダウンロードする。
                //storageAccountの作成（接続情報の定義）
                //アカウントネームやキー情報はAzureポータルから確認できる。
                var credential1 = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(IndexModel.accountName, IndexModel.accessKey);
                var storageAccount1 = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential1, true);
                //blob
                CloudBlobClient blobClient1 = storageAccount1.CreateCloudBlobClient();
                // Retrieve reference to a previously created container.
                CloudBlobContainer container1 = blobClient1.GetContainerReference(containerName_file);

                ConvLoc ConvLoc = new ConvLoc();
                ConvLoc.Items = new System.Collections.ArrayList();
                var count = 0;
                for (int i = 0; i <= LocGen.Items.Count - 1; i++)
                {
                    var locstr10 = (LocItem)LocGen.Items[i];
                    var locstr40 = locstr10.Status;
                    if (locstr40 == "ACTIVE")
                    {
                        ConvLoc.Items.Add(new ConvLocItem(i, count));
                        count = count + 1;
                    }
                }

                var locstr1 = (ConvLocItem)ConvLoc.Items[indx];
                var loc1 = locstr1.mainid;


                var titlestr1 = (TitleItem)result2.Items[loc1];
                var titlestr2 = titlestr1.Title;
                //titlestr2 = titlestr2.Replace(".csv", "-S.csv");

                CloudBlockBlob blockBlob1 = container1.GetBlockBlobReference(titlestr2);
                var options1 = new BlobRequestOptions()
                {
                    ServerTimeout = TimeSpan.FromMinutes(10)
                };
                using (var stream = new MemoryStream(Encoding.Default.GetBytes(csv_body1), false))
                {
                    await blockBlob1.UploadFromStreamAsync(stream);
                }
            }
        }
        public static async void saveLoc(string tab_no, string loc)
        {

            ////XMLシリアル化するオブジェクト
            //var count = 0;
            //LocClass Loc = new LocClass();
            //Loc.Items = new System.Collections.ArrayList();
            //ConvLoc ConvLoc = new ConvLoc();
            //ConvLoc.Items = new System.Collections.ArrayList();
            ////
            //for (int i = 0; i <= LocGen.Items.Count - 1; i++)
            //{
            //    var locstr10 = (LocItem)LocGen.Items[i];
            //    var locstr50 = locstr10.ID;
            //    var locstr20 = locstr10.Loc1;
            //    var locstr30 = locstr10.Loc2;
            //    var locstr40 = locstr10.Status;
            //    if (locstr40 == "ACTIVE")
            //    {
            //        Loc.Items.Add(new LocItem(locstr50, locstr20, locstr30, locstr40));
            //        ConvLoc.Items.Add(new ConvLocItem(i, count));
            //        count = count + 1;
            //    }
            //}
            ////
            //var locstr1 = (ConvLocItem)ConvLoc.Items[Int32.Parse(tab_no)];
            //var loc2 = locstr1.mainid;

            //var str = (LocItem)LocGen.Items[loc2];
            //str.Loc1 = Int32.Parse(loc);

            //using (var stringwriter = new System.IO.StringWriter())
            //{
            //    var serializer = new XmlSerializer(typeof(LocClass));
            //    serializer.Serialize(stringwriter, LocGen);
            //    var xmlstr = stringwriter.ToString();
            //    //blobから設定ファイルをダウンロードする。
            //    //storageAccountの作成（接続情報の定義）
            //    //アカウントネームやキー情報はAzureポータルから確認できる。
            //    var credential1 = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(IndexModel.accountName, IndexModel.accessKey);
            //    var storageAccount1 = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential1, true);
            //    //blob
            //    CloudBlobClient blobClient1 = storageAccount1.CreateCloudBlobClient();
            //    // Retrieve reference to a previously created container.
            //    CloudBlobContainer container1 = blobClient1.GetContainerReference(containerName_setting);

            //    //Create reference to a Blob that May or Maynot exist under the container
            //    CloudBlockBlob blockBlob = container1.GetBlockBlobReference("Loclist.xml");
            //    // Create or overwrite the "something.xml" blob with contents from a string
            //    await blockBlob.UploadTextAsync(xmlstr);
            //    await Task.Delay(500);
            //}
        }
        public static async void saveLoc2(string tab_no, string loc)
        {

            //var str = (LocItem)LocGen.Items[Int32.Parse(tab_no) - 1];
            //str.Loc2 = Int32.Parse(loc);
            //using (var stringwriter = new System.IO.StringWriter())
            //{
            //    var serializer = new XmlSerializer(typeof(LocClass));
            //    serializer.Serialize(stringwriter, LocGen);
            //    var xmlstr = stringwriter.ToString();
            //    //blobから設定ファイルをダウンロードする。
            //    //storageAccountの作成（接続情報の定義）
            //    //アカウントネームやキー情報はAzureポータルから確認できる。
            //    var credential1 = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(IndexModel.accountName, IndexModel.accessKey);
            //    var storageAccount1 = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential1, true);
            //    //blob
            //    CloudBlobClient blobClient1 = storageAccount1.CreateCloudBlobClient();
            //    // Retrieve reference to a previously created container.
            //    CloudBlobContainer container1 = blobClient1.GetContainerReference(containerName_setting);

            //    //Create reference to a Blob that May or Maynot exist under the container
            //    CloudBlockBlob blockBlob = container1.GetBlockBlobReference("Loclist.xml");
            //    // Create or overwrite the "something.xml" blob with contents from a string
            //    await blockBlob.UploadTextAsync(xmlstr);
            //    await Task.Delay(500);
            //}


        }

        public static async void saveTitle()
        {


            //using (var stringwriter = new System.IO.StringWriter())
            //{
            //    var serializer = new XmlSerializer(typeof(TitleClass));
            //    serializer.Serialize(stringwriter, result);
            //    var xmlstr = stringwriter.ToString();
            //    //blobから設定ファイルをダウンロードする。
            //    //storageAccountの作成（接続情報の定義）
            //    //アカウントネームやキー情報はAzureポータルから確認できる。
            //    var credential1 = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(IndexModel.accountName, IndexModel.accessKey);
            //    var storageAccount1 = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential1, true);
            //    //blob
            //    CloudBlobClient blobClient1 = storageAccount1.CreateCloudBlobClient();
            //    // Retrieve reference to a previously created container.
            //    CloudBlobContainer container1 = blobClient1.GetContainerReference(containerName_setting);

            //    //Create reference to a Blob that May or Maynot exist under the container
            //    CloudBlockBlob blockBlob = container1.GetBlockBlobReference("FILES.xml");
            //    // Create or overwrite the "something.xml" blob with contents from a string
            //    await blockBlob.UploadTextAsync(xmlstr);
            //    await Task.Delay(500);
            //}
            //using (var stringwriter = new System.IO.StringWriter())
            //{
            //    var serializer = new XmlSerializer(typeof(TitleClass));
            //    serializer.Serialize(stringwriter, result2);
            //    var xmlstr = stringwriter.ToString();
            //    //blobから設定ファイルをダウンロードする。
            //    //storageAccountの作成（接続情報の定義）
            //    //アカウントネームやキー情報はAzureポータルから確認できる。
            //    var credential1 = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(IndexModel.accountName, IndexModel.accessKey);
            //    var storageAccount1 = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential1, true);
            //    //blob
            //    CloudBlobClient blobClient1 = storageAccount1.CreateCloudBlobClient();
            //    // Retrieve reference to a previously created container.
            //    CloudBlobContainer container1 = blobClient1.GetContainerReference(containerName_setting);

            //    //Create reference to a Blob that May or Maynot exist under the container
            //    CloudBlockBlob blockBlob = container1.GetBlockBlobReference("S_FILES.xml");
            //    // Create or overwrite the "something.xml" blob with contents from a string
            //    await blockBlob.UploadTextAsync(xmlstr);
            //    await Task.Delay(500);
            //}
        }

        public static async void saveAllLoc()
        {
            //using (var stringwriter = new System.IO.StringWriter())
            //{
            //    var serializer = new XmlSerializer(typeof(LocClass));
            //    serializer.Serialize(stringwriter, LocGen);
            //    var xmlstr = stringwriter.ToString();
            //    //blobから設定ファイルをダウンロードする。
            //    //storageAccountの作成（接続情報の定義）
            //    //アカウントネームやキー情報はAzureポータルから確認できる。
            //    var credential1 = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(IndexModel.accountName, IndexModel.accessKey);
            //    var storageAccount1 = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential1, true);
            //    //blob
            //    CloudBlobClient blobClient1 = storageAccount1.CreateCloudBlobClient();
            //    // Retrieve reference to a previously created container.
            //    CloudBlobContainer container1 = blobClient1.GetContainerReference(containerName_setting);

            //    //Create reference to a Blob that May or Maynot exist under the container
            //    CloudBlockBlob blockBlob = container1.GetBlockBlobReference("Loclist.xml");
            //    // Create or overwrite the "something.xml" blob with contents from a string
            //    await blockBlob.UploadTextAsync(xmlstr);
            //    await Task.Delay(500);
            //}
        }
        public static string getlink(string folder, string attachfile)
        {
            var credential = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
            var storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential, true);
            //blob
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(containerName_folder);

            var accessKey2 = "DefaultEndpointsProtocol=https;AccountName=lifenote3;AccountKey=09mFQDrMj5fYMyEcoZoINUoHcZUJzY4T8ZYLOQ0pINNKa9wuj4Yzj4RhWI9z9ek8u4ynL2x9ONbg+AStbmQeOQ==;EndpointSuffix=core.windows.net";
            var blobServiceClient = new BlobServiceClient(accessKey2);

            //  Gets a reference to the container.
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName_folder);

            //  Gets a reference to the blob in the container
            BlobClient blobClient1 = blobContainerClient.GetBlobClient(folder + "/" + attachfile);

            //  Defines the resource being accessed and for how long the access is allowed.
            var blobSasBuilder = new BlobSasBuilder
            {
                ExpiresOn = DateTime.UtcNow.AddMinutes(60000),
                BlobContainerName = containerName_folder,
                BlobName = folder + "/" + attachfile,
            };

            //  Defines the type of permission.
            blobSasBuilder.SetPermissions(BlobSasPermissions.Read);

            //  Builds an instance of StorageSharedKeyCredential      
            var storageSharedKeyCredential = new StorageSharedKeyCredential(accountName, accessKey);

            //  Builds the Sas URI.
            BlobSasQueryParameters sasQueryParameters = blobSasBuilder.ToSasQueryParameters(storageSharedKeyCredential);
            var sasUrl = blobClient1.Uri.AbsoluteUri + "?" + sasQueryParameters;
            var gdoc = "";
            if (attachfile != null)
            {
                var attachsub = attachfile.Split(".");
                var attachext = attachsub[1];
                bool af = false;
                bool af2 = false;

                foreach (var element in extensions)
                {
                    if (element == attachext)
                    {
                        af = true;
                        break;
                    }
                }
                foreach (var element in extensions2)
                {
                    if (element == attachext)
                    {
                        af2 = true;
                        break;
                    }
                }
                if (af == true)
                {
                    gdoc = "https://view.officeapps.live.com/op/view.aspx?src=" + sasUrl;
                }
                else if (af2 == true)
                {
                    gdoc = "https://docs.google.com/viewer?url=" + sasUrl;
                }
                else
                {
                    gdoc = sasUrl;

                }
            }

            //var gdoc = "https://view.officeapps.live.com/op/view.aspx?src="  + sasUrl;
            //var gdoc = "https://docs.google.com/viewer?url=" + sasUrl;
            return gdoc;
        }
       
        public static async Task GetNumbers(string accountName,string accessKey)
        {
            
            getf = true;
            var credential = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
            var storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential, true);
            //blob
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("settingdata");
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
            await Task.Delay(500);
        }
        public static async Task GetColors(String accountName, String accessKey)
        {
            
            getf = true;
            var credential = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
            var storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential, true);
            //blob
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("settingdata");
            //HTML生成用序数詞リスト読み込み
            // Retrieve reference to a blob named "filename"
            //タブ色のリスト読み込み
            //HTML生成用序数詞リスト読み込み
            // Retrieve reference to a blob named "filename"
            CloudBlockBlob blockBlob8 = container.GetBlockBlobReference("hitcolour.csv");

            using (var memoryStream8 = new MemoryStream())
            {
                string hitcolourgen = "";
                await blockBlob8.DownloadToStreamAsync(memoryStream8);
                hitcolourgen = System.Text.Encoding.UTF8.GetString(memoryStream8.ToArray());
                Hitcolour = hitcolourgen.Split(",");

            }
            await Task.Delay(500);
        }
        //public static async Task GetFiles(string containerName_setting, string containerName_file,String accountName, String accessKey)
        //{

        //    getf = true;
        //    var credential = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
        //    var storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential, true);
        //    //blob
        //    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();


        //    //HTML生成用序数詞リスト読み込み
        //    // Retrieve reference to a blob named "filename"
        //    // Retrieve reference to a previously created container.
        //    CloudBlobContainer container_setting = blobClient.GetContainerReference(containerName_setting);
        //    CloudBlobContainer container2 = blobClient.GetContainerReference(containerName_file);

        //    //DB本体のテキストファイル読み込み
        //    // Retrieve reference to a blob named "filename"
        //    CloudBlockBlob blockBlob3 = container_setting.GetBlockBlobReference("FILES.xml");

        //    string files3;
        //    using (var memoryStream3 = new MemoryStream())
        //    {
        //        await blockBlob3.DownloadToStreamAsync(memoryStream3);
        //        files3 = System.Text.Encoding.UTF8.GetString(memoryStream3.ToArray());
        //    }

        //    // convert string to stream
        //    byte[] byteArray3 = Encoding.UTF8.GetBytes(files3);
        //    //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
        //    MemoryStream stream3 = new MemoryStream(byteArray3);
        //    // convert stream to string
        //    StreamReader reader3 = new StreamReader(stream3);
        //    XmlSerializer serializer = new XmlSerializer(typeof(TitleClass));
        //    result = null;
        //    result = (TitleClass)serializer.Deserialize(reader3);
        //    reader3.Close();
        //    await Task.Delay(500);

        //    // Retrieve reference to a previously created container.
        //    //CloudBlobContainer container_setting = blobClient.GetContainerReference(containerName_setting);

        //    //色の指定
        //    IndexModel.TabColours.Clear();


        //    for (int i = 0; i <= result.Items.Count - 1; i++)
        //    {
        //        var titlestr1 = (TitleItem)result.Items[i];
        //        var titlestr3 = titlestr1.Status;
        //        var titlestr4 = titlestr1.Colour;

        //        if (titlestr3 == "ACTIVE")
        //        {
        //            TabColours.Add(titlestr4);
        //        }
        //    }


        //}
        //public static async Task GetSFiles(string containerName_setting,String accountName, String accessKey)
        //{

        //    getf = true;
        //    var credential = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
        //    var storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential, true);
        //    //blob
        //    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();


        //    //HTML生成用序数詞リスト読み込み
        //    // Retrieve reference to a blob named "filename"
        //    // Retrieve reference to a previously created container.
        //    CloudBlobContainer container_setting = blobClient.GetContainerReference(containerName_setting);

        //    //DB本体のテキストファイル読み込み：検索後
        //    // Retrieve reference to a blob named "filename"
        //    CloudBlockBlob blockBlob7 = container_setting.GetBlockBlobReference("S_FILES.xml");

        //    string files7;
        //    using (var memoryStream7 = new MemoryStream())
        //    {
        //        await blockBlob7.DownloadToStreamAsync(memoryStream7);
        //        files7 = System.Text.Encoding.UTF8.GetString(memoryStream7.ToArray());
        //    }

        //    // convert string to stream
        //    byte[] byteArray7 = Encoding.UTF8.GetBytes(files7);
        //    //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
        //    MemoryStream stream7 = new MemoryStream(byteArray7);
        //    // convert stream to string
        //    StreamReader reader7 = new StreamReader(stream7);
        //    XmlSerializer serializer7 = new XmlSerializer(typeof(TitleClass));
        //    result2 = null;
        //    result2 = (TitleClass)serializer7.Deserialize(reader7);
        //    reader7.Close();

        //    await Task.Delay(500);
        //}
        //public static async Task GetLocList(string containerName_setting)
        //{
        //    //accountName = _configuration.GetValue<string>("accountName");
        //    //accessKey = _configuration.GetValue<string>("accessKey");
        //    getf = true;
        //    var credential = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
        //    var storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential, true);
        //    //blob
        //    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        //    CloudBlobContainer container_setting = blobClient.GetContainerReference(containerName_setting);
        //    //DB本体のテキストファイル読み込み：検索後
        //    // Retrieve reference to a blob named "filename"
        //    CloudBlockBlob blockBlob9 = container_setting.GetBlockBlobReference("Loclist.xml");

        //    string files9;

        //    using (var memoryStream9 = new MemoryStream())
        //    {
        //        await blockBlob9.DownloadToStreamAsync(memoryStream9);
        //        files9 = System.Text.Encoding.UTF8.GetString(memoryStream9.ToArray());
        //    }

        //    // convert string to stream
        //    byte[] byteArray9 = Encoding.UTF8.GetBytes(files9);
        //    //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
        //    MemoryStream stream9 = new MemoryStream(byteArray9);
        //    // convert stream to string
        //    StreamReader reader9 = new StreamReader(stream9);
        //    XmlSerializer serializer9 = new XmlSerializer(typeof(LocClass));
        //    LocGen = null;
        //    LocGen = (LocClass)serializer9.Deserialize(reader9);
        //    reader9.Close();
        //    await Task.Delay(500);
        //}
        //public static  async Task GetAllDoc(string containerName_setting)
        //{
        //    //accountName = _configuration.GetValue<string>("accountName");
        //    //accessKey = _configuration.GetValue<string>("accessKey");
        //    getf = true;
        //    var credential = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
        //    var storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential, true);
        //    //blob
        //    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

        //    //HTML生成用序数詞リスト読み込み
        //    // Retrieve reference to a blob named "filename"
        //    // Retrieve reference to a previously created container.
        //    //CloudBlobContainer container2 = blobClient.GetContainerReference(containerName_file);

        //    CloudBlobContainer container2 = blobClient.GetContainerReference(containerName_file);
        //    Alldoc = new List<List<Documentdata>>();
        //    //読み込んだテキストのリストへの展開：元ファイル
        //    for (int i = 0; i <= result.Items.Count - 1; i++)
        //    {
        //        var titlestr1 = (TitleItem)result.Items[i];
        //        var titlestr2 = titlestr1.Title;
        //        var titlestr3 = titlestr1.Status;
        //        if (titlestr3 == "ACTIVE")
        //        {
        //            CloudBlockBlob blockBlob4 = container2.GetBlockBlobReference(titlestr2);
        //            using (var memoryStream4 = new MemoryStream())
        //            {
        //                string data = "";
        //                await blockBlob4.DownloadToStreamAsync(memoryStream4);
        //                data = System.Text.Encoding.UTF8.GetString(memoryStream4.ToArray());
        //                string[] doc = data.Replace("<spert2>", ";").Split(';');
        //                var counter = 0;
        //                string id = "";
        //                string title = "";
        //                string body = "";
        //                string htmlbody = "";
        //                string upddate = "";
        //                string updauther = "";
        //                string attchfiles = "";
        //                string scroolloc = "";

        //                Documents = new List<Documentdata>();
        //                foreach (string value in doc)
        //                {

        //                    if (counter % 8 == 0)
        //                    {
        //                        id = doc[counter].Replace("<seprt1>", ";");
        //                    }
        //                    if (counter % 8 == 1)
        //                    {
        //                        title = doc[counter].Replace("<seprt1>", ";");
        //                    }
        //                    if (counter % 8 == 2)
        //                    {
        //                        body = doc[counter].Replace("<seprt1>", ";");
        //                    }
        //                    if (counter % 8 == 3)
        //                    {
        //                        htmlbody = doc[counter].Replace("<seprt1>", ";");
        //                    }
        //                    if (counter % 8 == 4)
        //                    {
        //                        upddate = doc[counter].Replace("<seprt1>", ";");
        //                    }
        //                    if (counter % 8 == 5)
        //                    {
        //                        updauther = doc[counter].Replace("<seprt1>", ";");
        //                    }
        //                    if (counter % 8 == 6)
        //                    {
        //                        attchfiles = doc[counter].Replace("<seprt1>", ";");
        //                    }
        //                    if (counter % 8 == 7)
        //                    {
        //                        scroolloc = doc[counter].Replace("<seprt1>", ";");
        //                        Document = new Documentdata();
        //                        Document.Id = id;
        //                        Document.Title = title;
        //                        Document.Body = body;
        //                        Document.Htmlbody = htmlbody;
        //                        Document.Upddate = upddate;
        //                        Document.Updauther = updauther;
        //                        Document.Attchfiles = attchfiles;
        //                        Document.Scroolloc = scroolloc;
        //                        Documents.Add(Document);
        //                    }
        //                    counter += 1;
        //                }
        //                Alldoc.Add(Documents);
        //            }
        //        }
        //    }

        //    await Task.Delay(500);
        //}
        //public static  async Task GetsrcAllDoc(string containerName_setting)
        //{
        //    //accountName = _configuration.GetValue<string>("accountName");
        //    //accessKey = _configuration.GetValue<string>("accessKey");
        //    getf = true;
        //    var credential = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
        //    var storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential, true);
        //    //blob
        //    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        //    CloudBlobContainer container2 = blobClient.GetContainerReference(containerName_file);
        //    //読み込んだテキストのリストへの展開：元ファイル
        //    srcAlldoc = new List<List<srcDocumentdata>>();
        //    for (int i = 0; i <= result2.Items.Count - 1; i++)
        //    {
        //        var titlestr1 = (TitleItem)result2.Items[i];
        //        var titlestr2 = titlestr1.Title;
        //        var titlestr3 = titlestr1.Status;
        //        if (titlestr3 == "ACTIVE")
        //        {
        //            CloudBlockBlob blockBlob4 = container2.GetBlockBlobReference(titlestr2);
        //            using (var memoryStream4 = new MemoryStream())
        //            {
        //                string data = "";
        //                await blockBlob4.DownloadToStreamAsync(memoryStream4);
        //                data = System.Text.Encoding.UTF8.GetString(memoryStream4.ToArray());
        //                string[] doc = data.Replace("<spert2>", ";").Split(';');
        //                var counter = 0;
        //                string id = "";
        //                string title = "";
        //                string body = "";
        //                string htmlbody = "";
        //                string upddate = "";
        //                string updauther = "";
        //                string attchfiles = "";
        //                string scroolloc = "";
        //                string loc = "";
        //                srcDocuments = new List<srcDocumentdata>();
        //                foreach (string value in doc)
        //                {
        //                    if (counter % 9 == 0)
        //                    {
        //                        id = doc[counter].Replace("<seprt1>", ";");
        //                    }
        //                    if (counter % 9 == 1)
        //                    {
        //                        title = doc[counter].Replace("<seprt1>", ";");
        //                    }
        //                    if (counter % 9 == 2)
        //                    {
        //                        body = doc[counter].Replace("<seprt1>", ";");
        //                    }
        //                    if (counter % 9 == 3)
        //                    {
        //                        htmlbody = doc[counter].Replace("<seprt1>", ";");
        //                    }
        //                    if (counter % 9 == 4)
        //                    {
        //                        upddate = doc[counter].Replace("<seprt1>", ";");
        //                    }
        //                    if (counter % 9 == 5)
        //                    {
        //                        updauther = doc[counter].Replace("<seprt1>", ";");
        //                    }
        //                    if (counter % 9 == 6)
        //                    {
        //                        attchfiles = doc[counter].Replace("<seprt1>", ";");
        //                    }
        //                    if (counter % 9 == 7)
        //                    {
        //                        scroolloc = doc[counter].Replace("<seprt1>", ";");
        //                    }
        //                    if (counter % 9 == 8)
        //                    {
        //                        loc = doc[counter].Replace("<seprt1>", ";");
        //                        srcDocument = new srcDocumentdata();
        //                        srcDocument.Id = id;
        //                        srcDocument.Title = title;
        //                        srcDocument.Body = body;
        //                        srcDocument.Htmlbody = htmlbody;
        //                        srcDocument.Upddate = upddate;
        //                        srcDocument.Updauther = updauther;
        //                        srcDocument.Attchfiles = attchfiles;
        //                        srcDocument.Scroolloc = scroolloc;
        //                        srcDocument.Loc = loc;
        //                        srcDocuments.Add(srcDocument);
        //                    }
        //                    counter += 1;
        //                }
        //                srcAlldoc.Add(srcDocuments);
        //            }
        //        }
        //        //IndexModel.datasize = counter - 1;
        //        var titlestr_12 = (TitleItem)result2.Items[activetab];
        //        SelectedTabName = titlestr_12.Title;
        //        SelectedTabName = SelectedTabName.Replace(".csv", "");
        //    }


        //    await Task.Delay(500);
        //}
    }
}