using DBLib.Record.Entities;
using DBLib.Setup.Entities;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firestore.Firebase
{
    public class FirestoreHafMinDataMap
    {
        public static Dictionary<string, object> MapDataForFirestore(IEnumerable<HalfHourSend> items)
        {
            var itemsMap = new Dictionary<string, object>();
            var fireItemDate = DateTime.MinValue;

            foreach (var item in items)
            {
                var fireItem = FirestoreDataProperty.HafMinData(item);
                var documentId = $"[{item.ItemId}_{item.FacilityCode}]";
                itemsMap.Add(documentId, fireItem);
                fireItemDate = DateTime.SpecifyKind(item.Date.AddHours(-9), DateTimeKind.Utc);
            }

            var sensorDataMap = new Dictionary<string, object>
        {
            { "sensorData", itemsMap }
        };

            var receivedDateMap = new Dictionary<string, object>
        {
            { "receivedDate", fireItemDate }
        };

            var combinedMap = sensorDataMap.Union(receivedDateMap).ToDictionary(pair => pair.Key, pair => pair.Value);

            return combinedMap;
        }


        public static DocumentReference GetFirestoreDocumentReference(Setting setting, FirestoreDb db)
        {
            var knexusServiceKey = $"GC{setting.CompanyCode}B{setting.CompanyRegistrationCode}";
            var collectionReference = db.Collection("gateways")
                .Document(knexusServiceKey)
                .Collection("chimneys")
                .Document(setting.ChimneyCode)
                .Collection("hafMin")
                .Document();

            return collectionReference;
        }
    }

}

