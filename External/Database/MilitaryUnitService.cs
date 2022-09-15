using External.DAO;
using External.DAO.DBTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Database {
    public class MilitaryUnitService : Service {

        public List<milunitinfo> SelectMilUnitInfoAll() {
            List<milunitinfo> milunitinfoList = new List<milunitinfo>();
            using (var db = new KISMContext()) {
                var infoList = from info in db.milunitinfo
                               select info;
                milunitinfoList = infoList.ToList();
            }
            return milunitinfoList;
        }

        public bool UpdateMilUnitInfoItem(int idx, string unit) {
            using (var db = new KISMContext()) {
                try {
                    var result = db.milunitinfo.SingleOrDefault(w => w.idx == idx);
                    result.unit = unit;
                    db.SaveChanges();
                    return true;
                }catch {
                    return false;
                }
                
            }
        }

        public bool InsertMilUnitInfoItem(string milUnit) {
            bool state = false;

            try {
                using (var db = new KISMContext()) {
                    List<milunitinfo> milunitinfoList = new List<milunitinfo>();
                    milunitinfoList.Add(new milunitinfo {
                        rank = "SUPER",
                        stat = "A",
                        timestamp = DateTime.Now,
                        uname = "SUPER",
                        uninum = "SUPER",
                        unit = milUnit
                    });
                    db.milunitinfo.AddRange(milunitinfoList);
                    db.SaveChanges();
                }
                state = true;
            } catch (Exception ex) {
                StaticAttribute.Function.logCommand.errorLog("[External.MilitaryUnitService.Insert] " + ex.Message);
                StaticAttribute.Function.logInfoService.InsertLogInfoItem("ERROR", "부대 정보를 저장하는 데에 실패했습니다.");
            }

            return state;
        }

        public bool DeleteMilUnitInfoItem(int idx) {
            bool state = false;

            try {
                using (var db = new KISMContext()) {
                    var result = db.milunitinfo.SingleOrDefault(w => w.idx == idx);
                    result.stat = "D";
                    db.SaveChanges();
                }

                state = true;
            } catch (Exception ex) {
                StaticAttribute.Function.logCommand.errorLog("[External.MilitaryUnitService.Delete] " + ex.Message);
                StaticAttribute.Function.logInfoService.InsertLogInfoItem("ERROR", "부대 정보를 삭제하는 데에 실패했습니다.");
            }

            return state;
        }
    }
}
