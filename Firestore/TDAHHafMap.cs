using DBLib.Record.Entities;
using DBLib.Setup.Entities;
using Google.Cloud.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firestore.AutoSendProtos
{
    public class TDAHHafMap
    {
        public static Dictionary<string, object> MapDataForFirestore(IEnumerable<HalfHourSend> items, string protoStatus)
        {
            Dictionary<string, object> docData = new();
            ArrayList itemsList = new();

            var fireItemDate = DateTime.MinValue;
            foreach (var item in items)
            {
                var fireItem = TDAHHafProperty.MapHafMin(item);

                itemsList.Add(fireItem);

                fireItemDate = DateTime.SpecifyKind(item.Date.AddHours(-9), DateTimeKind.Utc);
            }
            docData.Add("sensorsDate", fireItemDate);
            docData.Add("sensorsData", itemsList);
            docData.Add("protoStatus", protoStatus);
            return docData;
        }


        public static DocumentReference GetReference(Setting setting, FirestoreDb db)
        {
            var knexusServiceKey = $"GC{setting.CompanyCode}B{setting.CompanyRegistrationCode}";
            var collectionReference = db
                .Collection("gateways").Document(knexusServiceKey)
                .Collection("chimneys").Document(setting.ChimneyCode)
                .Collection("hafMin").Document();

            return collectionReference;
        }
    }

    [FirestoreData]
    public class TDAHHafProperty
    {
        [FirestoreProperty]
        public string? FacilityCode { get; set; }

        [FirestoreProperty]
        public string? ItemCode { get; set; }

        [FirestoreProperty]
        public int? ItemId { get; set; }

        [FirestoreProperty]
        public double RptValue { get; set; }

        [FirestoreProperty]
        public int RptState { get; set; }

        [FirestoreProperty]
        public int OprState { get; set; }

        [FirestoreProperty]
        public int PFState { get; set; }

        public static TDAHHafProperty MapHafMin(HalfHourSend data) => new()
        {
            FacilityCode = data.FacilityCode,
            ItemCode = data.ItemCode!,
            ItemId = data.ItemId!,
            RptValue = data.RptValue,
            RptState = data.RptState,
            OprState = data.OprState,
            PFState = data.PFState,
        };
    }
}
