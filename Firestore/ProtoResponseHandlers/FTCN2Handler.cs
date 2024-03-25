using DBLib.Setup.Entities;
using DBLib.Setup;
using DBLib.StateLogs;
using DBLib;
using Firestore.Firebase;
using Firestore.Helper;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Firestore.ProtoMessageMaps;

namespace Firestore.ProtoResponseHandlers
{
    public class FTCN2Handler
    {
        private readonly FirestoreDb? _db;
        private const string DefaultCompanyCode = "1111111";

        private readonly IRepoFactory<LogRepository<RemoteCommandLog>> _factoryLog;
        private readonly IRepoFactory<SetupRepository<Setting>> _factorySetting;
        private readonly IRepoFactory<SetupRepository<Target>> _factoryTarget;
        private readonly IRepoFactory<ItemDetailRepository> _factoryItemDetail;
        private readonly IRepoFactory<SetupRepository<Password>> _factoryPassword;       
        public FTCN2Handler(IRepoFactory<LogRepository<RemoteCommandLog>> factoryLog,
            IRepoFactory<SetupRepository<Setting>> factorySetting,
            IRepoFactory<SetupRepository<Target>> factoryTarget,
            IRepoFactory<ItemDetailRepository> factoryItemDetail,
            IRepoFactory<SetupRepository<Password>> factoryPassword)
        {
            _factoryLog = factoryLog;
            _factorySetting = factorySetting;
            _factoryTarget = factoryTarget;
            _factoryItemDetail = factoryItemDetail;
            _factoryPassword = factoryPassword;

            _db = FirestoreConfig.InitializeFirestoreDb();
        }

        public async Task HandleFTCN2(string docId)
        {
            using var workOfSetting = _factorySetting.Create();
            var setting = workOfSetting.Repo.GetFirst();

            if (setting.CompanyCode == DefaultCompanyCode || setting.IsConnFirebase == false) return;

            Trace.WriteLine("=====================> Task HandleTCN2...");

            using var workOfLog = _factoryLog.Create(DateTime.Now);
            using var targetWork = _factoryTarget.Create();
            var target = targetWork.Repo.GetFirst();

            using var itemWork = _factoryItemDetail.Create();
            var itemDetails = itemWork.Repo.GetAll();

            using var workPassword = _factoryPassword.Create();
            var password = workPassword.Repo.GetFirst();

            try
            {
                var collectionReference = FirestoreReferenceProvider.GetSendReference(setting, _db!);
                var tcn2 = FTCN2.MapFTCN2Data(setting, target, itemDetails, password, docId);
                var data = new Dictionary<string, object> { { "TCN2", tcn2 } };

                Trace.WriteLine("=====================> Task HandleTCN2  SetAsync...");
                await collectionReference.SetAsync(data);
                Trace.WriteLine("=====================> Task HandleTCN2  SetAsync Finished");

                workOfLog.Repo.AddNew($"FTCN2 성공되었습니다", "[Tx] FTCN2");
                workOfLog.Complete();
            }
            catch (Exception ex)
            {
                workOfLog.Repo.AddNew($"FTCN2 실패되었습니다 {ex}", "[Tx] FTCN2");
                workOfLog.Complete();
            }
        }
    }
}

