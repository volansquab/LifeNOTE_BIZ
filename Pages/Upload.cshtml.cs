using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Graph.SecurityNamespace;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LifeNOTE_BIZ.Pages
{
    [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
    public class UploadModel : PageModel
    {

        [BindProperty]
        public List<IFormFile>? Upload { get; set; }

        [BindProperty]
        public string? Name { get; set; }


        public static List<List<Documentdata>> Alldoc2 = new List<List<Documentdata>>();
        public static List<Documentdata> Documents2 = new List<Documentdata>();
        public static Documentdata Document2 = new Documentdata();
        public static List<List<srcDocumentdata>> srcAlldoc2 = new List<List<srcDocumentdata>>();
        public static List<srcDocumentdata> srcDocuments2 = new List<srcDocumentdata>();
        public static srcDocumentdata srcDocument2 = new srcDocumentdata();
        public static string? bodyelement;
        public static string? updatedate;
        public static string? author;
        public static string? attachfile;
        public static string? screenmode;
        public static TitleClass? result_1;
        public static TitleClass? result_2;
        public static string? doclocgen;
        public static string[]? Attachfiles;
        public static LocClass? Loc2;
        public static LocClass? LocGen2;
        public static string? username;
        public static string? containerName_file;
        public static string? containerName_folder;
        public static string? containerName_setting;

        public async Task OnGetAsync()
        {
            var credential = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(IndexModel.accountName, IndexModel.accessKey);
            var storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential, true);

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



            containerName_file = userOId + "-filedata";
            containerName_folder = userOId + "-folder";
            containerName_setting = userOId + "-setting";


            //blob
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(containerName_setting);

            //DB本体のテキストファイル読み込み：検索後
            // Retrieve reference to a blob named "filename"
            CloudBlockBlob blockBlob9 = container.GetBlockBlobReference("Loclist.xml");

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
            LocGen2 = null;
            LocGen2 = (LocClass)serializer9.Deserialize(reader9);
            reader9.Close();

            //DB本体のテキストファイル読み込み
            // Retrieve reference to a blob named "filename"
            CloudBlockBlob blockBlob3 = container.GetBlockBlobReference("FILES.xml");

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
            result_1 = null;
            result_1 = (TitleClass)serializer.Deserialize(reader3);
            reader3.Close();

            //DB本体のテキストファイル読み込み：検索後
            // Retrieve reference to a blob named "filename"
            CloudBlockBlob blockBlob7 = container.GetBlockBlobReference("S_FILES.xml");

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
            result_2 = null;
            result_2 = (TitleClass)serializer7.Deserialize(reader7);
            reader7.Close();


            //本体ファイルのクリア★
            Alldoc2.Clear();
            srcAlldoc2.Clear();

            //XMLシリアル化するオブジェクト
            var count = 0;
            LocClass Loc = new LocClass();
            Loc.Items = new System.Collections.ArrayList();
            ConvLoc ConvLoc = new ConvLoc();
            ConvLoc.Items = new System.Collections.ArrayList();
            for (int i = 0; i <= LocGen2.Items.Count - 1; i++)
            {
                var locstr100 = (LocItem)LocGen2.Items[i];
                var id = locstr100.ID;
                var locstr20 = locstr100.Loc1;
                var locstr30 = locstr100.Loc2;
                var locstr40 = locstr100.Status;
                if (locstr40 == "ACTIVE")
                {
                    Loc.Items.Add(new LocItem(id, locstr20, locstr30, locstr40));
                    ConvLoc.Items.Add(new ConvLocItem(i, count));
                    count = count + 1;
                }
            }

            CloudBlobContainer container2 = blobClient.GetContainerReference(containerName_file);

            if (IndexModel.screenmode == "searched")
            {
                //読み込んだテキストのリストへの展開：元ファイル
                srcAlldoc2 = new List<List<srcDocumentdata>>();
                //tabへ★
                //var locstr1 = (LocItem)Loc.Items[IndexModel.selecttab - 1];
                //var tabno = locstr1.ID;
                //var titlestr1 = (TitleItem)result2.Items[tabno];
                //var titlestr2 = titlestr1.Message;

                var locstr1 = (ConvLocItem)ConvLoc.Items[IndexModel.selecttab - 1];
                var loc10 = locstr1.mainid;
                var titlestr1 = (TitleItem)result_1.Items[loc10];
                var titlestr2 = titlestr1.Title;
                var titlestr3 = titlestr1.Status;



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
                    srcDocuments2 = new List<srcDocumentdata>();
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
                            srcDocument2 = new srcDocumentdata();
                            srcDocument2.Id = id;
                            srcDocument2.Title = title;
                            srcDocument2.Body = body;
                            srcDocument2.Htmlbody = htmlbody;
                            srcDocument2.Upddate = upddate;
                            srcDocument2.Updauther = updauther;
                            srcDocument2.Attchfiles = attchfiles;
                            srcDocument2.Scroolloc = scroolloc;
                            srcDocument2.Loc = loc;
                            srcDocuments2.Add(srcDocument2);
                        }
                        counter += 1;
                    }
                }
                srcAlldoc2.Add(srcDocuments2);


                //
                //元ファイルタブ表示の切り替え
                if (srcAlldoc2[0].Count != 0)
                {
                    var locstr10 = (LocItem)LocGen2.Items[loc10];
                    var locstr2 = locstr10.Loc2;
                    bodyelement = srcAlldoc2[0][locstr2].Htmlbody;
                    updatedate = srcAlldoc2[0][locstr2].Upddate;
                    author = srcAlldoc2[0][locstr2].Updauther;
                    attachfile = srcAlldoc2[0][locstr2].Attchfiles;
                }
                else
                {
                    bodyelement = "";
                }
            }
            else
            {
                Alldoc2 = new List<List<Documentdata>>();
                //読み込んだテキストのリストへの展開：元ファイル
                //tabへ★
                var locstr1 = (ConvLocItem)ConvLoc.Items[IndexModel.selecttab - 1];
                var loc = locstr1.mainid;
                var titlestr1 = (TitleItem)result_1.Items[loc];
                var titlestr2 = titlestr1.Title;
                var titlestr3 = titlestr1.Status;

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

                    Documents2 = new List<Documentdata>();
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
                            Document2 = new Documentdata();
                            Document2.Id = id;
                            Document2.Title = title;
                            Document2.Body = body;
                            Document2.Htmlbody = htmlbody;
                            Document2.Upddate = upddate;
                            Document2.Updauther = updauther;
                            Document2.Attchfiles = attchfiles;
                            Document2.Scroolloc = scroolloc;
                            Documents2.Add(Document2);
                        }
                        counter += 1;
                    }
                }
                Alldoc2.Add(Documents2);


                //元ファイルタブ表示の切り替え★
                var locstr10 = (LocItem)LocGen2.Items[loc];
                var locstr2 = locstr10.Loc1;
                bodyelement = Alldoc2[0][locstr2].Htmlbody;
                updatedate = Alldoc2[0][locstr2].Upddate;
                author = Alldoc2[0][locstr2].Updauther;
                attachfile = Alldoc2[0][locstr2].Attchfiles;
            }
            //await Task.Delay(500);
        }

        public async Task OnPostAsync()
        {
            //IndexModel.screenmode = Request.Form["index"];
            //読み込んだテキストのリストへの展開：元ファイル
            srcAlldoc2 = new List<List<srcDocumentdata>>();
            var credential = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(IndexModel.accountName, IndexModel.accessKey);
            var storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential, true);


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



            containerName_file = userOId + "-filedata";
            containerName_folder = userOId + "-folder";
            containerName_setting = userOId + "-setting";

            //blob
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container_setting = blobClient.GetContainerReference(containerName_setting);

            // Retrieve reference to a blob named "filename"
            CloudBlockBlob blockBlob9 = container_setting.GetBlockBlobReference("Loclist.xml");

            string files9 = "";

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
            LocGen2 = null;
            LocGen2 = (LocClass)serializer9.Deserialize(reader9);
            reader9.Close();

            //XMLシリアル化するオブジェクト
            var count2 = 0;
            LocClass Loc = new LocClass();
            Loc.Items = new System.Collections.ArrayList();
            ConvLoc ConvLoc = new ConvLoc();
            ConvLoc.Items = new System.Collections.ArrayList();
            for (int i = 0; i <= LocGen2.Items.Count - 1; i++)
            {
                var locstr100 = (LocItem)LocGen2.Items[i];
                var id = locstr100.ID;
                var locstr20 = locstr100.Loc1;
                var locstr30 = locstr100.Loc2;
                var locstr40 = locstr100.Status;
                if (locstr40 == "ACTIVE")
                {
                    Loc.Items.Add(new LocItem(id, locstr20, locstr30, locstr40));
                    ConvLoc.Items.Add(new ConvLocItem(i, count2));
                    count2 = count2 + 1;
                }
            }


            //tabへ★
            CloudBlobContainer container2 = blobClient.GetContainerReference(containerName_file);
            var locstr200 = (ConvLocItem)ConvLoc.Items[IndexModel.selecttab - 1];
            var loc10 = locstr200.mainid;
            var locstr1 = (LocItem)LocGen2.Items[loc10];
            var tabno = loc10;
            var titlestr1 = (TitleItem)result_2.Items[loc10];
            var titlestr2 = titlestr1.Title;
            var titlestr3 = titlestr1.Status;
            if (IndexModel.screenmode == "searched" || IndexModel.screenmode == "upload2")
            {
                //読み込んだテキストのリストへの展開：元ファイル
                srcAlldoc2 = new List<List<srcDocumentdata>>();
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
                    srcDocuments2 = new List<srcDocumentdata>();
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
                            srcDocument2 = new srcDocumentdata();
                            srcDocument2.Id = id;
                            srcDocument2.Title = title;
                            srcDocument2.Body = body;
                            srcDocument2.Htmlbody = htmlbody;
                            srcDocument2.Upddate = upddate;
                            srcDocument2.Updauther = updauther;
                            srcDocument2.Attchfiles = attchfiles;
                            srcDocument2.Scroolloc = scroolloc;
                            srcDocument2.Loc = loc;
                            srcDocuments2.Add(srcDocument2);
                        }
                        counter += 1;
                    }
                }
                srcAlldoc2.Add(srcDocuments2);


                //
                //元ファイルタブ表示の切り替え
                if (srcAlldoc2[0].Count != 0)
                {
                    var locstr10 = (LocItem)LocGen2.Items[loc10];
                    var locstr2 = locstr10.Loc2;
                    bodyelement = srcAlldoc2[0][locstr2].Htmlbody;
                    updatedate = srcAlldoc2[0][locstr2].Upddate;
                    author = srcAlldoc2[0][locstr2].Updauther;
                    attachfile = srcAlldoc2[0][locstr2].Attchfiles;
                }
                else
                {
                    bodyelement = "";
                }
            }
            else
            {
                Alldoc2 = new List<List<Documentdata>>();
                //読み込んだテキストのリストへの展開：元ファイル
                //tabへ★
                var locstr15 = (ConvLocItem)ConvLoc.Items[IndexModel.selecttab - 1];
                var loc = locstr15.mainid;
                var titlestr15 = (TitleItem)result_1.Items[loc];
                var titlestr25 = titlestr15.Title;


                CloudBlockBlob blockBlob4 = container2.GetBlockBlobReference(titlestr25);
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

                    Documents2 = new List<Documentdata>();
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
                            Document2 = new Documentdata();
                            Document2.Id = id;
                            Document2.Title = title;
                            Document2.Body = body;
                            Document2.Htmlbody = htmlbody;
                            Document2.Upddate = upddate;
                            Document2.Updauther = updauther;
                            Document2.Attchfiles = attchfiles;
                            Document2.Scroolloc = scroolloc;
                            Documents2.Add(Document2);
                        }
                        counter += 1;
                    }
                }
                Alldoc2.Add(Documents2);


                //元ファイルタブ表示の切り替え★
                var locstr10 = (LocItem)LocGen2.Items[loc];
                var locstr2 = locstr10.Loc1;
                bodyelement = Alldoc2[0][locstr2].Htmlbody;
                updatedate = Alldoc2[0][locstr2].Upddate;
                author = Alldoc2[0][locstr2].Updauther;
                attachfile = Alldoc2[0][locstr2].Attchfiles;
            }



            var attachfile2 = "";

            CloudBlobContainer container3 = blobClient.GetContainerReference(containerName_folder);

            if (Request.Form["type"] == "uploadup1")
            {
                List<string> attachlist = new List<string>();
                var flag = Request.Form["upflag"].ToString();


                var locstr10 = (LocItem)LocGen2.Items[loc10];
                var locstr2 = locstr10.Loc1;
                var locstr3 = locstr10.Loc2;

                if (IndexModel.screenmode == "normal")
                {
                    bodyelement = Alldoc2[0][locstr2].Htmlbody;
                    updatedate = Alldoc2[0][locstr2].Upddate;
                    author = Alldoc2[0][locstr2].Updauther;
                    attachfile = Alldoc2[0][locstr2].Attchfiles;
                }

                if (IndexModel.screenmode == "upload1")
                {
                    bodyelement = Alldoc2[0][locstr2].Htmlbody;
                    updatedate = Alldoc2[0][locstr2].Upddate;
                    author = Alldoc2[0][locstr2].Updauther;
                    attachfile = Alldoc2[0][locstr2].Attchfiles;
                }

                if (attachfile == "")
                {

                }
                else
                {
                    attachlist = attachfile.Split(",").ToList();
                }
                foreach (var formFile in Upload)
                {
                    if (formFile.Length > 0)
                    {
                        string folderName = "";
                        if (IndexModel.screenmode == "upload1")
                        {
                            var locstr15 = (LocItem)Loc.Items[IndexModel.selecttab - 1];
                            tabno = locstr15.ID;
                            folderName = (tabno).ToString() + "_" + Alldoc2[0][locstr2].Id + "/";
                        }
                        string folderfile = folderName + formFile.FileName;
                        var f = false;

                        foreach (var formFile1 in IndexModel.Attachfiles)
                        {
                            if (formFile1 == formFile.FileName)
                            {
                                f = true;
                                break;
                            }
                        }
                        if (f == false)
                        {
                            attachlist.Add(formFile.FileName);

                            using (var stream = formFile.OpenReadStream())
                            {
                                CloudBlockBlob blockBlob = container3.GetBlockBlobReference(folderfile);
                                await blockBlob.UploadFromStreamAsync(stream);
                            }
                        }
                        else
                        {
                            if (flag != "on")
                            {
                                using (var stream = formFile.OpenReadStream())
                                {
                                    CloudBlockBlob blockBlob = container3.GetBlockBlobReference(folderfile);
                                    await blockBlob.UploadFromStreamAsync(stream);
                                }
                            }
                        }
                    }
                }

                var count = 1;
                foreach (var l in attachlist)
                {
                    if (count != attachlist.Count)
                    {
                        attachfile2 = attachfile2 + l + ",";
                    }
                    else
                    {
                        attachfile2 = attachfile2 + l;
                    }
                    count += 1;
                }
                IndexModel.Attachfiles = attachfile2.Split(",");
                if (IndexModel.screenmode == "upload1")
                {
                    Alldoc2[0][locstr2].Attchfiles = attachfile2;
                    saveDoc(loc10);
                }
            }
            if (Request.Form["type"] == "uploadup2")
            {

                List<string> attachlist = new List<string>();
                var flag = Request.Form["upflag"].ToString();

                var locstr100 = (LocItem)LocGen2.Items[loc10];
                var locstr2 = locstr100.Loc1;
                var locstr3 = locstr100.Loc2;

                if (IndexModel.screenmode == "searched")
                {
                    bodyelement = srcAlldoc2[0][locstr3].Htmlbody;
                    updatedate = srcAlldoc2[0][locstr3].Upddate;
                    author = srcAlldoc2[0][locstr3].Updauther;
                    attachfile = srcAlldoc2[0][locstr3].Attchfiles;
                }
                if (IndexModel.screenmode == "upload2")
                {
                    bodyelement = srcAlldoc2[0][locstr3].Htmlbody;
                    updatedate = srcAlldoc2[0][locstr3].Upddate;
                    author = srcAlldoc2[0][locstr3].Updauther;
                    attachfile = srcAlldoc2[0][locstr3].Attchfiles;
                }

                if (attachfile == "")
                {

                }
                else
                {
                    attachlist = attachfile.Split(",").ToList();
                }
                foreach (var formFile in Upload)
                {
                    if (formFile.Length > 0)
                    {
                        string folderName = "";
                        if (IndexModel.screenmode == "upload2")
                        {
                            locstr1 = (LocItem)LocGen2.Items[loc10];
                            var locstr = locstr1.Loc2;
                            tabno = locstr1.ID;
                            var rerativeloc = srcAlldoc2[0][locstr].Loc;
                            folderName = (tabno).ToString() + "_" + rerativeloc + "/";
                        }

                        if (IndexModel.screenmode == "searched")
                        {
                            locstr1 = (LocItem)LocGen2.Items[loc10];
                            var locstr = locstr1.Loc2;
                            tabno = locstr1.ID;
                            var rerativeloc = srcAlldoc2[0][locstr].Loc;
                            folderName = (tabno).ToString() + "_" + rerativeloc + "/";
                        }

                        string folderfile = folderName + formFile.FileName;
                        var f = false;

                        foreach (var formFile1 in IndexModel.Attachfiles)
                        {
                            if (formFile1 == formFile.FileName)
                            {
                                f = true;
                                break;
                            }
                        }
                        if (f == false)
                        {
                            attachlist.Add(formFile.FileName);

                            using (var stream = formFile.OpenReadStream())
                            {
                                CloudBlockBlob blockBlob = container3.GetBlockBlobReference(folderfile);
                                await blockBlob.UploadFromStreamAsync(stream);
                            }
                        }
                        else
                        {
                            if (flag != "on")
                            {
                                using (var stream = formFile.OpenReadStream())
                                {
                                    CloudBlockBlob blockBlob = container3.GetBlockBlobReference(folderfile);
                                    await blockBlob.UploadFromStreamAsync(stream);
                                }
                            }
                        }
                    }
                }

                var count = 1;
                foreach (var l in attachlist)
                {
                    if (count != attachlist.Count)
                    {
                        attachfile2 = attachfile2 + l + ",";
                    }
                    else
                    {
                        attachfile2 = attachfile2 + l;
                    }
                    count += 1;
                }
                IndexModel.Attachfiles = attachfile2.Split(",");

                if (IndexModel.screenmode == "upload2")
                {
                    var locstr = locstr1.Loc2;
                    var rerativeloc = srcAlldoc2[0][locstr].Loc;
                    Alldoc2[0][Int32.Parse(rerativeloc)].Attchfiles = attachfile2;
                    srcAlldoc2[0][locstr].Attchfiles = attachfile2;
                    saveDoc2(loc10);
                    saveDoc(loc10);
                }
            }
            //screen mode別切り替え
            if (IndexModel.screenmode == "upload1")
            {
                if (Request.Form["evnt"] == "delete")
                {
                    var locstr10 = (LocItem)LocGen2.Items[loc10];
                    var locstr2 = locstr10.Loc1;
                    bodyelement = Alldoc2[0][locstr2].Htmlbody;
                    updatedate = Alldoc2[0][locstr2].Upddate;
                    author = Alldoc2[0][locstr2].Updauther;
                    attachfile = Alldoc2[0][locstr2].Attchfiles;

                    var filename = Request.Form["delete"];

                    List<string> attachlist2 = new List<string>();
                    attachlist2 = attachfile.Split(",").ToList();
                    attachlist2.Remove(filename);
                    var str = "";
                    var count = 1;
                    foreach (var l in attachlist2)
                    {
                        if (count != attachlist2.Count)
                        {
                            str = str + l + ",";
                        }
                        else
                        {
                            str = str + l;
                        }
                        count += 1;
                    }
                    IndexModel.Attachfiles = str.Split(",");

                    Alldoc2[0][locstr2].Attchfiles = str;
                    saveDoc(loc10);
                    tabno = locstr1.ID;
                    var folderName = (tabno).ToString() + "_" + Alldoc2[0][locstr2].Id;
                    var filepath = folderName + filename;
                    attachfile = Alldoc2[0][locstr2].Attchfiles;
                    var credential11 = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(IndexModel.accountName, IndexModel.accessKey);
                    var storageAccount11 = new CloudStorageAccount(credential11, true);
                    CloudBlobClient blobClient11 = storageAccount11.CreateCloudBlobClient();
                    CloudBlobContainer container11 = blobClient11.GetContainerReference(containerName_folder);
                    CloudBlockBlob blockBlob = container11.GetBlockBlobReference(filepath);
                    await blockBlob.DeleteIfExistsAsync();
                    //await Task.Delay(1000);
                }
                IndexModel.screenmode = "normal";
                await Task.Delay(500);
            }

            //screen mode別切り替え
            if (IndexModel.screenmode == "upload2")
            {
                if (Request.Form["evnt"] == "delete")
                {
                    var locstr10 = (LocItem)LocGen2.Items[loc10];
                    var locstr2 = locstr10.Loc1;
                    var locstr3 = locstr10.Loc2;
                    bodyelement = srcAlldoc2[0][locstr3].Htmlbody;
                    updatedate = srcAlldoc2[0][locstr3].Upddate;
                    author = srcAlldoc2[0][locstr3].Updauther;
                    attachfile = srcAlldoc2[0][locstr3].Attchfiles;

                    var filename = Request.Form["delete"];

                    List<string> attachlist2 = new List<string>();
                    attachlist2 = attachfile.Split(",").ToList();
                    attachlist2.Remove(filename);
                    var str = "";
                    var count = 1;
                    foreach (var l in attachlist2)
                    {
                        if (count != attachlist2.Count)
                        {
                            str = str + l + ",";
                        }
                        else
                        {
                            str = str + l;
                        }
                        count += 1;
                    }
                    IndexModel.Attachfiles = str.Split(",");

                    srcAlldoc2[0][locstr2].Attchfiles = str;
                    Alldoc2[0][Int32.Parse(srcAlldoc2[0][locstr3].Loc)].Attchfiles = str;
                    saveDoc(loc10);
                    saveDoc2(loc10);
                    tabno = locstr1.ID;

                    var folderName = (tabno).ToString() + "_" + Alldoc2[0][locstr3].Id;
                    var filepath = folderName + filename;
                    attachfile = srcAlldoc2[0][locstr3].Attchfiles;
                    var credential11 = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(IndexModel.accountName, IndexModel.accessKey);
                    var storageAccount11 = new CloudStorageAccount(credential11, true);
                    CloudBlobClient blobClient11 = storageAccount11.CreateCloudBlobClient();
                    CloudBlobContainer container11 = blobClient11.GetContainerReference(containerName_folder);
                    CloudBlockBlob blockBlob = container11.GetBlockBlobReference(filepath);
                    await blockBlob.DeleteIfExistsAsync();
                    //await Task.Delay(1000);
                }
            }

            if (IndexModel.screenmode == "normal")
            {
                var locstr10 = (LocItem)LocGen2.Items[loc10];
                var locstr2 = locstr10.Loc1;
                bodyelement = Alldoc2[0][locstr2].Htmlbody;
                updatedate = Alldoc2[0][locstr2].Upddate;
                author = Alldoc2[0][locstr2].Updauther;
                attachfile = Alldoc2[0][locstr2].Attchfiles;

                IndexModel.Attachfiles = attachfile2.Split(",");
                Alldoc2[0][locstr2].Attchfiles = attachfile2;
                saveDoc(loc10);
            }

            IndexModel.screenmode = "searched";
            await Task.Delay(500);
            //if (Request.Form["evnt"] == "uploadok1")
            //{
            //    IndexModel.screenmode = "normal";
            //    await Task.Delay(500);
            //}
            //if (Request.Form["evnt"] == "uploadok2")
            //{

            //}
        }
        public async void saveDoc(int loc10)
        {
            string csv_body1 = "";
            for (int i = 0; i <= Alldoc2[0].Count - 1; i++)
            {
                csv_body1 = csv_body1
                    + Alldoc2[0][i].Id.Replace(";", "<seprt1>") + "<spert2>"
                    + Alldoc2[0][i].Title.Replace(";", "<seprt1>") + "<spert2>"
                    + Alldoc2[0][i].Body.Replace(";", "<seprt1>") + "<spert2>"
                    + Alldoc2[0][i].Htmlbody.Replace(";", "<seprt1>") + "<spert2>"
                    + Alldoc2[0][i].Upddate.Replace(";", "<seprt1>") + "<spert2>"
                    + Alldoc2[0][i].Updauther.Replace(";", "<seprt1>") + "<spert2>"
                    + Alldoc2[0][i].Attchfiles.Replace(";", "<seprt1>") + "<spert2>"
                    + Alldoc2[0][i].Scroolloc.Replace(";", "<seprt1>") + "<spert2>";
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

            //// Retrieve reference to a blob named "filename"
            ////XMLシリアル化するオブジェクト
            //LocClass Loc = new LocClass();
            //Loc.Items = new System.Collections.ArrayList();
            //for (int i = 0; i <= LocGen.Items.Count - 1; i++)
            //{
            //    var locstr1 = (LocItem)LocGen.Items[i];
            //    var id = locstr1.ID;
            //    var locstr2 = locstr1.Loc1;
            //    var locstr3 = locstr1.Loc2;
            //    var locstr4 = locstr1.Status;
            //    if (locstr4 == "ACTIVE")
            //    {
            //        Loc.Items.Add(new LocItem(id, locstr2, locstr3, locstr4));
            //    }
            //}
            //var locstr10 = (LocItem)Loc.Items[loc10];
            //var tabno = locstr10.ID;
            var titlestr1 = (TitleItem)result_1.Items[loc10];
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

        }
        public async void saveDoc2(int loc10)
        {

            string csv_body1 = "";
            for (int i = 0; i <= srcAlldoc2[0].Count - 1; i++)
            {
                csv_body1 = csv_body1
                + srcAlldoc2[0][i].Id.Replace(";", "<seprt1>") + "<spert2>"
                + srcAlldoc2[0][i].Title.Replace(";", "<seprt1>") + "<spert2>"
                + srcAlldoc2[0][i].Body.Replace(";", "<seprt1>") + "<spert2>"
                + srcAlldoc2[0][i].Htmlbody.Replace(";", "<seprt1>") + "<spert2>"
                + srcAlldoc2[0][i].Upddate.Replace(";", "<seprt1>") + "<spert2>"
                + srcAlldoc2[0][i].Updauther.Replace(";", "<seprt1>") + "<spert2>"
                + srcAlldoc2[0][i].Attchfiles.Replace(";", "<seprt1>") + "<spert2>"
                + "0.0" + "<spert2>"
                + srcAlldoc2[0][i].Loc.Replace(";", "<seprt1>") + "<spert2>";
            }
            if (srcAlldoc2[0].Count != 0)
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

            // Retrieve reference to a blob named "filename"
            //XMLシリアル化するオブジェクト
            //LocClass Loc = new LocClass();
            //Loc.Items = new System.Collections.ArrayList();
            //for (int i = 0; i <= LocGen.Items.Count - 1; i++)
            //{
            //    var locstr1 = (LocItem)LocGen.Items[i];
            //    var id = locstr1.ID;
            //    var locstr2 = locstr1.Loc1;
            //    var locstr3 = locstr1.Loc2;
            //    var locstr4 = locstr1.Status;
            //    if (locstr4 == "ACTIVE")
            //    {
            //        Loc.Items.Add(new LocItem(id, locstr2, locstr3, locstr4));
            //    }
            //}

            var titlestr1 = (TitleItem)result_2.Items[loc10];
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
        }
    }
}

