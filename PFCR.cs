public bool HandleRequest(int chimidForSelection, List<string> body)
{
    try
    {
        sendByChimId = chimidForSelection;
        foreach (string data in body)
        {
            var key = data[..5];
            var value = data.Substring(5, 5);
            Console.WriteLine(key);
            Console.WriteLine(value);

            using var itemWork = factoryItem.Create();
            var targets = itemWork.Repo.GetAll()
                .Where(j => j.ChimIdForKey == sendByChimId && j.FullFacilityCode == key);


            //var updateRelations = itemWork.Repo.GetAll()
            //Where(j.FullFacilityCode == value);

            //updated to only include the item that is under the requested chimneyId,
            //because there may be the same numbered items in the system but under the different chimneyNumbers
            var updateRelations = itemWork.Repo.GetAll()
                .Where(j => j.ChimIdForKey == sendByChimId && j.FullFacilityCode == value);

            if (!targets.Any() || !updateRelations.Any()) return false;

            var updateTarget = targets.First();
            updateTarget.RelatedItem = updateRelations.First().Id;
            itemWork.Repo.Update(updateTarget);
            itemWork.Complete();
        }

        notifier.notifyItemUpdates();
        return true;
    }
    catch (Exception ex)
    {
        _serilog.LogError($"An error occurred while handling PFRS request. Exception: {ex.Message}");
        return false;
    }
}
