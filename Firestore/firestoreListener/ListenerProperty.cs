using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBLib;
using DBLib.Record.Entities;
using DBLib.Setup;
using DBLib.Setup.Entities;
using Google.Cloud.Firestore;


namespace Firestore.FirestoreHelper
{
    [FirestoreData]
    public class ListenerProperty
    {
        [FirestoreProperty]
        public string? ChimneyCode { get; set; }

        [FirestoreProperty]
        public DateTime? MessageDate { get; set; }

        [FirestoreProperty]
        public string? Id { get; set; }

        [FirestoreProperty]
        public List<string>? Message { get; set; }

        [FirestoreProperty]
        public string? Type { get; set; }

    }
}
