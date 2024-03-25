using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBLib;
using DBLib.Setup;
using DBLib.Setup.Entities;
using DBLib.StateLogs;
using Firestore.Firebase;
using Google.Cloud.Firestore;


namespace Firestore.ProtoMessageMaps
{
    [FirestoreData]
    public class FTCN2 : ListenerProperty
    {
        public static FTCN2 MapFTCN2Data(Setting setting, Target target, List<ItemDetail> itemDetail, Password password, string id)
        {
            var messageMap = GetData(setting, target, itemDetail, password);
            return new FTCN2
            {
                ChimneyCode = setting.ChimneyCode,
                MessageDate = DateTime.SpecifyKind(DateTime.Now.AddHours(-9), DateTimeKind.Utc),
                Id = id,
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





