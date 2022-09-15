using KISM.DAO;
using KISM.StaticAttribute.Enum;
using KISM.Util;
using KISM.View.Information;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KISM.ViewModel.Setting {
    class CommSettingPageVM : INotifyPropertyChanged, IObserver<LoginTimerDAO> {
        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #region DataContext
        private string loginTime = "10분 0초";
        public string LoginTime {
            get {
                return loginTime;
            }
            set {
                loginTime = value;
                onPropertyChanged("LoginTime");
            }
        }
        private List<ComboBoxItem> serialItems = new List<ComboBoxItem>();
        public List<ComboBoxItem> SerialItems {
            get {
                return serialItems;
            }
            set {
                serialItems = value;
                onPropertyChanged("SerialItems");
            }
        }

        private string tCPIPTXT = string.Empty;
        public string TCPIPTXT {
            get {
                return tCPIPTXT;
            }
            set {
                tCPIPTXT = value;
                onPropertyChanged("TCPIPTXT");
            }
        }

        private string tCPPORTTXT = string.Empty;
        public string TCPPORTTXT {
            get {
                return tCPPORTTXT;
            }
            set {
                tCPPORTTXT = value;
                onPropertyChanged("tCPPORTTXT");
            }
        }
        #endregion
        #region Serial Setting Info
        private string serialPortTxt = string.Empty;
        public string SerialPortTxt {
            get {
                return serialPortTxt;
            }
            set {
                serialPortTxt = value;
                onPropertyChanged("SerialPortTxt");
            }
        }

        private string serialBaudRateTxt = string.Empty;
        public string SerialBaudRateTxt {
            get {
                return serialBaudRateTxt;
            }
            set {
                serialBaudRateTxt = value;
                onPropertyChanged("SerialBaudRateTxt");
            }
        }

        private string serialParityTxt = string.Empty;
        public string SerialParityTxt {
            get {
                return serialParityTxt;
            }
            set {
                serialParityTxt = value;
                onPropertyChanged("SerialParityTxt");
            }
        }

        private string serialDataBitTxt = string.Empty;
        public string SerialDataBitTxt {
            get {
                return serialDataBitTxt;
            }
            set {
                serialDataBitTxt = value;
                onPropertyChanged("SerialDataBitTxt");
            }
        }

        private string serialStopBitTxt = string.Empty;
        public string SerialStopBitTxt {
            get {
                return serialStopBitTxt;
            }
            set {
                serialStopBitTxt = value;
                onPropertyChanged("SerialStopBitTxt");
            }
        }
        #endregion
        public void SetObserver(dynamic info) {
            StaticAttribute.Function.loginTimerTracker.Subscribe(this);
            StaticAttribute.Function.tcpIsConnectTracker.Subscribe(info);
            StaticAttribute.Function.tcpReceivedDataTracker2.Subscribe(info);
        }
        public void UnsetObserver(dynamic info) {
            StaticAttribute.Function.loginTimerTracker.UnSubscribe(this);
            StaticAttribute.Function.tcpIsConnectTracker.Unsubscribe(info);
            StaticAttribute.Function.tcpReceivedDataTracker2.Unsubscribe(info);
        }
        internal void Init() {
            // TODO : Select comm setting info
            var info = StaticAttribute.Function.selectCommInfoAllUseCase.Execute();
            if (info.Count > 0) {
                TCPIPTXT = info[info.Count-1].ip;
                TCPPORTTXT = info[info.Count - 1].port.ToString();
            }
        }
        public void InitPort() {
            string[] ports = SerialPort.GetPortNames();
            foreach(string port in ports) {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = port;

                SerialItems.Add(comboBoxItem);
            }
        }
        public void InitSettingInfo() {
            if (StaticAttribute.Function.serialInfoDAO.port != null && !StaticAttribute.Function.serialInfoDAO.port.Equals("")) {
                SerialPortTxt = StaticAttribute.Function.serialInfoDAO.port;
                SerialBaudRateTxt = StaticAttribute.Function.serialInfoDAO.baudRate;
                SerialParityTxt = StaticAttribute.Function.serialInfoDAO.parity;
                SerialDataBitTxt = StaticAttribute.Function.serialInfoDAO.dataBit;
                SerialStopBitTxt = StaticAttribute.Function.serialInfoDAO.stopBit;
            }
        }

        internal void SaveBtnEvent() {
            // TODO : Insert comm setting info
            int port = 2500;
            bool state = int.TryParse(TCPPORTTXT, out port);
            if (state) {
                InsertLog(StaticAttribute.Enum.LogEnum.INFO, "통신을 위한 연결 설정이 변경되었습니다.");
                StaticAttribute.Function.insertCommInfoItemUseCase.Execute(TCPIPTXT, port);
                StaticAttribute.Function.tcpConnectUseCase.Disconnect();
                InformationMessage.InformationShowDialog("통신을 위한 연결 설정이 변경되었습니다.");
            }
        }
        public void InsertLog(LogEnum logEnum, string message) {
            StaticAttribute.Function.insertLogInfoItemUseCase.Execute(logEnum, message);
        }

        public void OnNext(LoginTimerDAO value) {
            LoginTime = value.time;
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {
            
        }
    }
}
