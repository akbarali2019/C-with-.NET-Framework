using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBLib;
using DBLib.Helper;
using DBLib.Record;
using DBLib.Record.Entities;
using DBLib.Setup;
using DBLib.Setup.Entities;
using DBLib.StateLogs;
using Google.Cloud.Firestore;

namespace Firestore.Firebase
{
    public class FirestoreRepository
    {
        private const string DefaultCompanyCode = "1111111";
        public FirestoreDb? db;

        private readonly IRepoFactory<SetupRepository<Setting>> factorySetting;
        private readonly IRepoFactory<SendRecordRepository> factoryItem;
        private readonly IRepoFactory<LogRepository<RemoteCommandLog>> factoryLog;
        private readonly IRepoFactory<HalfHourRecordRepository> factoryHafItem;
        private readonly IRepoFactory<DayEndRecordRepository> factoryDayEnd;

        public FirestoreRepository(
            IRepoFactory<SetupRepository<Setting>> factorySetting, 
            IRepoFactory<SendRecordRepository> factoryItem, 
            IRepoFactory<LogRepository<RemoteCommandLog>> factoryLog,
            IRepoFactory<HalfHourRecordRepository> factoryHafItem,
            IRepoFactory<DayEndRecordRepository> factoryDayEnd) 
        {
            this.factorySetting = factorySetting;
            this.factoryItem = factoryItem;
            this.factoryLog = factoryLog;
            this.factoryHafItem = factoryHafItem;
            this.factoryDayEnd = factoryDayEnd;

            InitializeFirestoreDb();
        }


        public async void AddFivMinDataToFirestoreAsync(DateTime now)
        {
            var dbFile = DbFileManager.GetRootPath() + $"DAY_{now:yyyyMMdd}.db";
            if (!File.Exists(dbFile)) return;

            if (IsFirebaseDisabled() == true) return;

            using var workOfLog = factoryLog.Create(DateTime.Now);

            try
            {
                using var work = factoryItem.Create(now);
                var fivMinItems = work!.Repo.GetLastRecord();
                var mappedFivMinData = FirestoreFivMinDataMap.MapDataForFirestore(fivMinItems);

                using var workOfSetting = factorySetting.Create();
                var setting = workOfSetting.Repo.GetFirst();
                var collectionReference = FirestoreFivMinDataMap.GetFirestoreDocumentReference(setting, db!);

                await collectionReference.SetAsync(mappedFivMinData);

                workOfLog.Repo.AddNew("5분 데이터 전송", "[Tx] 5MIND");
                workOfLog.Complete();
            }
            catch (Exception ex)
            {
                workOfLog.Repo.AddNew($"5분 데이터 실패한. Error: {ex.Message}", "[Tx] 5MIND");
                workOfLog.Complete();
                return;
            }
        }



        public async void AddHafMinDataToFirestoreAsync(DateTime now)
        {
            var dbFile = DbFileManager.GetRootPath() + $"DAY_{now:yyyyMMdd}.db";
            if (!File.Exists(dbFile)) return;

            if (IsFirebaseDisabled() == true) return;

            using var workOfLog = factoryLog.Create(DateTime.Now);

            try
            {
                using var work = factoryHafItem.Create(now);
                var hafMinItems = work!.Repo.GetLastRecord();
                var mappedHafMinData = FirestoreHafMinDataMap.MapDataForFirestore(hafMinItems);

                using var workOfSetting = factorySetting.Create();
                var setting = workOfSetting.Repo.GetFirst();
                var collectionReference = FirestoreHafMinDataMap.GetFirestoreDocumentReference(setting, db!);

                await collectionReference.SetAsync(mappedHafMinData);

                workOfLog.Repo.AddNew("30분 데이터 전송", "[Tx] 30MIND");
                workOfLog.Complete();
            }
            catch (Exception ex)
            {
                workOfLog.Repo.AddNew($"30분 데이터 실패한. Error: {ex.Message}", "[Tx] 30MIND");
                workOfLog.Complete();
                return;
            }
        }



        public async void AddEndDayRecordToFirestoreAsync(DateTime now)
        {
            var dbFile = DbFileManager.GetRootPath() + $"DAY_{now:yyyyMMdd}.db";
            if (!File.Exists(dbFile)) return;

            if (IsFirebaseDisabled() == true) return;

            using var workOfLog = factoryLog.Create(DateTime.Now);

            try
            {
                using var work = factoryDayEnd.Create(now);
                var endDayRecord = work!.Repo.GetLastRecord();
                var mappedEndDayRecord = FirestoreEndDayRecordMap.MapDataForFirestore(endDayRecord);

                using var workOfSetting = factorySetting.Create();
                var setting = workOfSetting.Repo.GetFirst();
                var collectionReference = FirestoreEndDayRecordMap.GetFirestoreDocumentReference(setting, db!);

                await collectionReference.SetAsync(mappedEndDayRecord);

                workOfLog.Repo.AddNew("EndDayRecord 데이터 전송", "[Tx] EndDayRecord");
                workOfLog.Complete();
            }
            catch (Exception ex)
            {
                workOfLog.Repo.AddNew($"EndDayRecord 데이터 실패한. Error: {ex.Message}", "[Tx] EndDayRecord");
                workOfLog.Complete();
                return;
            }
        }



        private void InitializeFirestoreDb()
        {
            var filePath = GetFirestoreCredentialsFilePath();
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filePath);
            db = FirestoreDb.Create("iot-kefa");
        }

        private static string GetFirestoreCredentialsFilePath()
        {
            var dirPath = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.GetFullPath(Path.Combine(dirPath, "iot-kefa.json"));
            return filePath;
        }

        private bool IsFirebaseDisabled()
        {
            var defaultCompnayCode = DefaultCompanyCode;
            using var workOfSetting = factorySetting.Create();
            var setting = workOfSetting.Repo.GetFirst();

            if (setting.CompanyCode == defaultCompnayCode || setting.IsConnFirebase == false)
            {
                return true;
            }
            return false;
        }
    }
}


