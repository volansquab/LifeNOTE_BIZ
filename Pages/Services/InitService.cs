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
using Microsoft.Graph.ExternalConnectors;
using System.Net.Mail;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.Metrics;

namespace LifeNOTE_BIZ.Pages.Services
{
    public class Init
    {

        public static async void InitService(bool isNewUser, string accountName, string accessKey, string containerName_setting,string diststr_loclist,string containerName_file,string containerName_folder,string diststr_new,string diststr_new_s, string diststr_new_m, string diststr_new_m_s, string diststr_files, string diststr_files_s)
        {
            if (isNewUser)
            {
                //blob
                var credential50 = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
                var storageAccount50 = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential50, true);
                CloudBlobClient client50 = storageAccount50.CreateCloudBlobClient();
                CloudBlobContainer container50 = client50.GetContainerReference(containerName_setting);
                bool blobExists = await container50.GetBlockBlobReference(diststr_loclist).ExistsAsync();
                //errormessage = blobExists.ToString();
                if (blobExists == false)
                {
                    var accessKey2 = "DefaultEndpointsProtocol=https;AccountName=lifenote3;AccountKey=09mFQDrMj5fYMyEcoZoINUoHcZUJzY4T8ZYLOQ0pINNKa9wuj4Yzj4RhWI9z9ek8u4ynL2x9ONbg+AStbmQeOQ==;EndpointSuffix=core.windows.net";
                    var blobServiceClient = new BlobServiceClient(accessKey2);
                    //get a BlobContainerClient
                    var container10 = blobServiceClient.GetBlobContainerClient(containerName_file);
                    container10.CreateIfNotExists(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
                    var container20 = blobServiceClient.GetBlobContainerClient(containerName_folder);
                    container20.CreateIfNotExists(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
                    var container30 = blobServiceClient.GetBlobContainerClient(containerName_setting);
                    container30.CreateIfNotExists(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);


                    var credential5 = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
                    var storageAccount5 = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credential5, true);
                    ////blob
                    CloudBlobClient blobClient5 = storageAccount5.CreateCloudBlobClient();

                    ////// Retrieve reference to a previously created container.
                    CloudBlobContainer container5 = blobClient5.GetContainerReference("settingdata");

                    CloudBlockBlob source_res = container5.GetBlockBlobReference("resoucetemp.csv");
                    CloudBlockBlob source_res_2 = container5.GetBlockBlobReference("resoucetemp2.csv");
                    CloudBlobContainer container6 = blobClient5.GetContainerReference(containerName_file);
                    CloudBlockBlob target_new = container6.GetBlockBlobReference(diststr_new);
                    CloudBlockBlob target_new_s = container6.GetBlockBlobReference(diststr_new_s);
                    CloudBlockBlob target_new_m = container6.GetBlockBlobReference(diststr_new_m);
                    CloudBlockBlob target_new_m_s = container6.GetBlockBlobReference(diststr_new_m_s);
                    var ts1 = target_new.StartCopyAsync(source_res);
                    var ts2 = target_new_s.StartCopyAsync(source_res);
                    var ts3 = target_new_m.StartCopyAsync(source_res_2);
                    var ts4 = target_new_m_s.StartCopyAsync(source_res_2);

                    await Task.WhenAll(ts1, ts2, ts3, ts4);

                    CloudBlockBlob source_files = container5.GetBlockBlobReference("FILES_RES.xml");
                    CloudBlobContainer container_files = blobClient5.GetContainerReference(containerName_setting);
                    CloudBlockBlob target_files = container_files.GetBlockBlobReference(diststr_files);
                    var ts5 = target_files.StartCopyAsync(source_files);


                    CloudBlockBlob source_files_s = container5.GetBlockBlobReference("S_FILES_RES.xml");
                    CloudBlobContainer container_files_s = blobClient5.GetContainerReference(containerName_setting);
                    CloudBlockBlob target_files_s = container_files_s.GetBlockBlobReference(diststr_files_s);
                    var ts6 = target_files_s.StartCopyAsync(source_files_s);
                    await Task.Delay(500);

                    CloudBlockBlob source_files_loclist = container5.GetBlockBlobReference("Loclist_RES.xml");
                    CloudBlobContainer container_files_loclist = blobClient5.GetContainerReference(containerName_setting);
                    CloudBlockBlob target_files_loclist = container_files_loclist.GetBlockBlobReference(diststr_loclist);
                    var ts7 = target_files_loclist.StartCopyAsync(source_files_loclist);

                    await Task.WhenAll(ts1, ts2, ts3, ts4, ts5, ts6, ts7);

                    await Task.Delay(500);

                    //メールアドレスUIDNAME記録
                    //DB本体のテキストファイル読み込み：検索後
                    // Retrieve reference to a blob named "filename"
                    
                }
            }
        }
        public static void Settab(TitleClass result, List<string> titlenamelist, List<string> inacttitlenamelist)
        {
            var count3 = 0;
            for (int i = 0; i <= result.Items.Count - 1; i++)
            {
                var titlestr1 = (TitleItem)result.Items[i];
                var titlestr2 = titlestr1.Title;
                var titlestr3 = titlestr1.Status;

                var titlename = titlestr2.Replace(".csv", "");

                if (titlestr3 == "ACTIVE")
                {
                    titlenamelist.Add(titlename);

                    count3 += 1;
                }
                else
                {
                    inacttitlenamelist.Add(titlename);
                }
            }
            //読み込んだファイルの数を格納
            IndexModel.datasize = count3;

        }
        public static void AddactiveTab()
        {
            IndexModel.title = IndexModel.Alldoc[IndexModel.selecttab - 1][IndexModel.locstr].Title;
            IndexModel.bodyelement = IndexModel.Alldoc[IndexModel.selecttab - 1][IndexModel.locstr].Htmlbody;
            IndexModel.updatedate = IndexModel.Alldoc[IndexModel.selecttab - 1][IndexModel.locstr].Upddate;
            IndexModel.author = IndexModel.Alldoc[IndexModel.selecttab - 1][IndexModel.locstr].Updauther;
            IndexModel.attachfile = IndexModel.Alldoc[IndexModel.selecttab - 1][IndexModel.locstr].Attchfiles;
            IndexModel.scrollloc = IndexModel.Alldoc[IndexModel.selecttab - 1][IndexModel.locstr].Scroolloc;
        }

        public static void CreateConv(LocClass Loc, ConvLoc ConvLoc)
        {
            for (int i = 0; i <= IndexModel.LocGen.Items.Count - 1; i++)
            {
                var locstr150 = (LocItem)IndexModel.LocGen.Items[i];
                var id = locstr150.ID;
                var locstr2 = locstr150.Loc1;
                var locstr3 = locstr150.Loc2;
                var locstr4 = locstr150.Status;
                if (locstr4 == "ACTIVE")
                {
                    Loc.Items.Add(new LocItem(id, locstr2, locstr3, locstr4));
                    ConvLoc.Items.Add(new ConvLocItem(i, IndexModel.count2));
                    IndexModel.count2 = IndexModel.count2 + 1;
                }
            }
        }
        public static async void StateDivider(LocClass Loc, ConvLoc ConvLoc)
        {
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
        }
    }
}
