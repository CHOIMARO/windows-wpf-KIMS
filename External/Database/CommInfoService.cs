using External.DAO;
using External.DAO.DBTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Database {

    public class CommInfoService : Service {
        public List<comminfo> SelectCommInfoItem() {
            List<comminfo> comminfoList = new List<comminfo>();
            using (var db = new KISMContext()) {
                var infoList = from info in db.comminfo
                               select info;
                comminfoList = infoList.ToList();
            }
            return comminfoList;
        }

        public bool InsertCommInfoItem(string ip, int port) {
            bool state = false;
            try {
                
                using (var db = new KISMContext()) {
                    comminfo info = new comminfo {
                        timestamp = DateTime.Now,
                        ip = ip,
                        port = port
                    };

                    db.comminfo.Add(info);
                    db.SaveChanges();
                }
                state = true;
            } catch (Exception ex) {
                StaticAttribute.Function.logCommand.errorLog("[External.CommInfoService.Insert] " + ex.Message);
                StaticAttribute.Function.logInfoService.InsertLogInfoItem("ERROR", "통신을 설정하는 데 실패했습니다.");
            }

            return state;
        }
    }
}
