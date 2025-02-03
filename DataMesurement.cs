
// Updates 2025-02-03 알
// DataMesurement method to avoid -0. 
// Besides, it shows 0 if the rawData digit is between 3.91 mA and 4 mA for all sensors except temperature sensor

public static void DataMesurement(ref Item item, ItemDetail itemDetail)
{
    double ManResult = MeasureRawDataBymA(
        (int)itemDetail.ItemMaxRange,
        (int)itemDetail.ItemMinRange,
        item.mAValue);
    ManResult = double.Parse(ManResult.ToString(itemDetail.ItemType!.format));

    item.RptValue = Math.Min(ManResult, itemDetail.ItemMaxRange);

    if (item.RptState == 2) item.RptValue = 0; // if DataState is connection failure RptValue is 0
    if (item.RptState == 8 && item.mAValue < 1) item.RptValue = 0;

    if(item.RptValue.ToString() == "-0") item.RptValue = 0;

    if (itemDetail.ItemType!.mark != "°C" && item.RptValue < 0 && item.RptState != 1) item.RptValue = 0;        
}
