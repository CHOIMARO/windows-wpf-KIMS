using KISM.DAO;
using KISM.DAO.Injector;
using KISM.DAO.JSON;
using KISM.DAO.JSON.KISM3;
using KISM.DAO.TCP;
using KISM.StaticAttribute.Enum;
using KISM.Util;
using KISM.View.Loading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace KISM.ViewModel.SubPageVM {
    class ManagerRegistrationManagementPageVM : INotifyPropertyChanged, IObserver<LoginTimerDAO> {
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

        private ObservableCollection<InjectorDAO> registeredDataRow = new ObservableCollection<InjectorDAO>();

        public ObservableCollection<InjectorDAO> RegisteredDataRow {
            get {
                return registeredDataRow;
            }
            set {
                registeredDataRow = value;
                onPropertyChanged("RegisteredDataRow");
            }
        }
        private double userIconOpacity = 0.6;
        public double UserIconOpacity {
            get {
                return userIconOpacity;
            }
            set {
                userIconOpacity = value;
                onPropertyChanged("UserIconOpacity");
            }
        }
        private Visibility defaultWindow = Visibility.Collapsed;

        public Visibility DefaultWindow {
            get {
                return defaultWindow;
            }
            set {
                defaultWindow = value;
                onPropertyChanged("DefaultWindow");
            }
        }
        private Visibility connectKIS100Window = Visibility.Collapsed;

        public Visibility ConnectKIS100Window {
            get {
                return connectKIS100Window;
            }
            set {
                connectKIS100Window = value;
                onPropertyChanged("ConnectKIS100Window");
            }
        }
        private Visibility connectKIS200Window = Visibility.Collapsed;

        public Visibility ConnectKIS200Window {
            get {
                return connectKIS200Window;
            }
            set {
                connectKIS200Window = value;
                onPropertyChanged("ConnectKIS200Window");
            }
        }
        #endregion
        internal void Init() {
            
        }

        public void ShowRegisteredData() {
            try {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    RegisteredDataRow.Clear();
                    AddColumnData();
                }));


            } catch (NullReferenceException) {
                
            }
        }
        private void AddColumnData() {
            if(StaticAttribute.Function.tcpConnect == 1) {
                var injectorInfoItem = StaticAttribute.Function.selectInjectorInfoItemUseCase.Execute(StaticAttribute.Function.connectedInjectorDAO.id);
                if(injectorInfoItem.Count > 0) {
                    var injectorMgrList = StaticAttribute.Function.selectInjectorMgrListUseCase.Execute(injectorInfoItem[0].idx);

                    foreach(var injectorMgrItem in injectorMgrList) {
                        RegisteredDataRow.Add(new InjectorDAO {
                            Num = (RegisteredDataRow.Count + 1).ToString(),
                            RegDate = injectorInfoItem[0].timestamp.ToString(),
                            IjAccount = injectorMgrItem.uid,
                            IjName = injectorInfoItem[0].ijid,
                            Sn = injectorInfoItem[0].sn
                        });
                    }
                }
            } else {
                RegisteredDataRow.Clear();
            }
        }

        public void DeleteUser() {
            if(StaticAttribute.Function.connectedInjectorDAO.name.Equals("KIS-200") && StaticAttribute.Function.tcpConnect == 1 || StaticAttribute.Function.connectedInjectorDAO.name.Equals("KIS-100") && StaticAttribute.Function.tcpConnect == 1) {
                
            }else if (StaticAttribute.Function.connectedInjectorDAO.name.Equals("KIS-200") && StaticAttribute.Function.serialConnect) {

            }
            
            StaticAttribute.Function.logCommand.debugLog("[VM.ManagerRegistrationPage.Request User Delete]");
            InsertLog(LogEnum.INFO, "주입기 계정 삭제 메세지 송신");
        }
        internal void InsertInjectorMgrItem(string injectorID, string injectorPW) {
            StaticAttribute.Function.insertInjectorMgrItemUseCase.Execute(injectorID, injectorPW);
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {
            
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

        public void ShowDefaultWindow() {
            DefaultWindow = Visibility.Visible;
            ConnectKIS100Window = Visibility.Collapsed;
            ConnectKIS200Window = Visibility.Collapsed;
        }
        public void ShowConnectKIS100Window() {
            DefaultWindow = Visibility.Collapsed;
            ConnectKIS100Window = Visibility.Visible;
            ConnectKIS200Window = Visibility.Collapsed;
        }
        public void ShowConnectKIS200Window() {
            DefaultWindow = Visibility.Collapsed;
            ConnectKIS100Window = Visibility.Collapsed;
            ConnectKIS200Window = Visibility.Visible;
        }
        public void InsertLog(LogEnum logEnum, string message) {
            StaticAttribute.Function.insertLogInfoItemUseCase.Execute(logEnum, message);
        }

        public void OnNext(LoginTimerDAO value) {
            LoginTime = value.time;
        }
    }
}
