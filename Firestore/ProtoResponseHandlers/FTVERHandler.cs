using DBLib.Setup.Entities;
using DBLib.Setup;
using DBLib.StateLogs;
using DBLib;
using Firestore.Helper;
using Firestore.ProtoMessageMaps;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firestore.ProtoResponseHandlers
{
    public class FTVERHandler
    {
        private readonly FirestoreDb? _db;
        private const string DefaultCompanyCode = "1111111";

        private readonly IRepoFactory<LogRepository<RemoteCommandLog>> _factoryLog;
        private readonly IRepoFactory<SetupRepository<Setting>> _factorySetting;
        private readonly IRepoFactory<SetupRepository<Target>> _factoryTarget;
        private readonly IRepoFactory<ItemDetailRepository> _factoryItemDetail;
        public FTVERHandler(IRepoFactory<LogRepository<RemoteCommandLog>> factoryLog,
            IRepoFactory<SetupRepository<Setting>> factorySetting,
            IRepoFactory<SetupRepository<Target>> factoryTarget,
            IRepoFactory<ItemDetailRepository> factoryItemDetail)
        {
            _factoryLog = factoryLog;
            _factorySetting = factorySetting;
            _factoryTarget = factoryTarget;
            _factoryItemDetail = factoryItemDetail;

            _db = FirestoreConfig.InitializeFirestoreDb();
        }

        public async Task HandleFTVER(string docId)
        {
            using var workOfSetting = _factorySetting.Create();
            var setting = workOfSetting.Repo.GetFirst();

            if (setting.CompanyCode == DefaultCompanyCode || setting.IsConnFirebase == false) return;

            Trace.WriteLine("=====================> Task HandleTVER...");

            using var workOfLog = _factoryLog.Create(DateTime.Now);
            using var targetWork = _factoryTarget.Create();
            var target = targetWork.Repo.GetFirst();

            using var itemWork = _factoryItemDetail.Create();
            var itemDetails = itemWork.Repo.GetAll();

            try
            {
                var collectionReference = FirestoreReferenceProvider.GetSendReference(setting, _db!);
                var tver = FTVER.MapFTVERData(setting, target, itemDetails, docId);
                var data = new Dictionary<string, object> { { "TVER", tver } };

                Trace.WriteLine("=====================> Task HandleTVER  SetAsync...");
                await collectionReference.SetAsync(data);
                Trace.WriteLine("=====================> Task HandleTVER  SetAsync Finished");

                workOfLog.Repo.AddNew($"FTVER 성공되었습니다", "[Tx] FTVER");
                workOfLog.Complete();
            }
            catch (Exception ex)
            {
                workOfLog.Repo.AddNew($"FTVER 실패되었습니다 {ex}", "[Tx] FTVER");
                workOfLog.Complete();
            }
        }
    }
}
