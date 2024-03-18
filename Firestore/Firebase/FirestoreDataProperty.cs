using DBLib.Record.Entities;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firestore.Firebase
{
    [FirestoreData]
    public class FirestoreDataProperty
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



        public static FirestoreDataProperty MapFivMinData(SendItem data) => new ()
        {
            FacilityCode = data.FacilityCode,
            ItemCode = data.ItemCode!,
            ItemId = data.ItemId!,
            RptValue = data.RptValue,
            RptState = data.RptState,
            OprState = data.OprState,
            PFState = data.PFState,
        };

        public static FirestoreDataProperty HafMinData(HalfHourSend data) => new ()
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
