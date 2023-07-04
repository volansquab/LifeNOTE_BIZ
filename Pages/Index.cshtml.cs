using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Azure.Identity;
using Microsoft.Graph;
using System.ComponentModel;
using LifeNOTE_BIZ.Pages.Services;

namespace LifeNOTE_BIZ.Pages
{
    //[Authorize]
    [DisableRequestSizeLimit]
    [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
    public class IndexModel : PageModel
    {
        private IWebHostEnvironment _environment;
        public IndexModel(IWebHostEnvironment environment, ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _environment = environment;
            _logger = logger;
            _configuration = configuration;
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
        public static List<string> titlenamelist = new List<string>();
        public static List<string> inacttitlenamelist = new List<string>();
        public static List<string> TabColoursGen = new List<string>();
        public static List<string> TabColours = new List<string>();
        public static string screenmode = "normal";
        public static int datasize;
        public static int selecttab = 1;
        public static string? bodyelement;
        public static string? bodyelement2;
        public static string? list;
        public static string[]? Numbers;

        public static string[]? keywords;
        public static string[]? Hitcolour;
        public static string? activetabname;
        public static string? activetabname2;
        public static string[]? Attachfiles;
        public static int[] adddocmax;
        public static int[] addtabmax;
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
        public static string updatedatee;
        public static string authorr;
        public static int count2;


        public async Task OnGetAsync()
        {

            bool isNewUser = User.Claims.FirstOrDefault(x => x.Type == "newUser") == null ? false : true;
            string userOId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            string mailadress = "";

            List<string> emails = new List<string>();
            IEnumerable<Claim> emailClaims = User.Claims.Where(c => c.Type == ClaimTypes.Email);

            if (emailClaims.Any())
            {
                // get the roles' actual value
                foreach (Claim claim in emailClaims)
                {
                    mailadress = claim.Value;
                }
            }

            DateTime timeUtc = DateTime.UtcNow;
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
            username = User.Identity?.Name;
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
            //最初のファイル作成
            accountName = _configuration.GetValue<string>("accountName");
            accessKey = _configuration.GetValue<string>("accessKey");
            //Initialize Service
            Init.InitService(isNewUser, accountName, accessKey, containerName_setting, diststr_loclist, containerName_file, containerName_folder, diststr_new, diststr_new_s, diststr_new_m, diststr_new_m_s, diststr_files, diststr_files_s);
            saveAllUser(userOId, username, mailadress, enrolleddate);
            await Task.Delay(500);

            
            getf = true;
            var credential = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
            var storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential, true);
            //blob
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("settingdata");
            //HTML生成用序数詞リスト読み込み
            tss1 = GetNumbers(accountName,accessKey);
            tss2 = GetColors(accountName, accessKey);
            tss3 = GetFiles(containerName_setting, containerName_file, accountName, accessKey);
            tss4 = GetSFiles(containerName_setting, accountName, accessKey);
            tss5 = GetLocList(containerName_setting);
            await Task.WhenAll(tss1, tss2, tss3, tss4, tss5);
            await Task.Delay(500);


            CloudBlobContainer container_setting = blobClient.GetContainerReference(containerName_setting);

            //本体ファイルのクリア
            IndexModel.Alldoc.Clear();
            IndexModel.srcAlldoc.Clear();
            IndexModel.titlenamelist.Clear();
            IndexModel.inacttitlenamelist.Clear();
            //タイトルの取得
            Init.Settab(result, titlenamelist,inacttitlenamelist);
            //XMLシリアル化するオブジェクト
            count2 = 0;
            LocClass Loc = new LocClass();
            Loc.Items = new System.Collections.ArrayList();
            ConvLoc ConvLoc = new ConvLoc();
            ConvLoc.Items = new System.Collections.ArrayList();
            Init.CreateConv(Loc, ConvLoc);
            await Task.Delay(500);
            //★
            var locstr15 = (ConvLocItem)ConvLoc.Items[IndexModel.selecttab - 1];
            activetab = locstr15.mainid;

            //Init.StateDivider(Loc, ConvLoc);

            if (IndexModel.screenmode == "searched")
            {
                IndexModel.tss7 = IndexModel.GetAllDoc(IndexModel.containerName_setting);
                await Task.WhenAll(IndexModel.tss7);
                //元ファイルタブ表示の切り替え
                if (IndexModel.selecttab == 0)
                {
                    IndexModel.selecttab = 1;
                }
                //
                var locstr1 = (LocItem)IndexModel.Loc.Items[IndexModel.selecttab - 1];
                var locstr = locstr1.Loc1;
                //★
                IndexModel.title = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr].Title;
                IndexModel.bodyelement = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr].Htmlbody;
                IndexModel.updatedate = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr].Upddate;
                IndexModel.author = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr].Updauther;
                IndexModel.attachfile = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr].Attchfiles;
                IndexModel.scrollloc = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr].Scroolloc;
                var locstr_1 = (ConvLocItem)ConvLoc.Items[IndexModel.selecttab - 1];
                var loc2 = locstr_1.mainid;
                //var locstr_1 = (LocItem)Loc.Items[IndexModel.selecttab - 1];
                //var tabno = locstr_1.ID;
                var titlestr_1 = (TitleItem)IndexModel.result.Items[loc2];
                IndexModel.SelectedTabName = titlestr_1.Title;
                IndexModel.SelectedTabName = IndexModel.SelectedTabName.Replace(".csv", "");

                IndexModel.tss6 = IndexModel.GetsrcAllDoc(IndexModel.containerName_setting);
                await Task.WhenAll(IndexModel.tss6);

                //
                //元ファイルタブ表示の切り替え
                if (IndexModel.selecttab == 0)
                {
                    IndexModel.selecttab = 1;
                }
                var locstr12 = (LocItem)IndexModel.Loc.Items[IndexModel.selecttab - 1];
                locstr = locstr12.Loc2;

                if (IndexModel.srcAlldoc[IndexModel.selecttab - 1].Count != 0)
                {
                    IndexModel.title = IndexModel.srcAlldoc[IndexModel.selecttab - 1][locstr].Title;
                    IndexModel.bodyelement2 = IndexModel.srcAlldoc[IndexModel.selecttab - 1][locstr].Htmlbody;
                    IndexModel.bodyelement = IndexModel.srcAlldoc[IndexModel.selecttab - 1][locstr].Htmlbody;
                    IndexModel.updatedate = IndexModel.srcAlldoc[IndexModel.selecttab - 1][locstr].Upddate;
                    IndexModel.author = IndexModel.srcAlldoc[IndexModel.selecttab - 1][locstr].Updauther;
                    IndexModel.attachfile = IndexModel.srcAlldoc[IndexModel.selecttab - 1][locstr].Attchfiles;
                    IndexModel.scrollloc = IndexModel.srcAlldoc[IndexModel.selecttab - 1][locstr].Scroolloc;
                }
                else
                {
                    IndexModel.bodyelement = "";
                }
                IndexModel.datasize = IndexModel.titlenamelist.Count;
            }
            else
            {

                IndexModel.tss6 = IndexModel.GetAllDoc(IndexModel.containerName_setting);
                await Task.WhenAll(IndexModel.tss6);

                await Task.Delay(500);
                //元ファイルタブ表示の切り替え
                if (IndexModel.selecttab == 0)
                {
                    IndexModel.selecttab = 1;
                }
                //
                var locstr1 = (LocItem)Loc.Items[IndexModel.selecttab - 1];
                var locstr = locstr1.Loc1;
                //★2
                IndexModel.title = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr].Title;
                IndexModel.bodyelement = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr].Htmlbody;
                IndexModel.updatedate = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr].Upddate;
                IndexModel.author = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr].Updauther;
                IndexModel.attachfile = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr].Attchfiles;
                IndexModel.scrollloc = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr].Scroolloc;
                var locstr_1 = (ConvLocItem)ConvLoc.Items[IndexModel.selecttab - 1];
                var loc = locstr_1.mainid;
                //var locstr_1 = (LocItem)Loc.Items[IndexModel.selecttab - 1];
                //var tabno = locstr_1.ID;
                var titlestr_1 = (TitleItem)IndexModel.result.Items[loc];
                IndexModel.SelectedTabName = titlestr_1.Title;
                IndexModel.SelectedTabName = IndexModel.SelectedTabName.Replace(".csv", "");
            }
            if (IndexModel.selecttab == IndexModel.datasize)
            {
                IndexModel.activetabname = "tab3_" + IndexModel.selecttab;
                IndexModel.activetabname2 = "tab3_" + (IndexModel.selecttab - 1).ToString();
            }
            else
            {
                IndexModel.activetabname = "tab3_" + IndexModel.selecttab;
                IndexModel.activetabname2 = "tab3_" + IndexModel.selecttab + 1;
            }


            var height = 70 + 6 * (int)(datasize / 10);
            textheight = "height: " + height + "vh;";

        }

        public async Task<JsonResult> OnPostAsync()
        {

            string userOId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            containerName_file = userOId + "-filedata";
            containerName_folder = userOId + "-folder";
            containerName_setting = userOId + "-setting";
            evnt = Request.Form["evnt"];
            textdata = Request.Form["textbody"];
            htmldata = Request.Form["htmlbody"];
            //attachfile = Request.Form["attachname"];
            username = User.Identity?.Name;
            IndexModel.errormessage = "";


            //XMLシリアル化するオブジェクト
            var count = 0;
            LocClass Loc = new LocClass();
            Loc.Items = new System.Collections.ArrayList();
            ConvLoc ConvLoc = new ConvLoc();
            ConvLoc.Items = new System.Collections.ArrayList();

            for (int i = 0; i <= LocGen.Items.Count - 1; i++)
            {
                var locstr10 = (LocItem)LocGen.Items[i];
                var locstr50 = locstr10.ID;
                var locstr20 = locstr10.Loc1;
                var locstr30 = locstr10.Loc2;
                var locstr40 = locstr10.Status;
                if (locstr40 == "ACTIVE")
                {
                    Loc.Items.Add(new LocItem(locstr50, locstr20, locstr30, locstr40));
                    ConvLoc.Items.Add(new ConvLocItem(i, count));
                    count = count + 1;
                }
            }
            IndexModel.datasize = count;
            //abstruct normal
            locstr_l = (LocItem)Loc.Items[IndexModel.selecttab - 1];
            normal_row = locstr_l.Loc1;
            searched_row = locstr_l.Loc2;

            locstr_c = (ConvLocItem)ConvLoc.Items[IndexModel.selecttab - 1];
            all_loc_c = locstr_c.mainid;


            DateTime timeUtc = DateTime.UtcNow;
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);

            switch (evnt)
            {
                case "listclick":
                    indxid = Request.Form["indx"];
                    title = IndexModel.Alldoc[IndexModel.selecttab - 1][Int32.Parse(indxid)].Title;
                    bodyelement = IndexModel.Alldoc[IndexModel.selecttab - 1][Int32.Parse(indxid)].Htmlbody;
                    updatedate = IndexModel.Alldoc[IndexModel.selecttab - 1][Int32.Parse(indxid)].Upddate;
                    author = IndexModel.Alldoc[IndexModel.selecttab - 1][Int32.Parse(indxid)].Updauther;
                    attachfile = IndexModel.Alldoc[IndexModel.selecttab - 1][Int32.Parse(indxid)].Attchfiles;
                    scrollloc = IndexModel.Alldoc[IndexModel.selecttab - 1][Int32.Parse(indxid)].Scroolloc;
                    //var locstr1 = (LocItem)Loc.Items[IndexModel.selecttab-1];
                    //var tabno = locstr1.ID;
                    saveLoc((IndexModel.selecttab - 1).ToString(), indxid);
                    await Task.Delay(500);
                    IndexModel.screenmode = "normal";
                    break;
                case "listclick2":
                    indxid = Request.Form["indx"];
                    title = IndexModel.srcAlldoc[IndexModel.selecttab - 1][Int32.Parse(indxid)].Title;
                    bodyelement = IndexModel.srcAlldoc[IndexModel.selecttab - 1][Int32.Parse(indxid)].Htmlbody;
                    updatedate = IndexModel.srcAlldoc[IndexModel.selecttab - 1][Int32.Parse(indxid)].Upddate;
                    author = IndexModel.srcAlldoc[IndexModel.selecttab - 1][Int32.Parse(indxid)].Updauther;
                    attachfile = IndexModel.srcAlldoc[IndexModel.selecttab - 1][Int32.Parse(indxid)].Attchfiles;
                    scrollloc = IndexModel.Alldoc[IndexModel.selecttab - 1][Int32.Parse(indxid)].Scroolloc;
                    //var locstr1 = (LocItem)Loc.Items[IndexModel.selecttab-1];
                    //var tabno = locstr1.ID;
                    saveLoc2((IndexModel.selecttab - 1).ToString(), indxid);
                    await Task.Delay(500);
                    IndexModel.screenmode = "searched";
                    break;
                case "save":
                    String scrollTop = Request.Form["scrollTop"];
                    
                    if (IndexModel.screenmode == "normal" || IndexModel.screenmode == "exdoc1")
                    {
                        //locstr1 = (LocItem)Loc.Items[IndexModel.selecttab - 1];
                        //var locstr2 = locstr1.Loc1;
                        IndexModel.Alldoc[IndexModel.selecttab - 1][IndexModel.locstr2].Title = title;
                        IndexModel.Alldoc[IndexModel.selecttab - 1][IndexModel.locstr2].Body = textdata;
                        IndexModel.Alldoc[IndexModel.selecttab - 1][IndexModel.locstr2].Htmlbody = htmldata;
                        IndexModel.Alldoc[IndexModel.selecttab - 1][IndexModel.locstr2].Scroolloc = scrollTop;
                        IndexModel.Alldoc[IndexModel.selecttab - 1][IndexModel.locstr2].Upddate = cstTime.ToString("yyyy/MM/dd HH:mm:ss");
                        IndexModel.Alldoc[IndexModel.selecttab - 1][IndexModel.locstr2].Updauther = username;
                    }
                    if (IndexModel.screenmode == "searched" || IndexModel.screenmode == "exdoc2")
                    {
                        
                        rerativeloc = srcAlldoc[selecttab - 1][IndexModel.locstr2].Loc;
                        for (int i = 0; i <= IndexModel.Alldoc[IndexModel.selecttab - 1].Count - 1; i++)
                        {
                            if (IndexModel.Alldoc[selecttab - 1][i].Id == rerativeloc)
                            {
                                IndexModel.Alldoc[selecttab - 1][i].Title = title;
                                IndexModel.Alldoc[selecttab - 1][i].Body = textdata;
                                IndexModel.Alldoc[selecttab - 1][i].Htmlbody = htmldata;
                                IndexModel.Alldoc[selecttab - 1][i].Scroolloc = scrollTop;
                                IndexModel.Alldoc[selecttab - 1][i].Upddate = cstTime.ToString("yyyy/MM/dd HH:mm:ss");
                                IndexModel.Alldoc[selecttab - 1][i].Updauther = username;
                                break;
                            }
                        }
                        IndexModel.srcAlldoc[IndexModel.selecttab - 1][IndexModel.locstr2].Title = title;
                        IndexModel.srcAlldoc[IndexModel.selecttab - 1][IndexModel.locstr2].Body = textdata;
                        IndexModel.srcAlldoc[IndexModel.selecttab - 1][IndexModel.locstr2].Htmlbody = htmldata;
                        IndexModel.srcAlldoc[IndexModel.selecttab - 1][IndexModel.locstr2].Scroolloc = scrollTop;
                        IndexModel.srcAlldoc[IndexModel.selecttab - 1][IndexModel.locstr2].Upddate = cstTime.ToString("yyyy/MM/dd HH:mm:ss");
                        IndexModel.srcAlldoc[IndexModel.selecttab - 1][IndexModel.locstr2].Updauther = username;
                    }
                    saveDoc();
                    saveDoc2();
                    break;
                case "partialnew":
                    title2 = Request.Form["title"];
                    if (Chkdublication(title2) == true)
                    {
                        IndexModel.screenmode = "newdoc";
                        IndexModel.errormessage = "既存のタイトルと重複したタイトルには変更できません。";
                    }
                    else
                    {
                        

                        ////最終列の検出
                        int[] adddocmax = new int[Alldoc[IndexModel.selecttab - 1].Count];
                        for (int i = 0; i <= Alldoc[IndexModel.selecttab - 1].Count - 1; i++)
                        {
                            var id = Alldoc[IndexModel.selecttab - 1][i].Id;
                            adddocmax[i] = Int32.Parse(id);
                        }
                        var maxdoc = adddocmax.Max() + 1;
                        

                        IndexModel.Alldoc[IndexModel.selecttab - 1].Insert(Int32.Parse(IndexModel.locstr2.ToString()) + 1, new Documentdata()

                        {
                            Id = (maxdoc).ToString(),
                            Title = title2,
                            Body = textdata,
                            Htmlbody = htmldata,
                            Upddate = cstTime.ToString("yyyy/MM/dd HH:mm:ss"),
                            Updauther = username,
                            Attchfiles = "",
                            Scroolloc = "0.0",
                        }); ;
                        
                        //respf = true;
                        saveDoc();
                        saveAllLoc();
                        await Task.Delay(500);
                        IndexModel.screenmode = "normal";
                    }
                    break;
                case "tytlechanged":
                    title2 = "";
                    title2 = Request.Form["title"];
                    //IndexModel.errormessage = "既存のタイトルと重複したタイトルには変更できません。";
                    if (Chkdublication(title2) == true)
                    {
                        IndexModel.screenmode = "normal";
                        IndexModel.errormessage = "既存のタイトル名と重複したタイトル名には変更できません。";
                    }
                    else
                    {
                        if (IndexModel.screenmode == "normal" || IndexModel.screenmode == "exdoc1")
                        {

                            IndexModel.Alldoc[IndexModel.selecttab - 1][Int32.Parse(normal_row.ToString())].Title = title2;
                            IndexModel.Alldoc[IndexModel.selecttab - 1][Int32.Parse(normal_row.ToString())].Upddate = cstTime.ToString("yyyy/MM/dd HH:mm:ss");
                            IndexModel.Alldoc[IndexModel.selecttab - 1][Int32.Parse(normal_row.ToString())].Updauther = username;
                            IndexModel.screenmode = "normal";

                            saveDoc();
                        }
                        else if (IndexModel.screenmode == "searched" || IndexModel.screenmode == "exdoc2")
                        {

                            rerativeloc = srcAlldoc[selecttab - 1][searched_row].Loc;

                            for (int i = 0; i <= IndexModel.Alldoc[IndexModel.selecttab - 1].Count - 1; i++)
                            {
                                if (IndexModel.Alldoc[selecttab - 1][i].Id == rerativeloc)
                                {
                                    IndexModel.Alldoc[selecttab - 1][i].Title = title2;
                                    IndexModel.Alldoc[selecttab - 1][i].Upddate = cstTime.ToString("yyyy/MM/dd HH:mm:ss");
                                    IndexModel.Alldoc[selecttab - 1][i].Updauther = username;
                                    break;
                                }
                            }
                            IndexModel.srcAlldoc[IndexModel.selecttab - 1][searched_row].Title = title2;
                            IndexModel.srcAlldoc[IndexModel.selecttab - 1][searched_row].Upddate = cstTime.ToString("yyyy/MM/dd HH:mm:ss");
                            IndexModel.srcAlldoc[IndexModel.selecttab - 1][searched_row].Upddate = cstTime.ToString("yyyy/MM/dd HH:mm:ss");
                            IndexModel.srcAlldoc[IndexModel.selecttab - 1][searched_row].Updauther = username;

                            IndexModel.screenmode = "searched";
                            saveDoc();
                            saveDoc2();
                        }

                        await Task.Delay(500);

                    }

                    break;
                case "tabnamechanged":
                    title2 = Request.Form["tabname"];
                    //IndexModel.errormessage = "既存のタイトルと重複したタイトルには変更できません。";
                    if (Chkdublication2(title2) == true)
                    {
                        IndexModel.screenmode = "normal";
                        IndexModel.errormessage = "既存のタブ名と重複したタブ名には変更できません。";
                    }
                    else
                    {
                        if (IndexModel.screenmode == "normal" || IndexModel.screenmode == "exdoc")
                        {

                            //all to normal
                            var titlestr1 = (TitleItem)result.Items[all_loc_c];
                            //all to searched
                            var titlestr2 = (TitleItem)result2.Items[all_loc_c];
                            var beforetitle1 = titlestr1.Title;
                            var beforetitle2 = titlestr2.Title;
                            var aftertitle1 = title2 + ".csv";
                            var aftertitle2 = title2 + "-S.csv";
                            titlestr1.Title = aftertitle1;
                            titlestr2.Title = aftertitle2;


                            var credential5 = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
                            var storageAccount5 = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential5, true);
                            ////blob
                            CloudBlobClient blobClient5 = storageAccount5.CreateCloudBlobClient();

                            ////// Retrieve reference to a previously created container.
                            CloudBlobContainer container5 = blobClient5.GetContainerReference(containerName_file);

                            CloudBlockBlob source1 = container5.GetBlockBlobReference(beforetitle1);
                            CloudBlockBlob target1 = container5.GetBlockBlobReference(aftertitle1);

                            CloudBlockBlob source2 = container5.GetBlockBlobReference(beforetitle2);
                            CloudBlockBlob target2 = container5.GetBlockBlobReference(aftertitle2);

                            await target1.StartCopyAsync(source1);

                            await target2.StartCopyAsync(source2);

                            await source1.DeleteIfExistsAsync();

                            await source2.DeleteIfExistsAsync();

                            IndexModel.screenmode = "normal";

                            saveTitle();
                        }
                        else if (IndexModel.screenmode == "searched")
                        {

                            //all to normal
                            var titlestr1 = (TitleItem)result.Items[all_loc_c];
                            //all to searched
                            var titlestr2 = (TitleItem)result2.Items[all_loc_c];
                            var beforetitle1 = titlestr1.Title;
                            var beforetitle2 = titlestr2.Title;
                            var aftertitle1 = title2 + ".csv";
                            var aftertitle2 = title2 + "-S.csv";
                            titlestr1.Title = aftertitle1;
                            titlestr2.Title = aftertitle2;


                            var credential5 = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
                            var storageAccount5 = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential5, true);
                            ////blob
                            CloudBlobClient blobClient5 = storageAccount5.CreateCloudBlobClient();

                            ////// Retrieve reference to a previously created container.
                            CloudBlobContainer container5 = blobClient5.GetContainerReference(containerName_file);

                            CloudBlockBlob source1 = container5.GetBlockBlobReference(beforetitle1);
                            CloudBlockBlob target1 = container5.GetBlockBlobReference(aftertitle1);

                            CloudBlockBlob source2 = container5.GetBlockBlobReference(beforetitle2);
                            CloudBlockBlob target2 = container5.GetBlockBlobReference(aftertitle2);

                            await target1.StartCopyAsync(source1);

                            await target2.StartCopyAsync(source2);

                            await source1.DeleteIfExistsAsync();

                            await source2.DeleteIfExistsAsync();

                            IndexModel.screenmode = "searched";
                            saveTitle();
                        }

                        await Task.Delay(500);

                    }

                    break;
                case "tytlechanged2":
                    title2 = Request.Form["title"];

                    var locstr1 = (LocItem)Loc.Items[selecttab - 1];
                    var locstr2 = locstr1.Loc2;

                    
                    rerativeloc = srcAlldoc[selecttab - 1][locstr2].Loc;

                    for (int i = 0; i <= IndexModel.Alldoc[IndexModel.selecttab - 1].Count - 1; i++)
                    {
                        if (IndexModel.Alldoc[selecttab - 1][i].Id == rerativeloc)
                        {
                            IndexModel.Alldoc[selecttab - 1][i].Title = title2;
                            IndexModel.Alldoc[selecttab - 1][i].Upddate = cstTime.ToString("yyyy/MM/dd HH:mm:ss");
                            IndexModel.Alldoc[selecttab - 1][i].Updauther = username;
                            break;
                        }
                    }
                    IndexModel.srcAlldoc[IndexModel.selecttab - 1][locstr2].Title = title2;
                    IndexModel.srcAlldoc[IndexModel.selecttab - 1][locstr2].Upddate = cstTime.ToString("yyyy/MM/dd HH:mm:ss");
                    IndexModel.srcAlldoc[IndexModel.selecttab - 1][locstr2].Updauther = username;

                    IndexModel.screenmode = "searched";
                    saveDoc();
                    saveDoc2();

                    await Task.Delay(500);
                    break;
                case "back":
                    IndexModel.errormessage = "";
                    IndexModel.screenmode = "normal";
                    break;
                case "newchancel1":
                    IndexModel.screenmode = "normal";
                    IndexModel.errormessage = "";
                    break;
                case "newchancel2":
                    IndexModel.screenmode = "searched";
                    IndexModel.errormessage = "";
                    break;
                case "deleted":
                    if (IndexModel.screenmode == "normal")
                    {
                        IndexModel.title = IndexModel.Alldoc[IndexModel.selecttab - 1][Int32.Parse(searched_row.ToString())].Title;
                        deletemode = "normal";
                    }
                    else
                    {
                        IndexModel.title = IndexModel.srcAlldoc[IndexModel.selecttab - 1][searched_row].Title;
                        deletemode = "searched";
                    }
                    if (deletemode == "normal")
                    {
                        //カウントが1以下の時は削除しない。
                        if (IndexModel.Alldoc[IndexModel.selecttab - 1].Count <= 1)
                        {
                            IndexModel.errormessage = "ドキュメントは最低限１つである必要があります。";
                        }
                        else
                        {
                            IndexModel.Alldoc[IndexModel.selecttab - 1].RemoveAt(Convert.ToInt32(searched_row.ToString()));
                            //blobの削除ループ検索前
                            //abstruct to all

                            var folderName = (all_loc_c).ToString() + "_" + searched_row;
                            var credential11 = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(IndexModel.accountName, IndexModel.accessKey);
                            var storageAccount11 = new CloudStorageAccount(credential11, true);
                            CloudBlobContainer container = storageAccount11.CreateCloudBlobClient().GetContainerReference(containerName_folder);

                            ctoken = new BlobContinuationToken();
                            do
                            {
                                var result = await container.ListBlobsSegmentedAsync(folderName, true, BlobListingDetails.None, null, ctoken, null, null);
                                ctoken = result.ContinuationToken;
                                await Task.WhenAll(result.Results
                                    .Select(item => (item as CloudBlob)?.DeleteIfExistsAsync())
                                    .Where(task => task != null)
                                );
                            } while (ctoken != null);

                            if (searched_row - 1 <= 0)
                            {
                                saveLoc((IndexModel.selecttab - 1).ToString(), (0).ToString());


                            }
                            else
                            {
                                saveLoc((IndexModel.selecttab - 1).ToString(), (searched_row - 1).ToString());
                            }
                        }
                        saveDoc();
                        saveDoc2();
                        await Task.Delay(500);
                        IndexModel.screenmode = "normal";
                    }
                    else
                    {
                        //カウントが1以下の時は削除しない。
                        if (IndexModel.Alldoc[IndexModel.selecttab - 1].Count <= 1)
                        {
                            IndexModel.errormessage = "ドキュメントは最低限１つである必要があります。";
                        }
                        else
                        {
                            rerativeloc = srcAlldoc[selecttab - 1][searched_row].Loc;

                            for (int i = 0; i <= IndexModel.Alldoc[IndexModel.selecttab - 1].Count - 1; i++)
                            {
                                if (IndexModel.Alldoc[selecttab - 1][i].Id == rerativeloc)
                                {
                                    IndexModel.Alldoc[IndexModel.selecttab - 1].RemoveAt(i);
                                    break;
                                }
                            }
                            IndexModel.srcAlldoc[IndexModel.selecttab - 1].RemoveAt(Convert.ToInt32(searched_row));
                            //blobの削除ループ検索後
                            //abstruct to all

                            var folderName = (all_loc_c).ToString() + "_" + searched_row + "/";
                            var credential11 = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(IndexModel.accountName, IndexModel.accessKey);
                            var storageAccount11 = new CloudStorageAccount(credential11, true);
                            CloudBlobContainer container = storageAccount11.CreateCloudBlobClient().GetContainerReference(containerName_folder);

                            ctoken = new BlobContinuationToken();
                            do
                            {
                                var result = await container.ListBlobsSegmentedAsync(folderName, true, BlobListingDetails.None, null, ctoken, null, null);
                                ctoken = result.ContinuationToken;
                                await Task.WhenAll(result.Results
                                    .Select(item => (item as CloudBlob)?.DeleteIfExistsAsync())
                                    .Where(task => task != null)
                                );
                            } while (ctoken != null);
                            if (searched_row - 1 <= 0)
                            {
                                saveLoc2((IndexModel.selecttab).ToString(), (0).ToString());
                                IndexModel.errormessage = "ドキュメントは最低限１つである必要があります。";
                            }
                            else
                            {
                                saveLoc2((IndexModel.selecttab).ToString(), (searched_row - 1).ToString());
                            }
                            saveDoc();
                            saveDoc2();
                            await Task.Delay(500);
                            IndexModel.screenmode = "searched";
                        }

                    }
                    break;
                case "up":
                    
                    var locstr4 = (LocItem)LocGen.Items[all_loc_c];
                    var locstr21 = locstr4.Loc1;

                   
                    var bkid = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21 - 1].Id;
                    var bktytle = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21 - 1].Title;
                    var bkbody = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21 - 1].Body;
                    var bkhtmlbody = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21 - 1].Htmlbody;
                    var bkupddate = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21 - 1].Upddate;
                    var bkupdauthor = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21 - 1].Updauther;
                    var bkattchfiles = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21 - 1].Attchfiles;
                    var bkscrollock = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21 - 1].Scroolloc;
                    IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21 - 1].Id = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21].Id;
                    IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21 - 1].Title = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21].Title;
                    IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21 - 1].Body = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21].Body;
                    IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21 - 1].Htmlbody = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21].Htmlbody;
                    IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21 - 1].Upddate = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21].Upddate;
                    IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21 - 1].Updauther = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21].Updauther;
                    IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21 - 1].Attchfiles = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21].Attchfiles;
                    IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21 - 1].Scroolloc = IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21].Scroolloc;
                    IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21].Id = bkid;
                    IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21].Title = bktytle;
                    IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21].Body = bkbody;
                    IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21].Htmlbody = bkhtmlbody;
                    IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21].Upddate = bkupddate;
                    IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21].Updauther = bkupdauthor;
                    IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21].Attchfiles = bkattchfiles;
                    IndexModel.Alldoc[IndexModel.selecttab - 1][locstr21].Scroolloc = bkscrollock;
                    //    }
                    //}
                    //
                    IndexModel.indxid = (locstr21 - 1).ToString();
                    if (Int32.Parse(IndexModel.indxid) < 0)
                    {
                        IndexModel.indxid = "0";
                    }
                    saveDoc();
                    await Task.Delay(500);
                    bodyelement = IndexModel.Alldoc[IndexModel.selecttab - 1][Int32.Parse(locstr21.ToString())].Htmlbody;

                    //var locstr150 = (LocItem)Loc.Items[IndexModel.selecttab - 1];
                    //var tabno = locstr150.ID;
                    saveLoc((IndexModel.selecttab - 1).ToString(), indxid);

                    IndexModel.screenmode = "normal";
                    break;
                case "down":
                    for (int i = 0; i <= IndexModel.Alldoc[IndexModel.selecttab - 1].Count - 2; i++)
                    {
                        if (i == Int32.Parse(normal_row.ToString()))
                        {
                            var bkid2 = IndexModel.Alldoc[IndexModel.selecttab - 1][i + 1].Id;
                            var bktytle2 = IndexModel.Alldoc[IndexModel.selecttab - 1][i + 1].Title;
                            var bkbody2 = IndexModel.Alldoc[IndexModel.selecttab - 1][i + 1].Body;
                            var bkhtmlbody2 = IndexModel.Alldoc[IndexModel.selecttab - 1][i + 1].Htmlbody;
                            var bkupddate2 = IndexModel.Alldoc[IndexModel.selecttab - 1][i + 1].Upddate;
                            var bkupdauthor2 = IndexModel.Alldoc[IndexModel.selecttab - 1][i + 1].Updauther;
                            var bkattchfiles2 = IndexModel.Alldoc[IndexModel.selecttab - 1][i + 1].Attchfiles;
                            var bkscrollock2 = IndexModel.Alldoc[IndexModel.selecttab - 1][i + 1].Scroolloc;
                            IndexModel.Alldoc[IndexModel.selecttab - 1][i + 1].Id = IndexModel.Alldoc[IndexModel.selecttab - 1][i].Id;
                            IndexModel.Alldoc[IndexModel.selecttab - 1][i + 1].Title = IndexModel.Alldoc[IndexModel.selecttab - 1][i].Title;
                            IndexModel.Alldoc[IndexModel.selecttab - 1][i + 1].Body = IndexModel.Alldoc[IndexModel.selecttab - 1][i].Body;
                            IndexModel.Alldoc[IndexModel.selecttab - 1][i + 1].Htmlbody = IndexModel.Alldoc[IndexModel.selecttab - 1][i].Htmlbody;
                            IndexModel.Alldoc[IndexModel.selecttab - 1][i + 1].Upddate = IndexModel.Alldoc[IndexModel.selecttab - 1][i].Upddate;
                            IndexModel.Alldoc[IndexModel.selecttab - 1][i + 1].Updauther = IndexModel.Alldoc[IndexModel.selecttab - 1][i].Updauther;
                            IndexModel.Alldoc[IndexModel.selecttab - 1][i + 1].Attchfiles = IndexModel.Alldoc[IndexModel.selecttab - 1][i].Attchfiles;
                            IndexModel.Alldoc[IndexModel.selecttab - 1][i + 1].Scroolloc = IndexModel.Alldoc[IndexModel.selecttab - 1][i].Scroolloc;
                            IndexModel.Alldoc[IndexModel.selecttab - 1][i].Id = bkid2;
                            IndexModel.Alldoc[IndexModel.selecttab - 1][i].Title = bktytle2;
                            IndexModel.Alldoc[IndexModel.selecttab - 1][i].Body = bkbody2;
                            IndexModel.Alldoc[IndexModel.selecttab - 1][i].Htmlbody = bkhtmlbody2;
                            IndexModel.Alldoc[IndexModel.selecttab - 1][i].Upddate = bkupddate2;
                            IndexModel.Alldoc[IndexModel.selecttab - 1][i].Updauther = bkupdauthor2;
                            IndexModel.Alldoc[IndexModel.selecttab - 1][i].Attchfiles = bkattchfiles2;
                            IndexModel.Alldoc[IndexModel.selecttab - 1][i].Scroolloc = bkscrollock2;
                        }
                    }
                    IndexModel.indxid = (normal_row + 1).ToString();
                    if (Int32.Parse(IndexModel.indxid) <= IndexModel.Alldoc[IndexModel.selecttab - 1].Count - 1)
                    {

                    }
                    else
                    {
                        IndexModel.indxid = IndexModel.indxid = (normal_row).ToString();
                    }
                    saveDoc();
                    await Task.Delay(500);
                    bodyelement = IndexModel.Alldoc[IndexModel.selecttab - 1][Int32.Parse(normal_row.ToString())].Htmlbody;

                    saveLoc((IndexModel.selecttab - 1).ToString(), indxid);

                    IndexModel.screenmode = "normal";
                    break;
                case "right":
                    if (IndexModel.selecttab + 1 < ConvLoc.Items.Count + 1)
                    {
                        Actions.MoveRight();
                    }
                    break;
                case "left":
                    if (IndexModel.selecttab >= 0)
                    {
                        var locstr12 = (ConvLocItem)ConvLoc.Items[IndexModel.selecttab - 1];
                        var locstr22 = (ConvLocItem)ConvLoc.Items[IndexModel.selecttab - 2];
                        var loc1 = locstr12.mainid;
                        var loc2 = locstr22.mainid;
                        //検索前
                        var titlestr1 = (TitleItem)result.Items[loc2];
                        var num1 = titlestr1.ID;
                        var msg1 = titlestr1.Title;
                        var typ1 = titlestr1.Type;
                        var col1 = titlestr1.Colour;
                        var stus1 = titlestr1.Status;

                        var titlestr2 = (TitleItem)result.Items[loc1];
                        var num2 = titlestr2.ID;
                        var msg2 = titlestr2.Title;
                        var typ2 = titlestr2.Type;
                        var col2 = titlestr2.Colour;
                        var stus2 = titlestr2.Status;

                        titlestr2.ID = num1;
                        titlestr2.Title = msg1;
                        titlestr2.Type = typ1;
                        titlestr2.Colour = col1;
                        titlestr2.Status = stus1;

                        titlestr1.ID = num2;
                        titlestr1.Title = msg2;
                        titlestr1.Type = typ2;
                        titlestr1.Colour = col2;
                        titlestr1.Status = stus2;

                        var locstr10 = (LocItem)LocGen.Items[loc2];
                        var id1 = locstr10.ID;
                        var loc10 = locstr10.Loc1;
                        var loc20 = locstr10.Loc2;
                        var stus10 = locstr10.Status;

                        var locstr11 = (LocItem)LocGen.Items[loc1];
                        var id11 = locstr11.ID;
                        var loc11 = locstr11.Loc1;
                        var loc21 = locstr11.Loc2;
                        var stus11 = locstr11.Status;

                        locstr11.ID = id1;
                        locstr11.Loc1 = loc10;
                        locstr11.Loc2 = loc20;
                        locstr11.Status = stus10;

                        locstr10.ID = id11;
                        locstr10.Loc1 = loc11;
                        locstr10.Loc2 = loc21;
                        locstr10.Status = stus11;


                        //検索後

                        var titlestr4 = (TitleItem)result2.Items[loc2];
                        var num3 = titlestr4.ID;
                        var msg3 = titlestr4.Title;
                        var typ3 = titlestr4.Type;
                        var col3 = titlestr4.Colour;
                        var stus3 = titlestr4.Status;

                        var titlestr5 = (TitleItem)result2.Items[loc1];
                        var num4 = titlestr5.ID;
                        var msg4 = titlestr5.Title;
                        var typ4 = titlestr5.Type;
                        var col4 = titlestr5.Colour;
                        var stus4 = titlestr5.Status;


                        titlestr5.ID = num3;
                        titlestr5.Title = msg3;
                        titlestr5.Type = typ3;
                        titlestr5.Colour = col3;
                        titlestr5.Status = stus3;

                        titlestr4.ID = num4;
                        titlestr4.Title = msg4;
                        titlestr4.Type = typ4;
                        titlestr4.Colour = col4;
                        titlestr4.Status = stus4;


                        IndexModel.selecttab = IndexModel.selecttab - 1;

                        IndexModel.activetabname = "tab3_" + IndexModel.selecttab.ToString();

                        IndexModel.screenmode = "normal";
                        saveTitle();
                        saveAllLoc();
                    }
                    break;
                case "tabclick":
                    tabindx = Request.Form["tabno"];
                    if (IndexModel.selecttab == datasize)
                    {
                        IndexModel.activetabname = "tab3_" + tabindx;
                        IndexModel.activetabname2 = "tab3_" + (Int32.Parse(tabindx) - 1).ToString();
                    }
                    else
                    {
                        IndexModel.activetabname = "tab3_" + tabindx;
                        IndexModel.activetabname2 = "tab3_" + tabindx + 1;
                    }
                    IndexModel.selecttab = Int32.Parse(tabindx);
                    //abstruct normal low

                    bodyelement = IndexModel.Alldoc[IndexModel.selecttab - 1][Int32.Parse(normal_row.ToString())].Htmlbody;
                    attachfile = IndexModel.Alldoc[IndexModel.selecttab - 1][Int32.Parse(normal_row.ToString())].Attchfiles;
                    Attachfiles = attachfile.Split(",");

                    IndexModel.screenmode = "normal";
                    break;
                case "tabclick2":
                    tabindx = Request.Form["tabno"];
                    IndexModel.activetabname = "tab3_" + tabindx;
                    IndexModel.selecttab = Int32.Parse(tabindx);
                    bodyelement = IndexModel.srcAlldoc[IndexModel.selecttab - 1][0].Htmlbody;
                    attachfile = IndexModel.srcAlldoc[IndexModel.selecttab - 1][0].Attchfiles;
                    Attachfiles = attachfile.Split(",");

                    IndexModel.screenmode = "searched";
                    break;
                case "attach":
                    indxid = Request.Form["indx"];
                    if (IndexModel.screenmode == "normal")
                    {

                        var tabno = locstr_l.ID;
                        //
                        folderstring = (tabno).ToString() + "_" + IndexModel.Alldoc[IndexModel.selecttab - 1][Int32.Parse(indxid)].Id;
                        attachfile = Request.Form["attachname"];
                        attachlink1 = getlink(folderstring, attachfile);

                    }
                    else
                    {
                        //abstruct searched id

                        var tabno = locstr_l.ID;
                        var rerativeloc = srcAlldoc[IndexModel.selecttab - 1][searched_row].Loc;
                        folderstring = (tabno).ToString() + "_" + rerativeloc;
                        attachfile = Request.Form["attachname"];
                        attachlink1 = getlink(folderstring, attachfile);
                    }
                    break;
                case "search":
                    String key = Request.Form["keyword"];
                    String fi = Request.Form["field"];
                    String groval = Request.Form["global"];
                    Actions.SearchAction(key, fi, groval);
                    break;
                case "fileattach":
                    Actions.FileAttach();
                    break;
                case "tempchancel":
                    Actions.TmpChancel();
                    break;
                case "openattach":
                    Actions.OpenAttach();
                    break;
                case "upload1":
                    indxid = Request.Form["indx"];
                    temp = IndexModel.Alldoc[IndexModel.selecttab - 1][Int32.Parse(indxid)].Attchfiles;
                    IndexModel.Attachfiles = temp.Split(",");
                    IndexModel.screenmode = "upload1";
                    break;
                case "upload2":
                    indxid = Request.Form["indx"];
                    temp = IndexModel.srcAlldoc[IndexModel.selecttab - 1][Int32.Parse(indxid)].Attchfiles;
                    IndexModel.Attachfiles = temp.Split(",");
                    IndexModel.screenmode = "upload2";
                    break;
                case "addtab":
                    //リストに追加
                    addtab = Request.Form["addtab"];
                    Actions.AddTab(addtab, accountName, accessKey, containerName_file);
                    break;
                case "inactivetab":
                    Actions.InactiveTab();
                    break;
                case "showtab":
                    showtab = Request.Form["showtab"];
                    Actions.ShowTab(showtab);
                    break;
                case "deletetab":
                    
                    break;

                case "setting":
                    Actions.Setting();
                    break;

                case "expand1":
                    IndexModel.screenmode = "exdoc1";
                    break;
                case "expand2":
                    IndexModel.screenmode = "exdoc2";
                    break;
            }
            await Task.Delay(500);

            var v1 = new ViewModel
            {
                title = title,
                body = bodyelement,
                uploaddate = updatedate,
                author = author,
                attachfile = attachfile,
                attachlink = attachlink1,
                scrollloc = scrollloc

            };
            string jsonString = JsonSerializer.Serialize(v1);

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

            //XMLシリアル化するオブジェクト
            var count = 0;
            LocClass Loc = new LocClass();
            Loc.Items = new System.Collections.ArrayList();
            ConvLoc ConvLoc = new ConvLoc();
            ConvLoc.Items = new System.Collections.ArrayList();
            //
            for (int i = 0; i <= LocGen.Items.Count - 1; i++)
            {
                var locstr10 = (LocItem)LocGen.Items[i];
                var locstr50 = locstr10.ID;
                var locstr20 = locstr10.Loc1;
                var locstr30 = locstr10.Loc2;
                var locstr40 = locstr10.Status;
                if (locstr40 == "ACTIVE")
                {
                    Loc.Items.Add(new LocItem(locstr50, locstr20, locstr30, locstr40));
                    ConvLoc.Items.Add(new ConvLocItem(i, count));
                    count = count + 1;
                }
            }
            //
            var locstr1 = (ConvLocItem)ConvLoc.Items[Int32.Parse(tab_no)];
            var loc2 = locstr1.mainid;

            var str = (LocItem)LocGen.Items[loc2];
            str.Loc1 = Int32.Parse(loc);

            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(typeof(LocClass));
                serializer.Serialize(stringwriter, LocGen);
                var xmlstr = stringwriter.ToString();
                //blobから設定ファイルをダウンロードする。
                //storageAccountの作成（接続情報の定義）
                //アカウントネームやキー情報はAzureポータルから確認できる。
                var credential1 = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(IndexModel.accountName, IndexModel.accessKey);
                var storageAccount1 = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential1, true);
                //blob
                CloudBlobClient blobClient1 = storageAccount1.CreateCloudBlobClient();
                // Retrieve reference to a previously created container.
                CloudBlobContainer container1 = blobClient1.GetContainerReference(containerName_setting);

                //Create reference to a Blob that May or Maynot exist under the container
                CloudBlockBlob blockBlob = container1.GetBlockBlobReference("Loclist.xml");
                // Create or overwrite the "something.xml" blob with contents from a string
                await blockBlob.UploadTextAsync(xmlstr);
                await Task.Delay(500);
            }
        }
        public static async void saveLoc2(string tab_no, string loc)
        {

            var str = (LocItem)LocGen.Items[Int32.Parse(tab_no) - 1];
            str.Loc2 = Int32.Parse(loc);
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(typeof(LocClass));
                serializer.Serialize(stringwriter, LocGen);
                var xmlstr = stringwriter.ToString();
                //blobから設定ファイルをダウンロードする。
                //storageAccountの作成（接続情報の定義）
                //アカウントネームやキー情報はAzureポータルから確認できる。
                var credential1 = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(IndexModel.accountName, IndexModel.accessKey);
                var storageAccount1 = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential1, true);
                //blob
                CloudBlobClient blobClient1 = storageAccount1.CreateCloudBlobClient();
                // Retrieve reference to a previously created container.
                CloudBlobContainer container1 = blobClient1.GetContainerReference(containerName_setting);

                //Create reference to a Blob that May or Maynot exist under the container
                CloudBlockBlob blockBlob = container1.GetBlockBlobReference("Loclist.xml");
                // Create or overwrite the "something.xml" blob with contents from a string
                await blockBlob.UploadTextAsync(xmlstr);
                await Task.Delay(500);
            }


        }

        public static async void saveTitle()
        {


            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(typeof(TitleClass));
                serializer.Serialize(stringwriter, result);
                var xmlstr = stringwriter.ToString();
                //blobから設定ファイルをダウンロードする。
                //storageAccountの作成（接続情報の定義）
                //アカウントネームやキー情報はAzureポータルから確認できる。
                var credential1 = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(IndexModel.accountName, IndexModel.accessKey);
                var storageAccount1 = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential1, true);
                //blob
                CloudBlobClient blobClient1 = storageAccount1.CreateCloudBlobClient();
                // Retrieve reference to a previously created container.
                CloudBlobContainer container1 = blobClient1.GetContainerReference(containerName_setting);

                //Create reference to a Blob that May or Maynot exist under the container
                CloudBlockBlob blockBlob = container1.GetBlockBlobReference("FILES.xml");
                // Create or overwrite the "something.xml" blob with contents from a string
                await blockBlob.UploadTextAsync(xmlstr);
                await Task.Delay(500);
            }
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(typeof(TitleClass));
                serializer.Serialize(stringwriter, result2);
                var xmlstr = stringwriter.ToString();
                //blobから設定ファイルをダウンロードする。
                //storageAccountの作成（接続情報の定義）
                //アカウントネームやキー情報はAzureポータルから確認できる。
                var credential1 = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(IndexModel.accountName, IndexModel.accessKey);
                var storageAccount1 = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential1, true);
                //blob
                CloudBlobClient blobClient1 = storageAccount1.CreateCloudBlobClient();
                // Retrieve reference to a previously created container.
                CloudBlobContainer container1 = blobClient1.GetContainerReference(containerName_setting);

                //Create reference to a Blob that May or Maynot exist under the container
                CloudBlockBlob blockBlob = container1.GetBlockBlobReference("S_FILES.xml");
                // Create or overwrite the "something.xml" blob with contents from a string
                await blockBlob.UploadTextAsync(xmlstr);
                await Task.Delay(500);
            }
        }

        public static async void saveAllLoc()
        {
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(typeof(LocClass));
                serializer.Serialize(stringwriter, LocGen);
                var xmlstr = stringwriter.ToString();
                //blobから設定ファイルをダウンロードする。
                //storageAccountの作成（接続情報の定義）
                //アカウントネームやキー情報はAzureポータルから確認できる。
                var credential1 = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(IndexModel.accountName, IndexModel.accessKey);
                var storageAccount1 = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential1, true);
                //blob
                CloudBlobClient blobClient1 = storageAccount1.CreateCloudBlobClient();
                // Retrieve reference to a previously created container.
                CloudBlobContainer container1 = blobClient1.GetContainerReference(containerName_setting);

                //Create reference to a Blob that May or Maynot exist under the container
                CloudBlockBlob blockBlob = container1.GetBlockBlobReference("Loclist.xml");
                // Create or overwrite the "something.xml" blob with contents from a string
                await blockBlob.UploadTextAsync(xmlstr);
                await Task.Delay(500);
            }
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
        public static async void saveAllUser(string userOId, string username, string mailadress, string enrolleddate)
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

            var customerdata2 = customerdata1 + userOId + "," + username + "," + mailadress + "," + enrolleddate + ",";

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
        public static async Task GetFiles(string containerName_setting, string containerName_file,String accountName, String accessKey)
        {
            
            getf = true;
            var credential = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
            var storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential, true);
            //blob
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();


            //HTML生成用序数詞リスト読み込み
            // Retrieve reference to a blob named "filename"
            // Retrieve reference to a previously created container.
            CloudBlobContainer container_setting = blobClient.GetContainerReference(containerName_setting);
            CloudBlobContainer container2 = blobClient.GetContainerReference(containerName_file);

            //DB本体のテキストファイル読み込み
            // Retrieve reference to a blob named "filename"
            CloudBlockBlob blockBlob3 = container_setting.GetBlockBlobReference("FILES.xml");

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
            XmlSerializer serializer = new XmlSerializer(typeof(TitleClass));
            result = null;
            result = (TitleClass)serializer.Deserialize(reader3);
            reader3.Close();
            await Task.Delay(500);

            // Retrieve reference to a previously created container.
            //CloudBlobContainer container_setting = blobClient.GetContainerReference(containerName_setting);

            //色の指定
            IndexModel.TabColours.Clear();


            for (int i = 0; i <= result.Items.Count - 1; i++)
            {
                var titlestr1 = (TitleItem)result.Items[i];
                var titlestr3 = titlestr1.Status;
                var titlestr4 = titlestr1.Colour;

                if (titlestr3 == "ACTIVE")
                {
                    TabColours.Add(titlestr4);
                }
            }


        }
        public static async Task GetSFiles(string containerName_setting,String accountName, String accessKey)
        {
            
            getf = true;
            var credential = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
            var storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential, true);
            //blob
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();


            //HTML生成用序数詞リスト読み込み
            // Retrieve reference to a blob named "filename"
            // Retrieve reference to a previously created container.
            CloudBlobContainer container_setting = blobClient.GetContainerReference(containerName_setting);

            //DB本体のテキストファイル読み込み：検索後
            // Retrieve reference to a blob named "filename"
            CloudBlockBlob blockBlob7 = container_setting.GetBlockBlobReference("S_FILES.xml");

            string files7;
            using (var memoryStream7 = new MemoryStream())
            {
                await blockBlob7.DownloadToStreamAsync(memoryStream7);
                files7 = System.Text.Encoding.UTF8.GetString(memoryStream7.ToArray());
            }

            // convert string to stream
            byte[] byteArray7 = Encoding.UTF8.GetBytes(files7);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream7 = new MemoryStream(byteArray7);
            // convert stream to string
            StreamReader reader7 = new StreamReader(stream7);
            XmlSerializer serializer7 = new XmlSerializer(typeof(TitleClass));
            result2 = null;
            result2 = (TitleClass)serializer7.Deserialize(reader7);
            reader7.Close();

            await Task.Delay(500);
        }
        public static async Task GetLocList(string containerName_setting)
        {
            //accountName = _configuration.GetValue<string>("accountName");
            //accessKey = _configuration.GetValue<string>("accessKey");
            getf = true;
            var credential = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
            var storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential, true);
            //blob
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container_setting = blobClient.GetContainerReference(containerName_setting);
            //DB本体のテキストファイル読み込み：検索後
            // Retrieve reference to a blob named "filename"
            CloudBlockBlob blockBlob9 = container_setting.GetBlockBlobReference("Loclist.xml");

            string files9;

            using (var memoryStream9 = new MemoryStream())
            {
                await blockBlob9.DownloadToStreamAsync(memoryStream9);
                files9 = System.Text.Encoding.UTF8.GetString(memoryStream9.ToArray());
            }

            // convert string to stream
            byte[] byteArray9 = Encoding.UTF8.GetBytes(files9);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream9 = new MemoryStream(byteArray9);
            // convert stream to string
            StreamReader reader9 = new StreamReader(stream9);
            XmlSerializer serializer9 = new XmlSerializer(typeof(LocClass));
            LocGen = null;
            LocGen = (LocClass)serializer9.Deserialize(reader9);
            reader9.Close();
            await Task.Delay(500);
        }
        public static  async Task GetAllDoc(string containerName_setting)
        {
            //accountName = _configuration.GetValue<string>("accountName");
            //accessKey = _configuration.GetValue<string>("accessKey");
            getf = true;
            var credential = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
            var storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential, true);
            //blob
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //HTML生成用序数詞リスト読み込み
            // Retrieve reference to a blob named "filename"
            // Retrieve reference to a previously created container.
            //CloudBlobContainer container2 = blobClient.GetContainerReference(containerName_file);

            CloudBlobContainer container2 = blobClient.GetContainerReference(containerName_file);
            Alldoc = new List<List<Documentdata>>();
            //読み込んだテキストのリストへの展開：元ファイル
            for (int i = 0; i <= result.Items.Count - 1; i++)
            {
                var titlestr1 = (TitleItem)result.Items[i];
                var titlestr2 = titlestr1.Title;
                var titlestr3 = titlestr1.Status;
                if (titlestr3 == "ACTIVE")
                {
                    CloudBlockBlob blockBlob4 = container2.GetBlockBlobReference(titlestr2);
                    using (var memoryStream4 = new MemoryStream())
                    {
                        string data = "";
                        await blockBlob4.DownloadToStreamAsync(memoryStream4);
                        data = System.Text.Encoding.UTF8.GetString(memoryStream4.ToArray());
                        string[] doc = data.Replace("<spert2>", ";").Split(';');
                        var counter = 0;
                        string id = "";
                        string title = "";
                        string body = "";
                        string htmlbody = "";
                        string upddate = "";
                        string updauther = "";
                        string attchfiles = "";
                        string scroolloc = "";

                        Documents = new List<Documentdata>();
                        foreach (string value in doc)
                        {

                            if (counter % 8 == 0)
                            {
                                id = doc[counter].Replace("<seprt1>", ";");
                            }
                            if (counter % 8 == 1)
                            {
                                title = doc[counter].Replace("<seprt1>", ";");
                            }
                            if (counter % 8 == 2)
                            {
                                body = doc[counter].Replace("<seprt1>", ";");
                            }
                            if (counter % 8 == 3)
                            {
                                htmlbody = doc[counter].Replace("<seprt1>", ";");
                            }
                            if (counter % 8 == 4)
                            {
                                upddate = doc[counter].Replace("<seprt1>", ";");
                            }
                            if (counter % 8 == 5)
                            {
                                updauther = doc[counter].Replace("<seprt1>", ";");
                            }
                            if (counter % 8 == 6)
                            {
                                attchfiles = doc[counter].Replace("<seprt1>", ";");
                            }
                            if (counter % 8 == 7)
                            {
                                scroolloc = doc[counter].Replace("<seprt1>", ";");
                                Document = new Documentdata();
                                Document.Id = id;
                                Document.Title = title;
                                Document.Body = body;
                                Document.Htmlbody = htmlbody;
                                Document.Upddate = upddate;
                                Document.Updauther = updauther;
                                Document.Attchfiles = attchfiles;
                                Document.Scroolloc = scroolloc;
                                Documents.Add(Document);
                            }
                            counter += 1;
                        }
                        Alldoc.Add(Documents);
                    }
                }
            }

            await Task.Delay(500);
        }
        public static  async Task GetsrcAllDoc(string containerName_setting)
        {
            //accountName = _configuration.GetValue<string>("accountName");
            //accessKey = _configuration.GetValue<string>("accessKey");
            getf = true;
            var credential = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
            var storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential, true);
            //blob
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container2 = blobClient.GetContainerReference(containerName_file);
            //読み込んだテキストのリストへの展開：元ファイル
            srcAlldoc = new List<List<srcDocumentdata>>();
            for (int i = 0; i <= result2.Items.Count - 1; i++)
            {
                var titlestr1 = (TitleItem)result2.Items[i];
                var titlestr2 = titlestr1.Title;
                var titlestr3 = titlestr1.Status;
                if (titlestr3 == "ACTIVE")
                {
                    CloudBlockBlob blockBlob4 = container2.GetBlockBlobReference(titlestr2);
                    using (var memoryStream4 = new MemoryStream())
                    {
                        string data = "";
                        await blockBlob4.DownloadToStreamAsync(memoryStream4);
                        data = System.Text.Encoding.UTF8.GetString(memoryStream4.ToArray());
                        string[] doc = data.Replace("<spert2>", ";").Split(';');
                        var counter = 0;
                        string id = "";
                        string title = "";
                        string body = "";
                        string htmlbody = "";
                        string upddate = "";
                        string updauther = "";
                        string attchfiles = "";
                        string scroolloc = "";
                        string loc = "";
                        srcDocuments = new List<srcDocumentdata>();
                        foreach (string value in doc)
                        {
                            if (counter % 9 == 0)
                            {
                                id = doc[counter].Replace("<seprt1>", ";");
                            }
                            if (counter % 9 == 1)
                            {
                                title = doc[counter].Replace("<seprt1>", ";");
                            }
                            if (counter % 9 == 2)
                            {
                                body = doc[counter].Replace("<seprt1>", ";");
                            }
                            if (counter % 9 == 3)
                            {
                                htmlbody = doc[counter].Replace("<seprt1>", ";");
                            }
                            if (counter % 9 == 4)
                            {
                                upddate = doc[counter].Replace("<seprt1>", ";");
                            }
                            if (counter % 9 == 5)
                            {
                                updauther = doc[counter].Replace("<seprt1>", ";");
                            }
                            if (counter % 9 == 6)
                            {
                                attchfiles = doc[counter].Replace("<seprt1>", ";");
                            }
                            if (counter % 9 == 7)
                            {
                                scroolloc = doc[counter].Replace("<seprt1>", ";");
                            }
                            if (counter % 9 == 8)
                            {
                                loc = doc[counter].Replace("<seprt1>", ";");
                                srcDocument = new srcDocumentdata();
                                srcDocument.Id = id;
                                srcDocument.Title = title;
                                srcDocument.Body = body;
                                srcDocument.Htmlbody = htmlbody;
                                srcDocument.Upddate = upddate;
                                srcDocument.Updauther = updauther;
                                srcDocument.Attchfiles = attchfiles;
                                srcDocument.Scroolloc = scroolloc;
                                srcDocument.Loc = loc;
                                srcDocuments.Add(srcDocument);
                            }
                            counter += 1;
                        }
                        srcAlldoc.Add(srcDocuments);
                    }
                }
                //IndexModel.datasize = counter - 1;
                var titlestr_12 = (TitleItem)result2.Items[activetab];
                SelectedTabName = titlestr_12.Title;
                SelectedTabName = SelectedTabName.Replace(".csv", "");
            }


            await Task.Delay(500);
        }

    }
}