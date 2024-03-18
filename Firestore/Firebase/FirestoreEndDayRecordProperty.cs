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
    [FirestoreData]
    public class FirestoreEndDayRecordProperty
    {
        [FirestoreProperty]
        public int DataCount { get; set; }
        [FirestoreProperty]
        public int TDATCount { get; set; }
        [FirestoreProperty]
        public int TOFFCount { get; set; }
        [FirestoreProperty]
        public int ItemCount { get; set; }
        [FirestoreProperty]
        public int ItemId { get; set; }
        [FirestoreProperty]
        public string FacilityCode { get; set; } = "";
        [FirestoreProperty]
        public string ItemCode { get; set; } = "";
        [FirestoreProperty]
        public int NormalCount { get; set; }
        [FirestoreProperty]
        public int AbnormalCount { get; set; }
        [FirestoreProperty]
        public int ConnectionCount { get; set; }
        [FirestoreProperty]
        public int PowerOffCount { get; set; }
        [FirestoreProperty]
        public int ExaminCount { get; set; }


        public static FirestoreEndDayRecordProperty MapEndDayRecord(DayEndRecord data) => new ()
        {
            DataCount = data.DataCount,
            ItemCode = data.ItemCode!,
            ItemId = data.ItemId!,
            TDATCount = data.TDATCount,
            TOFFCount = data.TOFFCount,
            ItemCount = data.ItemCount,
            FacilityCode = data.FacCode,
            NormalCount = data.NormalCount,
            AbnormalCount = data.AbnormalCount,
            ConnectionCount = data.ConnectionCount,
            PowerOffCount = data.PowerOffCount,
            ExaminCount = data.ExaminCount,
        };
    }
}
