
//Firestore fiveMin dataSend
public async void AddFivMinDataToFirestoreAsync(DateTime now)
{
    if (IsFirebaseDisabled() == true) return;

    var dbFile = GetDatabaseFilePath(now);
    if (!File.Exists(dbFile)) return;

    using var workOfLog = factoryLog.Create(DateTime.Now);

    try
    { 
        using var work = factoryItem.Create(now);
        var fivMinItems = work!.Repo.GetLastRecord();

        var protoStatus = "TDAH";
        var mappedFivMinData = tdahMap.MapDataForFirestore(fivMinItems, protoStatus);
        var collectionReference = tdahMap.GetTDAHReference(db!, "fivMin");

        await collectionReference.SetAsync(mappedFivMinData);

        work.Repo.UpdateFirestoreSentValue(now);
        work.Complete();

        var logMessage = $"[TX] {fivMinItems[0].Date:yyMMddHHmm} ";
        foreach (var element in fivMinItems)
        {
            logMessage += $"{tfdhMap.GetStringLog(element)}, ";
        }
        logMessage = logMessage.TrimEnd('\n', ' ');

        workOfLog.Repo.AddNew(logMessage, "[Tx] [TDAH FIV]");
        workOfLog.Complete();
    }
    catch (Exception ex)
    {
        workOfLog.Repo.AddNew($"TDAH [FIV] 전송실패되었습니다. Error: {ex.Message}", "[Tx] TDAH[FIV]");
        workOfLog.Complete();
        return;
    }
}


//Firestore halfMin dataSend
public async void AddHafMinDataToFirestoreAsync(DateTime now)
{
    if (IsFirebaseDisabled() == true) return;

    var dbFile = DbFileManager.GetRootPath() + $"DAY_{now:yyyyMMdd}.db";
    if (!File.Exists(dbFile)) return;

    using var workOfLog = factoryLog.Create(DateTime.Now);

    try
    {
        using var work = factoryHafItem.Create(now);
        var hafMinItems = work!.Repo.GetLastRecord();
        var protoName = "TDAH";
        var mappedHafMinData = tdahMap.MapDataForFirestore(hafMinItems, protoName);

        using var workOfSetting = factorySetting.Create();
        var setting = workOfSetting.Repo.GetFirst();
        var collectionReference = tdahMap.GetTDAHReference(db!, "hafMin");

        await collectionReference.SetAsync(mappedHafMinData);

        work.Repo.UpdateFirestoreSentValue(now);
        work.Complete();

        var logMessage = $"[TX] {hafMinItems[0].Date:yyMMddHHmm} ";
        foreach (var element in hafMinItems)
        {
            logMessage += $"{tfdhMap.GetStringLog(element)}, ";
        }
        logMessage = logMessage.TrimEnd('\n', ' ');

        workOfLog.Repo.AddNew(logMessage, "[Tx] [TDAH HAF]");
        workOfLog.Complete();
    }
    catch (Exception ex)
    {
        workOfLog.Repo.AddNew($"TDAH [HAF] 전송실패되었습니다. Error: {ex.Message}", "[Tx] [TDAH HAF]");
        workOfLog.Complete();
        return;
    }
}
