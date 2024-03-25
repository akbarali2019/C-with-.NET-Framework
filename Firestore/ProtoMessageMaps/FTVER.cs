using DBLib.Setup.Entities;
using Firestore.Firebase;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firestore.ProtoMessageMaps
{
    [FirestoreData]
    public class FTVER : ListenerProperty
    {
        public static FTVER MapFTVERData(Setting setting, Target target, List<ItemDetail> itemDetail, string id)
        {
            var messageMap = GetData(setting, target, itemDetail);
            return new FTVER
            {
                ChimneyCode = setting.ChimneyCode,
                MessageDate = DateTime.SpecifyKind(DateTime.Now.AddHours(-9), DateTimeKind.Utc),
                Id = id,
                Message = messageMap,
                Type = "TVER"
            };
        }

        public static List<string> GetData(Setting setting, Target target, List<ItemDetail> items)
        {
            var header = new List<string>()
            {
                string.Format("{0,7}", setting.CompanyCode),
                string.Format("{0,3}", setting.ChimneyCode.PadLeft(3, '0')),
                string.Format("{0,4}", 126),
                string.Format("{0,15}", target.TCPIp),
                string.Format("{0,15}", setting.TCPIp),
                string.Format("{0,2}", setting.ManufactureCode),
                string.Format("{0,20}", setting.GWmodel),
                string.Format("{0,20}", setting.Firmware)
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
