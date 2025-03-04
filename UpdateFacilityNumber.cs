public void UpdateFacilityNumber(int num, ItemDetail item)
{
    if (!item.ItemType!.isMain) return;
    using var itemWork = factoryItem.Create();
    var itemUpdate = itemWork.Repo.GetById(item.Id);


    // Only perform the duplicate check if ItemCode is "A"
    if (item.ItemCode == "A")
    {
        string newFullFacilityCode = item.FullFacilityCode.Substring(0, item.FullFacilityCode.Length - 2) + num.ToString("D2");
        bool exists = itemWork.Repo.GetAll().Any(i => i.ChimIdForKey == item.ChimIdForKey && i.FullFacilityCode == newFullFacilityCode && i.Id != item.Id && i.ItemCode == "A");
        if (exists)
        {
            var dialog = new ConfirmDialog("동일한 시설코드가 발견되었습니다. 그래도 설정하시겠습니까?");

            if (!dialog.ShowDialog() == true) return;
        }
    }

    if (itemUpdate.IsAlter == true)
    {
        using var relWorkItems = factoryItem.Create();
        var relItemUpdates = relWorkItems.Repo.GetAll()
            .Where(i => i.ChimIdForKey == item.ChimIdForKey &&
                        i.FullFacilityCode == item.FullFacilityCode &&
                        i.ItemType!.mark == "A" &&
                        i.IsAlter == false);

        foreach (var relItems in relItemUpdates)
        {
            relItems.FacilityNum = num;
            relWorkItems.Repo.Update(relItems);
        }

        relWorkItems.Complete();
    }

    itemUpdate.FacilityNum = num;
    itemWork.Repo.Update(itemUpdate);
    itemWork.Complete();

    NotifyChangeToServer((int)item.ChimIdForKey!);
    
}
