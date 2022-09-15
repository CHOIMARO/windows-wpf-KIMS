using KISM.DAO;
using KISM.DAO.Device;
using KISM.DAO.JSON;
using KISM.DAO.JSON.KISM3;
using KISM.DAO.KeyRel;
using KISM.DAO.Serial;
using KISM.DAO.TCP;
using KISM.StaticAttribute.Enum;
using KISM.Util;
using KISM.View.Function.Logout;
using KISM.View.Function.RequestFromKIS_100;
using KISM.View.Function.TcpConnect;
using KISM.View.Information;
using KISM.View.Loading;
using KISM.View.SubPage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace KISM.ViewModel {
    public class MainPageVM : INotifyPropertyChanged, IObserver<LoginTimerDAO> {
        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        LoadingWindow loadingWindow = new LoadingWindow();
        System.Timers.Timer checkExpirationTimer;
        int checkingCount = 0;
        bool firstImplement = true;
        #region DataContext
        private ObservableCollection<OverExpKeyToolTipDAO> moduleList = new ObservableCollection<OverExpKeyToolTipDAO>();
        public ObservableCollection<OverExpKeyToolTipDAO> ModuleList {
            get {
                return moduleList;
            }
            set {
                moduleList = value;
                onPropertyChanged("ModuleList");
            }
        }

        private System.Windows.Media.Brush expirationKeyColor = System.Windows.Media.Brushes.White;
        public System.Windows.Media.Brush ExpirationKeyColor {
            get { return expirationKeyColor; }
            set {
                expirationKeyColor = value;
                onPropertyChanged("ExpirationKeyColor");
            }
        }
        private string remainLoginTime = "10분 0초";
        public string RemainLoginTime {
            get {
                return remainLoginTime;
            }
            set {
                remainLoginTime = value;
                onPropertyChanged("RemainLoginTime");
            }
        }
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
        private string connectBtnBackground = "Red";
        public string ConnectBtnBackground {
            get {
                return connectBtnBackground;
            }
            set {
                connectBtnBackground = value;
                onPropertyChanged("ConnectBtnBackground");
            }
        }
        private string connectBtnTxt = "주입기와 연결 안됨";
        public string ConnectBtnTxt {
            get {
                return connectBtnTxt;
            }
            set {
                connectBtnTxt = value;
                onPropertyChanged("ConnectBtnTxt");
            }
        }
        #endregion

        internal void ConnectInit() {
            StaticAttribute.Function.tcpConnectUseCase.Connect();
        }
        internal void InitOverExpTooltip() {
            ModuleList.Clear();
            var keyRelAll = StaticAttribute.Function.selectKeyRelAllUseCase.Execute();
            foreach (var keyRelData in keyRelAll) {
                if (!keyRelData.stat.Equals("DEL")) {
                    TimeSpan timeSpanData = keyRelData.expdate - DateTime.Now;
                    if (timeSpanData.Days <= 30) {
                        ModuleList.Add(new OverExpKeyToolTipDAO {
                            Dpt = keyRelData.dpt,
                            ExpDate = (timeSpanData.Days + 1).ToString() + "일 남음",
                            Ppose = keyRelData.ppose
                        });
                    }
                }
            }
        }
        internal void CheckExpirationKey() {
            var keyRelInfoAll = StaticAttribute.Function.selectKeyRelAllUseCase.Execute();
            List<TimeSpan> timeSpanList = new List<TimeSpan>();
            foreach(var keyRelData in keyRelInfoAll) {
                if(!keyRelData.stat.Equals("DEL")) {
                    TimeSpan timeSpanData = keyRelData.expdate - DateTime.Now;
                    timeSpanList.Add(timeSpanData);
                }
            }
            if(timeSpanList.Count > 0) {
                CalculateTimeSpan(timeSpanList);
            }
        }

        private void CalculateTimeSpan(List<TimeSpan> timeSpanList) {
            TimeSpan minTimeSpanData = timeSpanList.Min();
            int days = minTimeSpanData.Days;
            if (days < 31) {
                ExpirationKeyColor = Brushes.Green;
                if (days < 21) {
                    ExpirationKeyColor = Brushes.Yellow;
                    if (days < 11) {
                        ExpirationKeyColor = Brushes.Orange;
                        if (days < 0) {
                            ExpirationKeyColor = Brushes.Red;
                        }
                    }
                }
                if (firstImplement) {
                    firstImplement = false;
                }
            } else {
                ExpirationKeyColor = Brushes.White;
            }
        }

        public void TcpDisconnect() {
            StaticAttribute.Function.tcpConnectUseCase.Disconnect();
            ConnectBtnBackground = "Red";
            ConnectBtnTxt = "주입기와 연결 안됨";
        }

        internal void Init() {
            if (StaticAttribute.Function.tcpConnect == 1 && StaticAttribute.Function.authState == true) {
                ConnectBtnBackground = "Green";
                ConnectBtnTxt = "주입기와 연결 됨";
            } else if (StaticAttribute.Function.tcpConnect == 1 && StaticAttribute.Function.authState == false) {
                ConnectBtnBackground = "Orange";
                ConnectBtnTxt = "주입기와 연결 중..";
            } else if (StaticAttribute.Function.tcpConnect == 2) {
                ConnectBtnBackground = "Orange";
                ConnectBtnTxt = "주입기와 연결 중..";
            } else if (StaticAttribute.Function.tcpConnect == 0) {
                ConnectBtnBackground = "Red";
                ConnectBtnTxt = "주입기와 연결 안됨";
            }
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {
            
        }

        public void ConnectBtnClick() {
            if (StaticAttribute.Function.tcpConnect == 2) {
                Console.WriteLine("TCP 연결을 취소합니다.");
                Console.WriteLine("Serial 연결을 취소합니다.");
                TcpDisconnect();
                StaticAttribute.Function.tcpConnect = 0;
            } else if (StaticAttribute.Function.tcpConnect == 0) {
                Console.WriteLine("TCP 연결을 시도합니다.");
                Console.WriteLine("Serial 연결을 시도합니다.");
                ConnectBtnBackground = "Orange";
                ConnectBtnTxt = "주입기와 연결 중..";
                ConnectInit(); //TCP 연결 부분
                //serialInit(); //씨리얼 연결 부분
                StaticAttribute.Function.tcpConnect = 2;
            } else if (StaticAttribute.Function.tcpConnect == 1) {
                TcpDisconnectPage tcpDisconnectPage = new TcpDisconnectPage();
                tcpDisconnectPage.ShowDialog();

                if(StaticAttribute.Function.tcpDisconnectPageState) {
                    TcpDisconnect();
                    StaticAttribute.Function.tcpConnect = 0;
                }
            }
        }
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
       
        public void CheckingExpirationKeyTimer() {
            if(checkExpirationTimer == null) {
                checkExpirationTimer = new System.Timers.Timer();
                checkExpirationTimer.Interval = 1000;
                checkExpirationTimer.Elapsed += CheckExpirationTimer_Elapsed;
                checkExpirationTimer.Start();
            } else {
                checkExpirationTimer.Stop();
                checkExpirationTimer = new System.Timers.Timer();
                checkExpirationTimer.Interval = 1000;
                checkExpirationTimer.Elapsed += CheckExpirationTimer_Elapsed;
                checkExpirationTimer.Start();
            }
            
        }
        private void CheckExpirationTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            checkingCount++;
            if (checkingCount >9) {
                CheckExpirationKey();
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
                    InitOverExpTooltip();
                }));
                checkingCount = 0;
            }
        }

        public void InsertLog(LogEnum logEnum, string message) {
            StaticAttribute.Function.insertLogInfoItemUseCase.Execute(logEnum, message);
        }
        public void OnNext(LoginTimerDAO value) {
            LoginTime = value.time;
        }

        public void CurrentConnectedStatus() {
            ConnectBtnBackground = "Green";
            ConnectBtnTxt = "주입기와 연결 됨";
            //StaticAttribute.Function.tcpConnect = 1;
        }
        public void CurrentTryToConnectStatus() {
            ConnectBtnBackground = "Orange";
            ConnectBtnTxt = "주입기와 연결 중..";
            //StaticAttribute.Function.tcpConnect = 2;
        }
        public void CurrentNotConnectedStatus() {
            ConnectBtnBackground = "Red";
            ConnectBtnTxt = "주입기와 연결 안됨";
            //StaticAttribute.Function.tcpConnect = 0;
        }
    }
}
