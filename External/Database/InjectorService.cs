using External.DAO;
using External.DAO.DBTable;
using External.DAO.JSON.KIS100;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Database {
    public class InjectorService : Service {
        //int ijidx = -1;

        public void InsertInjectorMgrInfo(string id) {
            using (var db = new KISMContext()) {

                injectormgr ijMgr = new injectormgr {
                    timestamp = DateTime.Now,
                    ijidx = StaticAttribute.Function.ijidx,
                    uid = id
                };

                var injectorMgr = db.Set<injectormgr>();
                injectorMgr.Add(ijMgr);

                db.SaveChanges();
                StaticAttribute.Function.logCommand.infoLog("[External.InjectorService.insertIjUserInfo] Insert injector manager success");
            }
        }

        public bool InsertInfo(FromKIS100Hello info) {
            bool state = false;
            //string msg = info["msg"];
            //var value = JsonConvert.DeserializeObject<Dictionary<string, string>>(msg);

            try {
                if (checkedInfo(info.id) == -1) {
                    using (var db = new KISMContext()) {
                        injectorinfo ijInfo = new injectorinfo {
                            timestamp = DateTime.Now,
                            ijid = info.id,
                            hw = info.hw,
                            fw = info.fw,
                            sn = info.sn,
                            stat = "A"
                        };

                        var injectorInfo = db.Set<injectorinfo>();
                        injectorInfo.Add(ijInfo);

                        db.SaveChanges();
                        StaticAttribute.Function.logCommand.infoLog("[External.InjectorService.insertInfo] Insert injector info success");

                        var findInjectorInfo = db.injectorinfo.SingleOrDefault(w => w.ijid == info.id);
                        StaticAttribute.Function.ijidx = findInjectorInfo == null ? -1 : findInjectorInfo.idx;
                    }
                    state = true;
                }
            }catch(Exception ex) {
                StaticAttribute.Function.logCommand.errorLog("[External.InjectorService.insertInfo] " + ex.Message);
                StaticAttribute.Function.logInfoService.InsertLogInfoItem("ERROR", "주입기 정보를 입력하는 데 실패했습니다.");
                state = false;
            }
            return state;
        }

        private int checkedInfo(string id) {
            StaticAttribute.Function.ijidx = -1;
            try {
                using (var db = new KISMContext()) {
                    var ijInfo = db.injectorinfo.SingleOrDefault(w => w.ijid == id);
                    StaticAttribute.Function.ijidx = ijInfo == null ? -1 : ijInfo.idx;
                }
            } catch (Exception ex) {
                StaticAttribute.Function.ijidx = -1;
                System.Windows.MessageBox.Show("Error : " + ex.Message);
            }
            return StaticAttribute.Function.ijidx;
        }

        public List<injectorinfo> SelectInjectorInfoAll() {
            List<injectorinfo> ijInfoList = new List<injectorinfo>();
            using (var db = new KISMContext()) {
                var ijInfo = from info in db.injectorinfo
                             select info;
                ijInfoList = ijInfo.ToList();
            }
            return ijInfoList;
        }

        public List<injectorinfo> SelectInjectorInfoItem(string injectorId) {
            List<injectorinfo> ijInfoList = new List<injectorinfo>();
            using (var db = new KISMContext()) {
                var ijInfo = from info in db.injectorinfo
                             where info.ijid == injectorId
                             select info;
                ijInfoList = ijInfo.ToList();
            }
            return ijInfoList;
        }
        public List<injectormgr> SelectInjectorMgrAll() {
            List<injectormgr> ijMgrList = new List<injectormgr>();
            using (var db = new KISMContext()) {
                var ijMgr = from info in db.injectormgr 
                            select info;
                ijMgrList = ijMgr.ToList();
            }
            return ijMgrList;
        }

        public List<injectormgr> SelectInjectorMgrList (int injectorIdx) {
            List<injectormgr> ijMgrList = new List<injectormgr>();
            using (var db = new KISMContext()) {
                var ijMgr = injectorIdx == -1 ? from info in db.injectormgr select info : db.injectormgr.Where(w => w.ijidx == injectorIdx);
                ijMgrList = ijMgr.ToList();
            }
            return ijMgrList;
        }

        public int selectIdx(string id) {
            int ijidx = -1;
            using (var db = new KISMContext()) {
                var ijInfo = from info in db.injectorinfo
                             where info.ijid == id
                             select info.idx;
                ijidx = ijInfo.ToList()[0];
            }
            return ijidx;
        }
    }
}
