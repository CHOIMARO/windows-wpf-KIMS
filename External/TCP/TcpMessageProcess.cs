using External.DAO.JSON;
using External.DAO.JSON.KIS100;
using External.StaticAttribute.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.TCP {
    public class TcpMessageProcess {
        KIS100DataDAO dataObj = new KIS100DataDAO();
        List<char> dataList = new List<char>();
        int startIndex = -1;
        int endIndex = -1;

        internal void messageProcess(string data) {
            addData(data);
            if (checkedJsonType()) {
                setJsonData();
            }
        }

        private void addData(string data) {
            dataList.AddRange(data.Replace("\r\n", ""));
        }

        private bool checkedJsonType() {
            if (dataList.Count > 0) {
                int count = 0;
                startIndex = dataList.IndexOf('{');

                if (startIndex != -1) {
                    for (int index = startIndex; index < dataList.Count; index++) {
                        if (dataList[index].Equals('{')) {
                            count++;
                        } else if (dataList[index].Equals('}')) {
                            count--;
                        }
                        if (count == 0) {
                            endIndex = index;
                            break;
                        }
                    }
                    return count == 0 ? true : false;
                }
            }
            return false;
        }

        private void setJsonData() {
            string data = string.Empty;
            for (int index = startIndex; index < endIndex + 1; index++) {
                data += dataList[index];
            }
            try {
                dataObj = JsonConvert.DeserializeObject<KIS100DataDAO>(data);
                compareCmd();
            } catch {
                StaticAttribute.Function.logCommand.errorLog("[External.TcpMessageProcess.setJsonData] 프로토콜 정의에 따른 메시지 형식과 다릅니다.");
            }
            
            dataList.RemoveRange(0, endIndex + 1);

            //compareCmd();
        }

        private void compareCmd() {
            //var cmd = (commandEnum)Enum.Parse(typeof(commandEnum), dataObj.cmd);

            switch (dataObj.cmd) {
                case commandEnum.HELLO:
                    AuthMessage();
                    break;
                case commandEnum.GEN:
                    KeyGenMessage();
                    break;
                case commandEnum.KREG:
                    RegKeyMessage();
                    break;
                case commandEnum.LOG:
                    LogMessage();
                    break;
                case commandEnum.RLOG:
                    RlogMessage();
                    break;
                case commandEnum.DLOG:
                    DlogMessage();
                    break;
                case commandEnum.REK:
                case commandEnum.UCK:
                    UckMessage();
                    break;
                case commandEnum.UREG:
                    UregMessage();
                    break;
                case commandEnum.KDEL:
                case commandEnum.UDEL:
                    UdelMessage();
                    break;
                default:
                    StaticAttribute.Function.logCommand.errorLog("[External.MessageProcess.compareCmd] Error command");
                    return;
            }
            ReceivedDataTracker();
        }

        private void DlogMessage() {
            dataObj.msg = new FromKIS100Dlog {
                stat = dataObj.msg["stat"]
            };
        }

        private void UregMessage() {
            dataObj.msg = new FromKIS100Ureg {
                stat = dataObj.msg["stat"]
            };
        }

        private void UckMessage() {
            dataObj.msg = new FromKIS100Uck {
                stat = dataObj.msg["stat"]
            };
        }

        private void AuthMessage() {
            dataObj.msg = new FromKIS100Hello {
                id = dataObj.msg["id"],
                hw = dataObj.msg["hw"],
                fw = dataObj.msg["fw"],
                sn = dataObj.msg["sn"],
                name = dataObj.msg["name"],
            };
        }

        private void KeyGenMessage() {
            dataObj.msg = new FromKIS100KeyGen {
                id = dataObj.msg["ID"]
            };
        }

        private void RegKeyMessage() {
            dataObj.msg = new FromKIS100KeyReg {
                id = dataObj.msg["id"],
                key = dataObj.msg["key"],
            };
        }

        private void LogMessage() {
            JArray msgArr = (JArray)dataObj.msg["list"];
            //List<KIS100LOG> logs = (from json in msgArr
            //                        select JsonConvert.DeserializeObject<KIS100LOG>(json.ToString())).ToList();
            int idx = dataObj.msg["idx"];
            List<FromKIS100Log> logs = new List<FromKIS100Log>();
            foreach (var json in msgArr) {
                logs.Add(JsonConvert.DeserializeObject<FromKIS100Log>(json.ToString()));
            }
            foreach (var logData in logs) {
                logData.idx = idx;
            }
            dataObj.msg = logs;
        }

        private void RlogMessage() {
            dataObj.msg = "";
        }

        private void UdelMessage() {
            dataObj.msg = new FromKIS100Udel {
                stat = dataObj.msg["stat"]
            };
        }
        
        private void ReceivedDataTracker() {
            StaticAttribute.Function.tcpReceivedDataTracker.TrackReceivedDataNotify(new ReceivedFromKISDAO {
                sig = dataObj.sig,
                cmd = dataObj.cmd,
                type = dataObj.type,
                msg = dataObj.msg,
                ver = dataObj.ver

            });
        }
    }
}
