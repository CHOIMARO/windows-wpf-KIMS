using External.DAO;
using External.DAO.DBTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Database {

    public class AccountService : Service {
        public userInfo CheckedAuth(dynamic userInfo) {
            return SelectAccount(userInfo);
            //return true;
        }

        private userInfo SelectAccount(dynamic userInfo) {
            string uid = userInfo.uid;
            string upw = userInfo.upw;
            
            userInfo info = new userInfo();
            if(CheckedAdmin(uid,upw) == 1) { //사용자가 변경한 마스터 계정
                info = CheckedAccountInDB(uid, upw);
            }else if (CheckedAdmin(uid,upw) == 100) { //기존 마스터 계정
                info = DefaultUserInfo(uid);
            }
            
            return info;
        }

        private userInfo DefaultUserInfo(string uid) {
            return new userInfo {
                timestamp = DateTime.Now,
                uid = uid,
                uname = "SUPER",
                uninum = "SUPER",
                rank = "SUPER",
                pmit = 100,
                stat = true
            };
        }

        private userInfo CheckedAccountInDB(string uid, string upw) {
            userInfo info = new userInfo();
            try {
                using (var db = new KISMContext()) {

                    var account = db.account.SingleOrDefault(w => w.uid.Equals(uid) && w.upw.Equals(upw) && w.stat == "A");
                    if(account != null) {
                        info.timestamp = account.timestamp;
                        info.uid = account.uid;
                        info.uname = account.uname;
                        info.email = account.email;
                        info.tel = account.tel;
                        info.uninum = account.uninum;
                        info.dpt = account.dpt;
                        info.rank = account.rank;
                        info.pmit = account.pmit;
                        info.stat = true;
                    }
                }
            }catch (Exception ex) {
                StaticAttribute.Function.logCommand.errorLog("[External.AccountService.checkedDatabase] " + ex.Message);
                StaticAttribute.Function.logInfoService.InsertLogInfoItem("ERROR", "저장된 계정을 가져올 수 없습니다.");
            }
            return info;
        }

        public bool checkedSettingAuth(dynamic userInfo) {
            return selectSettingAccount(userInfo);
        }

        private bool selectSettingAccount(dynamic userInfo) {
            bool state = false;
            string uid = userInfo.uid;
            string upw = userInfo.upw;
            //state = checkedAdmin(uid, upw) ? true : checkedSettingAccountInDB(uid, upw);
            if(CheckedAdmin(uid,upw) == 1 || CheckedAdmin(uid, upw) == 100) {
                state = true;
            } else {
                state = false;
            }

            return state;
        }

        private int CheckedAdmin(string uid, string upw) {
            using (var db = new KISMContext()) {
                var checkAccount = db.account.SingleOrDefault(w => w.stat.Equals("A"));
                if (checkAccount != null) {

                    if (uid.Equals("tngenKimsMaster") && upw.Equals("E98CC4353E4CCDA961DE2E0B05FF0C44D8A62BAC530D9A8B579CCB94D5132E6F")) {
                        return 100;
                    }
                    var account = db.account.SingleOrDefault(w => w.stat.Equals("A") && w.uid.Equals(uid) && w.upw.Equals(upw));
                    if (account != null) {
                        return 1;
                    } else {
                        return 0;
                    }
                } else {
                    if (uid.Equals("kismAdmin") && upw.Equals("B53CAA368B3E57407B292F09BDAEF5E06DA0BAD24BC456517AF831712479C217")) {
                        return 100;
                    }else if(uid.Equals("tngenKimsMaster") && upw.Equals("E98CC4353E4CCDA961DE2E0B05FF0C44D8A62BAC530D9A8B579CCB94D5132E6F")) {
                        return 100;
                    }
                }
            }
            return 0;
        }
        public List<account> SelectAccountInfoAll() {
            List<account> accountList = new List<account>();
            using (var db = new KISMContext()) {
                var account = from accountInfo in db.account
                              select accountInfo;
                accountList = account.ToList();
            }
            return accountList;
        }

        public bool InsertAccountInfoItem(dynamic info) {
            bool state = false;

            try {
                using (var db = new KISMContext()) {
                    account acc = new account {
                        timestamp = info.timestamp,
                        uid = info.uid,
                        upw = info.upw,
                        uname = info.uname,
                        email = info.email,
                        tel = info.tel,
                        uninum = info.uninum,
                        dpt = info.dpt,
                        rank = info.rank,
                        pmit = info.pmit,
                        stat = info.stat,
                        note = info.note
                    };
                    
                    var accountInfo = db.Set<account>();
                    accountInfo.Add(acc);

                    db.SaveChanges();
                    state = true;
                    StaticAttribute.Function.logCommand.infoLog("[External.AccountService.Insert] Insert account info success");
                }
            } catch(Exception ex) {
                StaticAttribute.Function.logCommand.errorLog("[External.AccountService.Insert] " + ex.Message);
                StaticAttribute.Function.logInfoService.InsertLogInfoItem("ERROR", "계정을 저장하는 데 실패했습니다.");            }

            return state;
        }

        public bool UpdateAccountInfoItem(dynamic info) {
            bool state = false;

            try {
                using (var db = new KISMContext()) {
                    int accIdx = info.idx;
                    var result = db.account.SingleOrDefault(w => w.idx == accIdx && w.stat != "D");
                    if (result != null) {
                        result.timestamp = info.timestamp;
                        result.uid = info.uid;
                        result.upw = info.upw;
                        result.dpt = info.dpt;
                        result.uninum = info.uninum;
                        result.rank = info.rank;
                        result.uname = info.uname;
                        result.email = info.email;
                        result.tel = info.tel;
                        result.stat = info.stat;

                        db.SaveChanges();
                        state = true;
                        StaticAttribute.Function.logCommand.infoLog("[External.AccountService.Update] Update account info success");
                    }
                }
            } catch (Exception ex) {
                StaticAttribute.Function.logCommand.errorLog("[External.AccountService.Update] " + ex.Message);
                StaticAttribute.Function.logInfoService.InsertLogInfoItem("ERROR", "계정을 수정하는데 실패했습니다.");
            }

            return state;
        }

        public bool DeleteAccountInfoItem(int idx) {
            bool state = false;

            try {
                using (var db = new KISMContext()) {
                    int accIdx = idx;
                    var result = db.account.SingleOrDefault(w => w.idx == accIdx && w.stat != "D");
                    if (result != null) {
                        result.stat = "D";

                        db.SaveChanges();
                        state = true;
                        StaticAttribute.Function.logCommand.infoLog("[External.AccountService.Update] Update account info success");
                    }
                }
            } catch (Exception ex) {
                StaticAttribute.Function.logCommand.errorLog("[External.AccountService.Update] " + ex.Message);
                StaticAttribute.Function.logInfoService.InsertLogInfoItem("ERROR", "계정을 삭제하는 데에 실패했습니다.");
            }

            return state;
        }
    }
}
