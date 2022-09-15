using KISM.DAO.Distribution;
using KISM.DAO.KeyRel;
using KISM.DAO.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.Util {
    public class ProcessingDBData {
        public List<keyrel> ProcessingKeyRelAll(dynamic keyRelAll) {
            List<keyrel> result = new List<keyrel>();
            foreach (var keyRelItem in keyRelAll) {
                result.Add(new keyrel {
                    deldate = keyRelItem.deldate,
                    dpt = keyRelItem.dpt,
                    dtdate = keyRelItem.dtdate,
                    expdate = keyRelItem.expdate,
                    idx = keyRelItem.idx,
                    ijidx = keyRelItem.ijidx,
                    ppose = keyRelItem.ppose,
                    stat = keyRelItem.stat,
                    stdate = keyRelItem.stdate,
                    timestamp = keyRelItem.timestamp,
                    wkey = keyRelItem.wkey
                });
            }


            return result;
        }
        public List<dtinfo> ProcessingDtInfoAll(dynamic dtInfoAll) {
            List<dtinfo> result = new List<dtinfo>();

            foreach (var dtInfoItem in dtInfoAll) {
                result.Add(new dtinfo {
                    dpt = dtInfoItem.dpt,
                    hw = dtInfoItem.hw,
                    idx = dtInfoItem.idx,
                    ijidx = dtInfoItem.ijidx,
                    logid = dtInfoItem.logid,
                    mdid = dtInfoItem.mdid,
                    mdip = dtInfoItem.mdip,
                    ppose = dtInfoItem.ppose,
                    sn = dtInfoItem.sn,
                    stat = dtInfoItem.stat,
                    timestamp = dtInfoItem.timestamp
                });
            }

            return result;
        }
        public List<loginfo> ProcessingLogInfoAll(dynamic logInfoAll) {
            List<loginfo> result = new List<loginfo>();

            foreach (var logInfoItem in logInfoAll) {
                result.Add(new loginfo {
                    idx = logInfoItem.idx,
                    message = logInfoItem.message,
                    timestamp = logInfoItem.timestamp,
                    type = logInfoItem.type
                });
            }

            return result;
        }

    }
}
