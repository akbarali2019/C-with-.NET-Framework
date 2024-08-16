using DBLib;
using DBLib.Helper;
using DBLib.Record;
using DBLib.Record.Entities;
using DBLib.Setup;
using DBLib.Setup.Entities;
using DBLib.StateLogs;
using Firestore.Helper;
using Firestore.ProtoMessageMaps;
using Google.Cloud.Firestore;
using Google.Rpc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firestore.AutoSendProtos
{
    [FirestoreData]
    public class TOFFMap : ListenerProperty
    {
        //Firebase SendingMap

        //chimneyCode 001
        //Id HAF or FIV
        //type TOFF
        //message [0] DATE (empty)
        //        [1] number of off dates
        //        [2] 0000 (time)
        //        ...

        private readonly IRepoFactory<SetupRepository<Setting>> factorySetting;
        private readonly IRepoFactory<SendRecordRepository> factoryItem;
        private readonly IRepoFactory<HalfHourRecordRepository> factoryHafItem;
        private readonly IRepoFactory<LogRepository<RemoteCommandLog>> factoryLog;
        public TOFFMap(IRepoFactory<SetupRepository<Setting>> factorySetting, IRepoFactory<SendRecordRepository> factoryItem,
            IRepoFactory<HalfHourRecordRepository> factoryHafItem, IRepoFactory<LogRepository<RemoteCommandLog>> factoryLog) 
        {
            this .factorySetting = factorySetting;
            this .factoryItem = factoryItem;
            this.factoryHafItem = factoryHafItem;
            this .factoryLog = factoryLog;
        }
        public TOFFMap() { }
        
        public void AddGwOffDataToFirestoreAsync(DateTime from, DateTime to, FirestoreDb db, string mode)
        {
            List<DateTime> offDates = new();
            var curmode = mode;
            using var workOfLog = factoryLog.Create(DateTime.Now);
            using var workOfSetting = factorySetting.Create();
            var setting = workOfSetting.Repo.GetFirst();
            try
            {
                for (var date = from.Date; date <= to.Date; date = date.AddDays(1))
                {
                    var dbFile = DbFileManager.GetRootPath() + $"DAY_{to:yyyyMMdd}.db";
                    if (!File.Exists(dbFile)) continue;

                    List<DateTime> collectedOffData = new();
                    if (curmode == "ALL" || curmode == "FIV")
                    {
                        using var work = factoryItem.Create(date);
                        collectedOffData = work!.Repo.GetTurnOffDate(from, to);
                        if (collectedOffData.Any()) OffDataExist(collectedOffData, ref offDates, db, mode, date);
                        else workOfLog.Repo.AddNew($"[TOFF] <<{from:yyyy-MM-dd HH:mm} ~ {to:yyyy-MM-dd HH:mm}>> 데이터가 없습니다", "[TX] [TOFF]");
                    }

                    if (curmode == "ALL" || curmode == "HAF")
                    {
                        using var work = factoryHafItem.Create(date);
                        collectedOffData = work!.Repo.GetTurnOffDate(from, to);
                        if (collectedOffData.Any()) OffDataExist(collectedOffData, ref offDates, db, mode, date);
                        else workOfLog.Repo.AddNew($"[TOFF] <<{from:yyyy-MM-dd HH:mm} ~ {to:yyyy-MM-dd HH:mm}>> 데이터가 없습니다", "[TX] [TOFF]");
                    }
                }
                workOfLog.Repo.AddNew("<EOT>", "[TX] [TOFF]");
                workOfLog.Complete();
                

            }
            catch (Exception ex)
            {
                workOfLog.Repo.AddNew("<NAK>", "[RX] [TOFF] " + ex);
                workOfLog.Repo.AddNew("<EOT>", "[TX] [TOFF]");
                workOfLog.Complete();
            }
        }
        private void OffDataExist(List<DateTime> collectedOffData,  ref List<DateTime> offDates, FirestoreDb db, string mode, DateTime date)
        {
            using var workOfLog = factoryLog.Create(DateTime.Now);
            using var workOfSetting = factorySetting.Create();
            var setting = workOfSetting.Repo.GetFirst();

            offDates.AddRange(collectedOffData);

            var mappedOffData = MapTOFFData(collectedOffData, setting, mode);
            var collectionReference = GetReference(setting, db!);
            collectionReference.SetAsync(mappedOffData);

            workOfLog.Repo.AddNew($"[TOFF] {date:yyyyMMdd} [{string.Join("],[", collectedOffData.Select(ft => ft.ToString("HHmm")))}]", "[Tx] [TOFF]");
            workOfLog.Repo.AddNew("<ACK>", "[RX] [TOFF]");
            workOfLog.Complete();

            UpdateSentValues(offDates);
        }
        public static TOFFMap MapTOFFData(List<DateTime> collectedOffDates, Setting setting, string mode)
        {
            var messageMap = GetData(collectedOffDates, mode);
            return new TOFFMap
            {
                ChimneyCode = setting.ChimneyCode,
                //MessageDate = DateTime.SpecifyKind(DateTime.Now.AddHours(-9), DateTimeKind.Utc),
                MessageDate = DateTime.SpecifyKind(collectedOffDates.First().AddHours(-9), DateTimeKind.Utc),
                Id = mode,
                Message = messageMap,
                Type = "TOFF",

            };
        }
        private static List<string> GetData(List<DateTime> collectedOffDates, string mode)
        {
            var header = new List<string>()
                {
                    string.Format("{0,8}", collectedOffDates.First().ToString("yyyyMMdd")),
                    string.Format("{0,3}", collectedOffDates.Count),
                };
            foreach (var item in collectedOffDates)
            {
                header.Add(item.ToString("HHmm"));
            }
            return header;
        }

        public static DocumentReference GetReference(Setting setting, FirestoreDb db)
        {
            var knexusServiceKey = $"GC{setting.CompanyCode}B{setting.CompanyRegistrationCode}";
            var collectionReference = db
                .Collection("gateways").Document(knexusServiceKey)
                .Collection("chimneys").Document(setting.ChimneyCode)
                .Collection("offData").Document();

            return collectionReference;
        }


        // to test toff data tranformation we generated a new path "test-offData" 
        // inside "tests" collection of the "gw-tests" collection
        public static DocumentReference GetTestReference(Setting setting, FirestoreDb db)
        {
            var knexusServiceKey = $"GC{setting.CompanyCode}B{setting.CompanyRegistrationCode}";
            var collectionReference = db
                .Collection("gw-tests").Document(knexusServiceKey)
                .Collection("tests").Document(setting.ChimneyCode)
                .Collection("test-offData").Document();

            return collectionReference;
        }
        
        private void UpdateSentValues(List<DateTime> offDates)
        {
            foreach (var date in offDates)
            {
                if (date.Second == 0)
                {
                    using var work = factoryItem.Create(date);
                    work.Repo.UpdateFirestoreSentValue(date);
                    work.Complete();
                }

                if (date.Second == 1)
                {
                    using var work = factoryHafItem.Create(date);
                    work.Repo.UpdateFirestoreSentValue(date);
                    work.Complete();
                }
            }
        }        
    }
}
