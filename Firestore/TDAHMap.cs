using DBLib;
using DBLib.Helper;
using DBLib.Record;
using DBLib.Record.Entities;
using DBLib.Setup;
using DBLib.Setup.Entities;
using DBLib.StateLogs;
using Firestore.Helper;
using Google.Cloud.Firestore;
using Google.Rpc;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Firestore.AutoSendProtos
{
    public class TDAHMap
    {
        private readonly IRepoFactory<SetupRepository<Setting>> factorySetting;
        private readonly IRepoFactory<SendRecordRepository> factoryItem;
        private readonly IRepoFactory<HalfHourRecordRepository> factoryHafItem;
        private readonly IRepoFactory<LogRepository<RemoteCommandLog>> factoryLog;
        private readonly Helpers helpers;

        private string ProtoStatus = "TDAH";
        public TDAHMap(IRepoFactory<SetupRepository<Setting>> factorySetting,
            IRepoFactory<SendRecordRepository> factoryItem, 
            IRepoFactory<HalfHourRecordRepository> factoryHafItem, 
            IRepoFactory<LogRepository<RemoteCommandLog>> factoryLog, Helpers helpers)
        {
            this.factorySetting = factorySetting;
            this.factoryItem = factoryItem;
            this.factoryHafItem = factoryHafItem;
            this.factoryLog = factoryLog;
            this.helpers = helpers;
        }


        /*        public void FivMinData(DateTime now, FirestoreDb db)
                {

                    var dbFile = helpers.GetDatabaseFilePath(now);
                    if (!File.Exists(dbFile)) return;

                    Trace.WriteLine($"FFFFFFFFFF FivMinData");
                    using var work = factoryItem.Create(now);
                    var fivMinItems = work!.Repo.GetRecordInTime(now);

                    var protoStatus = "TDAH";
                    var mappedFivMinData = MapDataForFirestore(fivMinItems, protoStatus);
                    var collectionReference = GetTDAHReference(db!, "fivMin");

                    var logMessage = $"[TDAH] {now:yyMMddHHmm} ";

                    foreach (var element in fivMinItems)
                    {
                        logMessage += $"{helpers.GetDataStringLog(element)}, ";
                    }
                    logMessage = logMessage.TrimEnd('\n', ' ');
                    using var workOfLog = factoryLog.Create(DateTime.Now);
                    workOfLog.Repo.AddNew(logMessage, "[TX] [TDAH FIV]");

                    try
                    {
                        collectionReference.SetAsync(mappedFivMinData);
                        workOfLog.Repo.AddNew("<ACK>", "[RX] [TDAH FIV]");
                        workOfLog.Repo.AddNew("<EOT>", "[TX] [TDAH FIV]");
                        workOfLog.Complete();

                        work.Repo.UpdateFirestoreSentValue(now);
                        work.Complete();
                    }
                    catch (Exception)
                    {
                        workOfLog.Repo.AddNew("<NAK>", "[RX] [TDAH FIV]");
                        workOfLog.Repo.AddNew("<EOT>", "[TX] [TDAH FIV]");
                        workOfLog.Complete();
                    }
                }

                public void HafMinData(DateTime now, FirestoreDb db)
                {

                    var dbFile = helpers.GetDatabaseFilePath(now);
                    if (!File.Exists(dbFile)) return;


                    Trace.WriteLine($"FFFFFFFFFF HafMinData");
                    using var work = factoryHafItem.Create(now);
                    var hafMinItems = work!.Repo.GetRecordInTime(now.AddSeconds(1));
                    var protoStatus = "TDAH";
                    var mappedHafMinData = MapDataForFirestore(hafMinItems, protoStatus);

                    using var workOfSetting = factorySetting.Create();
                    var setting = workOfSetting.Repo.GetFirst();
                    var collectionReference = GetTDAHReference(db!, "hafMin");

                    var logMessage = $"[TDAH] {hafMinItems[0].Date:yyMMddHHmm} ";
                    foreach (var element in hafMinItems)
                    {
                        logMessage += $"{helpers.GetDataStringLog(element)}, ";
                    }
                    logMessage = logMessage.TrimEnd('\n', ' ');
                    using var workOfLog = factoryLog.Create(DateTime.Now);
                    workOfLog.Repo.AddNew(logMessage, "[TX] [TDAH HAF]");

                    try
                    {
                        collectionReference.SetAsync(mappedHafMinData);
                        workOfLog.Repo.AddNew("<ACK>", "[RX] [TDAH HAF]");
                        workOfLog.Repo.AddNew("<EOT>", "[TX] [TDAH HAF]");
                        workOfLog.Complete();

                        work.Repo.UpdateFirestoreSentValue(now.AddSeconds(1));
                        work.Complete();
                    }
                    catch (Exception)
                    {
                        workOfLog.Repo.AddNew("<NAK>", "[RX] [TDAH HAF]");
                        workOfLog.Repo.AddNew("<EOT>", "[TX] [TDAH HAF]");
                        workOfLog.Complete();
                    }
                }*/

        public void TdahData(DateTime now, FirestoreDb db, string mode)
        {

            var dbFile = helpers.GetDatabaseFilePath(now);
            if (!File.Exists(dbFile)) return;

            Trace.WriteLine($"FFFFFFFFFF FivMinData");

            if (mode == "FIV")
            {
                using var work = factoryItem.Create(now);
                var fivMinItems = work!.Repo.GetRecordInTime(now);
                var mappedFivMinData = MapDataForFirestore(fivMinItems, ProtoStatus);
                var collectionReference = GetTDAHReference(db!, "fivMin");
                SetFivMinAndLog(now, fivMinItems, collectionReference, mappedFivMinData, mode);
            }

            else if (mode == "HAF")
            {
                using var work = factoryHafItem.Create(now);
                var hafMinItems = work!.Repo.GetRecordInTime(now.AddSeconds(1));
                var mappedHafMinData = MapDataForFirestore(hafMinItems, ProtoStatus);
                var collectionReference = GetTDAHReference(db!, "hafMin");
                SetHafMinAndLog(now, hafMinItems, collectionReference, mappedHafMinData, mode);
            }

            
        }

        public void SetFivMinAndLog(DateTime now, List<SendItem> fivMinItems, DocumentReference collectionReference, Dictionary<string, object> mappedFivMinData, string mode)
        {
            var logMessage = $"[TDAH] {now:yyMMddHHmm} ";

            foreach (var element in fivMinItems)
            {
                logMessage += $"{helpers.GetDataStringLog(element)}, ";
            }
            logMessage = logMessage.TrimEnd('\n', ' ');
            using var workOfLog = factoryLog.Create(DateTime.Now);
            workOfLog.Repo.AddNew(logMessage, "[TX] [TDAH FIV]");

            try
            {
                collectionReference.SetAsync(mappedFivMinData);

                workOfLog.Repo.AddNew("<ACK>", "[RX] [TDAH FIV]");
                workOfLog.Repo.AddNew("<EOT>", "[TX] [TDAH FIV]");
                workOfLog.Complete();

                UpdateSentValue(now, mode);

            }
            catch (Exception)
            {
                workOfLog.Repo.AddNew("<NAK>", "[RX] [TDAH FIV]");
                workOfLog.Repo.AddNew("<EOT>", "[TX] [TDAH FIV]");
                workOfLog.Complete();
            }
        }


        public void SetHafMinAndLog(DateTime now, List<HalfHourSend> hafMinItems, DocumentReference collectionReference, Dictionary<string, object> mappedHafMinData, string mode)
        {
            var logMessage = $"[TDAH] {now:yyMMddHHmm} ";
            foreach (var element in hafMinItems)
            {
                logMessage += $"{helpers.GetDataStringLog(element)}, ";
            }
            logMessage = logMessage.TrimEnd('\n', ' ');
            using var workOfLog = factoryLog.Create(DateTime.Now);
            workOfLog.Repo.AddNew(logMessage, "[TX] [TDAH HAF]");

            try
            {
                collectionReference.SetAsync(mappedHafMinData);
                workOfLog.Repo.AddNew("<ACK>", "[RX] [TDAH HAF]");
                workOfLog.Repo.AddNew("<EOT>", "[TX] [TDAH HAF]");
                workOfLog.Complete();

                UpdateSentValue(now, mode);
            }
            catch (Exception)
            {
                workOfLog.Repo.AddNew("<NAK>", "[RX] [TDAH HAF]");
                workOfLog.Repo.AddNew("<EOT>", "[TX] [TDAH HAF]");
                workOfLog.Complete();
            }
        }


        public void UpdateSentValue(DateTime now, string mode)
        {
            if (mode == "FIV")
            {
                using var work = factoryItem.Create(now);
                work.Repo.UpdateSentValue(now);
                work.Complete();
            }
            else if (mode == "HAF")
            {
                using var work = factoryHafItem.Create(now);
                work.Repo.UpdateSentValue(now);
                work.Complete();
            }
        }

        //Get Local Db file
        /*        private static string GetDatabaseFilePath(DateTime date)
                {
                    return DbFileManager.GetRootPath() + $"DAY_{date:yyyyMMdd}.db";
                }*/

        public  Dictionary<string, object> MapDataForFirestore<T>(IEnumerable<T> items, string protoStatus) where T : Item
        {
            Dictionary<string, object> docData = new();
            ArrayList itemsList = new();

            var fireItemDate = DateTime.MinValue;
            foreach (var item in items)
            {
                var fireItem = TDAHProperty.MapTDAH(item);

                itemsList.Add(fireItem);

                fireItemDate = DateTime.SpecifyKind(item.Date.AddHours(-9), DateTimeKind.Utc);
            }

            docData.Add("sensorsDate", fireItemDate);
            docData.Add("sensorsData", itemsList);
            docData.Add("protoStatus", protoStatus);
            return docData;
        }

        public  DocumentReference GetTDAHReference(FirestoreDb db, string path)
        {
            using var workOfSetting = factorySetting.Create();
            var setting = workOfSetting.Repo.GetFirst();

            var knexusServiceKey = $"GC{setting.CompanyCode}B{setting.CompanyRegistrationCode}";

            var collectionReference = db!
                .Collection("gateways").Document(knexusServiceKey)
                .Collection("chimneys").Document(setting.ChimneyCode)
                .Collection(path).Document();

            return collectionReference;
        }

    }

    [FirestoreData]
    public class TDAHProperty
    {
        [FirestoreProperty]
        public string? FacilityCode { get; set; }

        [FirestoreProperty]
        public string? ItemCode { get; set; }

        [FirestoreProperty]
        public int? ItemId { get; set; }

        [FirestoreProperty]
        public double RptValue { get; set; }

        [FirestoreProperty]
        public int RptState { get; set; }

        [FirestoreProperty]
        public int OprState { get; set; }

        [FirestoreProperty]
        public int PFState { get; set; }

        public static TDAHProperty MapTDAH<T>(T data) where T : Item => new()
        {
            FacilityCode = data.FacilityCode,
            ItemCode = data.ItemCode!,
            ItemId = data.ItemId!,
            RptValue = data.RptValue,
            RptState = data.RptState,
            OprState = data.OprState,
            PFState = int.Parse(data.SendFormat[5]),
        };
    }
}
