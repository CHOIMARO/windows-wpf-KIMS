using External.DAO.JSON;
using External.DAO.JSON.KIS100;
using External.StaticAttribute.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Serial {
    public class SerialService : Service {
        SerialPort serialPort = new SerialPort();
        int startIndex = -1;
        int endIndex = -1;
        List<char> dataList = new List<char>();
        string receivedSerialData = string.Empty;

        public int connect(dynamic serialSettingInfo) {
            int state = 0;
            if (objectChecked(serialSettingInfo)) {
                state = init(serialSettingInfo);
            }
            return state;
        }
        private bool objectChecked(dynamic serialSettingInfo) {
            if (serialSettingInfo.port == null && serialSettingInfo.port.Equals("")
                && serialSettingInfo.baudRate == null && serialSettingInfo.baudRate.Equals("")
                && serialSettingInfo.parity == null && serialSettingInfo.parity.Equals("")
                && serialSettingInfo.dataBit == null && serialSettingInfo.dataBit.Equals("")
                && serialSettingInfo.stopBit == null && serialSettingInfo.stopBit.Equals("")) {
                return false;
            }
            return true;
        }
        private int init(dynamic serialSettingInfo) {
            try {
                if (!serialPort.IsOpen) {
                    serialPort.PortName = serialSettingInfo.port;
                    serialPort.BaudRate = int.Parse(serialSettingInfo.baudRate);
                    serialPort.Parity = serialParity(serialSettingInfo.parity);
                    serialPort.DataBits = int.Parse(serialSettingInfo.dataBit);
                    serialPort.StopBits = serialStopBits(serialSettingInfo.stopBit);

                    return serialPortOpen();
                } else {
                    serialPort.Close();
                    serialPort.PortName = serialSettingInfo.port;
                    serialPort.BaudRate = int.Parse(serialSettingInfo.baudRate);
                    serialPort.Parity = serialParity(serialSettingInfo.parity);
                    serialPort.DataBits = int.Parse(serialSettingInfo.dataBit);
                    serialPort.StopBits = serialStopBits(serialSettingInfo.stopBit);
                    return serialPortOpen();
                }
            } catch (Exception) {
                //TODO : Serial port setting error logging
            }
            return 0;
        }

        private int serialPortOpen() {
            try {
                if (!serialPort.IsOpen) {
                    serialPort.Open();
                    serialPort.DataReceived += SerialPort_DataReceived;
                } else {
                    serialPort.Close();
                }
                return 1;
            } catch (Exception) {
                //TODO : Serial port open error logging
                return 2;
            }
        }
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e) {
            Dictionary<string, string> jsonData = new Dictionary<string, string>();
            serialPort.Encoding = System.Text.Encoding.GetEncoding(65001);
            string receivedJSONData = string.Empty;
            string buff = serialPort.ReadExisting();

            receivedSerialData += buff;
            char[] charData = receivedSerialData.ToCharArray();
            receivedSerialData = string.Empty;
            foreach (var data in charData) {
                dataList.Add(data);
            }

            if (checkedJsonType()) {
                receivedJSONData = setJsonData();
                Console.WriteLine("수신한 Serial 메세지 : " + receivedJSONData);
                KIS100DataDAO kis100DataDAO = JsonConvert.DeserializeObject<KIS100DataDAO>(receivedJSONData);
                kis100DataDAO = compareCmd(kis100DataDAO);
                StaticAttribute.Function.serialReceivedDataTracker.TrackReceivedDataNotify(kis100DataDAO);
            }
        }

        public KIS100DataDAO compareCmd(KIS100DataDAO kis100DataDAO) {
            switch(kis100DataDAO.cmd) {
                case commandEnum.HELLO:
                    kis100DataDAO = helloMessage(kis100DataDAO);
                    break;
                case commandEnum.UCK:
                    kis100DataDAO = stateMessage(kis100DataDAO);
                    break;
                case commandEnum.UREG:
                    kis100DataDAO = stateMessage(kis100DataDAO);
                    break;
                case commandEnum.UDEL:
                    kis100DataDAO = stateMessage(kis100DataDAO);
                    break;
                case commandEnum.LOG:
                    kis100DataDAO = logMessage(kis100DataDAO);
                    break;
                case commandEnum.DLOG:
                    kis100DataDAO = stateMessage(kis100DataDAO);
                    break;
                case commandEnum.REK:
                    kis100DataDAO = stateMessage(kis100DataDAO);
                    break;
                case commandEnum.KDEL:
                    kis100DataDAO = stateMessage(kis100DataDAO);
                    break;
                case commandEnum.GEN:
                    kis100DataDAO = genMessage(kis100DataDAO);
                    break;
            }
            return kis100DataDAO;
        }

        public KIS100DataDAO helloMessage(KIS100DataDAO kis100DataDAO) {
            kis100DataDAO.msg = new FromKIS100Hello {
                id = kis100DataDAO.msg["id"],
                hw = kis100DataDAO.msg["hw"],
                fw = kis100DataDAO.msg["fw"],
                sn = kis100DataDAO.msg["sn"]
            };
            return kis100DataDAO;
        }
        public KIS100DataDAO stateMessage(KIS100DataDAO kis100DataDAO) {
            kis100DataDAO.msg = new FromKIS100State {
                stat = kis100DataDAO.msg["stat"]
            };
            return kis100DataDAO;
        }
        public KIS100DataDAO logMessage(KIS100DataDAO kis100DataDAO) {
            JArray msgArr = (JArray)kis100DataDAO.msg["list"];
            int idx = kis100DataDAO.msg["idx"];
            //List<KIS100LOG> logs = (from json in msgArr
            //                        select JsonConvert.DeserializeObject<KIS100LOG>(json.ToString())).ToList();

            List<FromKIS100Log> logs = new List<FromKIS100Log>();
            foreach(var json in msgArr) {
                logs.Add(JsonConvert.DeserializeObject<FromKIS100Log>(json.ToString()));
            }
            foreach(var logData in logs) {
                logData.idx = idx;
            }

            kis100DataDAO.msg = logs;
            return kis100DataDAO;
        }

        public KIS100DataDAO genMessage(KIS100DataDAO kis100DataDAO) {
            return kis100DataDAO;
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
        private string setJsonData() {
            string data = string.Empty;
            for (int index = startIndex; index < endIndex + 1; index++) {
                data += dataList[index];
            }
            dataList.RemoveRange(0, endIndex + 1);
            return data;
        }
        private Parity serialParity(string serialSetting) {
            Parity parity = Parity.None;
            ParityInfoEnum parityInfo = (ParityInfoEnum)Enum.Parse(typeof(ParityInfoEnum), serialSetting.ToUpper());

            switch (parityInfo) {
                case ParityInfoEnum.NONE:
                    parity = Parity.None;
                    break;
                case ParityInfoEnum.ODD:
                    parity = Parity.Odd;
                    break;
                case ParityInfoEnum.EVEN:
                    parity = Parity.Even;
                    break;
                case ParityInfoEnum.MARK:
                    parity = Parity.Mark;
                    break;
                case ParityInfoEnum.SPACE:
                    parity = Parity.Space;
                    break;
            }
            return parity;
        }
        private StopBits serialStopBits(string serialSetting) {
            StopBits stopBits = StopBits.One;
            StopBitsInfoEnum stopBitsInfo = (StopBitsInfoEnum)Enum.Parse(typeof(StopBitsInfoEnum), serialSetting);

            switch (stopBitsInfo) {
                case StopBitsInfoEnum.NONE:
                    stopBits = StopBits.None;
                    break;
                case StopBitsInfoEnum.ONE:
                    stopBits = StopBits.One;
                    break;
                case StopBitsInfoEnum.OnePointFive:
                    stopBits = StopBits.OnePointFive;
                    break;
                case StopBitsInfoEnum.TWO:
                    stopBits = StopBits.Two;
                    break;
            }

            return stopBits;
        }

        public void setObserver(dynamic info) {
            StaticAttribute.Function.serialReceivedDataTracker.Subscribe(info);
        }
        public bool disconnect() {
            try {
                if (serialPort.IsOpen) {
                    serialPort.Close();
                    return true;
                }
            } catch (Exception) {
                //TODO : Serial port open error logging
            }
            return false;
        }
        public bool send(string json) {
            if (serialPort.IsOpen) {
                serialPort.Write(json);
                Console.WriteLine("송신한 Serial 메세지 : " + json);
                return true;
            } else {
                Console.WriteLine("시리얼 포트가 닫혀있습니다.");
                return true;
            }
        }
    }

}
