using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBLib.Setup.Entities;
using DBLib.Setup;
using DBLib;
using Google.Cloud.Firestore;
using System.Diagnostics;
using Firestore.FirestoreHelper;
using System.Runtime.InteropServices;
using DBLib.StateLogs;

namespace Firestore.ProtoMessageMaps
{
    public class FTTIM
    {
        private readonly FirestoreNotifier _notifier;
        private readonly ISetDateTime _setter;
        private readonly IRepoFactory<LogRepository<RemoteCommandLog>> _factoryLog;
        public FTTIM(FirestoreNotifier notifier, ISetDateTime setter, IRepoFactory<LogRepository<RemoteCommandLog>> factoryLog)
        { 
            this._notifier = notifier;
            this._setter = setter;
            this._factoryLog = factoryLog;
        }
        public async void HandleFirestoreTTIM(FirestoreDb db, Setting setting)
        {
            using var logWork = _factoryLog.Create();
            logWork.Repo.AddNew($"[TTIM] []", "[TX] [TTIM]");
            logWork.Complete();
            try
            {
                // Get current time from Firestore server
                DateTime currentTime = await GetCurrentTimeFromFirestore(db, setting);
                Trace.WriteLine($"Current Sserver time: {currentTime.AddHours(9)}");
                _setter.SetDateTime( currentTime );
                logWork.Repo.AddNew($"[PTIM] [현재 KN-서버 시간: {currentTime.AddHours(9):yyyy-MM-dd HH:mm:ss}]", "[RX] [PTIM]");
                logWork.Complete();
                _notifier.NotifyPTIMEvent();
                Trace.WriteLine($"_notifier.NotifyPTIMEvent();");
            }
            catch (Exception ex)
            {
                logWork.Repo.AddNew($"[PTIM] [NAK]", "[RX] [PTIM]");
                logWork.Repo.AddNew($"[PTIM] [현재 KN-서버 시간 Exception Error: {ex.Message}]", "[RX] [PTIM]");
                logWork.Complete();
                Trace.WriteLine(ex.Message);
            }

        }

        private async Task<DateTime> GetCurrentTimeFromFirestore(FirestoreDb db, Setting setting)
        {
            var knexusServiceKey = $"GC{setting.CompanyCode}B{setting.CompanyRegistrationCode}";
            DocumentReference docRef = db.Collection("gateways").Document(knexusServiceKey);           

            // Update the document with server timestamp
            Dictionary<string, object> updates = new ()
            {
                { "timestamp", FieldValue.ServerTimestamp }
            };
            try
            {
                await docRef.SetAsync(updates);
            }
            catch (Exception ex)
            {
                using var logWork = _factoryLog.Create();
                logWork.Repo.AddNew($"[PTIM] [NAK]", "[RX] [PTIM]");
                logWork.Repo.AddNew($"[PTIM] [현재 KN-서버 시간 Exception Error: {ex.Message}]", "[RX] [PTIM]");
                logWork.Complete();
            }
            // Retrieve the document to get the server timestamp
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            DateTime serverTime = snapshot.GetValue<DateTime>("timestamp");

            return serverTime;
        }
    }

    public interface ISetDateTime
    {
        void SetDateTime(DateTime dateTime);
    }

    public class SetDateTime : ISetDateTime
    {
        private readonly IRepoFactory<SetupRepository<Setting>> factorySetting;

        public SetDateTime(IRepoFactory<SetupRepository<Setting>> factorySetting)
        {
            this.factorySetting = factorySetting;
        }

        void ISetDateTime.SetDateTime(DateTime dateTime)
        {
            using var work = factorySetting.Create();
            var setting = work.Repo.GetFirst();
            if (!setting.IsDebug)
            {
                var utc = dateTime.ToUniversalTime();
                if (utc.Minute % 5 == 0 && utc.Second == 0) utc = utc.AddSeconds(1);
                SetTime(utc);
            }
        }

        [DllImport("kernel32.dll")]
        static extern uint GetLastError();
        [DllImport("kernel32.dll", SetLastError = true)]
        private extern static void GetSystemTime(ref SYSTEMTIME systime);
        [DllImport("kernel32.dll", EntryPoint = "SetSystemTime", SetLastError = true)]
        private extern static bool SetSystemTime(ref SYSTEMTIME systime);

        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEMTIME
        {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;
        }

        private static void GetTime()
        {
            // Call the native GetSystemTime method
            // with the defined structure.
            SYSTEMTIME stime = new ();
            GetSystemTime(ref stime);
            Console.WriteLine(stime.ToString());
        }

        private static void SetTime(DateTime d)
        {
            GetTime();
            SYSTEMTIME systime = new ();
            try
            {
                systime.wYear = (short)d.Year;
                systime.wMonth = (short)d.Month;
                systime.wDayOfWeek = (short)d.DayOfWeek;
                systime.wDay = (short)d.Day;
                systime.wHour = (short)d.Hour;
                systime.wMinute = (short)d.Minute;
                systime.wSecond = (short)d.Second;
                systime.wMilliseconds = 100;

                Console.WriteLine(SetSystemTime(ref systime));
                Console.WriteLine(GetLastError());
                GetTime();
            }
            catch (Exception)
            {
                GetSystemTime(ref systime);
                SetSystemTime(ref systime);
            }

        }

    }
}
