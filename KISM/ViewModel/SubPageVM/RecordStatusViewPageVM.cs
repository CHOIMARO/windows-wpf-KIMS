using KISM.DAO;
using KISM.DAO.TCP;
using KISM.StaticAttribute.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KISM.ViewModel.SubPageVM {

    public class RecordStatusViewPageVM : INotifyPropertyChanged, IObserver<LoginTimerDAO> {
        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #region
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
        private Visibility keyManagementWindow = Visibility.Visible;

        public Visibility KeyManagementWindow {
            get {
                return keyManagementWindow;
            }
            set {
                keyManagementWindow = value;
                onPropertyChanged("KeyManagementWindow");
            }
        }
        private Visibility keyDistributionWindow = Visibility.Collapsed;

        public Visibility KeyDistributionWindow {
            get {
                return keyDistributionWindow;
            }
            set {
                keyDistributionWindow = value;
                onPropertyChanged("KeyDistributionWindow");
            }
        }

        private ObservableCollection<string> statItems = new ObservableCollection<string> {
            "암호키 등록 현황", "암호키 배포 현황"
        };
        public ObservableCollection<string> StatItems {
            get {
                return statItems;
            }
            set {
                statItems = value;
                onPropertyChanged("StatItems");
            }
        }
        #endregion

        internal void Init() {
            
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
        public void ShowKeyManagementWindow() {
            KeyManagementWindow = Visibility.Visible;
            KeyDistributionWindow = Visibility.Hidden;
        }
        public void ShowKeyDistributionWindow() {
            KeyManagementWindow = Visibility.Hidden;
            KeyDistributionWindow = Visibility.Visible;
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
