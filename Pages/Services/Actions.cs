using Azure.Core;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace LifeNOTE_BIZ.Pages.Services
{
    public class Actions
    {
        public static void MoveRight()
        {
            var locstr11 = (ConvLocItem)IndexModel.ConvLoc.Items[IndexModel.selecttab - 1];
            var locstr21 = (ConvLocItem)IndexModel.ConvLoc.Items[IndexModel.selecttab];
            var loc1 = locstr11.mainid;
            var loc2 = locstr21.mainid;
            //検索前
            var titlestr1 = (TitleItem)IndexModel.result.Items[loc2];
            var num1 = titlestr1.ID;
            var msg1 = titlestr1.Title;
            var typ1 = titlestr1.Type;
            var col1 = titlestr1.Colour;
            var stus1 = titlestr1.Status;

            var titlestr2 = (TitleItem)IndexModel.result.Items[loc1];
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

            var locstr10 = (LocItem)IndexModel.LocGen.Items[loc2];
            var id1 = locstr10.ID;
            var loc10 = locstr10.Loc1;
            var loc20 = locstr10.Loc2;
            var stus10 = locstr10.Status;

            var locstr110 = (LocItem)IndexModel.LocGen.Items[loc1];
            var id11 = locstr110.ID;
            var loc11 = locstr110.Loc1;
            var loc21 = locstr110.Loc2;
            var stus11 = locstr110.Status;

            locstr110.ID = id1;
            locstr110.Loc1 = loc10;
            locstr110.Loc2 = loc20;
            locstr110.Status = stus10;

            locstr10.ID = id11;
            locstr10.Loc1 = loc11;
            locstr10.Loc2 = loc21;
            locstr10.Status = stus11;


            //検索後

            var titlestr4 = (TitleItem)IndexModel.result2.Items[loc2];
            var num3 = titlestr4.ID;
            var msg3 = titlestr4.Title;
            var typ3 = titlestr4.Type;
            var col3 = titlestr4.Colour;
            var stus3 = titlestr4.Status;

            var titlestr5 = (TitleItem)IndexModel.result2.Items[loc1];
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

            IndexModel.selecttab = IndexModel.selecttab + 1;
            IndexModel.activetabname = "tab3_" + IndexModel.selecttab.ToString();
            IndexModel.screenmode = "normal";
            IndexModel.saveTitle();
            IndexModel.saveAllLoc();
        }

        public static void SearchAction(String key,string fi,string groval)
        {
            var key2 = "ｷｰﾜｰﾄﾞ:" + key;
            IndexModel.srcAlldoc.Clear();
            
            var fi2 = fi.Split(",");
            IndexModel.field = fi2[0];
            
            var keyword = key.Replace("　", " ");
            var keywords = keyword.Split(" ");
            //タブループ
            IndexModel.srcAlldoc = new List<List<srcDocumentdata>>();
            //タブ分ループ
            for (int i = 0; i <= IndexModel.datasize - 1; i++)
            {

                //if (IndexModel.global == "false")
                //{
                if (IndexModel.field == "body")
                {
                    //ドキュメントループ
                    IndexModel.srcDocuments = new List<srcDocumentdata>();
                    for (int d = 0; d <= IndexModel.Alldoc[i].Count - 1; d++)
                    {
                        //キーワードループ
                        var counter = 0;
                        var html = IndexModel.Alldoc[i][d].Htmlbody;
                        var replace_string = "";

                        for (int k = 0; k <= keywords.Length - 1; k++)
                        {
                            var text = IndexModel.Alldoc[i][d].Body;
                            var pattern = keywords[k];
                            if (Regex.IsMatch(text, pattern))
                            {
                                counter += 1;
                                var colour = "";
                                if (k <= 9)
                                {
                                    colour = IndexModel.Hitcolour[k];
                                }
                                else
                                {
                                    colour = IndexModel.Hitcolour[k % 10];
                                }
                                replace_string = "<i><font color=\"" + colour + "\" size=\"6\">" + pattern + "</font></i>";
                                html = html.Replace(pattern, replace_string);
                            }
                        }

                        if (counter == keywords.Length)
                        {
                            IndexModel.srcDocument = new srcDocumentdata();
                            IndexModel.srcDocument.Id = d.ToString();
                            IndexModel.srcDocument.Title = IndexModel.Alldoc[i][d].Title;
                            IndexModel.srcDocument.Body = IndexModel.Alldoc[i][d].Body;
                            IndexModel.srcDocument.Htmlbody = html;
                            IndexModel.srcDocument.Upddate = IndexModel.Alldoc[i][d].Upddate;
                            IndexModel.srcDocument.Updauther = IndexModel.Alldoc[i][d].Updauther;
                            IndexModel.srcDocument.Attchfiles = IndexModel.Alldoc[i][d].Attchfiles;
                            IndexModel.srcDocument.Scroolloc = IndexModel.Alldoc[i][d].Scroolloc;
                            IndexModel.srcDocument.Loc = IndexModel.Alldoc[i][d].Id;
                            IndexModel.srcDocuments.Add(IndexModel.srcDocument);
                        }
                    }
                }
                else
                {
                    //ドキュメントループ
                    IndexModel.srcDocuments = new List<srcDocumentdata>();
                    for (int d = 0; d <= IndexModel.Alldoc[i].Count - 1; d++)
                    {
                        //キーワードループ
                        var counter = 0;
                        for (int k = 0; k <= keywords.Length - 1; k++)
                        {
                            var text = IndexModel.Alldoc[i][d].Title;
                            var pattern = keywords[k];
                            if (Regex.IsMatch(text, pattern))
                            {
                                counter += 1;
                            }
                        }
                        if (counter == keywords.Length)
                        {

                            IndexModel.srcDocument = new srcDocumentdata();
                            IndexModel.srcDocument.Id = d.ToString();
                            IndexModel.srcDocument.Title = IndexModel.Alldoc[i][d].Title;
                            IndexModel.srcDocument.Body = IndexModel.Alldoc[i][d].Body;
                            IndexModel.srcDocument.Htmlbody = IndexModel.Alldoc[i][d].Htmlbody;
                            IndexModel.srcDocument.Upddate = IndexModel.Alldoc[i][d].Upddate;
                            IndexModel.srcDocument.Updauther = IndexModel.Alldoc[i][d].Updauther;
                            IndexModel.srcDocument.Attchfiles = IndexModel.Alldoc[i][d].Attchfiles;
                            IndexModel.srcDocument.Scroolloc = IndexModel.Alldoc[i][d].Scroolloc;
                            IndexModel.srcDocument.Loc = IndexModel.Alldoc[i][d].Id;
                            IndexModel.srcDocuments.Add(IndexModel.srcDocument);
                            counter = 0;
                        }

                    }
                }
                IndexModel.srcAlldoc.Add(IndexModel.srcDocuments);
            }

            for (int i = 0; i <= IndexModel.LocGen.Items.Count - 1; i++)
            {
                IndexModel.saveLoc2((i + 1).ToString(), "0");
            }
            //abstruct normal searched
            var locstr1 = (LocItem)IndexModel.Loc.Items[IndexModel.selecttab - 1];

            var locstr3 = locstr1.Loc2;
            if (IndexModel.screenmode == "normal")
            {
                var tabno = IndexModel.locstr_l.ID;
                IndexModel.bodyelement = IndexModel.Alldoc[IndexModel.selecttab - 1][IndexModel.normal_row].Htmlbody;
                IndexModel.updatedate = IndexModel.Alldoc[IndexModel.selecttab - 1][IndexModel.normal_row].Upddate;
                IndexModel.author = IndexModel.Alldoc[IndexModel.selecttab - 1][IndexModel.normal_row].Updauther;
                IndexModel.attachfile = IndexModel.Alldoc[IndexModel.selecttab - 1][IndexModel.normal_row].Attchfiles;
                IndexModel.Attachfiles = IndexModel.attachfile.Split(",");
            }
            else
            {
                var tabno = IndexModel.locstr_l.ID;
                IndexModel.bodyelement = IndexModel.srcAlldoc[IndexModel.selecttab - 1][locstr3].Htmlbody;
                IndexModel.updatedate = IndexModel.srcAlldoc[IndexModel.selecttab - 1][locstr3].Upddate;
                IndexModel.author = IndexModel.srcAlldoc[IndexModel.selecttab - 1][locstr3].Updauther;
                IndexModel.attachfile = IndexModel.srcAlldoc[IndexModel.selecttab - 1][locstr3].Attchfiles;
                IndexModel.Attachfiles = IndexModel.attachfile.Split(",");
            }
            IndexModel.saveDoc2();
            IndexModel.screenmode = "searched";

        }
        public static async void AddTab(String addtab,
            String accountName,
            String accessKey,
            String containerName_file)
        {
            var diststr = addtab + ".csv";
            var diststr1 = addtab + "-S.csv";
            var chkflag = IndexModel.Chkdublication2(diststr);
            if (chkflag == true)
            {
                IndexModel.errormessage = "既存のタブ名と重複したタブ名には変更できません。";
            }
            if (chkflag == false)
            {


                var credential5 = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
                var storageAccount5 = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential5, true);
                ////blob
                CloudBlobClient blobClient5 = storageAccount5.CreateCloudBlobClient();

                ////// Retrieve reference to a previously created container.
                CloudBlobContainer container5 = blobClient5.GetContainerReference("settingdata");
                var sourcestr = "resoucetemp.csv";
                CloudBlockBlob source = container5.GetBlockBlobReference(sourcestr);
                CloudBlobContainer container6 = blobClient5.GetContainerReference(containerName_file);
                CloudBlockBlob target = container6.GetBlockBlobReference(diststr);
                CloudBlockBlob target1 = container6.GetBlockBlobReference(diststr1);
                await target.StartCopyAsync(source);
                await target1.StartCopyAsync(source);

                ////最終列の検出
                int[] addtabmax = new int[IndexModel.LocGen.Items.Count];
                for (int i = 0; i <= IndexModel.LocGen.Items.Count - 1; i++)
                {
                    var locstr1 = (LocItem)IndexModel.LocGen.Items[i];
                    var id = locstr1.ID;
                    addtabmax[i] = id;
                }
                var maxtab = addtabmax.Max() + 1;


                //色の追加
                // Retrieve reference to a blob named "filename"
                CloudBlockBlob blockBlob2 = container5.GetBlockBlobReference("TabColours.csv");

                using (var memoryStream2 = new MemoryStream())
                {
                    string TabColoursgen = "";
                    await blockBlob2.DownloadToStreamAsync(memoryStream2);
                    TabColoursgen = System.Text.Encoding.UTF8.GetString(memoryStream2.ToArray());
                    IndexModel.TabColoursGen = TabColoursgen.Split(",").ToList();
                }
                var newclour = "";
                if (maxtab <= IndexModel.TabColoursGen.Count)
                {
                    newclour = IndexModel.TabColoursGen[maxtab];
                }
                else
                {
                    var indxnum = 0;
                    indxnum = maxtab % IndexModel.TabColoursGen.Count;
                    newclour = IndexModel.TabColoursGen[indxnum];
                }

                ////ひな形を追加

                IndexModel.result.Items.Add(new TitleItem(maxtab, diststr, "DOC", "ACTIVE", newclour));
                IndexModel.result2.Items.Add(new TitleItem(maxtab, diststr1, "DOC", "ACTIVE", newclour));
                IndexModel.LocGen.Items.Add(new LocItem(maxtab, 0, 0, "ACTIVE"));
                IndexModel.saveTitle();
                IndexModel.saveAllLoc();
                await Task.Delay(500);
            }
            else
            {

            }

        }

        public static async void InactiveTab()
        {
            //リストに追加
            var diststr = IndexModel.titlenamelist[IndexModel.selecttab - 1] + ".csv";
            var diststr2 = IndexModel.titlenamelist[IndexModel.selecttab - 1] + "-S.csv";

            for (int i = 0; i <= IndexModel.result.Items.Count - 1; i++)
            {
                var titlestr1 = (TitleItem)IndexModel.result.Items[i];
                var titlestr2 = titlestr1.Title;
                var locstr2 = (LocItem)IndexModel.LocGen.Items[i];
                if (diststr == titlestr2)
                {
                    titlestr1.Status = "INACTIVE";
                    locstr2.Status = "INACTIVE";
                }
            }
            for (int i = 0; i <= IndexModel.result2.Items.Count - 1; i++)
            {
                var titlestr1 = (TitleItem)IndexModel.result2.Items[i];
                var titlestr2 = titlestr1.Title;
                var locstr2 = (LocItem)IndexModel.LocGen.Items[i];
                if (diststr2 == titlestr2)
                {
                    titlestr1.Status = "INACTIVE";
                    locstr2.Status = "INACTIVE";
                }
            }
            ////IndexModel.selecttab = IndexModel.selecttab - 1;
            var counter = 0;
            for (int i = 0; i <= IndexModel.result.Items.Count - 1; i++)
            {
                var titlestr1 = (TitleItem)IndexModel.result.Items[i];
                if (titlestr1.Status == "ACTIVE")
                {
                    counter = counter + 1;
                }
            }
            if (counter <= 0)
            {
                IndexModel.errormessage = "タブは最低限１つである必要があります。";
            }
            else
            {
                if (counter == IndexModel.selecttab - 1)
                {
                    IndexModel.selecttab = IndexModel.selecttab - 1;
                }
                IndexModel.saveTitle();
                IndexModel.saveAllLoc();
                await Task.Delay(500);
            }

        }

        public static  void OpenAttach()
        {
            //abstruct normal searched

            if (IndexModel.screenmode == "normal")
            {
                var tabno = IndexModel.locstr_l.ID;
                IndexModel.bodyelement = IndexModel.Alldoc[IndexModel.selecttab - 1][IndexModel.normal_row].Htmlbody;
                IndexModel.updatedate = IndexModel.Alldoc[IndexModel.selecttab - 1][IndexModel.normal_row].Upddate;
                IndexModel.author = IndexModel.Alldoc[IndexModel.selecttab - 1][IndexModel.normal_row].Updauther;
                IndexModel.attachfile = IndexModel.Alldoc[IndexModel.selecttab - 1][IndexModel.normal_row].Attchfiles;
                IndexModel.Attachfiles = IndexModel.attachfile.Split(",");
            }
            else
            {
                var tabno = IndexModel.locstr_l.ID;
                IndexModel.bodyelement = IndexModel.srcAlldoc[IndexModel.selecttab - 1][IndexModel.searched_row].Htmlbody;
                IndexModel.updatedate = IndexModel.srcAlldoc[IndexModel.selecttab - 1][IndexModel.searched_row].Upddate;
                IndexModel.author = IndexModel.srcAlldoc[IndexModel.selecttab - 1][IndexModel.searched_row].Updauther;
                IndexModel.attachfile = IndexModel.srcAlldoc[IndexModel.selecttab - 1][IndexModel.searched_row].Attchfiles;
                IndexModel.Attachfiles = IndexModel.attachfile.Split(",");
            }

        }
        public static void FileAttach()
        {
            IndexModel.screenmode = "attachset";
        }
        public static void TmpChancel()
        {
            IndexModel.screenmode = "normal";
        }
        public static async void ShowTab(string showtab)
        {
            //リストに追加
            
            //deleteno = Request.Form["deleteno"];
            var diststr = showtab + ".csv";
            var diststr2 = showtab + "-S.csv";

            for (int i = 0; i <= IndexModel.result2.Items.Count - 1; i++)
            {
                var titlestr1 = (TitleItem)IndexModel.result.Items[i];
                var titlestr3 = (TitleItem)IndexModel.result2.Items[i];
                var titlestr2 = titlestr1.Title;
                var locstr10 = (LocItem)IndexModel.LocGen.Items[i];

                if (diststr == titlestr2)
                {
                    titlestr1.Status = "ACTIVE";
                    titlestr3.Status = "ACTIVE";
                    locstr10.Status = "ACTIVE";
                }
            }
            IndexModel.saveTitle();
            IndexModel.saveAllLoc();
            await Task.Delay(500);
        }

        public static  void Setting()
        {
            IndexModel.screenmode = "setting";
        }
    }

   
}
