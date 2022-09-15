using External.DAO;
using External.DAO.DBTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Database {

    public class PurposeInfoService : Service {

        public List<pposeinfo> SelectPposeInfoAll() {
            List<pposeinfo> pposeinfoList = new List<pposeinfo>();
            using (var db = new KISMContext()) {
                var infoList = from info in db.pposeinfo
                               select info;
                pposeinfoList = infoList.ToList();
            }
            return pposeinfoList;
        }

        public bool UpdatePposeInfoItem(int idx, string ppose) {
            using (var db = new KISMContext()) {
                try {
                    var result = db.pposeinfo.SingleOrDefault(w => w.idx == idx);
                    result.ppose = ppose;
                    db.SaveChanges();
                    return true;
                } catch {
                    return false;
                }

            }
        }

        public bool InsertPposeInfoItem(string ppose) {
            bool state = false;

            try {
                using (var db = new KISMContext()) {
                    List<pposeinfo> pposeinfoList = new List<pposeinfo>();
                    pposeinfoList.Add(new pposeinfo {
                        rank = "SUPER",
                        stat = "A",
                        timestamp = DateTime.Now,
                        uname = "SUPER",
                        uninum = "SUPER",
                        ppose = ppose,
                    });
                    db.pposeinfo.AddRange(pposeinfoList);
                    db.SaveChanges();
                }
                state = true;
            } catch (Exception ex) {
                StaticAttribute.Function.logCommand.errorLog("[External.PurposeInfoService.Insert] " + ex.Message);
                StaticAttribute.Function.logInfoService.InsertLogInfoItem("ERROR", "용도 정보를 저장하는 데에 실패했습니다.");
            }

            return state;
        }

        private List<pposeinfo> setInfo(dynamic pposeInfoList) {
            List<pposeinfo> ppInfo = new List<pposeinfo>();
            foreach (var pposeInfo in pposeInfoList) {
                try {
                    string statChange = pposeInfo.Stat.Equals("활성화") || pposeInfo.Stat.ToString().Equals("생성 예정") ? "A" : "D";
                    ppInfo.Add(new pposeinfo {
                        timestamp = DateTime.Parse(pposeInfo.Timestamp.ToString()),
                        ppose = pposeInfo.Ppose,
                        uninum = pposeInfo.UniNum.ToString(),
                        rank = pposeInfo.Rank.ToString(),
                        uname = pposeInfo.UserName.ToString(),
                        stat = statChange
                    });
                } catch (Exception ex) {
                    StaticAttribute.Function.logCommand.errorLog("[External.MilitaryUnitService.SetInfo] " + ex.Message);
                    StaticAttribute.Function.logInfoService.InsertLogInfoItem("ERROR", "용도 정보를 불러오는 데에 실패했습니다.");
                }
            }

            return ppInfo;
        }

        private bool checkedData(object[] itemArray) {
            foreach (var item in itemArray) {
                if (item.Equals("")) {
                    return false;
                }
            }
            return true;
        }

        public bool DeletePposeInfoItem(int idx) {
            bool state = false;

            try {
                using (var db = new KISMContext()) {
                    var result = db.pposeinfo.SingleOrDefault(w => w.idx == idx);
                    result.stat = "D";
                    db.SaveChanges();
                }

                state = true;
            } catch (Exception ex) {
                StaticAttribute.Function.logCommand.errorLog("[External.PurposeInfoService.Delete] " + ex.Message);
                StaticAttribute.Function.logInfoService.InsertLogInfoItem("ERROR", "용도 정보를 삭제하는 데에 실패했습니다.");
            }

            return state;
        }
    }
}
