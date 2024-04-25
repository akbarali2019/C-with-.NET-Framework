using DBLib;
using DBLib.Helper;
using DBLib.Record;
using DBLib.Record.Entities;
using DBLib.Setup;
using DBLib.Setup.Entities;
using DBLib.StateLogs;
using Knexus.Firebase;
using Knexus.Helper;
using Knexus.Pages;
using Microsoft.Extensions.DependencyInjection;
using PCBInput;
using PCBInput.DataProvider;
using PCBInput.Models;
using Protocol;
using Protocol.GWProtocol;
using Protocol.Helper;
using Protocol.Notifier;
using Protocol.ProtocolHandler;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Firestore;
using Firestore.Firebase;
using Firestore.ProtoResponseHandlers;
using Firestore.AutoSendProtos;
using Firestore.FirestoreHelper;

namespace Knexus.Configuration
{
    public class SystemDivider
    {
        public static bool IsDebug
        {
#if DEBUG
            get { return true; }
#else
                get { return false; }
#endif
        }

        readonly IServiceProvider Service;
        public SystemDivider(IServiceProvider service)
        {
            Service = service;
        }

        public void InitListenersGreenLink()
        {

            var GLnotifier = Service.GetService<GLServerNotifier>();
            GLnotifier!.PTIMEvent += async delegate (object? sender, EventArgs e)
            {
                Trace.WriteLine("TTTTTTTTTTTTTTT Lnotifier!.PTIMEvent");
                await Task.Run(async () =>
                {
                    await OffDataGeneration(DateTime.Now);
                    //Delete30DayData();
                    DeleteOneYearData();
                });
            };

            var TimeCaller = Service.GetService<TimeScheduleEvent>();
            TimeCaller!.CheckForOffDataElapsed += async delegate (object? sender, EventArgs e)
            {
                await OffDataGeneration(DateTime.Now);
                //Delete30DayData();
                DeleteOneYearData();
            };

            TimeCaller!.FiveSecondElapsed += delegate (object? sender, TimeScheduleEventArgs e)
            {
                var SecondData = Service.GetService<TimedTask<RawInput, Item>>();
                SecondData!.Execute(e.DateTime, "[FiveSecondElapsed]");
            };

            TimeCaller!.FiveMinuteElapsed += async delegate (object? sender, TimeScheduleEventArgs e)
            {
                var MinutData = Service.GetService<TimedTask<Item, SendItem>>();
                await MinutData!.Executer(e.DateTime, "[FiveMinuteElapsed]");
            };

            TimeCaller!.FiveMinuteSendElapsed += delegate (object? sender, TimeScheduleEventArgs e)
            {
                var tdat = Service.GetService<GWResponse<TDAH>>();
                tdat!.Proto.SetTargetTime(e.DateTime.AddMinutes(-5));
                tdat!.Proto.Mode = "FIV";
                tdat!.StartProtocol();
            };


            TimeCaller!.FiveMinuteSendExeptionElapsed += delegate (object? sender, TimeScheduleEventArgs e)
            {
                var targetTime = e.DateTime;

                var tnoh = Service.GetService<GWRangeResponse<TNOH>>();
                tnoh!.StartProtocol(targetTime, targetTime);
                for (var date = targetTime; date < targetTime.AddMinutes(30); date = date.AddMinutes(5))
                {
                    var tdat = Service.GetService<GWResponse<TDAH>>();
                    tdat!.Proto.SetTargetTime(date);
                    tdat!.Proto.Mode = "FIV";
                    tdat!.StartProtocol();
                }
            };

            TimeCaller!.HalfHourElapsed += async delegate (object? sender, TimeScheduleEventArgs e)
            {
                var HalfHourData = Service.GetService<TimedTask<IGrouping<int, SendItem>, HalfHourSend>>();
                await HalfHourData!.Executer(e.DateTime, "[HalfHourElapsed]");
            };

            TimeCaller!.HalfHourSendElapsed += delegate (object? sender, TimeScheduleEventArgs e)
            {
                Trace.WriteLine($"##### Half Hour data send");
                var tdat = Service.GetService<GWResponse<TDAH>>();
                tdat!.Proto.SetTargetTime(e.DateTime.AddMinutes(-30));
                tdat!.Proto.Mode = "HAF";
                tdat!.StartProtocol();
            };


            TimeCaller!.DayElapsed += async delegate (object? sender, TimeScheduleEventArgs e)
            {
                Trace.WriteLine("FFFFFFFFFFFF DayElapsed Data Send ELAPSED");
                await DayEndDataGenerate(e.DateTime);
                try
                {
                    Service.GetService<TTIM>()!.StartProtocol();
                }
                catch { }
                //Delete30DayData();
                DeleteOneYearData();
            };

            TimeCaller!.UnsendTimeElapsed += delegate (object? sender, TimeScheduleEventArgs e)
            {
                var tfdt = Service.GetService<GWRangeResponse<TFDH>>();
                tfdt!.StartProtocol(e.DateTime.AddDays(-3), e.DateTime);
            };


            TimeCaller!.TimeCorrectionElapsed += delegate (object? sender, TimeScheduleEventArgs e)
            {
                try
                {
                    Service.GetService<TTIM>()!.StartProtocol();
                }
                catch { }
            };
        }

        public void InitListenersGreenLinkAndKnexus()
        {
            KnexusListenerWithGreenLink();

            var GLnotifier = Service.GetService<GLServerNotifier>();
            GLnotifier!.PTIMEvent += async delegate (object? sender, EventArgs e)
            {
                Trace.WriteLine("TTTTTTTTTTTTTTT Lnotifier!.PTIMEvent");
                await Task.Run(async () =>
                {
                    await OffDataGeneration(DateTime.Now);
                    //Delete30DayData();
                    DeleteOneYearData();
                });
            };

            var TimeCaller = Service.GetService<TimeScheduleEvent>();
            TimeCaller!.CheckForOffDataElapsed += async delegate (object? sender, EventArgs e)
            {
                await OffDataGeneration(DateTime.Now);
                //Delete30DayData();
                DeleteOneYearData();
            };

            TimeCaller!.FiveSecondElapsed += delegate (object? sender, TimeScheduleEventArgs e)
            {
                var SecondData = Service.GetService<TimedTask<RawInput, Item>>();
                SecondData!.Execute(e.DateTime, "[FiveSecondElapsed]");
            };

            TimeCaller!.FiveMinuteElapsed += async delegate (object? sender, TimeScheduleEventArgs e)
            {
                var MinutData = Service.GetService<TimedTask<Item, SendItem>>();
                await MinutData!.Executer(e.DateTime, "[FiveMinuteElapsed]");
            };

            TimeCaller!.FiveMinuteSendElapsed += delegate (object? sender, TimeScheduleEventArgs e)
            {

                Task task = Task.Run(() =>
                {
                    Trace.WriteLine("FFFFFFFFFFFF FivMin Data Send ELAPSED");
                    var fireBase = Service.GetService<FirestoreRepository>();
                    var mode = "FIV";
                    fireBase!.AddTDAH(e.DateTime.AddMinutes(-5), mode);
                });

                var tdat = Service.GetService<GWResponse<TDAH>>();
                tdat!.Proto.SetTargetTime(e.DateTime.AddMinutes(-5));
                tdat!.Proto.Mode = "FIV";
                tdat!.StartProtocol();
            };


            TimeCaller!.FiveMinuteSendExeptionElapsed += delegate (object? sender, TimeScheduleEventArgs e)
            {
                var targetTime = e.DateTime;

                Task task = Task.Run(() =>
                {
                    Trace.WriteLine("EEEEEEEEEEEEEEE ExceptionDataChecking Started");
                    var fireBase = Service.GetService<FirestoreRepository>();
                    fireBase!.AddExceptionDataToFirestore(targetTime, targetTime);
                    var mode = "FIV";
                    for (var date = targetTime; date < targetTime.AddMinutes(30); date = date.AddMinutes(5))
                    {
                        Trace.WriteLine($"EEEEEEEEEEEEEEE ExceptionDataChecking DATE: {date}");

                        fireBase!.AddTDAH(date, mode);
                    }
                    Trace.WriteLine("EEEEEEEEEEEEEEE ExceptionDataChecking Finished");
                });

                var tnoh = Service.GetService<GWRangeResponse<TNOH>>();
                tnoh!.StartProtocol(targetTime, targetTime);
                for (var date = targetTime; date < targetTime.AddMinutes(30); date = date.AddMinutes(5))
                {
                    var tdat = Service.GetService<GWResponse<TDAH>>();
                    tdat!.Proto.SetTargetTime(date);
                    tdat!.Proto.Mode = "FIV";
                    tdat!.StartProtocol();
                }
            };

            TimeCaller!.HalfHourElapsed += async delegate (object? sender, TimeScheduleEventArgs e)
            {
                var HalfHourData = Service.GetService<TimedTask<IGrouping<int, SendItem>, HalfHourSend>>();
                await HalfHourData!.Executer(e.DateTime, "[HalfHourElapsed]");
            };

            TimeCaller!.HalfHourSendElapsed += delegate (object? sender, TimeScheduleEventArgs e)
            {

                Task task = Task.Run(() =>
                {
                    Trace.WriteLine("FFFFFFFFFFFF HalfHour Data Send ELAPSED");
                    var fireBase = Service.GetService<FirestoreRepository>();
                    var mode = "HAF";
                    fireBase!.AddTDAH(e.DateTime.AddMinutes(-30), mode);
                });

                Trace.WriteLine($"##### Half Hour data send");
                var tdat = Service.GetService<GWResponse<TDAH>>();
                tdat!.Proto.SetTargetTime(e.DateTime.AddMinutes(-30));
                tdat!.Proto.Mode = "HAF";
                tdat!.StartProtocol();
            };


            TimeCaller!.DayElapsed += async delegate (object? sender, TimeScheduleEventArgs e)
            {
                Trace.WriteLine("FFFFFFFFFFFF DayElapsed Data Send ELAPSED");
                await DayEndDataGenerate(e.DateTime);
                try
                {
                    Service.GetService<TTIM>()!.StartProtocol();
                }
                catch { }
                //Delete30DayData();
                DeleteOneYearData();
            };

            TimeCaller!.UnsendTimeElapsed += delegate (object? sender, TimeScheduleEventArgs e)
            {

                Task task = Task.Run(() =>
                {
                    Trace.WriteLine("FFFFFFFFFFFF TFDH Data Send ELAPSED");
                    var fireBase = Service.GetService<FirestoreRepository>();
                    //fireBase!.AddUnsendDataToFirestore(e.DateTime.AddDays(-3), e.DateTime);
                    fireBase!.AddUnsendDataToFirestore(e.DateTime.AddHours(-2), e.DateTime);
                });

                var tfdt = Service.GetService<GWRangeResponse<TFDH>>();
                tfdt!.StartProtocol(e.DateTime.AddDays(-3), e.DateTime);
            };


            TimeCaller!.TimeCorrectionElapsed += delegate (object? sender, TimeScheduleEventArgs e)
            {
                try
                {
                    Service.GetService<TTIM>()!.StartProtocol();
                }
                catch { }
            };
        }

        public void InitListenersKnexus()
        {
            KnexusListener();

            var FirestorNotifier = Service.GetService<FirestoreNotifier>();
            FirestorNotifier!.PTIMEvent += async delegate (object? sender, EventArgs e)
            {
                await OffDataGeneration(DateTime.Now);
                DeleteOneYearData();               
            };

            var TimeCaller = Service.GetService<TimeScheduleEvent>();
            TimeCaller!.CheckForOffDataElapsed += async delegate (object? sender, EventArgs e)
            {
                await OffDataGeneration(DateTime.Now);
                DeleteOneYearData();
            };

            TimeCaller!.FiveSecondElapsed += delegate (object? sender, TimeScheduleEventArgs e)
            {
                var SecondData = Service.GetService<TimedTask<RawInput, Item>>();
                SecondData!.Execute(e.DateTime, "[FiveSecondElapsed]");
            };

            TimeCaller!.FiveMinuteElapsed += async delegate (object? sender, TimeScheduleEventArgs e)
            {
                var MinutData = Service.GetService<TimedTask<Item, SendItem>>();
                await MinutData!.Executer(e.DateTime, "[FiveMinuteElapsed]");
            };

            TimeCaller!.FiveMinuteSendElapsed += delegate (object? sender, TimeScheduleEventArgs e)
            {
                Trace.WriteLine("FFFFFFFFFFFF FivMin Data Send ELAPSED");
                var fireBase = Service.GetService<FirestoreRepository>();
                var mode = "FIV";
                fireBase!.AddTDAH(e.DateTime.AddMinutes(-5), mode);
            };


            TimeCaller!.FiveMinuteSendExeptionElapsed += delegate (object? sender, TimeScheduleEventArgs e)
            {
                var targetTime = e.DateTime;

                Trace.WriteLine("EEEEEEEEEEEEEEE ExceptionDataChecking Started");
                var fireBase = Service.GetService<FirestoreRepository>();
                fireBase!.AddExceptionDataToFirestore(targetTime, targetTime);
                var mode = "FIV";
                for (var date = targetTime; date < targetTime.AddMinutes(30); date = date.AddMinutes(5))
                {
                    Trace.WriteLine($"EEEEEEEEEEEEEEE ExceptionDataChecking DATE: {date}");

                    fireBase!.AddTDAH(date, mode);
                }
                Trace.WriteLine("EEEEEEEEEEEEEEE ExceptionDataChecking Finished");              
            };

            TimeCaller!.HalfHourElapsed += async delegate (object? sender, TimeScheduleEventArgs e)
            {
                var HalfHourData = Service.GetService<TimedTask<IGrouping<int, SendItem>, HalfHourSend>>();
                await HalfHourData!.Executer(e.DateTime, "[HalfHourElapsed]");
            };

            TimeCaller!.HalfHourSendElapsed += delegate (object? sender, TimeScheduleEventArgs e)
            {
                Trace.WriteLine("FFFFFFFFFFFF HalfHour Data Send ELAPSED");
                var fireBase = Service.GetService<FirestoreRepository>();
                var mode = "HAF";
                fireBase!.AddTDAH(e.DateTime.AddMinutes(-30), mode);
            };


            TimeCaller!.DayElapsed += async delegate (object? sender, TimeScheduleEventArgs e)
            {
                Trace.WriteLine("FFFFFFFFFFFF DayElapsed Data Send ELAPSED");
                await DayEndDataGenerate(e.DateTime);
                DeleteOneYearData();
            };

            TimeCaller!.UnsendTimeElapsed += delegate (object? sender, TimeScheduleEventArgs e)
            {
                Trace.WriteLine("FFFFFFFFFFFF TFDH Data Send ELAPSED");
                var fireBase = Service.GetService<FirestoreRepository>();
                //fireBase!.AddUnsendDataToFirestore(e.DateTime.AddDays(-3), e.DateTime);
                fireBase!.AddUnsendDataToFirestore(e.DateTime.AddHours(-2), e.DateTime);
            };
        }

        public void InitGreenLink()
        {
            Service.GetService<GatewayServer>()!.Start();

            string version = ConfigurationManager.AppSettings.Get("Version")!.ToString();
            string hash = Helpers.GetHashOfEXEFile("Knexus.exe");
            using (var SettingWork = Service.GetService<IRepoFactory<SetupRepository<Setting>>>()?.Create())
            {
                bool isNewVersion = false;
                var setting = SettingWork!.Repo.GetFirst();
                setting.HashCode = hash;
                setting.IsDebug = IsDebug;
                var isReboot = setting.IsReboot;
                setting.IsReboot = false;
                if (version != setting.Firmware)
                {
                    setting.Firmware = version;
                    isNewVersion = true;
                }
                SettingWork!.Repo.Update(setting);
                SettingWork.Complete();

                if (isNewVersion)
                {
                    var tupg = Service.GetService<GWResponse<TUPG>>();
                    tupg!.StartProtocol();
                }

                if (isReboot)
                {
                    var tupg = Service.GetService<GWResponse<TCN2>>();
                    tupg!.StartProtocol();
                }
            }

            try
            {
                Service.GetService<TTIM>()!.StartProtocol();
            }
            catch { }
        }

        public void InitGreenLinkAndKnexus()
        {
            Service.GetService<GatewayServer>()!.Start();

            string version = ConfigurationManager.AppSettings.Get("Version")!.ToString();
            string hash = Helpers.GetHashOfEXEFile("Knexus.exe");
            using (var SettingWork = Service.GetService<IRepoFactory<SetupRepository<Setting>>>()?.Create())
            {
                bool isNewVersion = false;
                var setting = SettingWork!.Repo.GetFirst();
                setting.HashCode = hash;
                setting.IsDebug = IsDebug;
                var isReboot = setting.IsReboot;
                setting.IsReboot = false;
                if (version != setting.Firmware)
                {
                    setting.Firmware = version;
                    isNewVersion = true;
                }
                SettingWork!.Repo.Update(setting);
                SettingWork.Complete();

                if (isNewVersion)
                {
                    Task task = Task.Run(() =>
                    {
                        if (setting.CompanyCode == "1111111" || setting.IsConnFirebase == false) return;

                        var fireBase = Service.GetService<PVERHandler>();
                        var requestId = setting.PUPGRequestId!;
                        fireBase!.HandlePVER(requestId, "TUPG");
                        setting.PUPGRequestId = "";
                        SettingWork.Repo.Update(setting);
                        SettingWork.Complete();

                    });

                    var tupg = Service.GetService<GWResponse<TUPG>>();
                    tupg!.StartProtocol();
                }

                if (isReboot)
                {
                    Task task = Task.Run(() =>
                    {
                        if (setting.CompanyCode == "1111111" || setting.IsConnFirebase == false) return;
                        var fireBase = Service.GetService<PCN2Handler>();
                        var requestId = setting.PRBTRequestId!;
                        fireBase!.HandleTCN2(requestId);
                    });

                    var tupg = Service.GetService<GWResponse<TCN2>>();
                    tupg!.StartProtocol();
                }
            }

            try
            {
                Service.GetService<TTIM>()!.StartProtocol();
            }
            catch { }
        }

        public void InitKnexus()
        {

            string version = ConfigurationManager.AppSettings.Get("Version")!.ToString();
            string hash = Helpers.GetHashOfEXEFile("Knexus.exe");
            using (var SettingWork = Service.GetService<IRepoFactory<SetupRepository<Setting>>>()?.Create())
            {
                bool isNewVersion = false;
                var setting = SettingWork!.Repo.GetFirst();
                setting.HashCode = hash;
                setting.IsDebug = IsDebug;
                var isReboot = setting.IsReboot;
                setting.IsReboot = false;
                if (version != setting.Firmware)
                {
                    setting.Firmware = version;
                    isNewVersion = true;
                }
                SettingWork!.Repo.Update(setting);
                SettingWork.Complete();

                if (isNewVersion)
                {
                    if (setting.CompanyCode == "1111111" || setting.IsConnFirebase == false) return;
                    var fireBase = Service.GetService<PVERHandler>();
                    var requestId = setting.PUPGRequestId!;
                    fireBase!.HandlePVER(requestId, "TUPG");
                    setting.PUPGRequestId = "";
                    SettingWork.Repo.Update(setting);
                    SettingWork.Complete();
                }

                if (isReboot)
                {
                    if (setting.CompanyCode == "1111111" || setting.IsConnFirebase == false) return;
                    var fireBase = Service.GetService<PCN2Handler>();
                    var requestId = setting.PRBTRequestId!;
                    fireBase!.HandleTCN2(requestId);                    
                }
            }
        }


        //Use when the user wants both 
        //to use GreenLinkServer and KnexusService.
        //This method includes only:
        //ListenKproto().
        private void KnexusListenerWithGreenLink()
        {
            using (var SettingWork = Service.GetService<IRepoFactory<SetupRepository<Setting>>>()?.Create())
            {
                var setting = SettingWork!.Repo.GetFirst();
                if (setting.CompanyCode == "1111111" || setting.IsConnFirebase == false) return;
                Task.Run(() => Service.GetService<FirestoreListener>()!.ListenKproto());
            }
        }

        //Use when the user wants only
        //to use KnexusService.
        //This method includes:
        //ListenKproto(),
        //TTIM Event,
        //TOFF Generation.
        private void KnexusListener()
        {
            using (var SettingWork = Service.GetService<IRepoFactory<SetupRepository<Setting>>>()?.Create())
            {
                var setting = SettingWork!.Repo.GetFirst();
                if (setting.CompanyCode == "1111111" || setting.IsConnFirebase == false) return;
                Task.Run(() => Service.GetService<FirestoreListener>()!.ListenKproto());
                Service.GetService<FirestoreListener>()!.FirestoreTTIMHandler();
            }
        }

        private async Task OffDataGeneration(DateTime now)
        {
            var offDataProvider = Service!.GetService<IDataProvider<DateTime>>()!;
            var offList = offDataProvider.GetData(now);
            if (offList.Any())
            {
                var OffData = Service!.GetService<TimedTask<DateTime, IGrouping<DateTime, SendItem>>>()!;
                await OffData!.Executer(now, "[OffDataGeneration]");

                await HAFDataGenerate(offList);

                var startDate = GetTOFHStartDateTime(offList);

                if (Math.Abs((now - startDate).Days) > 3) startDate = now.AddDays(-3);

                var factorySetting = Service.GetService<IRepoFactory<SetupRepository<Setting>>>();
                using var work = factorySetting!.Create();
                var setting = work.Repo.GetFirst();

                if (setting.IsConnFirebase == true)
                {
                    Task task = Task.Run(() =>
                    {
                        var fireBase = Service!.GetService<FirestoreRepository>();
                        fireBase!.AddOffDataToFirestore(startDate, now);
                    });
                }

                if (setting.IsConnGreenLink == true)
                {
                    var toff = Service?.GetService<GWRangeResponse<TOFH>>()!;
                    toff.StartProtocol(startDate, now);
                }

                var dayGroup = offList.Where(i => i.ToString("HHmm") == "2355").GroupBy(i => i);
                foreach (var day in dayGroup)
                {
                    await DayEndDataGenerate(day.Key);
                }
            }
        }

        private DateTime GetTOFHStartDateTime(List<DateTime> offList)
        {
            var startDate = offList.First();

            var factorySetting = Service.GetService<IRepoFactory<SetupRepository<Setting>>>();
            using var work = factorySetting!.Create();
            var setting = work.Repo.GetFirst();

            if (setting.IsConnFirebase == true)
            {
                Task task = Task.Run(() =>
                {
                    if (setting.SendMode != 0 && isSecondDataExist(startDate))
                    {
                        var firebase = Service.GetService<FirestoreRepository>();
                        var mode = "FIV";
                        firebase!.AddTDAH(startDate, mode);
                        startDate = startDate.AddMinutes(5);
                    }
                    else if (setting.SendMode == 0 && startDate.Minute % 30 != 0)
                    {
                        var targetDate = GetClosest30MinuteDateTime(startDate);
                        var firebase = Service.GetService<FirestoreRepository>();
                        var mode = "HAF";
                        firebase!.AddTDAH(targetDate, mode);
                        startDate = startDate.AddMinutes(5);
                        startDate = targetDate.AddMinutes(30);
                        Service.GetService<TimeScheduleEvent>()?.CheckFiveMinuteExeption(targetDate);
                    }
                });
            }

            if (setting.IsConnGreenLink == true)
            {
                if (setting.SendMode != 0 && isSecondDataExist(startDate))
                {
                    var tdat = Service.GetService<GWResponse<TDAH>>();
                    tdat!.Proto.SetTargetTime(startDate);
                    tdat!.Proto.Mode = "FIV";
                    tdat!.StartProtocol();
                    startDate = startDate.AddMinutes(5);
                }
                else if (setting.SendMode == 0 && startDate.Minute % 30 != 0)
                {
                    var targetDate = GetClosest30MinuteDateTime(startDate);
                    var tdat = Service.GetService<GWResponse<TDAH>>();
                    tdat!.Proto.SetTargetTime(targetDate);
                    tdat!.Proto.Mode = "HAF";
                    tdat!.StartProtocol();
                    startDate = targetDate.AddMinutes(30);
                    Service.GetService<TimeScheduleEvent>()?.CheckFiveMinuteExeption(targetDate);
                }
            }

            return startDate;
        }

        private bool isSecondDataExist(DateTime startDate)
        {
            var secondProvider = Service.GetService<IDataProvider<Item>>()!;
            var secondRecords = secondProvider.GetData(startDate.AddMinutes(5));
            return secondRecords.Count > 0;
        }

        private DateTime GetClosest30MinuteDateTime(DateTime dateTime)
        {
            var specificDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);

            while (specificDateTime.Minute % 30 != 0)
            {
                specificDateTime = specificDateTime.AddMinutes(-1);
            }
            return specificDateTime;
        }

        private async Task HAFDataGenerate(List<DateTime> list)
        {
            var hafDates = list.Where(d => d.Minute == 55 || d.Minute == 25).ToList();
            if (!hafDates.Any()) return;
            var HAFGen = Service!.GetService<TimedTask<IGrouping<int, SendItem>, HalfHourSend>>()!;
            var tasks = hafDates.Select(async date => await HAFGen!.Executer(date.AddMinutes(5), "[HAFDataGenerate]")).ToList();
            await Task.WhenAll(tasks);
        }

        private async Task DayEndDataGenerate(DateTime date)
        {
            var dbFile = DbFileManager.GetRootPath()
                + $"DAY_{date:yyyyMMdd}.db";

            if (!File.Exists(dbFile)) return;

            using (var DayWork = Service.GetService<IRepoFactory<DayEndRecordRepository>>()?.Create(date))
            {
                if (DayWork!.Repo.GetLastRecord().Any()) return;
            }

            var DayEndRecord = Service.GetService<TimedTask<SendItem, DayEndRecord>>();
            await DayEndRecord!.Executer(date, "[DayEndDataGenerate]");

            var factorySetting = Service.GetService<IRepoFactory<SetupRepository<Setting>>>();
            using var work = factorySetting!.Create();
            var setting = work.Repo.GetFirst();

            if (setting.IsConnFirebase == true)
            {
                Task task = Task.Run(() =>
                {
                    Trace.WriteLine("FFFFFFFFFFFF TDDH Data Send Firestore");
                    var fireBase = Service.GetService<FirestoreRepository>();
                    fireBase!.AddEndDayRecordToFirestore(date, date);
                });
            }

            if (setting.IsConnGreenLink == true)
            {
                var tddd = Service.GetService<GWRangeResponse<TDDH>>();
                tddd!.StartProtocol(date, date);
            }
            
        }

        private void DeleteOneYearData()
        {
            try
            {
                int expYear = 1;
                var dir = DbFileManager.GetRootPath();
                var dbFiles = Directory.GetFiles(dir).Where(f => f.EndsWith(".db")).ToList();
                var list = SendRecordRepository.DeleteDataOutofOneYear(dbFiles, expYear);
                string DayType = $"DAY_{DateTime.Now:yyyyMMdd}.db";
                string HourType = $"DAT_{DateTime.Now:yyyyMMddHH}.db";
                foreach (var file in list)
                {
                    try
                    {
                        if (file.Contains(DayType) || file.Contains(HourType)) throw new Exception();
                        File.Delete(file);
                    }
                    catch { CleaupDB(file); }
                }
                // Delete Logs More Than 1 year
                var dirLog = AppDomain.CurrentDomain.BaseDirectory + $"\\SETTING\\LOG\\";
                var dbLogFiles = Directory.GetFiles(dirLog).Where(f => f.EndsWith(".db")).ToList();
                var listLog = LogRepository<AlarmLog>.DeleteLogOutofOneYear(dbLogFiles, expYear);
                foreach (var file in listLog)
                {
                    try { File.Delete(file); }
                    catch { Trace.WriteLine(file); }
                }
            }
            catch { } //
        }

        private void CleaupDB(string path)
        {
            IDbFileManager manager = Service.GetService<IDbFileManager>()!;
            bool isDAT = Path.GetFileName(path).Split("_")[0] == "DAT";
            DateTime fileDate = manager.GetDateFromDBFilePath(path);
            var now = GetLastMinuteDividableByFiveFromCurrent();

            try
            {
                if (isDAT)
                {
                    using var secWork = Service.GetService<IRepoFactory<ItemRecordRepository>>()!.Create(fileDate);
                    secWork.Repo.CleanWithDates(now.AddDays(-31), now);
                    return;
                }

                using var logWork = Service.GetService<IRepoFactory<LogRepository<PowerLog>>>()?.Create(now)!;
                logWork.Repo.AddNew($"Delete Possible Future Data" +
                    $" Starting from {now:yyyy-MM-dd HH:mm:ss} in {Path.GetFileName(path)}", "DELFTDAT");
                logWork.Complete();

                Trace.WriteLine($"##### Db CleanUp future time: {now}");
                using var fivWork = Service.GetService<IRepoFactory<SendRecordRepository>>()!.Create(fileDate);
                fivWork.Repo.CleanWithDates(now.AddDays(-31), now);
            }
            catch (Exception e)
            {
                using var logWork = Service.GetService<IRepoFactory<LogRepository<PowerLog>>>()?.Create(now)!;
                logWork.Repo.AddNew($"{e.Message} {e.StackTrace}", "DELERROR");
                logWork.Complete();
            }
        }

        private DateTime GetLastMinuteDividableByFiveFromCurrent()
        {
            var current = DateTime.Now;
            current = current.AddMinutes(-(current.Minute % 5));
            foreach (int _ in Enumerable.Range(0, 60))
            {
                if (current.Minute % 5 == 0 && current.Second == 0) break;
                current = current.AddSeconds(-1);
            }

            return new DateTime(current.Year, current.Month, current.Day, current.Hour, current.Minute, current.Second, 0);
        }
    }
}
