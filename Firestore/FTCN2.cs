using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBLib;
using DBLib.Setup;
using DBLib.Setup.Entities;
using Google.Cloud.Firestore;


namespace Firestore.GatewayProtos
{
    [FirestoreData]
    public class FTCN2
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


        public static DocumentReference GetReference(Setting setting, FirestoreDb db)
        {
            var knexusServiceKey = $"GC{setting.CompanyCode}B{setting.CompanyRegistrationCode}";
            var collectionReference = db
                .Collection("gateways").Document(knexusServiceKey)
                .Collection("gproto").Document();
            return collectionReference;
        }

        // a temporary refernce for Firestore TCN2 Proto to test communication with GW
        public static DocumentReference GetTempReference(Setting setting, FirestoreDb db)
        {
            var knexusServiceKey = $"GC{setting.CompanyCode}B{setting.CompanyRegistrationCode}";
            var collectionReference = db
                .Collection("gateways").Document(knexusServiceKey)
                .Collection("Test-GProto").Document();
            return collectionReference;
        }

        public static FTCN2 MapFTCN2Data(Setting setting, Target target, List<ItemDetail> itemDetail, Password password) 
        {

            Trace.WriteLine($"-------------------------> TCN2 FTCN2 MapFTCN2Data...GetData()");
            var messageMap = GetData(setting, target, itemDetail, password);
            Trace.WriteLine($"-------------------------> TCN2 FTCN2 MapFTCN2Data...GetData() Done Returning");
            return new FTCN2
            {
                ChimneyCode = setting.ChimneyCode,
                MessageDate = DateTime.SpecifyKind(DateTime.Now.AddHours(-9), DateTimeKind.Utc),
                Id = "20240121",
                Message = messageMap,
                Type = "TCN2"
            };
        }

        public static List<string> GetData(Setting setting, Target target, List<ItemDetail> items, Password password)
        {
            var header = new List<string>()
            {
                String.Format("{0,15}", target.TCPIp),
                String.Format("{0,15}", setting.TCPIp),
                String.Format("{0,2}", setting.GWmodel.Substring(0,2)),
                String.Format("{0,20}", setting.GWmodel),
                String.Format("{0,20}", setting.Firmware),
                String.Format("{0,32}", setting.HashCode),
                String.Format("{0,10}", password.PassCode),
                String.Format("{0,4}", setting.UnsendTime),
                String.Format("{0,1}", setting.SendMode),
                String.Format("{0,3}", setting.EMUptime),
                String.Format("{0,3}", setting.PRStoptime),
                String.Format("{0,2}", items.Count)
            };
            foreach (var item in items)
            {
                header.AddRange(new List<string> {

                    String.Format("{0,5}", item.Facility!.FacilityCode + item.FacilityNum.ToString().PadLeft(2,'0')),
                    String.Format("{0,1}", item.ItemType!.code),
                    String.Format("{0,6}", item.ItemMinRange.ToString("F" + 2)),
                    String.Format("{0,6}", item.ItemMaxRange.ToString("F" + 2)),
                    String.Format("{0,6}", item.DefaultValue.ToString("F" + 2)),
                });
            }
            return header;
        }
    }

}
