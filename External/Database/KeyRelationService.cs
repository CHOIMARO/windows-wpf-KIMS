using External.DAO;
using External.DAO.DBTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Database {
    public class KeyRelationService : Service {

        public bool InsertKeyRelItem(string wrappedKey, string dpt, string ppose, string usingDate, int expDate) {
            bool state = false;
            DateTime usingDateConvert = Convert.ToDateTime(usingDate);
            DateTime expirationDate = usingDateConvert.AddMonths(expDate);
            try {
                using (var db = new KISMContext()) {
                    keyrel rel = new keyrel {
                        timestamp = DateTime.Now,
                        //ijidx = db.injectorinfo.SingleOrDefault(w => w.ijid == id).idx,
                        dpt = dpt,
                        ppose = ppose,
                        //regdate = DateTime.Now,
                        stdate = usingDateConvert,
                        expdate = expirationDate,
                        wkey = wrappedKey,
                        stat = "REG"
                    };

                    var relInfo = db.Set<keyrel>();
                    relInfo.Add(rel);

                    db.SaveChanges();
                }
                state = true;
            } catch (Exception ex) {
                StaticAttribute.Function.logCommand.errorLog("[External.KeyRelationServiceImpl.insertKey] " + ex.Message);
                StaticAttribute.Function.logInfoService.InsertLogInfoItem("ERROR", "암호키를 저장하는데 실패했습니다.");
            }
            return state;
        }

        public string selectKey(int ijidx, int krIdx) {
            string wkey = string.Empty;

            using (var db = new KISMContext()) {
                var keyRel = from rel in db.keyrel
                             //where rel.ijidx == ijidx
                             where rel.idx == krIdx
                             select rel.wkey;
                wkey = keyRel.ToList()[0];
            }
            return wkey;
        }

        public bool checkedKey(string dpt, string ppose) { //false가 중복된 것 없음, true가 중복됨
            bool state = false;
            using (var db = new KISMContext()) {
                var result = db.keyrel.Where(w => w.dpt.Equals(dpt)
                                               && w.ppose.Equals(ppose) && !w.stat.Equals("DEL"));
                if(result.ToList().Count>0) {
                    if (result.ToList()[0].expdate < DateTime.Now.Date) {
                        result.ToList()[0].stat = "DEL";
                        db.SaveChanges();
                        state = false;
                    } else {
                        state = true;
                    }
                } else {
                    state = false;
                }
                //state = result.ToList().Count > 0 ? true : false;
            }
            return state;
        }
        private bool checkedExpKey (string dpt, string ppose) {
            bool state = false;
            using (var db = new KISMContext()) {

                var result = from rel in db.keyrel
                             where rel.dpt == dpt
                             where rel.ppose == ppose
                             select rel;
                var data = result.ToList()[0];
                if(data.expdate < DateTime.Now.Date) {
                    data.stat = "DEL";
                    db.SaveChanges();
                    state = true;
                }
            }
            return state;
        }
        public List<keyrel> SelectKeyRelAll() {
            List<keyrel> keyRelList = new List<keyrel>();
            using (var db = new KISMContext()) {
                var keyRel = from rel in db.keyrel
                             select rel;
                keyRelList = keyRel.ToList();
            }
            return keyRelList;
        }
        public List<keyrel> selectKeyRelExceptDel() {
            List<keyrel> keyRelList = new List<keyrel>();
            using (var db = new KISMContext()) {
                var keyRel = from rel in db.keyrel
                             where rel.stat != "DEL"
                             select rel;
                keyRelList = keyRel.ToList();
            }
            return keyRelList;
        }
        public List<keyrel> connectedInjectorSelectKeyRel() {
            List<keyrel> keyRelList = new List<keyrel>();
            using (var db = new KISMContext()) {

                var keyRel = from rel in db.keyrel
                             where rel.ijidx == StaticAttribute.Function.ijidx
                             select rel;

                keyRelList = keyRel.ToList();
            }

            return keyRelList;
        }
        public bool DeleteKeyRelItem(int idx) {
            bool state = false;

            using (var db = new KISMContext()) {

                var result = db.keyrel.SingleOrDefault(w => w.idx == idx);
                if (result != null) {
                    result.stat = "DEL";
                    db.SaveChanges();
                    state = true;
                }
            }
            return state;
        }

        public void updateKeyRelStatus(dynamic DistributionLogDAOList) {
            using (var db = new KISMContext()) {
                foreach (var DistributionLogDAO in DistributionLogDAOList) {
                }
            }
        }
        public bool UpdateKeyRelItem(int krIdx, string injectorId) {
            using (var db = new KISMContext()) {
                var injectorResult = db.injectorinfo.SingleOrDefault(w => w.ijid.Equals(injectorId));
                var result = db.keyrel.SingleOrDefault(w => w.idx == krIdx);
                result.stat = "DT";
                result.ijidx = injectorResult.idx;

                db.SaveChanges();
            }
            return true;
        }
        public void updateExpirationKeyRelStatus() {
            using (var db = new KISMContext()) {
                var result = from rel in db.keyrel
                             where rel.expdate < DateTime.Now
                             select rel;
                foreach(var relData in result) {
                    relData.stat = "DEL";
                }

                db.SaveChanges();
            }
        }
    }
}
